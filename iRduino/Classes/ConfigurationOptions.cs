//========================================//
// iRduino - Created by Mark Silverwood  //
//======================================//

namespace iRduino.Classes
{
    using System.Globalization;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ArduinoInterfaces;
    using iRduino.Windows;

    /// <summary>
    ///     Class to represent the data that the Options Window works with. Will include methods to read from
    ///     a normal configuration class and save to one.
    /// </summary>
    public class ConfigurationOptions
    {
        public List<DisplayUnitConfiguration> DisplayUnitConfigurations;
        public int EditNumber; //only used in options window for passing information in DataContext
        public string FileLocation = "";
        public List<ControllerButtonConfiguration> ControllerConfigurations { get; set; }
        
        public int NumberControllers { get; set; }
        
        public string Name { get; set; }

        public int NumberDisplays { get; set; }

        public int Intensity { get; set; }

        public bool ShiftIntensity { get; set; }

        public bool ShiftIntensityType { get; set; }

        public int ShiftIntensityAmount { get; set; }

        public bool ShowHeader { get; set; }

        public int HeaderDisplayTime { get; set; }

        public int QuickInfoDisplayTime { get; set; }

        public int LapTimeDisplayTime { get; set; }

        public int PreferredComPort { get; set; }

        public string DisplayRefreshRate { get; set; }

        public string LEDRefreshRate { get; set; }

        public string SerialPortSpeed { get; set; }

        public bool LogArduinoMessages { get; set; }

        public bool UseCustomSerialSpeed { get; set; }

        public bool ColourDeltaByDD { get; set; }

        public bool DeltaLightsOnDefault { get; set; }

        public int DeltaMessageScreen { get; set; }

        public int DeltaRange { get; set; }

        public bool UseCustomFuelCalculationOptions { get; set; }

        public int FuelCalculationLaps { get; set; }

        public bool UseWeightedFuelCalculations { get; set; }

        public Dictionarys Dictionarys { get; set; }

        public void LoadConfiguration(Configuration configuration, Dictionarys dicts)
        {
            Name = configuration.Name;
            FileLocation = configuration.FileLocation;
            Intensity = configuration.Intensity;
            ShiftIntensity = configuration.ShiftIntensity;
            ShiftIntensityType = configuration.ShiftIntensityType;
            ShiftIntensityAmount = configuration.ShiftIntensityAmount;
            ShowHeader = configuration.ShowHeaders;
            HeaderDisplayTime = configuration.HeaderDisplayTime - 1;
            QuickInfoDisplayTime = configuration.QuickInfoDisplayTime - 1;
            LapTimeDisplayTime = configuration.LapDisplayTime - 1;
            PreferredComPort = configuration.PreferredComPort;
            LogArduinoMessages = configuration.LogArduinoMessages;
            SerialPortSpeed = configuration.SerialPortSpeed.ToString(CultureInfo.InvariantCulture);
            DisplayRefreshRate = configuration.DisplayRefreshRate.ToString(CultureInfo.InvariantCulture);
            LEDRefreshRate = configuration.LEDRefreshRate.ToString(CultureInfo.InvariantCulture);
            UseCustomSerialSpeed = configuration.UseCustomSerialSpeed;
            ColourDeltaByDD = configuration.ColourDeltaByDD;
            DeltaLightsOnDefault = configuration.DeltaLightsOnDefault;
            DeltaMessageScreen = configuration.DeltaMessageScreen;
            DeltaRange = configuration.DeltaRange - 1;
            UseCustomFuelCalculationOptions = configuration.UseCustomFuelCalculationOptions;
            FuelCalculationLaps = configuration.FuelCalculationLaps - 2;
            UseWeightedFuelCalculations = configuration.UseWeightedFuelCalculations;

            ControllerConfigurations = new List<ControllerButtonConfiguration>();
            foreach (ControllerConfiguration item in configuration.ControllerConfigurations)
            {
                var temp = new ControllerButtonConfiguration
                    {
                        DeviceGuid = item.DeviceGuid,
                        ButtonNumbers = item.ButtonNumbers,
                        ButtonOptions = item.ButtonOptions,
                        ButtonOptionsScreens = item.ButtonOptionsScreens,
                        Selected = item.Selected
                    };
                for (int k = 0; k < Constants.MaxNumberJoystickButtons; k++)
                {
                    temp.ButtonFunctions[k] = dicts.ButtonFunctions[item.ButtonFunctions[k]].Name;
                }
                ControllerConfigurations.Add(temp);
            }
            NumberControllers = configuration.ControllerConfigurations.Count;
            DisplayUnitConfigurations = new List<DisplayUnitConfiguration>();
            int unitCount = 0;
            foreach (DisplayConfiguration displayConf in configuration.DisplayConfigurations)
            {
                unitCount++;
                var temp = new DisplayUnitConfiguration
                    {
                        NumScreens = displayConf.NumScreens - 1,
                        Inverted = displayConf.Inverted,
                        IsTM1640 = displayConf.IsTM1640,
                        SwitchLEDs = displayConf.SwitchLEDs,
                        ShowDC = displayConf.ShowDC,
                        DCDisplayTime = displayConf.DCDisplayTime,
                        ShowLap = displayConf.ShowLap,
                        ButtonConfigurations = new ButtonConfiguration(),
                        UnitNumber = unitCount
                    };
                DisplayConfiguration conf = displayConf;
                foreach (var lstyle in dicts.LapDisplayStyles.Where(lstyle => conf.LapStyle == lstyle.Value))
                {
                    temp.LapStyle = lstyle.Key;
                }
                for (int n = 0; n < Constants.NumberButtonsOnTm1638; n++)
                {
                    temp.ButtonConfigurations.ButtonFunctions[n] =
                        dicts.ButtonFunctions[displayConf.ButtonFunctions[n]].Name;
                    temp.ButtonConfigurations.ButtonOptions[n] = displayConf.ButtonOptions[n];
                    temp.ButtonConfigurations.ButtonOptionsScreens[n] = displayConf.ButtonOptionsScreens[n];
                }
                temp.LEDsConfigurations = new LEDsConfiguration
                                              {
                                                      FFBClippingLights = displayConf.FFBClippingLights,
                                                      FFBClippingScreen = displayConf.FFBClippingScreen - 1,
                                                      PitLights = displayConf.PitLights,
                                                      RevLimiterLights = displayConf.RevLimiterLights,
                                                      ShowShiftLights = displayConf.ShowShiftLights,
                                                      ShiftClumps = displayConf.ShiftClumps,
                                                      MatchCarShiftLights = displayConf.MatchCarShiftStyle,
                                                      MatchRedShift = displayConf.MatchRedShift,
                                                      DeltaLightsShow = displayConf.DeltaLightsShow,
                                                      MatchCarShiftOptions = "",
                                              };
                foreach (var opt in dicts.MatchCarShiftOptions)
                {
                    if (opt.Value == displayConf.MatchCarShiftStyleOption)
                    {
                        temp.LEDsConfigurations.MatchCarShiftOptions = opt.Key;
                    }
                }
                temp.LEDsConfigurations.DeltaLightsPosition = "";
                foreach (var opt in dicts.DeltaLightsPositionOptions)
                {
                    if (opt.Value == displayConf.DeltaLightsPosition)
                    {
                        temp.LEDsConfigurations.DeltaLightsPosition = opt.Key;
                    }
                }
                foreach (var plspeed in dicts.PitFlashSpeeds)
                {
                    if (plspeed.Value == displayConf.PitLimiterSpeed)
                    {
                        temp.LEDsConfigurations.PitLimiterSpeed = plspeed.Key;
                    }
                }
                foreach (var plstyle in dicts.PitFlashStyles)
                {
                    if (plstyle.Value == displayConf.PitLimiterStyle)
                    {
                        temp.LEDsConfigurations.PitLimiterStyle = plstyle.Key;
                    }
                }
                foreach (var revlstyle in dicts.RevFlashStyles)
                {
                    if (revlstyle.Value == displayConf.RevLimiterStyle)
                    {
                        temp.LEDsConfigurations.RevLimiterStyle = revlstyle.Key;
                    }
                }
                foreach (var shiftstyle in dicts.ShiftStyles)
                {
                    if (shiftstyle.Value == displayConf.ShiftLightStyle)
                    {
                        temp.LEDsConfigurations.ShiftLightStyle = shiftstyle.Key;
                    }
                }
                temp.Screens = displayConf.Screens;
                DisplayUnitConfigurations.Add(temp);
            }
        }

        public Configuration SaveConfiguration(Dictionarys dicts)
        {
            var returnConf = new Configuration
                                 {
                                     Name = Name,
                                     FileLocation = FileLocation,
                                     Intensity = Intensity,
                                     ShiftIntensity = ShiftIntensity,
                                     ShiftIntensityType = ShiftIntensityType,
                                     ShiftIntensityAmount = ShiftIntensityAmount,
                                     ShowHeaders = ShowHeader,
                                     HeaderDisplayTime = HeaderDisplayTime + 1,
                                     QuickInfoDisplayTime = QuickInfoDisplayTime + 1,
                                     LapDisplayTime = LapTimeDisplayTime + 1,
                                     LogArduinoMessages = LogArduinoMessages,
                                     ColourDeltaByDD = ColourDeltaByDD,
                                     DeltaRange = DeltaRange + 1,
                                     DeltaMessageScreen = DeltaMessageScreen,
                                     DeltaLightsOnDefault = DeltaLightsOnDefault,
                                     PreferredComPort = PreferredComPort,
                                     UseCustomSerialSpeed = UseCustomSerialSpeed,
                                     DisplayConfigurations = new List<DisplayConfiguration>(),
                                     ControllerConfigurations = new List<ControllerConfiguration>(),
                                     SerialPortSpeed =
                                         AdvancedOptions.ParseSerialSpeedString(this.SerialPortSpeed),
                                     DisplayRefreshRate =
                                         AdvancedOptions.ParseRefreshRatesString(
                                             this.DisplayRefreshRate),
                                     LEDRefreshRate =
                                 AdvancedOptions.ParseRefreshRatesString(
                                     this.LEDRefreshRate),
                                     UseCustomFuelCalculationOptions = UseCustomFuelCalculationOptions,
                                     UseWeightedFuelCalculations = UseWeightedFuelCalculations,
                                     FuelCalculationLaps = FuelCalculationLaps + 2
                                 };
            if(returnConf.DeltaRange <= 0)
            {
                returnConf.DeltaRange = 1;
            }
            if (!UseCustomSerialSpeed) //Calculate and Auto Set Serial Speed
            {
                int numberTM1638 = 0;
                int numberTM1640 = 0;
                foreach (var unit in DisplayUnitConfigurations)
                {
                    if (unit.IsTM1640)
                    {
                        numberTM1640++;

                    }
                    else
                    {
                        numberTM1638++;
                    }

                }
                returnConf.SerialPortSpeed = AdvancedOptions.CalculateRecommendSerialSpeed(
                returnConf.DisplayRefreshRate,returnConf.LEDRefreshRate, numberTM1638, numberTM1640);
            }
            //int controllerCount = 0;
            foreach (ControllerButtonConfiguration item in ControllerConfigurations)
            {
                var temp = new ControllerConfiguration();
                for (int y = 0; y < Constants.MaxNumberJoystickButtons; y++)
                {
                    var butFuncTemp = ButtonFunctionsEnum.None;

                    foreach (var butFunc in dicts.ButtonFunctions)
                    {
                        if (butFunc.Value.Name == item.ButtonFunctions[y])
                        {
                            butFuncTemp = butFunc.Key;
                        }
                    }
                    temp.ButtonFunctions[y] = butFuncTemp;
                    temp.ButtonOptions[y] = item.ButtonOptions[y];
                    temp.ButtonOptionsScreens[y] = item.ButtonOptionsScreens[y];
                    temp.ButtonNumbers[y] = item.ButtonNumbers[y];
                    temp.DeviceGuid = item.DeviceGuid;
                    temp.Selected = item.Selected;
                }
                //controllerCount++;
                returnConf.ControllerConfigurations.Add(temp);
            }
            returnConf.NumberControllers = returnConf.ControllerConfigurations.Count;
            foreach (DisplayUnitConfiguration displayConf in DisplayUnitConfigurations)
            {
                var temp = new DisplayConfiguration(false)
                    {
                        NumScreens = displayConf.NumScreens + 1,
                        Inverted = displayConf.Inverted,
                        IsTM1640 = displayConf.IsTM1640,
                        SwitchLEDs = displayConf.SwitchLEDs,
                        ShowDC = displayConf.ShowDC,
                        DCDisplayTime = displayConf.DCDisplayTime,
                        ShowLap = displayConf.ShowLap,
                        FFBClippingLights = displayConf.LEDsConfigurations.FFBClippingLights,
                        FFBClippingScreen = displayConf.LEDsConfigurations.FFBClippingScreen + 1,
                        PitLights = displayConf.LEDsConfigurations.PitLights,
                        ShiftClumps = displayConf.LEDsConfigurations.ShiftClumps,
                        RevLimiterLights = displayConf.LEDsConfigurations.RevLimiterLights,
                        ShowShiftLights = displayConf.LEDsConfigurations.ShowShiftLights,
                        DeltaLightsShow = displayConf.LEDsConfigurations.DeltaLightsShow
                    };
                List<Screen> screenTemp = displayConf.Screens;
                foreach (Screen screen in screenTemp)
                {
                    int place = 0;
                    while (place < screen.Variables.Count)
                    {
                        if (screen.Variables[place] == null || screen.Variables[place] == "")
                        {
                            screen.Variables.RemoveAt(place);
                        }
                        else
                        {
                            place++;
                        }
                    }
                    if (screen.Variables.Count == 0)
                    {
                        screen.Variables.Add("Space");
                    }
                }
                temp.Screens = displayConf.Screens;
                try
                {
                    temp.LapStyle = dicts.LapDisplayStyles[displayConf.LapStyle];
                }
                catch
                {
                    Enum.TryParse(displayConf.LapStyle, out temp.LapStyle);
                }
                try
                {
                    temp.PitLimiterStyle = dicts.PitFlashStyles[displayConf.LEDsConfigurations.PitLimiterStyle];
                }
                catch
                {
                    Enum.TryParse(displayConf.LEDsConfigurations.PitLimiterStyle, out temp.PitLimiterStyle);
                }
                try
                {
                    temp.PitLimiterSpeed = dicts.PitFlashSpeeds[displayConf.LEDsConfigurations.PitLimiterSpeed];
                }
                catch
                {
                    Enum.TryParse(displayConf.LEDsConfigurations.PitLimiterSpeed, out temp.PitLimiterSpeed);
                }
                try
                {
                    temp.ShiftLightStyle = dicts.ShiftStyles[displayConf.LEDsConfigurations.ShiftLightStyle];
                }
                catch
                {
                    Enum.TryParse(displayConf.LEDsConfigurations.ShiftLightStyle, out temp.ShiftLightStyle);
                }
                try
                {
                    temp.RevLimiterStyle = dicts.RevFlashStyles[displayConf.LEDsConfigurations.RevLimiterStyle];
                }
                catch
                {
                    Enum.TryParse(displayConf.LEDsConfigurations.RevLimiterStyle, out temp.RevLimiterStyle);
                }
                temp.MatchCarShiftStyle = displayConf.LEDsConfigurations.MatchCarShiftLights;
                temp.MatchRedShift = displayConf.LEDsConfigurations.MatchRedShift;
                try
                {
                    temp.MatchCarShiftStyleOption =
                        dicts.MatchCarShiftOptions[displayConf.LEDsConfigurations.MatchCarShiftOptions];
                }
                catch
                {
                    Enum.TryParse(displayConf.LEDsConfigurations.MatchCarShiftOptions, out temp.MatchCarShiftStyleOption);
                }
                temp.DeltaLightsShow = displayConf.LEDsConfigurations.DeltaLightsShow;
                try
                {
                    temp.DeltaLightsPosition =
                        dicts.DeltaLightsPositionOptions[displayConf.LEDsConfigurations.DeltaLightsPosition];
                }
                catch
                {
                    Enum.TryParse(displayConf.LEDsConfigurations.DeltaLightsPosition, out temp.DeltaLightsPosition);
                }
                for (int y = 0; y < Constants.NumberButtonsOnTm1638; y++)
                {
                    var butFuncTemp = ButtonFunctionsEnum.None;

                    foreach (var butFunc in dicts.ButtonFunctions)
                    {
                        if (butFunc.Value.Name == displayConf.ButtonConfigurations.ButtonFunctions[y])
                        {
                            butFuncTemp = butFunc.Key;
                        }
                    }
                    temp.ButtonFunctions[y] = butFuncTemp;
                    temp.ButtonOptions[y] = displayConf.ButtonConfigurations.ButtonOptions[y];
                    temp.ButtonOptionsScreens[y] = displayConf.ButtonConfigurations.ButtonOptionsScreens[y];
                }
                returnConf.DisplayConfigurations.Add(temp);
            }
            return returnConf;
        }
    }

    public class DisplayUnitConfiguration
    {

        public MainWindow HostApp;
        public int ScreenToEdit;

        public DisplayUnitConfiguration()
        {
            NumScreens = 0;
        }

// ReSharper disable UnusedParameter.Local
        public DisplayUnitConfiguration(bool t)
// ReSharper restore UnusedParameter.Local
        {
            ButtonConfigurations = new ButtonConfiguration
                {
                    ButtonFunctions = new List<string>
                        {
                            "None",
                            "None",
                            "None",
                            "None",
                            "None",
                            "None",
                            "None",
                            "None"
                        },
                    ButtonOptions = new List<string> {"", "", "", "", "", "", "", ""},
                    ButtonOptionsScreens = new List<int> {-1, -1, -1, -1, -1, -1, -1, -1}
                };
            Inverted = false;
            IsTM1640 = false;
            SwitchLEDs = false;
            ShowDC = false;
            DCDisplayTime = 0;
            LapStyle = "Lap Time (12.34) and Personal Delta (-0.12)"; //LapDisplayStylesEnum.LapTimeDeltaPersonal;
            NumScreens = 0;
            LEDsConfigurations = new LEDsConfiguration
                {
                    PitLights = false,
                    PitLimiterSpeed = "Full Speed",
                    PitLimiterStyle = "Green & Red Alternate Switch",
                    RevLimiterLights = false,
                    RevLimiterStyle = "Stay Red",
                    ShiftLightStyle = "Green and Red Progressive with Red Shift",
                    ShiftClumps = false
                };
            ShowLap = false;
            LEDsConfigurations.ShowShiftLights = false;
            LEDsConfigurations.FFBClippingLights = false;
            LEDsConfigurations.FFBClippingScreen = 0;

            var temp = new Screen
                {
                    Example = "148 3 43",
                    Variables = new List<string> {"Speed", "Space", "Gear", "Space", "Laps2"}
                };
            Screens = new List<Screen> {temp};
        }

        public bool Inverted { get; set; }

        public bool IsTM1640 { get; set; }

        public bool SwitchLEDs { get; set; }

        public bool ShowDC { get; set; }

        public int DCDisplayTime { get; set; }

        public string LapStyle { get; set; }

        public bool ShowLap { get; set; }

        public int NumScreens { get; set; }

        public List<Screen> Screens { get; set; }

        public int TotalUnits { get; set; }

        public ButtonConfiguration ButtonConfigurations { get; set; }

        public int UnitNumber { get; set; }

        public LEDsConfiguration LEDsConfigurations { get; set; }
    }

    public class ButtonConfiguration
    {
        private List<string> buttonFunctions = new List<string>
            {
                "None",
                "None",
                "None",
                "None",
                "None",
                "None",
                "None",
                "None"
            }; //ButtonFunctionsEnum

        private List<string> buttonOptions = new List<string> {"", "", "", "", "", "", "", ""};
        private List<int> buttonOptionsScreens = new List<int> {-1, -1, -1, -1, -1, -1, -1, -1};

        public List<string> ButtonFunctions
        {
            get { return this.buttonFunctions; }
            set { this.buttonFunctions = value; }
        }

        public List<string> ButtonOptions
        {
            get { return this.buttonOptions; }
            set { this.buttonOptions = value; }
        }

        public List<int> ButtonOptionsScreens
        {
            get { return this.buttonOptionsScreens; }
            set { this.buttonOptionsScreens = value; }
        }
    }

    public class ControllerButtonConfiguration : ButtonConfiguration
    {
        public Guid DeviceGuid;
        public bool Selected = false; // use this flag to check if this controller should be used.
        private List<int> buttonNumbers = new List<int>(); //{ -1, -1, -1, -1, -1, -1, -1, -1 };

        public ControllerButtonConfiguration()
        {
            const int NumButtons = Constants.MaxNumberJoystickButtons;
            this.buttonNumbers = new List<int>();
            ButtonFunctions = new List<string>();
            ButtonOptions = new List<string>();
            ButtonOptionsScreens = new List<int>();
            for (int i = 0; i < NumButtons; i++)
            {
                this.buttonNumbers.Add(-1);
                ButtonFunctions.Add("None");
                ButtonOptions.Add("");
                ButtonOptionsScreens.Add(-1);
            }
        }

        public List<int> ButtonNumbers
        {
            get { return this.buttonNumbers; }
            set { this.buttonNumbers = value; }
        }
    }

    public class LEDsConfiguration
    {
        public bool FFBClippingLights { get; set; }

        public int FFBClippingScreen { get; set; }

        public bool PitLights { get; set; }

        public bool ShiftClumps { get; set; }

        public string PitLimiterStyle { get; set; }

        public string PitLimiterSpeed { get; set; }

        public bool RevLimiterLights { get; set; }

        public string RevLimiterStyle { get; set; }

        public bool ShowShiftLights { get; set; }

        public string ShiftLightStyle { get; set; }

        public bool MatchCarShiftLights { get; set; }

        public string MatchCarShiftOptions { get; set; }

        public bool MatchRedShift { get; set; }

        public bool DeltaLightsShow { get; set; }

        public string DeltaLightsPosition { get; set; }
    }
}