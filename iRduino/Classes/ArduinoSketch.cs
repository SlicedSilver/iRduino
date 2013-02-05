//========================================//
// iRduino - Created by Mark Silverwood  //
//======================================//
namespace iRduino.Classes
{
    using System.Globalization;
    using System.IO;
    using System.Windows;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ArduinoInterfaces;
    using Microsoft.Win32;

    public class ArduinoPins
    {
        public int DataPin;

        public int ClockPin;

        public List<int> UnitStrobePins;

        public List<int> TM1640DataPins;

        public List<int> TM1640ClockPins;
    }

    public class ArduinoSketch
    {
        public static void GenerateSketch(ConfigurationOptions configurationOptions, ArduinoPins pins)
        {
            var numberTm1640S = 0;
            var tm1640UnitNumbers = new List<int>();
            for (var m = 0; m < configurationOptions.DisplayUnitConfigurations.Count; m++)
            {
                if (configurationOptions.DisplayUnitConfigurations[m].IsTM1640)
                {
                    tm1640UnitNumbers.Add(m);
                    numberTm1640S += 1;
                }
            }
            
            
            var sb = new StringBuilder("");
            //Write header
            sb.AppendLine(
                "/*\r\niRduino - Arduino Sketch\r\nAuthor: Mark Silverwood\r\n\r\nUsing TM1638 library from Ricardo Batista <rjbatista at gmail dot com>\r\n\r\nThis Arduino Sketch interfaces to the iRduino windows application.");

            //Write Date and Configuration Name into File
            sb.AppendLine(String.Format("This Sketch was generated on: {0}", DateTime.Now.ToShortDateString()));
            string temp = "";
            int numberTM1638 = configurationOptions.DisplayUnitConfigurations.Count - numberTm1640S;
            int messageLengthInt = 0;
            string serialSpeed = configurationOptions.SerialPortSpeed;
            switch (configurationOptions.DisplayUnitConfigurations.Count)
            {
                case 1:
                    temp = "single (1)";
                    messageLengthInt = 16;
                    break;
                case 2:
                    temp = "double (2)";
                    messageLengthInt = 27;
                    break;
                case 3:
                    temp = "triple (3)";
                    messageLengthInt = 38;
                    break;
                case 4:
                    temp = "quadruple (4)";
                    messageLengthInt = 49;
                    break;
                case 5:
                    temp = "quintuple (5)";
                    messageLengthInt = 60;
                    break;
                case 6:
                    temp = "sextuple (6)";
                    messageLengthInt = 71;
                    break;
            }
            string messageLength = (messageLengthInt + Constants.ExtraMessageLengthTM1640 * numberTm1640S).ToString(CultureInfo.InvariantCulture);
            sb.AppendLine(String.Format("This Sketch is for a {0} Display Unit Configuration", temp));
            sb.AppendLine("\r\n */");
            //includes TM1638 & maybe InvertedTM1638?
            sb.AppendLine("");
            bool normal = false;
            bool inverted = false;
            foreach (var unit in configurationOptions.DisplayUnitConfigurations)
            {
                if (unit.Inverted)
                {
                    inverted = true;
                }
                else
                {
                    normal = true;
                }
            }
            if (numberTM1638 > 0)
            {
                if (normal)
                {
                    sb.AppendLine("#include <TM1638.h>");
                }
                if (inverted)
                {
                    sb.AppendLine("#include <InvertedTM1638.h>");
                }
            }
            if (numberTm1640S > 0)
            {
                sb.AppendLine("#include <TM1640.h>");
            }
            sb.AppendLine("");
            //Setup Pins
            sb.AppendLine("//////Setup Here");
            if (numberTM1638 > 0)
            {
                sb.AppendLine(String.Format("#define dataPin {0}", pins.DataPin));
                sb.AppendLine(String.Format("#define clockPin {0}", pins.ClockPin));
            }
            int tmCount = 0;
            for (var p = 0; p < configurationOptions.DisplayUnitConfigurations.Count; p++)
            {
                if (!configurationOptions.DisplayUnitConfigurations[p].IsTM1640)
                {
                    sb.AppendLine(String.Format("#define strobePin{0} {1}", p + 1, pins.UnitStrobePins[p]));
                }
                else
                {
                    sb.AppendLine(String.Format("#define TM1640dataPin{0} {1}", p + 1,
                                                pins.TM1640DataPins[tmCount])); //error here
                    sb.AppendLine(String.Format("#define TM1640clockPin{0} {1}", p + 1,
                                                pins.TM1640ClockPins[tmCount]));
                    tmCount++;
                }
            }
            sb.AppendLine(String.Format("#define NumberUnits {0}", configurationOptions.DisplayUnitConfigurations.Count));
            sb.AppendLine(String.Format("#define NumberTM1638Units {0}",
                                        configurationOptions.DisplayUnitConfigurations.Count - numberTm1640S));

            sb.AppendLine("//////Setup Finished");
            sb.AppendLine("");
            //Setup Message Length
            sb.AppendLine(String.Format("#define startByte1 {0}",Constants.MessageStartByte1));
            sb.AppendLine(String.Format("#define startByte2 {0}",Constants.MessageStartByte2));
            sb.AppendLine(String.Format("#define endByte {0}", Constants.MessageEndByte));
            sb.AppendLine(String.Format("#define messageLength {0}", messageLength));
            sb.AppendLine("");
            //Add Modules
            for (var p = 0; p < configurationOptions.DisplayUnitConfigurations.Count; p++)
            {
                if (configurationOptions.DisplayUnitConfigurations[p].IsTM1640)
                {
                    sb.AppendLine(String.Format("TM1640 module{0}(TM1640dataPin{0},TM1640clockPin{0},false,0);",
                                                p + 1));
                }
                else
                {
                    if (!configurationOptions.DisplayUnitConfigurations[p].Inverted)
                    {
                        sb.AppendLine(String.Format("TM1638 module{0}(dataPin,clockPin,strobePin{0},false,0);", p + 1));
                    }
                    else
                    {
                        sb.AppendLine(String.Format("InvertedTM1638 module{0}(dataPin,clockPin,strobePin{0},false,0);",
                                                    p + 1));
                    }
                }
            }
            sb.AppendLine("");
            //Variable Declarations
            sb.AppendLine("//// Variable Declarations");
            sb.AppendLine(
                "word leds; \r\n" +
                "byte segments[8], TM1640segments[16], redLeds, greenLeds, intensity, unit, sum, i, readCount, checkerPosition;\r\n" +
                "byte buttons[NumberTM1638Units], oldbuttons[NumberTM1638Units], lastButtonSend[NumberTM1638Units];\r\n" +
                "long lastCheck, debounceDelay = 100;\r\n" +
                "boolean sendButtons = false;\r\n" +
                "byte messageHolder[messageLength], message[messageLength-4], messagePosition = -1;");
            sb.AppendLine("");
            //void Setup() - Set Serial Speed!!
            sb.AppendLine("void setup() {");
            sb.AppendLine(String.Format("Serial.begin({0});", serialSpeed));
            sb.AppendLine(
                "for(int u = 0; u < NumberTM1638Units; u++)\r\n" +
                "{\r\n" +
                "oldbuttons[u] = 0;\r\n" +
                "}\r\n" +
                "intensity = 0;\r\n" +
                "lastCheck = millis();}");
            sb.AppendLine("");
            //void loop()
            sb.AppendLine("void loop() {");// +
            if (numberTM1638 > 0)
            {
                sb.AppendLine("buttonsCheck();");
            }
            sb.AppendLine("if (Serial.available() > 0){\r\n" +
                "messagePosition++;\r\n" +
                "if(messagePosition == messageLength) messagePosition = 0;\r\n" +
                "messageHolder[messagePosition] = Serial.read();\r\n" +
                "messageChecker();\r\n" +
                "}\r\n" +
                "}");
            sb.AppendLine("");

            //void messageChecker()
            sb.AppendLine("void messageChecker()\r\n" +
                          "{\r\n" +
                          "checkerPosition = (messagePosition == 0) ? messageLength-1 : messagePosition - 1;\r\n" +
                          "if(messageHolder[checkerPosition] != endByte) return; //end byte\r\n" +
                          "checkerPosition = (messagePosition == messageLength -1) ? 0 : messagePosition + 1;\r\n" +
                          "if(messageHolder[checkerPosition]!= startByte1) return; //start byte1\r\n" +
                          "checkerPosition = (checkerPosition == messageLength - 1) ? 0 : checkerPosition+1;\r\n" +
                          "if(messageHolder[checkerPosition]!= startByte2) return; // start byte 2\r\n" +
                          "checkerPosition++;\r\n" +
                          "readCount = 0;\r\n" +
                          "sum = 0;\r\n" +
                          "while (readCount < messageLength-4)\r\n" +
                          "{\r\n" +
                          "if(checkerPosition == messageLength) checkerPosition = 0;\r\n" +
                          "message[readCount] = messageHolder[checkerPosition];\r\n" +
                          "sum += messageHolder[checkerPosition];\r\n" +
                          "readCount++;\r\n" +
                          "checkerPosition++;\r\n" +
                          "}\r\n" +
                          "if(sum == messageHolder[messagePosition]) messageRead(message);\r\n" +
                          "}");
            sb.AppendLine("");
            //void serialRead()
            sb.AppendLine("void messageRead(byte _message[]){\r\n" +
                          "i = 0;\r\n" +
                          "intensity = _message[i++];\r\n" +
                          "for(int u = 1; u <= NumberUnits; u++)\r\n" +
                          "{\r\n" +
                          "unit = _message[i++];");
            var tempStr = "if(";
            var count = 0;
            for (var p = 0; p < configurationOptions.DisplayUnitConfigurations.Count; p++)
            {
                if (configurationOptions.DisplayUnitConfigurations[p].IsTM1640)
                {
                    if (count != 0)
                    {
                        tempStr += " || ";
                    }
                    tempStr += string.Format("u=={0}", p + 1);
                    count++;
                }
            }
            if (count == 0)
            {
                tempStr += "false";
            }

            tempStr += ")\r\n{";
            sb.AppendLine(tempStr);
            sb.AppendLine("for(int x = 0;x<16;x++){\r\n" +
                          "TM1640segments[x] = _message[i++];\r\n" +
                          "}\r\n" +
                          "updateDisplay(unit,word(0,0),TM1640segments,intensity);\r\n" +
                          "}\r\n" +
                          "else\r\n" +
                          "{\r\n" +
                          "greenLeds = _message[i++];\r\n" +
                          "redLeds = _message[i++];\r\n" +
                          "for(int x = 0;x<8;x++){\r\n" +
                          "segments[x] = _message[i++];\r\n" +
                          "}\r\n" +
                          "updateDisplay(unit,word(greenLeds,redLeds),segments,intensity);\r\n" +
                          "}\r\n" +
                          "}\r\n" +
                          "}");
            sb.AppendLine("");

            //void buttonCheck() - set right number of modules here
            sb.AppendLine("void buttonsCheck(){\r\n" +
                          "if ((millis() - lastCheck)<debounceDelay) return;\r\n" +
                          "lastCheck = millis();");
            int buttonCount = 0;
            for (var p = 0; p < configurationOptions.DisplayUnitConfigurations.Count; p++)
            {
                if (!configurationOptions.DisplayUnitConfigurations[p].IsTM1640)
                {
                    sb.AppendLine(String.Format("buttons[{0}] = module{1}.getButtons();", buttonCount, p + 1));
                    buttonCount++;
                }
            }
            sb.AppendLine("sendButtons = false;\r\n" +
                          "for(int u = 0; u < NumberTM1638Units; u++)\r\n" +
                          "{\r\n" +
                          "if (buttons[u] != lastButtonSend[u] && buttons[u] != oldbuttons[u]){\r\n" +
                          "sendButtons = true;\r\n" +
                          "}\r\n" +
                          "oldbuttons[u] = buttons[u];\r\n" +
                          "}");
            sb.AppendLine("if(sendButtons){\r\n" +
                          "Serial.write(startByte1);\r\n" +
                          "for(int u = 0; u < NumberTM1638Units; u++)\r\n" +
                          "{\r\n" +
                          "lastButtonSend[u] = buttons[u];\r\n" +
                          "Serial.write(lastButtonSend[u]);\r\n" +
                          "}\r\n" +
                          "}\r\n" +
                          "}");
            sb.AppendLine("");
            //void UpdateDisplay() - right number of case statements here
            sb.AppendLine("void updateDisplay(byte unit, word _leds, byte _segments[], byte _intensity){\r\n" +
                          "switch(unit){");
            for (var p = 0; p < configurationOptions.DisplayUnitConfigurations.Count; p++)
            {
                if (configurationOptions.DisplayUnitConfigurations[p].IsTM1640)
                {
                    sb.AppendLine(String.Format("case {0}:", p + 1));
                    sb.AppendLine(String.Format("module{0}.setupDisplay(true,_intensity);", p + 1));
                    sb.AppendLine(String.Format("module{0}.setDisplay(_segments,16);", p + 1));
                    sb.AppendLine("break;");
                }
                else
                {
                    sb.AppendLine(String.Format("case {0}:", p + 1));
                    sb.AppendLine(String.Format("module{0}.setupDisplay(true,_intensity);", p + 1));
                    sb.AppendLine(String.Format("module{0}.setLEDs(_leds);", p + 1));
                    sb.AppendLine(String.Format("module{0}.setDisplay(_segments);", p + 1));
                    sb.AppendLine("break;");
                }
            }
            sb.AppendLine("}\r\n" +
                          "}");
            //Ask for file location

            // Configure save file dialog box
            var dlg = new SaveFileDialog
            {
                FileName = configurationOptions.Name,
                DefaultExt = ".ino",
                Filter = "Arduino Sketch (.ino)|*.ino"
            };

            // Show save file dialog box
            bool? result = dlg.ShowDialog();

            // Process save file dialog box results 
            if (result != true) return;
            // Save document 
            string fileLocation = dlg.FileName;

            //Save files
            using (var outfile = new StreamWriter(fileLocation))
            {
                outfile.Write(sb.ToString());
            }
            //Show Done Messagebox
            //UploaderDialog uploaderDialogWindow = new UploaderDialog(fileLocation);
            //uploaderDialogWindow.ShowDialog();
            MessageBox.Show("Finished Saving Arduino Sketch");
        }
    }
}
