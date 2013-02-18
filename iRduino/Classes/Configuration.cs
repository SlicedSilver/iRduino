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

    public class RefreshRates
    {
        public int DisplayRefreshRate = 15;
        public int LEDRefreshRate = 30;
    }

    public class TMDisplaySettings
    {
        public int LapDisplayTime = 1;
        public int HeaderDisplayTime = 1;
        public int QuickInfoDisplayTime = 1;
        public bool QuickInfoLightsColour = false; //red=true
        public int WarningTextDisplayTime = 1;
        public bool ShiftIntensity = false;
        public int ShiftIntensityAmount = 1;
        public bool ShiftIntensityType;
        public bool ShowHeaders = false;
        public int NumDisplayUnits = 1;
        public int Intensity = 3;
        public int MaxNumScreens;
        public bool DeltaLightsOnDefault = false;
        public int DeltaMessageScreen = -1;
        public int DeltaRange = 1;
        public bool ColourDeltaByDD = false;
    }

    public class SerialPortConfiguration
    {
        public int PreferredComPort;
        public int SerialPortSpeed;
        public bool UseCustomSerialSpeed;
    }

    public class AdvancedSettings
    {
        public bool LogArduinoMessages;
    }

    public class OtherSettings
    {
        public bool UseCustomFuelCalculationOptions = false;
        public int FuelCalculationLaps = 3;
        public bool UseWeightedFuelCalculations = false;
    }

    public class Configuration
    {
        public List<ControllerConfiguration> ControllerConfigurations = new List<ControllerConfiguration>();
        public List<DisplayConfiguration> DisplayConfigurations = new List<DisplayConfiguration>();
        public RefreshRates RefreshRates = new RefreshRates();
        public TMDisplaySettings TMDisplaySettings = new TMDisplaySettings();
        public SerialPortConfiguration SerialPortSettings = new SerialPortConfiguration();
        public AdvancedSettings AdvancedSettings = new AdvancedSettings();
        public OtherSettings OtherSettings = new OtherSettings();
        
        public string FileLocation = "";            
        public string Name;       
        public int NumberControllers;

        public Configuration()
        {
        }

        //fill with defaults
        // ReSharper disable UnusedParameter.Local
        public Configuration(bool t)
        // ReSharper restore UnusedParameter.Local
        {
            if (!t) return;
            TMDisplaySettings.HeaderDisplayTime = 1;
            TMDisplaySettings.LapDisplayTime = 2;
            TMDisplaySettings.QuickInfoDisplayTime = 3;
            TMDisplaySettings.WarningTextDisplayTime = 5;
            TMDisplaySettings.ShowHeaders = false;
            TMDisplaySettings.Intensity = 3;
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
                                int.TryParse(value, out newConf.SerialPortSettings.PreferredComPort);
                                break;
                            case "SerialSpeed":
                                int.TryParse(value, out newConf.SerialPortSettings.SerialPortSpeed);
                                break;
                            case "LogArduinoMessages":
                                Boolean.TryParse(value, out newConf.AdvancedSettings.LogArduinoMessages);
                                break;
                            case "UseCustomSerialSpeed":
                                Boolean.TryParse(value, out newConf.SerialPortSettings.UseCustomSerialSpeed);
                                break;
                            case "DisplayRefreshRate":
                                int.TryParse(value, out newConf.RefreshRates.DisplayRefreshRate);
                                break;
                            case "LEDRefreshRate":
                                int.TryParse(value, out newConf.RefreshRates.LEDRefreshRate);
                                break;
                            case "UseCustomFuelCalculations":
                                Boolean.TryParse(value, out newConf.OtherSettings.UseCustomFuelCalculationOptions);
                                break;
                            case "UseWeightedFuelCalculation":
                                Boolean.TryParse(value, out newConf.OtherSettings.UseWeightedFuelCalculations);
                                break;
                            case "CustomFuelLaps":
                                int.TryParse(value, out newConf.OtherSettings.FuelCalculationLaps);
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
                                Int32.TryParse(value, out newConf.TMDisplaySettings.HeaderDisplayTime);
                                break;
                            case "LapDisplayTime":
                                Int32.TryParse(value, out newConf.TMDisplaySettings.LapDisplayTime);
                                break;
                            case "ShowEngineWarnings":
                                Boolean.TryParse(value, out newConf.DisplayConfigurations[currentDisplayUnit].ShowEngineWarnings);
                                break;
                            case "WarningType":
                                Enum.TryParse(value, out newConf.DisplayConfigurations[currentDisplayUnit].WarningType);
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
                                Int32.TryParse(value, out newConf.TMDisplaySettings.QuickInfoDisplayTime);
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
                                Boolean.TryParse(value, out newConf.TMDisplaySettings.ShowHeaders);
                                break;

                            case "ColourDeltaByDD":
                                Boolean.TryParse(value, out newConf.TMDisplaySettings.ColourDeltaByDD);
                                break;
                            case "DeltaLightsOnDefault":
                                Boolean.TryParse(value, out newConf.TMDisplaySettings.DeltaLightsOnDefault);
                                break;
                            case "DeltamessageScreen":
                                Int32.TryParse(value, out newConf.TMDisplaySettings.DeltaMessageScreen);
                                break;
                            case "DeltaRange":
                                Int32.TryParse(value, out newConf.TMDisplaySettings.DeltaRange);
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
                                Int32.TryParse(value, out newConf.TMDisplaySettings.Intensity);
                                break;
                            case "ShiftIntensity":
                                Boolean.TryParse(value, out newConf.TMDisplaySettings.ShiftIntensity);
                                break;
                            case "ShiftIntensityType":
                                Boolean.TryParse(value, out newConf.TMDisplaySettings.ShiftIntensityType);
                                break;
                            case "WarningTextDisplayTime":
                                Int32.TryParse(value, out newConf.TMDisplaySettings.WarningTextDisplayTime);
                                break;
                            case "QuickInfoLightsColour":
                                Boolean.TryParse(value, out newConf.TMDisplaySettings.QuickInfoLightsColour);
                                break;
                            case "ShiftIntensityAmount":
                                Int32.TryParse(value, out newConf.TMDisplaySettings.ShiftIntensityAmount);
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
            sb.AppendLine(String.Format("{0},{1}", "ComPort", conf.SerialPortSettings.PreferredComPort));
            sb.AppendLine(String.Format("{0},{1}", "SerialSpeed", conf.SerialPortSettings.SerialPortSpeed));
            sb.AppendLine(String.Format("{0},{1}", "UseCustomSerialSpeed", conf.SerialPortSettings.UseCustomSerialSpeed));
            sb.AppendLine(String.Format("{0},{1}", "LogArduinoMessages", conf.AdvancedSettings.LogArduinoMessages));
            sb.AppendLine(String.Format("{0},{1}", "DisplayRefreshRate", conf.RefreshRates.DisplayRefreshRate));
            sb.AppendLine(String.Format("{0},{1}", "LEDRefreshRate", conf.RefreshRates.LEDRefreshRate));
            sb.AppendLine(String.Format("{0},{1}", "UseCustomFuelCalculations", conf.OtherSettings.UseCustomFuelCalculationOptions));
            sb.AppendLine(String.Format("{0},{1}", "UseWeightedFuelCalculation", conf.OtherSettings.UseWeightedFuelCalculations));
            sb.AppendLine(String.Format("{0},{1}", "CustomFuelLaps", conf.OtherSettings.FuelCalculationLaps));
            sb.AppendLine("[ Common Display Options ]");
            sb.AppendLine(String.Format("{0},{1}", "HeaderDisplayTime",
                                        conf.TMDisplaySettings.HeaderDisplayTime.ToString(
                                            CultureInfo.InvariantCulture)));
            sb.AppendLine(String.Format("{0},{1}", "LapDisplayTime",
                                        conf.TMDisplaySettings.LapDisplayTime.ToString(
                                            CultureInfo.InvariantCulture)));
            sb.AppendLine(String.Format("{0},{1}", "QuickInfoDisplayTime",
                                        conf.TMDisplaySettings.QuickInfoDisplayTime.ToString(
                                            CultureInfo.InvariantCulture)));
            sb.AppendLine(String.Format("{0},{1}", "ShowHeaders",
                                        conf.TMDisplaySettings.ShowHeaders));
            sb.AppendLine(String.Format("{0},{1}", "Intensity",
                                        conf.TMDisplaySettings.Intensity.ToString(
                                            CultureInfo.InvariantCulture)));
            sb.AppendLine(String.Format("{0},{1}", "ShiftIntensity",
                                        conf.TMDisplaySettings.ShiftIntensity.ToString(
                                            CultureInfo.InvariantCulture)));
            sb.AppendLine(String.Format("{0},{1}", "ShiftIntensityType",
                                        conf.TMDisplaySettings.ShiftIntensityType.ToString(
                                            CultureInfo.InvariantCulture)));
            sb.AppendLine(String.Format("{0},{1}", "ShiftIntensityAmount",
                                        conf.TMDisplaySettings.ShiftIntensityAmount.ToString(
                                            CultureInfo.InvariantCulture)));
            sb.AppendLine(String.Format("{0},{1}", "ColourDeltaByDD",
                                        conf.TMDisplaySettings.ColourDeltaByDD.ToString(
                                            CultureInfo.InvariantCulture)));
            sb.AppendLine(String.Format("{0},{1}", "DeltaLightsOnDefault",
                                        conf.TMDisplaySettings.DeltaLightsOnDefault.ToString(
                                            CultureInfo.InvariantCulture)));
            sb.AppendLine(String.Format("{0},{1}", "DeltamessageScreen",
                                        conf.TMDisplaySettings.DeltaMessageScreen.ToString(
                                            CultureInfo.InvariantCulture)));
            sb.AppendLine(String.Format("{0},{1}", "DeltaRange",
                                        conf.TMDisplaySettings.DeltaRange.ToString(
                                            CultureInfo.InvariantCulture)));
            sb.AppendLine(String.Format("{0},{1}", "QuickInfoLightsColour",
                                        conf.TMDisplaySettings.QuickInfoLightsColour.ToString(
                                            CultureInfo.InvariantCulture)));
            sb.AppendLine(String.Format("{0},{1}", "WarningTextDisplayTime",
                                        conf.TMDisplaySettings.WarningTextDisplayTime.ToString(
                                            CultureInfo.InvariantCulture)));
            sb.AppendLine("[ TM Display Units ]");
            //Start looping display units
            for (int unit = 0; unit < conf.DisplayConfigurations.Count; unit++)
            {
                //displayUnitString
                sb.AppendLine(String.Format("{0},{1}", "DisplayUnit", unit));
                sb.AppendLine("     [ Display Unit Button Functions ]");
                sb.AppendLine(String.Format("{0},{1}", "ButtonFunction1",
                                            conf.DisplayConfigurations[unit].ButtonFunctions[0]));
                sb.AppendLine(String.Format("{0},{1}", "ButtonFunction2",
                                            conf.DisplayConfigurations[unit].ButtonFunctions[1]));
                sb.AppendLine(String.Format("{0},{1}", "ButtonFunction3",
                                            conf.DisplayConfigurations[unit].ButtonFunctions[2]));
                sb.AppendLine(String.Format("{0},{1}", "ButtonFunction4",
                                            conf.DisplayConfigurations[unit].ButtonFunctions[3]));
                sb.AppendLine(String.Format("{0},{1}", "ButtonFunction5",
                                            conf.DisplayConfigurations[unit].ButtonFunctions[4]));
                sb.AppendLine(String.Format("{0},{1}", "ButtonFunction6",
                                            conf.DisplayConfigurations[unit].ButtonFunctions[5]));
                sb.AppendLine(String.Format("{0},{1}", "ButtonFunction7",
                                            conf.DisplayConfigurations[unit].ButtonFunctions[6]));
                sb.AppendLine(String.Format("{0},{1}", "ButtonFunction8",
                                            conf.DisplayConfigurations[unit].ButtonFunctions[7]));
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
                                            conf.DisplayConfigurations[unit].FFBClippingLights));
                sb.AppendLine(String.Format("{0},{1}", "FFBClipScreen",
                                            conf.DisplayConfigurations[unit].FFBClippingScreen.ToString(
                                                CultureInfo.InvariantCulture)));
                sb.AppendLine(String.Format("{0},{1}", "Inverted",
                                            conf.DisplayConfigurations[unit].Inverted));
                sb.AppendLine(String.Format("{0},{1}", "TM1640",
                                            conf.DisplayConfigurations[unit].IsTM1640));
                sb.AppendLine(String.Format("{0},{1}", "SwitchLEDs",
                                            conf.DisplayConfigurations[unit].SwitchLEDs));
                sb.AppendLine(String.Format("{0},{1}", "ShowDC",
                                            conf.DisplayConfigurations[unit].ShowDC));
                sb.AppendLine(String.Format("{0},{1}", "DCDisplayTime",
                                            conf.DisplayConfigurations[unit].DCDisplayTime.ToString(
                                                CultureInfo.InvariantCulture)));
                sb.AppendLine(String.Format("{0},{1}", "LapStyle",
                                            conf.DisplayConfigurations[unit].LapStyle));
                sb.AppendLine(String.Format("{0},{1}", "PitLights",
                                            conf.DisplayConfigurations[unit].PitLights));
                sb.AppendLine(String.Format("{0},{1}", "PitLimiterSpeed",
                                            conf.DisplayConfigurations[unit].PitLimiterSpeed));
                sb.AppendLine(String.Format("{0},{1}", "PitLimiterStyle",
                                            conf.DisplayConfigurations[unit].PitLimiterStyle));
                sb.AppendLine(String.Format("{0},{1}", "RevLimiterLights",
                                            conf.DisplayConfigurations[unit].RevLimiterLights));
                sb.AppendLine(String.Format("{0},{1}", "RevLimiterStyle",
                                            conf.DisplayConfigurations[unit].RevLimiterStyle));
                sb.AppendLine(String.Format("{0},{1}", "ShiftLightStyle",
                                            conf.DisplayConfigurations[unit].ShiftLightStyle));
                sb.AppendLine(String.Format("{0},{1}", "MatchCarShiftStyle", conf.DisplayConfigurations[unit].MatchCarShiftStyle));
                sb.AppendLine(String.Format("{0},{1}", "MatchCarShiftConfiguration", conf.DisplayConfigurations[unit].MatchCarShiftStyleOption));
                sb.AppendLine(String.Format("{0},{1}", "MatchRedShift", conf.DisplayConfigurations[unit].MatchRedShift));
                sb.AppendLine(String.Format("{0},{1}", "ShowLap",
                                            conf.DisplayConfigurations[unit].ShowLap));
                sb.AppendLine(String.Format("{0},{1}", "ShowShiftLights",
                                            conf.DisplayConfigurations[unit].ShowShiftLights));
                sb.AppendLine(String.Format("{0},{1}", "ShiftClumps",
                                            conf.DisplayConfigurations[unit].ShiftClumps));
                sb.AppendLine(String.Format("{0},{1}", "DeltaLightsShow",
                                            conf.DisplayConfigurations[unit].DeltaLightsShow));
                sb.AppendLine(String.Format("{0},{1}", "DeltaLightsPosition",
                                            conf.DisplayConfigurations[unit].DeltaLightsPosition));
                sb.AppendLine(String.Format("{0},{1}", "WarningType",
                                            conf.DisplayConfigurations[unit].WarningType));
                sb.AppendLine(String.Format("{0},{1}", "ShowEngineWarnings",
                                            conf.DisplayConfigurations[unit].ShowEngineWarnings));
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
                                                conf.ControllerConfigurations[c].ButtonFunctions[t - 1]));
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
        public bool ShowEngineWarnings = true;
        public WarningTypesEnum WarningType = WarningTypesEnum.Both;


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