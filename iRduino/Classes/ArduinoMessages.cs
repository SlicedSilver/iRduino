//========================================//
// iRduino - Created by Mark Silverwood  //
//======================================//

namespace iRduino.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Collections;
    using System.Windows.Threading;

    using ArduinoInterfaces;

    public class TMStringMessage
    {
        public List<string> Display;

        public List<byte[]> Dots;

        public int Intensity;

        public List<bool> UnitType;
    }

    public class TMLEDSMessage
    {
        public List<byte> Green;

        public List<byte> Red;

        public int Intensity;
    }

    public class ArduinoMessages
    {
        private DispatcherTimer testTimer;
        private int testCounter;
        private List<bool> unitTypes;
        public delegate void TestEventhandler();
        public event TestEventhandler TestFinished;
        public ArduinoLink ArduinoConnection;

        public static byte[] SendTMStrings(TMStringMessage tmStringMessage)
        {
            int expectedLength = 1;
            foreach (var ut in tmStringMessage.UnitType)
            {           
                if (ut)
                {
                    expectedLength += 16;
                }
                else
                {
                    expectedLength += 8;
                }
            }
            if (expectedLength < 2) return null;
            byte[] messageData = new byte[expectedLength];
            int serialCount = -1;
            messageData[++serialCount] = Convert.ToByte(IntValueCheck(tmStringMessage.Intensity, 0, Constants.MaxIntensityTM));
            for (int i = 0; i < tmStringMessage.Display.Count; i++)
            {
                //messageData[++serialCount] = Convert.ToByte(i+1); //unit number
                int textLength;
                BitArray dotsArray;
                var display = TMDisplayStringConverter(tmStringMessage, i, out textLength, out dotsArray);
                for (var k = 0; k < textLength; k++)
                {
                    if (display.Length - 1 < k)
                    {
                        messageData[++serialCount] = 0;
                    }
                    else
                    {
                        messageData[++serialCount] = display[k];
                    }
                    if (dotsArray[k])
                    {
                        messageData[serialCount] += 128; //doesn't increment serialCount because it alters last byte
                    }
                }

            }


            return messageData;
        }

        private static byte[] TMDisplayStringConverter(
                TMStringMessage tmStringMessage, int i, out int textLength, out BitArray dotsArray)
        {
            // Process string
            byte[] display;
            if (tmStringMessage.UnitType[i])
            {
                //true means 16-segment display
                display = tmStringMessage.Display[i].Length > 0
                                  ? Encoding.ASCII.GetBytes(tmStringMessage.Display[i])
                                  : new byte[] { 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32 };
            }
            else
            {
                display = tmStringMessage.Display[i].Length > 0
                                  ? Encoding.ASCII.GetBytes(tmStringMessage.Display[i])
                                  : new byte[] { 32, 32, 32, 32, 32, 32, 32, 32 };
            }

            for (var j = 0; j < display.Length; j++)
            {
                display[j] = ArduinoLink.FontBytes[display[j] - 32];
            }
            //add dots
            byte[] tempDots;
            if (tmStringMessage.UnitType[i])
            {
                textLength = 16;
                tempDots = new byte[2];
                tempDots[0] = Convert.ToByte(tmStringMessage.Dots[i][0]);
                if (tmStringMessage.Dots[i].Length < 2)
                {
                    tempDots[1] = Convert.ToByte(0);
                }
                else
                {
                    tempDots[1] = Convert.ToByte(tmStringMessage.Dots[i][1]);
                }
            }
            else
            {
                tempDots = new byte[1];
                tempDots[0] = Convert.ToByte(tmStringMessage.Dots[i][0]);
                textLength = 8;
            }
            dotsArray = new BitArray(tempDots);
            return display;
        }

        public static byte[] SendTMLEDS(TMLEDSMessage tmLEDMessage)
        {
            int expectedLength = (tmLEDMessage.Green.Count * 2) + 1;
            if (expectedLength < 3) return null;
            byte[] messageData = new byte[expectedLength];
            int serialCount = -1;
            messageData[++serialCount] = Convert.ToByte(IntValueCheck(tmLEDMessage.Intensity, 0, Constants.MaxIntensityTM));
            for (int i = 0; i < tmLEDMessage.Green.Count;i++)
            {
                messageData[++serialCount] = tmLEDMessage.Green[i];
                messageData[++serialCount] = tmLEDMessage.Red[i];
            }
            return messageData;
        }

        /// <summary>
        ///     Checks and corrects integer values for being either too large or too small
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="min">Minimum possible value</param>
        /// <param name="max">Maximum possible value</param>
        /// <returns></returns>
        private static int IntValueCheck(int value, int min, int max)
        {
            if (value > max)
            {
                value = max;
            }
            else if (value < min)
            {
                value = min;
            }
            return value;
        }

        #region Test Sequence

        public ArduinoMessages()
        {
            this.testTimer = new DispatcherTimer();
            this.testTimer.Tick += this.TestTimerTick;
            this.testTimer.Interval = new TimeSpan(0, 0, 0, 0, 150);
            this.testCounter = 0; 
        }

        public void TMDisplayTest(List<bool> tmUnitTypes, ArduinoLink currentArduinoConnection)
        {
            if (tmUnitTypes == null || currentArduinoConnection == null)
            {
                return;
            }
            this.unitTypes = tmUnitTypes;
            this.ArduinoConnection = currentArduinoConnection;
            this.testCounter = 0;
            this.testTimer.Start();
        }

        private void TestTimerTick(object sender, EventArgs e)
        {
            this.testCounter++;
            int numberUnits = this.unitTypes.Count;
            var displays = new List<string>(numberUnits);
            var greens = new List<byte>(numberUnits);
            var reds = new List<byte>(numberUnits);
            var dotsList = new List<byte[]>(numberUnits);

            byte green;
            byte red;
            byte dots;

            for (int t = 0; t < numberUnits; t++)
            {
                displays.Add("");
                greens.Add(0);
                reds.Add(0);
                dotsList.Add(this.unitTypes[t] ? new Byte[] { 0, 0 } : new Byte[] { 0 });
            }

            string display = this.TestSequence(this.testCounter, out green, out red, out dots);

            this.DuplicateDisplayAcrossAllUnits(display, green, red, dots, displays, greens, reds, dotsList, this.unitTypes);
            var tmLEDs = new TMLEDSMessage
            {
                Green = greens,
                Red = reds,
                Intensity = 3
            };
            var tmDisplay = new TMStringMessage
            {
                Display = displays,
                Dots = dotsList,
                Intensity = 3,
                UnitType = this.unitTypes
            };
            ArduinoConnection.SendSerialMessage(Constants.MessageID_TMLED, SendTMLEDS(tmLEDs));
            ArduinoConnection.SendSerialMessage(Constants.MessageID_TMString, SendTMStrings(tmDisplay));
        }

        private void DuplicateDisplayAcrossAllUnits(string display, byte green, byte red, byte dots, List<string> displays, List<byte> greens, List<byte> reds, List<byte[]> dotsList, List<bool> tm1640UnitsIn)
        {
            for (int i = 0; i < this.unitTypes.Count; i++)
            {
                if (tm1640UnitsIn[i])
                {
                    displays[i] = display + display;
                    greens[i] = green;
                    reds[i] = red;
                    dotsList[i][0] = dots;
                    dotsList[i][1] = dots;
                }
                else
                {
                    displays[i] = display;
                    greens[i] = green;
                    reds[i] = red;
                    dotsList[i][0] = dots;
                }
            }
        }

        private string TestSequence(int count, out byte green, out byte red, out byte dots)
        {
            string display;
            switch (count)
            {
                case 1:
                    display = "*";
                    green = 0;
                    red = 0;
                    dots = 128;
                    break;
                case 2:
                    display = " o";
                    green = 0;
                    red = 0;
                    dots = 64;
                    break;
                case 3:
                    display = "  *";
                    green = 0;
                    red = 0;
                    dots = 32;
                    break;
                case 4:
                    display = "   o";
                    green = 0;
                    red = 0;
                    dots = 16;
                    break;
                case 5:
                    display = "    *";
                    green = 0;
                    red = 0;
                    dots = 8;
                    break;
                case 6:
                    display = "     o";
                    green = 0;
                    red = 0;
                    dots = 4;
                    break;
                case 7:
                    display = "      *";
                    green = 0;
                    red = 0;
                    dots = 2;
                    break;
                case 8:
                    display = "       o";
                    green = 0;
                    red = 0;
                    dots = 1;
                    break;
                case 9:
                    display = "88888888";
                    green = 255;
                    red = 0;
                    dots = 255;
                    break;
                case 10:
                    display = "88888888";
                    green = 0;
                    red = 255;
                    dots = 255;
                    break;
                case 11:
                    display = "        ";
                    green = 0;
                    red = 0;
                    dots = 0;
                    this.testTimer.Stop();
                    TestEventhandler temp = this.TestFinished;
                    if (temp != null)
                    {
                        temp();
                    }
                    break;
                default:
                    display = "        ";
                    green = 0;
                    red = 0;
                    dots = 0;
                    break;
            }
            return display;
        }

        #endregion
    }
}
