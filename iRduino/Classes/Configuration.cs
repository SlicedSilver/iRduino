//========================================//
// iRduino - Created by Mark Silverwood  //
//======================================//

namespace iRduino.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Windows;
    using ArduinoInterfaces;

    public class Configuration
    {
        public List<ControllerConfiguration> ControllerConfigurations = new List<ControllerConfiguration>();
        public List<DisplayConfiguration> DisplayConfigurations = new List<DisplayConfiguration>();
        public string FileLocation = "";
        public int HeaderDisplayTime;
        public int Intensity;
        public int LapDisplayTime;
        public int MaxNumScreens;
        public string Name;
        public int NumDisplayUnits = 1;
        public int NumberControllers;
        public int PreferredComPort;
        public int SerialPortSpeed;
        public bool UseCustomSerialSpeed;
        public bool LogArduinoMessages;
        public int DisplayRefreshRate;
        public int QuickInfoDisplayTime;
        public bool ShiftIntensity;
        public int ShiftIntensityAmount;
        public bool ShiftIntensityType;
        public bool ShowHeaders;
        public bool DeltaLightsOnDefault = false;
        public int DeltaMessageScreen = -1;
        public int DeltaRange = 1;
        public bool ColourDeltaByDD = false;

        public Configuration()
        {
        }

        //fill with defaults
        // ReSharper disable UnusedParameter.Local
        public Configuration(bool t)
        // ReSharper restore UnusedParameter.Local
        {
            if (!t) return;
            HeaderDisplayTime = 1;
            LapDisplayTime = 2;
            QuickInfoDisplayTime = 3;
            ShowHeaders = false;
            Intensity = 3;
            DisplayConfigurations.Add(new DisplayConfiguration(true));
        }

        public static bool LoadConfigurationFromFile(string fileLocation, out Configuration loaded)
        {
            var newConf = new Configuration { FileLocation = fileLocation };
            bool fileReadOk = true;
            int currentDisplayUnit = 0;
            int currentController = 0;
            int currentButton = 0;
            int currentScreen = 0;
            try
            {
                using (var sr = new StreamReader(fileLocation))
                {
                    bool fileEnded = false;
                    while (sr.EndOfStream == false && !fileEnded)
                    {
                        string line = sr.ReadLine();
                        // ReSharper disable StringIndexOfIsCultureSpecific.1
                        if (line == null) continue;
                        int commaLoc = line.IndexOf(",");
                        // ReSharper restore StringIndexOfIsCultureSpecific.1
                        if (commaLoc <= 0) continue;
                        string parameter = line.Substring(0, commaLoc);
                        string value = line.Substring(commaLoc + 1);

                        ButtonFunctionsEnum temp;
                        switch (parameter)
                        {
                            //ButtonOptionsScreens
                            case "ConfName":
                                newConf.Name = value;
                                break;
                            case "ComPort":
                                int.TryParse(value, out newConf.PreferredComPort);
                                break;
                            case "SerialSpeed":
                                int.TryParse(value, out newConf.SerialPortSpeed);
                                break;
                            case "LogArduinoMessages":
                                Boolean.TryParse(value, out newConf.LogArduinoMessages);
                                break;
                            case "UseCustomSerialSpeed":
                                Boolean.TryParse(value, out newConf.UseCustomSerialSpeed);
                                break;
                            case "DisplayRefreshRate":
                                int.TryParse(value, out newConf.DisplayRefreshRate);
                                break;
                            case "ButtonFunction1":
                                if (Enum.TryParse(value, out temp))
                                {
                                    newConf.DisplayConfigurations[currentDisplayUnit].ButtonFunctions[0] = temp;
                                }
                                break;
                            case "ButtonFunction2":
                                if (Enum.TryParse(value, out temp))
                                {
                                    newConf.DisplayConfigurations[currentDisplayUnit].ButtonFunctions[1] = temp;
                                }
                                break;
                            case "ButtonFunction3":
                                if (Enum.TryParse(value, out temp))
                                {
                                    newConf.DisplayConfigurations[currentDisplayUnit].ButtonFunctions[2] = temp;
                                }
                                break;
                            case "ButtonFunction4":
                                if (Enum.TryParse(value, out temp))
                                {
                                    newConf.DisplayConfigurations[currentDisplayUnit].ButtonFunctions[3] = temp;
                                }
                                break;
                            case "ButtonFunction5":
                                if (Enum.TryParse(value, out temp))
                                {
                                    newConf.DisplayConfigurations[currentDisplayUnit].ButtonFunctions[4] = temp;
                                }
                                break;
                            case "ButtonFunction6":
                                if (Enum.TryParse(value, out temp))
                                {
                                    newConf.DisplayConfigurations[currentDisplayUnit].ButtonFunctions[5] = temp;
                                }
                                break;
                            case "ButtonFunction7":
                                if (Enum.TryParse(value, out temp))
                                {
                                    newConf.DisplayConfigurations[currentDisplayUnit].ButtonFunctions[6] = temp;
                                }
                                break;
                            case "ButtonFunction8":
                                if (Enum.TryParse(value, out temp))
                                {
                                    newConf.DisplayConfigurations[currentDisplayUnit].ButtonFunctions[7] = temp;
                                }
                                break;
                            case "ButtonOption1":
                                newConf.DisplayConfigurations[currentDisplayUnit].ButtonOptions[0] = value;
                                break;
                            case "ButtonOption2":
                                newConf.DisplayConfigurations[currentDisplayUnit].ButtonOptions[1] = value;
                                break;
                            case "ButtonOption3":
                                newConf.DisplayConfigurations[currentDisplayUnit].ButtonOptions[2] = value;
                                break;
                            case "ButtonOption4":
                                newConf.DisplayConfigurations[currentDisplayUnit].ButtonOptions[3] = value;
                                break;
                            case "ButtonOption5":
                                newConf.DisplayConfigurations[currentDisplayUnit].ButtonOptions[4] = value;
                                break;
                            case "ButtonOption6":
                                newConf.DisplayConfigurations[currentDisplayUnit].ButtonOptions[5] = value;
                                break;
                            case "ButtonOption7":
                                newConf.DisplayConfigurations[currentDisplayUnit].ButtonOptions[6] = value;
                                break;
                            case "ButtonOption8":
                                newConf.DisplayConfigurations[currentDisplayUnit].ButtonOptions[7] = value;
                                break; /////// 
                            case "ButtonOptionScreens1":
                                int buttonOptionScreenInt1;
                                int.TryParse(value, out buttonOptionScreenInt1);
                                newConf.DisplayConfigurations[currentDisplayUnit].ButtonOptionsScreens[0] =
                                    buttonOptionScreenInt1;
                                break;
                            case "ButtonOptionScreens2":
                                int buttonOptionScreenInt2;
                                int.TryParse(value, out buttonOptionScreenInt2);
                                newConf.DisplayConfigurations[currentDisplayUnit].ButtonOptionsScreens[1] =
                                    buttonOptionScreenInt2;
                                break;
                            case "ButtonOptionScreens3":
                                int buttonOptionScreenInt3;
                                int.TryParse(value, out buttonOptionScreenInt3);
                                newConf.DisplayConfigurations[currentDisplayUnit].ButtonOptionsScreens[2] =
                                    buttonOptionScreenInt3;
                                break;
                            case "ButtonOptionScreens4":
                                int buttonOptionScreenInt4;
                                int.TryParse(value, out buttonOptionScreenInt4);
                                newConf.DisplayConfigurations[currentDisplayUnit].ButtonOptionsScreens[3] =
                                    buttonOptionScreenInt4;
                                break;
                            case "ButtonOptionScreens5":
                                int buttonOptionScreenInt5;
                                int.TryParse(value, out buttonOptionScreenInt5);
                                newConf.DisplayConfigurations[currentDisplayUnit].ButtonOptionsScreens[4] =
                                    buttonOptionScreenInt5;
                                break;
                            case "ButtonOptionScreens6":
                                int buttonOptionScreenInt6;
                                int.TryParse(value, out buttonOptionScreenInt6);
                                newConf.DisplayConfigurations[currentDisplayUnit].ButtonOptionsScreens[5] =
                                    buttonOptionScreenInt6;
                                break;
                            case "ButtonOptionScreens7":
                                int buttonOptionScreenInt7;
                                int.TryParse(value, out buttonOptionScreenInt7);
                                newConf.DisplayConfigurations[currentDisplayUnit].ButtonOptionsScreens[6] =
                                    buttonOptionScreenInt7;
                                break;
                            case "ButtonOptionScreens8":
                                int buttonOptionScreenInt8;
                                int.TryParse(value, out buttonOptionScreenInt8);
                                newConf.DisplayConfigurations[currentDisplayUnit].ButtonOptionsScreens[7] =
                                    buttonOptionScreenInt8;
                                break;
                            case "FFBClipLights":
                                Boolean.TryParse(value,
                                                 out newConf.DisplayConfigurations[currentDisplayUnit].FFBClippingLights);
                                break;
                            case "FFBClipScreen":
                                Int32.TryParse(value,
                                               out newConf.DisplayConfigurations[currentDisplayUnit].FFBClippingScreen);
                                break;
                            case "HeaderDisplayTime":
                                Int32.TryParse(value, out newConf.HeaderDisplayTime);
                                break;
                            case "LapDisplayTime":
                                Int32.TryParse(value, out newConf.LapDisplayTime);
                                break;
                            case "LapStyle":
                                Enum.TryParse(value, out newConf.DisplayConfigurations[currentDisplayUnit].LapStyle);
                                break;
                            case "PitLights":
                                Boolean.TryParse(value, out newConf.DisplayConfigurations[currentDisplayUnit].PitLights);
                                break;
                            case "ShiftClumps":
                                Boolean.TryParse(value,
                                                 out newConf.DisplayConfigurations[currentDisplayUnit].ShiftClumps);
                                break;
                            case "PitLimiterSpeed":
                                Enum.TryParse(value,
                                              out newConf.DisplayConfigurations[currentDisplayUnit].PitLimiterSpeed);
                                break;
                            case "PitLimiterStyle":
                                Enum.TryParse(value,
                                              out newConf.DisplayConfigurations[currentDisplayUnit].PitLimiterStyle);
                                break;
                            case "QuickInfoDisplayTime":
                                Int32.TryParse(value, out newConf.QuickInfoDisplayTime);
                                break;
                            case "RevLimiterLights":
                                Boolean.TryParse(value,
                                                 out newConf.DisplayConfigurations[currentDisplayUnit].RevLimiterLights);
                                break;
                            case "RevLimiterStyle":
                                Enum.TryParse(value,
                                              out newConf.DisplayConfigurations[currentDisplayUnit].RevLimiterStyle);
                                break;
                            case "ShiftLightStyle":
                                Enum.TryParse(value,
                                              out newConf.DisplayConfigurations[currentDisplayUnit].ShiftLightStyle);
                                break;
                            case "MatchCarShiftStyle":
                                Boolean.TryParse(
                                    value, out newConf.DisplayConfigurations[currentDisplayUnit].MatchCarShiftStyle);
                                break;
                            case "MatchCarShiftConfiguration":
                                Enum.TryParse(
                                    value,
                                    out newConf.DisplayConfigurations[currentDisplayUnit].MatchCarShiftStyleOption);
                                break;
                            case "MatchRedShift":
                                Boolean.TryParse(value, out newConf.DisplayConfigurations[currentDisplayUnit].MatchRedShift);
                                break;
                            case "ShowHeaders":
                                Boolean.TryParse(value, out newConf.ShowHeaders);
                                break;

                            case "ColourDeltaByDD":
                                Boolean.TryParse(value, out newConf.ColourDeltaByDD);
                                break;
                            case "DeltaLightsOnDefault":
                                Boolean.TryParse(value, out newConf.DeltaLightsOnDefault);
                                break;
                            case "DeltamessageScreen":
                                Int32.TryParse(value, out newConf.DeltaMessageScreen);
                                break;
                            case "DeltaRange":
                                Int32.TryParse(value, out newConf.DeltaRange);
                                break;

                            case "ShowLap":
                                Boolean.TryParse(value, out newConf.DisplayConfigurations[currentDisplayUnit].ShowLap);
                                break;
                            case "Inverted":
                                Boolean.TryParse(value, out newConf.DisplayConfigurations[currentDisplayUnit].Inverted);
                                break;
                            case "TM1640":
                                Boolean.TryParse(value, out newConf.DisplayConfigurations[currentDisplayUnit].IsTM1640);
                                break;
                            case "DeltaLightsPosition":
                                Enum.TryParse(
                                    value,
                                    out newConf.DisplayConfigurations[currentDisplayUnit].DeltaLightsPosition);
                                break;
                            case "DeltaLightsShow":
                                Boolean.TryParse(value, out newConf.DisplayConfigurations[currentDisplayUnit].DeltaLightsShow);
                                break;

                            case "SwitchLEDs":
                                Boolean.TryParse(value, out newConf.DisplayConfigurations[currentDisplayUnit].SwitchLEDs);
                                break;
                            case "ShowDC":
                                Boolean.TryParse(value, out newConf.DisplayConfigurations[currentDisplayUnit].ShowDC);
                                break;
                            case "DCDisplayTime":
                                Int32.TryParse(value,
                                               out newConf.DisplayConfigurations[currentDisplayUnit].DCDisplayTime);
                                break;
                            case "ShowShiftLights":
                                Boolean.TryParse(value,
                                                 out newConf.DisplayConfigurations[currentDisplayUnit].ShowShiftLights);
                                break;
                            case "NumScreens":
                                Int32.TryParse(value, out newConf.DisplayConfigurations[currentDisplayUnit].NumScreens);
                                break;
                            case "Intensity":
                                Int32.TryParse(value, out newConf.Intensity);
                                break;
                            case "ShiftIntensity":
                                Boolean.TryParse(value, out newConf.ShiftIntensity);
                                break;
                            case "ShiftIntensityType":
                                Boolean.TryParse(value, out newConf.ShiftIntensityType);
                                break;
                            case "ShiftIntensityAmount":
                                Int32.TryParse(value, out newConf.ShiftIntensityAmount);
                                break;
                            case "ScreenNumber":
                                Int32.TryParse(value, out currentScreen);
                                if (newConf.DisplayConfigurations[currentDisplayUnit].Screens.Count <= currentScreen)
                                {
                                    var tempScreen = new Screen();
                                    tempScreen.Variables.Clear();
                                    newConf.DisplayConfigurations[currentDisplayUnit].Screens.Add(tempScreen);
                                }
                                break;
                            case "DisplayUnit":
                                Int32.TryParse(value, out currentDisplayUnit);
                                if (newConf.DisplayConfigurations.Count <= currentDisplayUnit)
                                {
                                    var unitTemp = new DisplayConfiguration(true);
                                    unitTemp.Screens.Clear();
                                    newConf.DisplayConfigurations.Add(unitTemp);
                                }
                                currentScreen = 0;
                                break;
                            case "UseCustomHeader":
                                bool tempUch;
                                Boolean.TryParse(value, out tempUch);
                                newConf.DisplayConfigurations[currentDisplayUnit].Screens[currentScreen].UseCustomHeader = tempUch;
                                break;
                            case "CustomHeader":
                                newConf.DisplayConfigurations[currentDisplayUnit].Screens[currentScreen].CustomHeader = value;
                                break;
                            case "Variable":
                                newConf.DisplayConfigurations[currentDisplayUnit].Screens[currentScreen].Variables.Add(
                                    value);
                                break;
                            case "ControllerNumber":
                                currentButton = -1;
                                Int32.TryParse(value, out currentController);
                                if (newConf.ControllerConfigurations.Count <= currentController)
                                {
                                    var controllerTemp = new ControllerConfiguration();
                                    newConf.ControllerConfigurations.Add(controllerTemp);
                                }
                                break;
                            case "DeviceGuid":
                                Guid.TryParse(value, out newConf.ControllerConfigurations[currentController].DeviceGuid);
                                break;
                            case "ConButtonFunction":
                                currentButton++;
                                if (Enum.TryParse(value, out temp))
                                {
                                    newConf.ControllerConfigurations[currentController].ButtonFunctions[currentButton] =
                                        temp;
                                }
                                break;
                            case "ConButtonOption":
                                newConf.ControllerConfigurations[currentController].ButtonOptions[currentButton] = value;
                                break;
                            case "ConButtonOptionScreens":
                                int conButtonOptionScreenInt1;
                                int.TryParse(value, out conButtonOptionScreenInt1);
                                newConf.ControllerConfigurations[currentController].ButtonOptionsScreens[currentButton]
                                    =
                                    conButtonOptionScreenInt1;
                                break;
                            case "ConButtonNumber":
                                int conButtonNumberInt1;
                                int.TryParse(value, out conButtonNumberInt1);
                                newConf.ControllerConfigurations[currentController].ButtonNumbers[currentButton] =
                                    conButtonNumberInt1;
                                break;
                            case "ContSelected":
                                Boolean.TryParse(value, out newConf.ControllerConfigurations[currentController].Selected);
                                break;
                                
                            case "END":
                                fileEnded = true;
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(String.Format("The file could not be read: {0}", e.Message), "File Read Error");
                fileReadOk = false;
            }
            loaded = fileReadOk ? newConf : null;
            return fileReadOk;
        }

        // Add Error checking for BOOL Return!!!
        public static bool SaveConfigurationToFile(string fileLocation, Configuration conf)
        {
            var sb = new StringBuilder("");
            sb.AppendLine("iRduino Configuration File");
            sb.AppendLine(String.Format("File saved on: {0}  {1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString()));
            sb.AppendLine("[ Configuration Options ]");
            sb.AppendLine(String.Format("{0},{1}", "ConfName", conf.Name));
            sb.AppendLine(String.Format("{0},{1}", "ComPort", conf.PreferredComPort));
            sb.AppendLine(String.Format("{0},{1}", "SerialSpeed", conf.SerialPortSpeed));
            sb.AppendLine(String.Format("{0},{1}", "UseCustomSerialSpeed", conf.UseCustomSerialSpeed));
            sb.AppendLine(String.Format("{0},{1}", "LogArduinoMessages", conf.LogArduinoMessages));
            sb.AppendLine(String.Format("{0},{1}", "DisplayRefreshRate", conf.DisplayRefreshRate));
            sb.AppendLine("[ Common Display Options ]");
            sb.AppendLine(String.Format("{0},{1}", "HeaderDisplayTime",
                                        conf.HeaderDisplayTime.ToString(
                                            CultureInfo.InvariantCulture)));
            sb.AppendLine(String.Format("{0},{1}", "LapDisplayTime",
                                        conf.LapDisplayTime.ToString(
                                            CultureInfo.InvariantCulture)));
            sb.AppendLine(String.Format("{0},{1}", "QuickInfoDisplayTime",
                                        conf.QuickInfoDisplayTime.ToString(
                                            CultureInfo.InvariantCulture)));
            sb.AppendLine(String.Format("{0},{1}", "ShowHeaders",
                                        conf.ShowHeaders.ToString()));
            sb.AppendLine(String.Format("{0},{1}", "Intensity",
                                        conf.Intensity.ToString(
                                            CultureInfo.InvariantCulture)));
            sb.AppendLine(String.Format("{0},{1}", "ShiftIntensity",
                                        conf.ShiftIntensity.ToString(
                                            CultureInfo.InvariantCulture)));
            sb.AppendLine(String.Format("{0},{1}", "ShiftIntensityType",
                                        conf.ShiftIntensityType.ToString(
                                            CultureInfo.InvariantCulture)));
            sb.AppendLine(String.Format("{0},{1}", "ShiftIntensityAmount",
                                        conf.ShiftIntensityAmount.ToString(
                                            CultureInfo.InvariantCulture)));
            sb.AppendLine(String.Format("{0},{1}", "ColourDeltaByDD",
                                        conf.ColourDeltaByDD.ToString(
                                            CultureInfo.InvariantCulture)));
            sb.AppendLine(String.Format("{0},{1}", "DeltaLightsOnDefault",
                                        conf.DeltaLightsOnDefault.ToString(
                                            CultureInfo.InvariantCulture)));
            sb.AppendLine(String.Format("{0},{1}", "DeltamessageScreen",
                                        conf.DeltaMessageScreen.ToString(
                                            CultureInfo.InvariantCulture)));
            sb.AppendLine(String.Format("{0},{1}", "DeltaRange",
                                        conf.DeltaRange.ToString(
                                            CultureInfo.InvariantCulture)));
            sb.AppendLine("[ TM Display Units ]");
            //Start looping display units
            for (int unit = 0; unit < conf.DisplayConfigurations.Count; unit++)
            {
                //displayUnitString
                sb.AppendLine(String.Format("{0},{1}", "DisplayUnit", unit));
                sb.AppendLine("     [ Display Unit Button Functions ]");
                sb.AppendLine(String.Format("{0},{1}", "ButtonFunction1",
                                            conf.DisplayConfigurations[unit].ButtonFunctions[0].ToString()));
                sb.AppendLine(String.Format("{0},{1}", "ButtonFunction2",
                                            conf.DisplayConfigurations[unit].ButtonFunctions[1].ToString()));
                sb.AppendLine(String.Format("{0},{1}", "ButtonFunction3",
                                            conf.DisplayConfigurations[unit].ButtonFunctions[2].ToString()));
                sb.AppendLine(String.Format("{0},{1}", "ButtonFunction4",
                                            conf.DisplayConfigurations[unit].ButtonFunctions[3].ToString()));
                sb.AppendLine(String.Format("{0},{1}", "ButtonFunction5",
                                            conf.DisplayConfigurations[unit].ButtonFunctions[4].ToString()));
                sb.AppendLine(String.Format("{0},{1}", "ButtonFunction6",
                                            conf.DisplayConfigurations[unit].ButtonFunctions[5].ToString()));
                sb.AppendLine(String.Format("{0},{1}", "ButtonFunction7",
                                            conf.DisplayConfigurations[unit].ButtonFunctions[6].ToString()));
                sb.AppendLine(String.Format("{0},{1}", "ButtonFunction8",
                                            conf.DisplayConfigurations[unit].ButtonFunctions[7].ToString()));
                sb.AppendLine("     [ Display Unit Button Options ]");
                sb.AppendLine(String.Format("{0},{1}", "ButtonOption1",
                                            conf.DisplayConfigurations[unit].ButtonOptions[0]));
                sb.AppendLine(String.Format("{0},{1}", "ButtonOption2",
                                            conf.DisplayConfigurations[unit].ButtonOptions[1]));
                sb.AppendLine(String.Format("{0},{1}", "ButtonOption3",
                                            conf.DisplayConfigurations[unit].ButtonOptions[2]));
                sb.AppendLine(String.Format("{0},{1}", "ButtonOption4",
                                            conf.DisplayConfigurations[unit].ButtonOptions[3]));
                sb.AppendLine(String.Format("{0},{1}", "ButtonOption5",
                                            conf.DisplayConfigurations[unit].ButtonOptions[4]));
                sb.AppendLine(String.Format("{0},{1}", "ButtonOption6",
                                            conf.DisplayConfigurations[unit].ButtonOptions[5]));
                sb.AppendLine(String.Format("{0},{1}", "ButtonOption7",
                                            conf.DisplayConfigurations[unit].ButtonOptions[6]));
                sb.AppendLine(String.Format("{0},{1}", "ButtonOption8",
                                            conf.DisplayConfigurations[unit].ButtonOptions[7]));
                sb.AppendLine("     [ Display Unit Button Screens ]");
                sb.AppendLine(String.Format("{0},{1}", "ButtonOptionScreens1",
                                            conf.DisplayConfigurations[unit].ButtonOptionsScreens[0]));
                sb.AppendLine(String.Format("{0},{1}", "ButtonOptionScreens2",
                                            conf.DisplayConfigurations[unit].ButtonOptionsScreens[1]));
                sb.AppendLine(String.Format("{0},{1}", "ButtonOptionScreens3",
                                            conf.DisplayConfigurations[unit].ButtonOptionsScreens[2]));
                sb.AppendLine(String.Format("{0},{1}", "ButtonOptionScreens4",
                                            conf.DisplayConfigurations[unit].ButtonOptionsScreens[3]));
                sb.AppendLine(String.Format("{0},{1}", "ButtonOptionScreens5",
                                            conf.DisplayConfigurations[unit].ButtonOptionsScreens[4]));
                sb.AppendLine(String.Format("{0},{1}", "ButtonOptionScreens6",
                                            conf.DisplayConfigurations[unit].ButtonOptionsScreens[5]));
                sb.AppendLine(String.Format("{0},{1}", "ButtonOptionScreens7",
                                            conf.DisplayConfigurations[unit].ButtonOptionsScreens[6]));
                sb.AppendLine(String.Format("{0},{1}", "ButtonOptionScreens8",
                                            conf.DisplayConfigurations[unit].ButtonOptionsScreens[7]));
                sb.AppendLine("     [ Display Unit Configuration ]");
                sb.AppendLine(String.Format("{0},{1}", "FFBClipLights",
                                            conf.DisplayConfigurations[unit].FFBClippingLights.ToString()));
                sb.AppendLine(String.Format("{0},{1}", "FFBClipScreen",
                                            conf.DisplayConfigurations[unit].FFBClippingScreen.ToString(
                                                CultureInfo.InvariantCulture)));
                sb.AppendLine(String.Format("{0},{1}", "Inverted",
                                            conf.DisplayConfigurations[unit].Inverted.ToString()));
                sb.AppendLine(String.Format("{0},{1}", "TM1640",
                                            conf.DisplayConfigurations[unit].IsTM1640.ToString()));
                sb.AppendLine(String.Format("{0},{1}", "SwitchLEDs",
                                            conf.DisplayConfigurations[unit].SwitchLEDs.ToString()));
                sb.AppendLine(String.Format("{0},{1}", "ShowDC",
                                            conf.DisplayConfigurations[unit].ShowDC.ToString()));
                sb.AppendLine(String.Format("{0},{1}", "DCDisplayTime",
                                            conf.DisplayConfigurations[unit].DCDisplayTime.ToString(
                                                CultureInfo.InvariantCulture)));
                sb.AppendLine(String.Format("{0},{1}", "LapStyle",
                                            conf.DisplayConfigurations[unit].LapStyle.ToString()));
                sb.AppendLine(String.Format("{0},{1}", "PitLights",
                                            conf.DisplayConfigurations[unit].PitLights.ToString()));
                sb.AppendLine(String.Format("{0},{1}", "PitLimiterSpeed",
                                            conf.DisplayConfigurations[unit].PitLimiterSpeed.ToString()));
                sb.AppendLine(String.Format("{0},{1}", "PitLimiterStyle",
                                            conf.DisplayConfigurations[unit].PitLimiterStyle.ToString()));
                sb.AppendLine(String.Format("{0},{1}", "RevLimiterLights",
                                            conf.DisplayConfigurations[unit].RevLimiterLights.ToString()));
                sb.AppendLine(String.Format("{0},{1}", "RevLimiterStyle",
                                            conf.DisplayConfigurations[unit].RevLimiterStyle.ToString()));
                sb.AppendLine(String.Format("{0},{1}", "ShiftLightStyle",
                                            conf.DisplayConfigurations[unit].ShiftLightStyle.ToString()));
                sb.AppendLine(String.Format("{0},{1}", "MatchCarShiftStyle", conf.DisplayConfigurations[unit].MatchCarShiftStyle.ToString()));
                sb.AppendLine(String.Format("{0},{1}", "MatchCarShiftConfiguration", conf.DisplayConfigurations[unit].MatchCarShiftStyleOption.ToString()));
                sb.AppendLine(String.Format("{0},{1}", "MatchRedShift", conf.DisplayConfigurations[unit].MatchRedShift.ToString()));
                sb.AppendLine(String.Format("{0},{1}", "ShowLap",
                                            conf.DisplayConfigurations[unit].ShowLap.ToString()));
                sb.AppendLine(String.Format("{0},{1}", "ShowShiftLights",
                                            conf.DisplayConfigurations[unit].ShowShiftLights.ToString()));
                sb.AppendLine(String.Format("{0},{1}", "ShiftClumps",
                                            conf.DisplayConfigurations[unit].ShiftClumps.ToString()));
                sb.AppendLine(String.Format("{0},{1}", "DeltaLightsShow",
                                            conf.DisplayConfigurations[unit].DeltaLightsShow.ToString()));
                sb.AppendLine(String.Format("{0},{1}", "DeltaLightsPosition",
                                            conf.DisplayConfigurations[unit].DeltaLightsPosition.ToString()));
                sb.AppendLine("     [ Display Unit Screens ]");
                sb.AppendLine(String.Format("{0},{1}", "NumScreens",
                                            conf.DisplayConfigurations[unit].NumScreens.ToString(
                                                CultureInfo.InvariantCulture)));
                for (int i = 0; i < conf.DisplayConfigurations[unit].NumScreens; i++)
                {
                    sb.AppendLine(String.Format("{0},{1}", "ScreenNumber", i.ToString(CultureInfo.InvariantCulture)));
                    sb.AppendLine(String.Format("{0},{1}", "UseCustomHeader", conf.DisplayConfigurations[unit].Screens[i].UseCustomHeader));
                    sb.AppendLine(String.Format("{0},{1}", "CustomHeader", conf.DisplayConfigurations[unit].Screens[i].CustomHeader));
                    foreach (string dv in conf.DisplayConfigurations[unit].Screens[i].Variables)
                    {
                        sb.AppendLine(String.Format("{0},{1}", "Variable", dv));
                    }
                }
            }
            sb.AppendLine("[ Controller Configuration ]");
            for (int c = 0; c < conf.ControllerConfigurations.Count; c++)
            {
                sb.AppendLine(String.Format("{0},{1}", "ControllerNumber", c.ToString(CultureInfo.InvariantCulture)));
                sb.AppendLine(String.Format("{0},{1}", "DeviceGuid", conf.ControllerConfigurations[c].DeviceGuid));
                sb.AppendLine(String.Format("{0},{1}", "ContSelected", conf.ControllerConfigurations[c].Selected));
                for (int t = 1; t <= Constants.MaxNumberJoystickButtons; t++)
                {
                    sb.AppendLine(String.Format("{0},{1}", "ConButtonFunction",
                                                conf.ControllerConfigurations[c].ButtonFunctions[t - 1].ToString()));
                    sb.AppendLine(String.Format("{0},{1}", "ConButtonOption",
                                                conf.ControllerConfigurations[c].ButtonOptions[t - 1]));
                    sb.AppendLine(String.Format("{0},{1}", "ConButtonOptionScreens",
                                                conf.ControllerConfigurations[c].ButtonOptionsScreens[t - 1]));
                    sb.AppendLine(String.Format("{0},{1}", "ConButtonNumber",
                                                conf.ControllerConfigurations[c].ButtonNumbers[t - 1]));
                }
            }
            sb.AppendLine(String.Format("{0},{1}", "END", "END"));
            sb.AppendLine("[ END ]");
            using (var outfile = new StreamWriter(fileLocation))
            {
                outfile.Write(sb.ToString());
            }
            return true;
        }
    }

    public class DisplayConfiguration
    {
        public List<ButtonFunctionsEnum> ButtonFunctions = new List<ButtonFunctionsEnum>(Constants.NumberButtonsOnTm1638);
        public List<string> ButtonOptions = new List<string>(Constants.NumberButtonsOnTm1638);
        public List<int> ButtonOptionsScreens = new List<int>(Constants.NumberButtonsOnTm1638);
        public int DCDisplayTime = 0;
        public bool FFBClippingLights;
        public int FFBClippingScreen;
        public bool Inverted = false;
        public LapDisplayStylesEnum LapStyle;
        public int NumScreens = 1;
        public bool PitLights;
        public PitFlashSpeedsEnum PitLimiterSpeed;
        public PitFlashStyleEnum PitLimiterStyle;
        public bool RevLimiterLights;
        public RevFlashStyleEnum RevLimiterStyle;
        public List<Screen> Screens = new List<Screen>();
        public ShiftStyleEnum ShiftLightStyle;
        public bool ShiftClumps = false;
        public bool MatchCarShiftStyle = false;
        public bool MatchRedShift = false;
        public MatchCarShiftOptions MatchCarShiftStyleOption;
        public bool ShowDC = false;
        public bool ShowLap;
        public bool ShowShiftLights;
        public bool SwitchLEDs = false;
        public bool IsTM1640 = false;
        public DeltaLightsOptions DeltaLightsPosition;
        public bool DeltaLightsShow = false;


        public DisplayConfiguration()
        {
        }

        //fill with defaults
        public DisplayConfiguration(bool t)
        {
            ButtonFunctions = new List<ButtonFunctionsEnum>
                {
                    ButtonFunctionsEnum.None,
                    ButtonFunctionsEnum.None,
                    ButtonFunctionsEnum.None,
                    ButtonFunctionsEnum.None,
                    ButtonFunctionsEnum.None,
                    ButtonFunctionsEnum.None,
                    ButtonFunctionsEnum.None,
                    ButtonFunctionsEnum.None
                };
            ButtonOptions = new List<string> {"", "", "", "", "", "", "", ""};
            ButtonOptionsScreens = new List<int> {-1, -1, -1, -1, -1, -1, -1, -1};
            //LapDisplayTime = 2;
            LapStyle = LapDisplayStylesEnum.LapTimeDeltaPersonal;
            NumScreens = 1;
            PitLights = false;
            PitLimiterSpeed = PitFlashSpeedsEnum.Full;
            PitLimiterStyle = PitFlashStyleEnum.GreenRedAlternateFlash;
            RevLimiterLights = false;
            RevLimiterStyle = RevFlashStyleEnum.StayRed;
            ShiftLightStyle = ShiftStyleEnum.GreenRedProgressiveRedShift;
            ShowLap = false;
            ShowShiftLights = false;
            FFBClippingLights = false;
            FFBClippingScreen = -1;
            if (t)
            {
                var temp = new Screen
                    {
                        Example = "148 3 43",
                        Variables = new List<string> {"Speed", "Space", "Gear", "Space", "Laps2"}
                    };
                Screens = new List<Screen> {temp};
            }
        }
    }

    public class ControllerConfiguration
    {
        public List<ButtonFunctionsEnum> ButtonFunctions = new List<ButtonFunctionsEnum>(Constants.MaxNumberJoystickButtons);
        public List<int> ButtonNumbers = new List<int>(Constants.MaxNumberJoystickButtons);
        public List<string> ButtonOptions = new List<string>(Constants.MaxNumberJoystickButtons);
        public List<int> ButtonOptionsScreens = new List<int>(Constants.MaxNumberJoystickButtons);
        public Guid DeviceGuid;
        public bool Selected;

        public ControllerConfiguration()
        {
            const int NumButtons = Constants.MaxNumberJoystickButtons;
            ButtonNumbers = new List<int>();
            ButtonFunctions = new List<ButtonFunctionsEnum>();
            ButtonOptions = new List<string>();
            ButtonOptionsScreens = new List<int>();
            for (int i = 0; i < NumButtons; i++)
            {
                ButtonNumbers.Add(-1);
                ButtonFunctions.Add(ButtonFunctionsEnum.None);
                ButtonOptions.Add("");
                ButtonOptionsScreens.Add(-1);
            }
        }
    }

}