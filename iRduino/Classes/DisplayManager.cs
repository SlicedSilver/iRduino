//========================================//
// iRduino - Created by Mark Silverwood  //
//======================================//

namespace iRduino.Classes
{
    using ArduinoInterfaces;
    using iRacingSdkWrapper;
    using iRacingSdkWrapper.Bitfields;
    using iRSDKSharp;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Forms;
    using System.Windows.Threading;

    using iRduino.Windows;

    public class DisplayManager
    {
        #region Public Fields Properties

        public MainWindow HostApp;
        public ArduinoLink ArduinoConnection;
        public List<Configuration> Configurations = new List<Configuration>();
        public bool ConfSet = false;
        public DispatcherTimer ControllerCheckTimer;
        public CarShiftStyles CurrentCarShiftStyle;
        public int CurrentDeltaType = 0;
        public List<int> CurrentScreen = new List<int>();

        public CarShiftRPMData CurrentShiftRPMData;
        public bool DeltaLightsOn = false;
        public Dictionarys Dictionarys = new Dictionarys();

        public int DisplayRefreshRate;
        public int LEDRefreshRate;
        public bool LEDSOn = true;
        public List<ShiftStyleEnum> MatchedShiftStyles;
        public bool Previewing;

        public List<TMDisplayVariables> RequestedTMDisplayVariables = new List<TMDisplayVariables>();
        public List<TMLEDVariables> RequestedTMLEDVariables = new List<TMLEDVariables>();

        public SavedTelemetryValues SavedTelemetry;// = new SavedTelemetryValues();
        public ShiftData ShiftLightData;
        public bool Test;
        public bool UseCustomShiftLights;
        public List<bool> UseTripleSegmentStyle;
        public List<DateTime> WaitTimeTMDisplay;
        public List<DateTime> WaitTimeTMLEDS;
        public SdkWrapper Wrapper;
        public List<bool> TM1640Units;
        #endregion Public Fields Properties

        #region Private Fields Properties

        private readonly DispatcherTimer cleanUpTimer;
        private readonly ShiftRPMS shiftRPMs = new ShiftRPMS();
        // ReSharper disable InconsistentNaming
        private TrackSurfaces _2NdLastTrackSurface;

        private List<ControllerDevice> controllers;
        private bool dcStart = true;
        private Dictionary<DCVariablesEnum, float> dcVars;
        private int displayRefreshRateFactor;
        // ReSharper disable FieldCanBeMadeReadOnly.Local
        private List<int> engWarnCounter = new List<int> { 0, 0, 0, 0, 0, 0, 0 };

        private bool useEngineWarnings; //used for warning display messages only

        private DateTime engineWarningsTimeOut = DateTime.Now;
        // ReSharper restore FieldCanBeMadeReadOnly.Local
        private bool firstTelemetryUpdate = true;

        private List<List<int>> lastPress;
        // ReSharper restore InconsistentNaming
        private TrackSurfaces lastTrackSurface;

        private readonly List<int> ledScaleLeftCenterRight = new List<int> { 15, 14, 12, 8, 0, 16, 48, 112, 240 };
        private readonly List<int> ledScaleLeftToRight = new List<int> { 0, 1, 3, 7, 15, 31, 63, 127, 255 };
        private readonly List<int> ledScaleRightToLeft = new List<int> { 0, 128, 192, 224, 240, 248, 252, 254, 255 };
        private bool newSession;
        private bool pastShiftPoint;
        private int refreshCount;
        private bool shiftLightsReady;
        private bool useDC;
        private bool useDeltaTiming;
        private bool useFuelCalcs;
        private bool useLapTiming;
        private int telemetryRefreshRate;
        #endregion Private Fields Properties

        /// <summary>
        ///     Construcutor for Display Manager
        /// </summary>
        public DisplayManager(MainWindow host)
        {
            this.HostApp = host;
            Intensity = 3;
            this.refreshCount = 0;
            Test = false;
            this.WaitTimeTMDisplay = new List<DateTime>();
            this.WaitTimeTMLEDS = new List<DateTime>();
            this.cleanUpTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 1) };
            this.cleanUpTimer.Tick += this.CleanUpTimerTick;
            this.cleanUpTimer.Start();
            ControllerCheckTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, 25) };
            ControllerCheckTimer.Tick += this.ControllerCheckTimerTick;
        }

        public Configuration CurrentConfiguration { get; set; }

        public int Intensity { get; set; }

        /// <summary>
        ///     Static function for correctly removing dots from strings and setting the revelant bit in the dots byte
        /// </summary>
        /// <param name="input">String to be parsed</param>
        /// <param name="displayString">Output display string</param>
        /// <param name="dotsOut">Output dots byte</param>
        /// <param name="isTM1640"></param>
        public static void ParseDisplayString(string input, out string displayString, out byte[] dotsOut, bool isTM1640)
        {
            //string header = hostApp.displayMngr.currentConfiguration.screens[screenEditCBOX.SelectedIndex].header;
            string newDisplay = "";

            int maxLength = isTM1640 ? 16 : 8;
            var dots = new BitArray(maxLength);
            var j = 0;

            // ReSharper disable ForCanBeConvertedToForeach
            for (int i = 0; i < input.Length; i++)

            // ReSharper restore ForCanBeConvertedToForeach
            {
                if (input[i] == '.' && j > 0)
                {
                    if (j >= maxLength + 1)
                    {
                        j = maxLength;
                    }
                    dots[j - 1] = true;
                }
                else
                {
                    newDisplay += input[i];
                    j++;
                }
            }
            displayString = newDisplay;
            dotsOut = BitArrayToByteArray(dots);
        }

        /// <summary>
        ///     Builds current header string
        /// </summary>
        /// <returns>Header string for Display</returns>
        public string GetHeader(int unit)
        {
            if (CurrentConfiguration.DisplayConfigurations[unit].Screens[CurrentScreen[unit]].UseCustomHeader)
            {
                //Use Custom header and skip the rest.
                return CurrentConfiguration.DisplayConfigurations[unit].Screens[CurrentScreen[unit]].CustomHeader;
            }
            string header = "";
            foreach (
                string dvString in
                    CurrentConfiguration.DisplayConfigurations[unit].Screens[CurrentScreen[unit]].Variables)
            {
                DisplayVarsEnum dv;
                if (Enum.TryParse(dvString, out dv))
                {
                    header += Dictionarys.DisplayVariables[dv].DisplayName;
                    if (dv != DisplayVarsEnum.Space && dv != DisplayVarsEnum.DoubleSpace)
                    {
                        header += ".";
                    }
                }
            }
            return header;
        }

        /// <summary>
        ///     Load configuration file from disk
        /// </summary>
        /// <param name="filename">File path</param>
        /// <param name="confOut">New Loaded Configuration</param>
        /// <returns>Bool. True if good, False if failed</returns>
        public bool LoadConfFile(string filename, out Configuration confOut)
        {
            Configuration configurationLoaded;
            if (Configuration.LoadConfigurationFromFile(filename, out configurationLoaded))
            {
                confOut = configurationLoaded;
                return true;
            }

            //Not True below...
            confOut = configurationLoaded;
            return false;
        }

        public void ResetSavedTelemetryValues()
        {

            this.SavedTelemetry = new SavedTelemetryValues(CurrentConfiguration.OtherSettings.FuelCalculationLaps, CurrentConfiguration.OtherSettings.UseWeightedFuelCalculations, this.telemetryRefreshRate);
        }

        public void SendTMLEDS()
        {
            if (CurrentConfiguration == null) return;
            List<TMLEDVariables> final = new List<TMLEDVariables>();
            for (int i = 0; i < CurrentConfiguration.TMDisplaySettings.NumDisplayUnits; i++)
            {
                final.Add(new TMLEDVariables());
                if (this.LEDSOn && !(Wrapper.IsConnected == false && ArduinoConnection.Running && !Previewing))
                {
                    final[i].GreenLEDS = RequestedTMLEDVariables[i].GreenLEDS;
                    final[i].RedLEDS = RequestedTMLEDVariables[i].RedLEDS;
                    int result = DateTime.Now.CompareTo(this.WaitTimeTMLEDS[i]);
                    if (result < 0) //use wait Leds
                    {
                        if (RequestedTMLEDVariables[i].WaitPassThrough)
                        {
                            //bit function like && for bytes
                            var greenTemp = final[i].GreenLEDS | RequestedTMLEDVariables[i].WaitGreen;
                            var redTemp = final[i].RedLEDS | RequestedTMLEDVariables[i].WaitRed;
                            final[i].GreenLEDS = Convert.ToByte(greenTemp);
                            final[i].RedLEDS = Convert.ToByte(redTemp);
                        }
                        else
                        {
                            final[i].GreenLEDS = RequestedTMLEDVariables[i].WaitGreen;
                            final[i].RedLEDS = RequestedTMLEDVariables[i].WaitRed;
                        }
                    }
                }
                else
                {
                    //no LEDS
                    final[i].GreenLEDS = 0;
                    final[i].RedLEDS = 0;
                }
            }       
            //send to display
            if (ArduinoConnection.Running && !Test)
            {
                //Merge
                var greenLEDSList = new List<byte>();
                var redLEDSList = new List<byte>();
                for (int i = 0; i < final.Count; i++)
                {
                    if (CurrentConfiguration.DisplayConfigurations[i].SwitchLEDs)
                    {
                        greenLEDSList.Add(final[i].RedLEDS);
                        redLEDSList.Add(final[i].GreenLEDS);
                    }
                    else
                    {
                        greenLEDSList.Add(final[i].GreenLEDS);
                        redLEDSList.Add(final[i].RedLEDS);
                    }
                }

                int newInt = Intensity;
                if (this.pastShiftPoint && CurrentConfiguration.TMDisplaySettings.ShiftIntensity)
                {
                    if (CurrentConfiguration.TMDisplaySettings.ShiftIntensityType)
                    {
                        //relative
                        newInt += CurrentConfiguration.TMDisplaySettings.ShiftIntensityAmount + 1;
                    }
                    else
                    {
                        newInt += CurrentConfiguration.TMDisplaySettings.ShiftIntensityAmount;
                    }
                }
                var tmLEDs = new TMLEDSMessage
                {
                    Green = greenLEDSList,
                    Red = redLEDSList,
                    Intensity = newInt
                };
                ArduinoConnection.SendSerialMessage(Constants.MessageID_TMLED, ArduinoMessagesSending.SendTMLEDS(tmLEDs));
            }

        }

        public void SendTMDisplay()
        {
            if (CurrentConfiguration == null) return;

            List<TMDisplayVariables> final = new List<TMDisplayVariables>();

            for (int i = 0; i < CurrentConfiguration.TMDisplaySettings.NumDisplayUnits; i++)
            {
                final.Add(new TMDisplayVariables());
                int result = DateTime.Now.CompareTo(this.WaitTimeTMDisplay[i]);
                if (result > 0) //dont wait
                {
                    //normal situation
                    if (this.SavedTelemetry.OnTrack && Wrapper.IsConnected)
                    {
                        //send normal message
                        ParseDisplayString(RequestedTMDisplayVariables[i].Display,
                                               out final[i].Display,
                                               out final[i].Dots,
                                               CurrentConfiguration.DisplayConfigurations[i].IsTM1640);
                    }
                    else
                    {
                        //send blank message please
                        final[i].Display = "        ";
                        if (CurrentConfiguration.DisplayConfigurations[i].IsTM1640)
                        {
                            final[i].Display += "                ";
                        }
                        for (int x = 0; x < final[i].Dots.Count(); x++)
                        {
                            final[i].Dots[x] = 0;
                        }
                    }
                }
                else //do wait!!
                {
                    //check for testing
                    if (!Test && !Previewing) //if not testing then
                    {
                        ////
                        //send wait message and new lights
                        ////
                        ParseDisplayString(RequestedTMDisplayVariables[i].WaitString,
                                               out final[i].Display,
                                               out final[i].Dots,
                                               CurrentConfiguration.DisplayConfigurations[i].IsTM1640);
                    } //if testing then leave alone
                }
            }

            //send to display
            if (ArduinoConnection.Running && !Test)
            {
                //Merge
                var displayList = new List<string>();
                var dotsList = new List<byte[]>();
                for (int i = 0; i < final.Count; i++)
                {
                    displayList.Add(final[i].Display);

                    if (CurrentConfiguration.DisplayConfigurations[i].Inverted)
                    {
                        //Shift dots one place to the left
                        for (int y = 0; y < final[i].Dots.Count(); y++)
                        {
                            var temp = (byte)(final[i].Dots[y] << 1);
                            final[i].Dots[y] = temp;
                        }
                        dotsList.Add(final[i].Dots);
                    }
                    else
                    {
                        dotsList.Add(final[i].Dots);
                    }
                }

                int newInt = Intensity;
                if (this.pastShiftPoint && CurrentConfiguration.TMDisplaySettings.ShiftIntensity)
                {
                    if (CurrentConfiguration.TMDisplaySettings.ShiftIntensityType)
                    {
                        //relative
                        newInt += CurrentConfiguration.TMDisplaySettings.ShiftIntensityAmount + 1;
                    }
                    else
                    {
                        newInt += CurrentConfiguration.TMDisplaySettings.ShiftIntensityAmount;
                    }
                }
                var tmDisplay = new TMStringMessage
                {
                    Display = displayList,
                    Dots = dotsList,
                    Intensity = newInt,
                    UnitType = TM1640Units
                };
                ArduinoConnection.SendSerialMessage(Constants.MessageID_TMString, ArduinoMessagesSending.SendTMStrings(tmDisplay));
            }
        }

        /// <summary>
        /// Setup Display Manager Instance before using the Start Method
        /// </summary>
        public void SetupDisplayMngr(int telemetryRefreshRateParameter, List<bool> tm1640Units)
        {
            this.telemetryRefreshRate = telemetryRefreshRateParameter;
            this.TM1640Units = tm1640Units;
            CurrentConfiguration.TMDisplaySettings.NumDisplayUnits = CurrentConfiguration.DisplayConfigurations.Count;
            CurrentConfiguration.NumberControllers = CurrentConfiguration.ControllerConfigurations.Count;
            this.DisplayRefreshRate = telemetryRefreshRateParameter / CurrentConfiguration.RefreshRates.DisplayRefreshRate;
            this.LEDRefreshRate = telemetryRefreshRateParameter / CurrentConfiguration.RefreshRates.LEDRefreshRate;
            int units = CurrentConfiguration.TMDisplaySettings.NumDisplayUnits;
            this.WaitTimeTMDisplay.Clear();
            this.WaitTimeTMLEDS.Clear();
            if (!CurrentConfiguration.OtherSettings.UseCustomFuelCalculationOptions)
            {
                CurrentConfiguration.OtherSettings.FuelCalculationLaps = 3;
                CurrentConfiguration.OtherSettings.UseWeightedFuelCalculations = true;
            }
            SavedTelemetry = new SavedTelemetryValues(CurrentConfiguration.OtherSettings.FuelCalculationLaps,CurrentConfiguration.OtherSettings.UseWeightedFuelCalculations, this.telemetryRefreshRate);
            RequestedTMDisplayVariables = new List<TMDisplayVariables>();
            RequestedTMLEDVariables = new List<TMLEDVariables>();
            for (int k = 1; k <= units; k++)
            {
                this.WaitTimeTMDisplay.Add(DateTime.Now);
                this.WaitTimeTMLEDS.Add(DateTime.Now);
                RequestedTMLEDVariables.Add(new TMLEDVariables());
                RequestedTMDisplayVariables.Add(new TMDisplayVariables());
                CurrentScreen.Add(0);
            }
            displayRefreshRateFactor = this.telemetryRefreshRate / CurrentConfiguration.RefreshRates.DisplayRefreshRate;
            useFuelCalcs = false;
            useDeltaTiming = true; // Add Checks for whether to log delta variables
            useLapTiming = false;
            useEngineWarnings = false;
            if (CurrentConfiguration.TMDisplaySettings.NumDisplayUnits > 0)
            {
                foreach (var unit in CurrentConfiguration.DisplayConfigurations)
                {
                    if (unit.ShowLap)
                    {
                        useLapTiming = true;
                    }
                    if (unit.ShowEngineWarnings)
                    {
                        useEngineWarnings = true;
                    }
                    foreach (var screen in unit.Screens)
                    {
                        foreach (var dvar in screen.Variables)
                        {
                            if (dvar == DisplayVarsEnum.FuelBurnRate.ToString() || dvar == DisplayVarsEnum.FuelLapsLeft.ToString() || dvar == DisplayVarsEnum.FuelBurnRateGallons.ToString())
                            {
                                useFuelCalcs = true;
                            }
                        }
                    }
                    foreach (var button in unit.ButtonOptions)
                    {
                        if (button == "Fuel Burn Rate (Litres/Lap)" || button == "Laps of Fuel Remaining" || button == "Fuel Burn Rate (Gallons/Lap)")
                        {
                            useFuelCalcs = true;
                        }
                    }
                }
            }
            foreach (var controller in CurrentConfiguration.ControllerConfigurations)
            {
                foreach (var button in controller.ButtonOptions)
                {
                    if (button == "Fuel Burn Rate (Litres/Lap)" || button == "Laps of Fuel Remaining" || button == "Fuel Burn Rate (Gallons/Lap)")
                    {
                        useFuelCalcs = true;
                    }
                }
            }

            //setup controllers
            if (CurrentConfiguration.NumberControllers > 0)
            {
                IList<ControllerDevice> systemControllers = ControllerDevice.Available();
                this.controllers = new List<ControllerDevice>();
                this.lastPress = new List<List<int>>();
                for (int z = 0; z < CurrentConfiguration.ControllerConfigurations.Count; z++)
                {
                    //find controller
                    int z1 = z;
                    foreach (
                        ControllerDevice systemController in
                            systemControllers.Where(
                                systemController =>
                                systemController.Guid == CurrentConfiguration.ControllerConfigurations[z1].DeviceGuid))
                    {
                        systemController.Acquire(new Form());
                        this.lastPress.Add(new List<int>());
                        this.controllers.Add(systemController);
                    }
                }
            }
            DeltaLightsOn = CurrentConfiguration.TMDisplaySettings.DeltaLightsOnDefault;
        }

        /// <summary>
        ///     Shows a specific string on the display for a specified amount of time
        /// </summary>
        /// <param name="display">String to show</param>
        /// <param name="delaytime">Time in seconds to display for.</param>
        /// <param name="unit"></param>
        public void ShowStringTimed(string display, int delaytime, int unit)
        {
            RequestedTMDisplayVariables[unit].WaitString = display;
            this.WaitTimeTMDisplay[unit] = DateTime.Now.AddSeconds(delaytime);
            if (!Wrapper.IsConnected)
            {
                SendTMDisplay();
            }
        }

        public void ShowLEDTimed(byte green, byte red, bool passThrough, int delaytime, int unit)
        {
            RequestedTMLEDVariables[unit].WaitGreen = green;
            RequestedTMLEDVariables[unit].WaitRed = red;
            RequestedTMLEDVariables[unit].WaitPassThrough = passThrough;
            this.WaitTimeTMLEDS[unit] = DateTime.Now.AddSeconds(delaytime);
            if (!Wrapper.IsConnected)
            {
                SendTMLEDS();
            }
        }

        internal void ButtonPress(int unit, int num, bool control)
        {
            ButtonFunctionsClass.ButtonPress(this, unit, num, control);
        }

        /// <summary>
        ///     Executes button press commands
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="num">Number of button pressed</param>
        internal void SLIButtonPress(int unit, int num)
        {
            unit = unit - 1;
            ButtonPress(unit, num, false);
        }

        /// <summary>
        ///     Static method for changing BitArrays to Byte Arrays //Use with caution. Only for 8-bit bytes
        /// </summary>
        /// <param name="bits">Bit Array</param>
        /// <returns>Byte Array</returns>
        private static byte[] BitArrayToByteArray(BitArray bits) //Only works nicely with 8-bit bytes
        {
            var ret = new byte[bits.Length / 8];
            bits.CopyTo(ret, 0);
            return ret;
        }

        /// <summary>
        ///     Clears the screen if not connected. Timer used because normal loops are executed by telemetryUpdated Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CleanUpTimerTick(object sender, EventArgs e)
        {
            if (Wrapper.IsConnected == false && ArduinoConnection.Running && !Previewing)
            {
                SendTMDisplay();
                SendTMLEDS();
            }
        }

        /// <summary>
        /// Converts a Controller Button Press to a form which is accepted in the main button press method
        /// </summary>
        /// <param name="d">Controller Number</param>
        /// <param name="i">Button Number</param>
        private void ControllerButtonPress(int d, int i)
        {
            ButtonPress(d, i, true);
        }

        /// <summary>
        /// Checks for Joystick Controller Button Presses
        /// </summary>
        /// <param name="sender">whatever</param>
        /// <param name="e">whatever</param>
        private void ControllerCheckTimerTick(object sender, EventArgs e)
        {
            if (this.controllers == null)
            {
                return;
            }
            for (int d = 0; d < this.controllers.Count; d++)
            {
                List<int> currentPress = this.controllers[d].GetButtons();
                IEnumerable<int> temp = currentPress.Except(this.lastPress[d]); //only get new button presses
                foreach (int i in temp)
                {
                    this.ControllerButtonPress(d, i);
                }
                this.lastPress[d] = currentPress;
            }
        }

        /// <summary>
        ///     Determines and shows measured lap time and delta on the screen using a ShowStringTimed method call
        /// </summary>
        private void ShowLapTimeDisplay()
        {
            for (int i = 0; i < CurrentConfiguration.TMDisplaySettings.NumDisplayUnits; i++)
            {
                if (!CurrentConfiguration.DisplayConfigurations[i].ShowLap) return;
                if (this.SavedTelemetry.LastLapTimeMeasured < 5) return;
                ShowStringTimed(
                    LapDisplays.BuildLapDisplayString(
                        CurrentConfiguration.DisplayConfigurations[i].LapStyle, this.SavedTelemetry),
                    CurrentConfiguration.TMDisplaySettings.LapDisplayTime,
                    i);
            }
        }

        #region Telemetry Update Functions

        /// <summary>
        ///     Called everytime telemetry is updated. Starting point for cycles
        /// </summary>
        /// <param name="e"></param>
        internal void TelemetryUpdate(SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            if (this.firstTelemetryUpdate)
            {
                this.FirstTelemetryUpdate();
            }
            if (this.SavedTelemetry.CurrentSessionNum != e.TelemetryInfo.SessionNum.Value)
            {
                this.NewTelemetrySession(e);
            }
            double updateTime = e.UpdateTime;
            TrackSurfaces[] surfaces = e.TelemetryInfo.CarIdxTrackSurface.Value;
            TrackSurfaces mySurface = surfaces[Wrapper.DriverId]; // Your car data is at your id index;
            if (refreshCount % 15 == 0)
            {
                this._2NdLastTrackSurface = this.lastTrackSurface; //used for DC Vars
            }
            this.lastTrackSurface = mySurface;
            if (useLapTiming)
            {
                updateTime = this.TelemetryLapTimer(e, mySurface, updateTime);
            }

            //store delta times
            if (useDeltaTiming)
            {
                this.SavedTelemetry.DeltaBest = e.TelemetryInfo.LapDeltaToBestLap.Value;
                this.SavedTelemetry.DeltaBestOK = e.TelemetryInfo.LapDeltaToBestLap_OK.Value;
                this.SavedTelemetry.DeltaHistory[0].Push(
                        e.TelemetryInfo.LapDeltaToBestLap_OK.Value ? e.TelemetryInfo.LapDeltaToBestLap.Value : 500f);
                this.SavedTelemetry.DeltaOpt = e.TelemetryInfo.LapDeltaToOptimalLap.Value;
                this.SavedTelemetry.DeltaOptOK = e.TelemetryInfo.LapDeltaToOptimalLap_OK.Value;
                this.SavedTelemetry.DeltaHistory[1].Push(
                        e.TelemetryInfo.LapDeltaToOptimalLap_OK.Value ? e.TelemetryInfo.LapDeltaToOptimalLap.Value : 500f);
                this.SavedTelemetry.DeltaSesBest = e.TelemetryInfo.LapDeltaToSessionBestLap.Value;
                this.SavedTelemetry.DeltaSesBestOK = e.TelemetryInfo.LapDeltaToSessionBestLap_OK.Value;
                this.SavedTelemetry.DeltaHistory[2].Push(
                        e.TelemetryInfo.LapDeltaToSessionBestLap_OK.Value ? e.TelemetryInfo.LapDeltaToSessionBestLap.Value : 500f);
                this.SavedTelemetry.DeltaSesOpt = e.TelemetryInfo.LapDeltaToSessionOptimalLap.Value;
                this.SavedTelemetry.DeltaSesOptOK = e.TelemetryInfo.LapDeltaToSessionOptimalLap_OK.Value;
                this.SavedTelemetry.DeltaHistory[3].Push(
                        e.TelemetryInfo.LapDeltaToSessionOptimalLap_OK.Value ? e.TelemetryInfo.LapDeltaToSessionOptimalLap.Value : 500f);
            }

            if (this.refreshCount % 30 == 0 && useFuelCalcs) //Only Check Fuel Consumption Stuff Once a Second
            {
                this.FuelTelemetry(e, mySurface);
            }
            if (this.refreshCount % DisplayRefreshRate == 0)
            {
                UpdateDisplayString(e);
                if (this.useDC)
                {
                    UpdateDCVars(e);
                }
            }

            this.SavedTelemetry.LastTelemetryUpdate = updateTime;
            this.SavedTelemetry.CurrentLap = e.TelemetryInfo.Lap.Value;
            this.SavedTelemetry.CurrentFuelPCT = e.TelemetryInfo.FuelLevelPct.Value;
            this.SavedTelemetry.OnTrack = e.TelemetryInfo.IsOnTrack.Value;

            if (useEngineWarnings)
            {
                this.EngineWarningsDisplayUpdate(e);
            }
            if (this.CurrentConfiguration.DisplayConfigurations.Count > 0)
            {
                if (this.refreshCount % LEDRefreshRate == 0)
                {
                    UpdateLEDs(e);
                    SendTMLEDS();
                }
                if (this.refreshCount % displayRefreshRateFactor == 0)
                {
                    SendTMDisplay();
                }
            }
            this.refreshCount++;
            if (this.refreshCount >= 2000000000) //well before overflow
            {
                this.refreshCount = 0;
            }
        }

        private void EngineWarningsDisplayUpdate(SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            EngineWarning engine = e.TelemetryInfo.EngineWarnings.Value;
            bool warningActive = false;
            string warningText = "";
            if (engine.Contains(EngineWarnings.WaterTemperatureWarning))
            {
                warningActive = true;
                warningText = "ET HigH";
            }
            else if (engine.Contains(EngineWarnings.OilPressureWarning))
            {
                warningActive = true;
                warningText = "OP louu";
            }
            else if (engine.Contains(EngineWarnings.FuelPressureWarning))
            {
                warningActive = true;
                warningText = "FP Louu";
            }
            if (warningActive)
            {
                int result = DateTime.Now.CompareTo(this.engineWarningsTimeOut);
                for (var d = 0; d < this.CurrentConfiguration.DisplayConfigurations.Count; d++)
                {
                    if (this.CurrentConfiguration.DisplayConfigurations[d].ShowEngineWarnings)
                    {
                        switch (this.CurrentConfiguration.DisplayConfigurations[d].WarningType)
                        {
                            case WarningTypesEnum.Both:
                                this.ShowLEDTimed(0, 170, true, 1, d);
                                if (result > 0)
                                {
                                    this.ShowStringTimed(
                                            warningText, this.CurrentConfiguration.TMDisplaySettings.WarningTextDisplayTime, d);
                                    this.engineWarningsTimeOut =
                                            DateTime.Now.AddSeconds(
                                                    this.CurrentConfiguration.TMDisplaySettings.WarningTextDisplayTime * 4);
                                }
                                break;
                            case WarningTypesEnum.Lights:
                                this.ShowLEDTimed(0, 170, true, 1, d);
                                break;
                            case WarningTypesEnum.Text:
                                if (result > 0)
                                {
                                    this.ShowStringTimed(
                                            warningText, this.CurrentConfiguration.TMDisplaySettings.WarningTextDisplayTime, d);
                                    this.engineWarningsTimeOut =
                                            DateTime.Now.AddSeconds(
                                                    this.CurrentConfiguration.TMDisplaySettings.WarningTextDisplayTime * 4);
                                }
                                break;
                        }
                    }
                }
            }
        }

        private void FirstTelemetryUpdate()
        {
            this.SetDCVarsList();
            this.lastTrackSurface = TrackSurfaces.NotInWorld;
            this._2NdLastTrackSurface = TrackSurfaces.NotInWorld;
            this.firstTelemetryUpdate = false;
        }

        private void FuelTelemetry(SdkWrapper.TelemetryUpdatedEventArgs e, TrackSurfaces mySurface)
        {
            bool update = false;
            float currentLapDistPct = e.TelemetryInfo.LapDistPct.Value;
            if (currentLapDistPct <= 0.15 && this.SavedTelemetry.Fuel.LastLapDistPct > 0.85)
            {
                if (mySurface != TrackSurfaces.AproachingPits && mySurface != TrackSurfaces.InPitStall)
                {
                    //crossed line therefore update fuel
                    update = true;
                }
            }
            else
            {
                //didn't cross line therefore check if crossed quarter points
                if (currentLapDistPct >= 0.25 && this.SavedTelemetry.Fuel.LastLapDistPct < 0.25)
                {
                    update = true;
                }
                else if (currentLapDistPct >= 0.5 && this.SavedTelemetry.Fuel.LastLapDistPct < 0.5)
                {
                    update = true;
                }
                else if (currentLapDistPct >= 0.75 && this.SavedTelemetry.Fuel.LastLapDistPct < 0.75)
                {
                    update = true;
                }
            }
            this.SavedTelemetry.Fuel.LastLapDistPct = currentLapDistPct;
            if (update)
            {
                this.SavedTelemetry.Fuel.FuelHistory.Push(e.TelemetryInfo.FuelLevel.Value);
            }
            if (this.SavedTelemetry.Fuel.CurrentFuelLevel - e.TelemetryInfo.FuelLevel.Value > 0.05f)
            {
                this.SavedTelemetry.Fuel.ResetFuel();
            }
            else
            {
                this.SavedTelemetry.Fuel.CurrentFuelLevel = e.TelemetryInfo.FuelLevel.Value;
                this.SavedTelemetry.Fuel.UpdateCalculatedFuelValues();
            }
        }

        private ShiftRPMs GetCustomRPMS(SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            //read map var from xml
            var mapVar = this.CurrentShiftRPMData.MapVariable;
            int gear = e.TelemetryInfo.Gear.Value;
            ShiftRPMs rpms = null;
            if (mapVar != "" || mapVar != null)
            {
                //get dcVarvalue
                var found = false;
                if (this.Wrapper.containsKey(mapVar))
                {
                    var value = this.Wrapper.GetData(mapVar) is float ? (float)this.Wrapper.GetData(mapVar) : 0;
                    foreach (var map in this.CurrentShiftRPMData.Maps)
                    {
                        const float Epsilon = 0.005f;
                        if (Math.Abs(map.MapValue - value) < Epsilon)
                        {
                            found = true;
                            foreach (var shiftRpmsFE in map.Gears.Where(shiftRpmsFE => shiftRpmsFE.Gear == gear))
                            {
                                rpms = shiftRpmsFE;
                            }
                        }
                    }
                }
                if (!found)
                {
                    foreach (
                        var shiftRpmsFE in
                            this.CurrentShiftRPMData.Maps[0].Gears.Where(shiftRpmsFE => shiftRpmsFE.Gear == gear))
                    {
                        rpms = shiftRpmsFE;
                    }
                }
            }
            else
            {
                foreach (
                    var shiftRpmsFE in this.CurrentShiftRPMData.Maps[0].Gears.Where(shiftRpmsFE => shiftRpmsFE.Gear == gear))
                {
                    rpms = shiftRpmsFE;
                }
            }
            return rpms;
        }

        private void NewTelemetrySession(SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            this.SavedTelemetry.CurrentSessionNum = e.TelemetryInfo.SessionNum.Value;
            this.newSession = true;
            this.SetDCVarsList();
        }

        private void SetDCVarsList()
        {
            this.useDC = false;
            foreach (DisplayConfiguration unit in CurrentConfiguration.DisplayConfigurations)
            {
                if (unit.ShowDC)
                {
                    this.useDC = true;
                }
            }
            if (!this.useDC) return;
            this.dcStart = true;
            this.dcVars = DCVariablesFunctions.SetDCList(Wrapper);
        }

        /// <summary>
        ///     Set LED bytes for engine flags such as pit limiter and Rev Limiter
        /// </summary>
        /// <param name="engine">Enginewarnings variable</param>
        /// <param name="red">byte for red leds</param>
        /// <param name="green">byte for green leds</param>
        /// <param name="unit"></param>
        private void SetEngineWarningsLEDS(BitfieldBase<EngineWarnings> engine, ref byte red, ref byte green, int unit)
        {
            int currentSpeed;
            switch (CurrentConfiguration.DisplayConfigurations[unit].PitLimiterSpeed)
            {
                case PitFlashSpeedsEnum.Full:
                    currentSpeed = 4;
                    break;

                case PitFlashSpeedsEnum.Half:
                    currentSpeed = 8;
                    break;

                case PitFlashSpeedsEnum.Quarter:
                    currentSpeed = 16;
                    break;

                case PitFlashSpeedsEnum.Eighth:
                    currentSpeed = 32;
                    break;

                default:
                    currentSpeed = 8;
                    break;
            }

            if (this.engWarnCounter[unit] < currentSpeed)
            {
                if (engine.Contains(EngineWarnings.PitSpeedLimiter))
                {
                    //pitlimiterLbl.Content = "ON ***";
                    if (this.engWarnCounter[unit] < currentSpeed / 2)
                    {
                        //first
                        Lights temp =
                            Dictionarys.PitStyles[CurrentConfiguration.DisplayConfigurations[unit].PitLimiterStyle]
                                .Normal[0];
                        red = temp.Red;
                        green = temp.Green;
                    }
                    else
                    {
                        //second
                        Lights temp =
                            Dictionarys.PitStyles[CurrentConfiguration.DisplayConfigurations[unit].PitLimiterStyle]
                                .Normal[1];
                        red = temp.Red;
                        green = temp.Green;
                    }
                }
                else if (engine.Contains(EngineWarnings.RevLimiterActive))
                {
                    //revlimiterLbl.Content = "ON ***";
                    if (this.engWarnCounter[unit] % 2 == 0)
                    {
                        //on
                        Lights temp =
                            Dictionarys.RevLimStyles[CurrentConfiguration.DisplayConfigurations[unit].RevLimiterStyle]
                                .Normal[0];
                        if (
                            Dictionarys.RevLimStyles[CurrentConfiguration.DisplayConfigurations[unit].RevLimiterStyle]
                                .SecondRevFall)
                        {
                            red = Convert.ToByte(red | temp.Red);
                            green = Convert.ToByte(green | temp.Green);
                        }
                        else
                        {
                            red = temp.Red;
                            green = temp.Green;
                        }
                    }
                    else
                    {
                        Lights temp =
                            Dictionarys.RevLimStyles[CurrentConfiguration.DisplayConfigurations[unit].RevLimiterStyle]
                                .Normal[1];
                        if (
                            Dictionarys.RevLimStyles[CurrentConfiguration.DisplayConfigurations[unit].RevLimiterStyle]
                                .SecondRevFall)
                        {
                            red = Convert.ToByte(red | temp.Red);
                            green = Convert.ToByte(green | temp.Green);
                        }
                        else
                        {
                            red = temp.Red;
                            green = temp.Green;
                        }
                    }
                }
                this.engWarnCounter[unit]++;
                if (this.engWarnCounter[unit] >= currentSpeed)
                {
                    this.engWarnCounter[unit] = 0;
                }
            }
            else
            {
                this.engWarnCounter[unit] = 0;
            }
        }

        private double TelemetryLapTimer(
            SdkWrapper.TelemetryUpdatedEventArgs e, TrackSurfaces mySurface, double updateTime)
        {
            if ((((e.TelemetryInfo.LapCurrentLapTime.Value - SavedTelemetry.LastMeasuredCurrentLapTime) < -5) || (this.SavedTelemetry.LastLapTimeAPI < 1 && e.TelemetryInfo.LapLastLapTime.Value > 5)) && (mySurface == TrackSurfaces.OnTrack || mySurface == TrackSurfaces.OffTrack))
            {
                //crossed line
                this.SavedTelemetry.LastLapTimeMeasured = e.TelemetryInfo.LapLastLapTime.Value;
                    this.ShowLapTimeDisplay();
                    foreach (Stack<float> t in this.SavedTelemetry.DeltaHistory)
                    {
                        t.Push(500f);
                    }
            }
            this.SavedTelemetry.PersonalBestLap = e.TelemetryInfo.LapBestLapTime.Value;
            this.SavedTelemetry.LastMeasuredCurrentLapTime = e.TelemetryInfo.LapCurrentLapTime.Value;
            this.SavedTelemetry.LastLapTimeAPI = e.TelemetryInfo.LapLastLapTime.Value;
            return updateTime;
        }
        private void UpdateDCVars(SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            var keys = new List<DCVariablesEnum>(this.dcVars.Keys);
            foreach (DCVariablesEnum key in keys)
            {
                float last = this.dcVars[key];

                //Gets and Sets the DC Variable into dcVars
                DCVariablesFunctions.GetDCVariableValue(dcVars, e, key);
                if (!this.dcStart)
                {
                    //check values
                    const float Epsilon = 0.005f;
                    if (Math.Abs(last - this.dcVars[key]) > Epsilon)
                    {
                        //value changed so show on display
                        for (int p = 0; p < CurrentConfiguration.DisplayConfigurations.Count; p++)
                        {
                            if (CurrentConfiguration.DisplayConfigurations[p].ShowDC &&
                                this.lastTrackSurface != TrackSurfaces.NotInWorld &&
                                this._2NdLastTrackSurface != TrackSurfaces.NotInWorld)
                            {
                                int temp2 = Convert.ToInt32(this.dcVars[key]);
                                if (Math.Abs(temp2 - this.dcVars[key]) > 0.005) //decide if should show decimal places
                                {
                                    ShowStringTimed("dc " + this.dcVars[key].ToString("0.00"),
                                                    CurrentConfiguration.DisplayConfigurations[p].DCDisplayTime + 1, p);
                                }
                                else
                                {
                                    ShowStringTimed("dc " + temp2.ToString(CultureInfo.InvariantCulture),
                                                    CurrentConfiguration.DisplayConfigurations[p].DCDisplayTime + 1, p);
                                }
                            }
                        }
                    }
                }
            }
            this.dcStart = false;
        }
        /// <summary>
        ///     Builds a display string using current screens display variables. (This is where telemetry variables are converted into useable strings)
        /// </summary>
        /// <param name="e">Telemetry Argument from SDKWrapper</param>
        private void UpdateDisplayString(SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            for (int i = 0; i < CurrentConfiguration.TMDisplaySettings.NumDisplayUnits; i++)
            {
                //build the display string
                var displayString = DisplayVariablesClass.BuildDisplayString(
                    e, CurrentConfiguration.DisplayConfigurations[i].Screens[CurrentScreen[i]].Variables, Dictionarys, SavedTelemetry, CurrentDeltaType);
                RequestedTMDisplayVariables[i].Display = displayString;
            }
        }

        /// <summary>
        ///     Updates LED bytes. Considers RPM and FFB PCT
        /// </summary>
        /// <param name="e">Telemetry Argument from SDKWrapper</param>
        private void UpdateLEDs(SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            float currentDelta = 0f;
            float currentDeltaDD = 0f;
            bool currentDeltaOK = false;
            switch (CurrentDeltaType)
            {
                case 0:
                    currentDelta = e.TelemetryInfo.LapDeltaToBestLap.Value;
                    currentDeltaDD = e.TelemetryInfo.LapDeltaToBestLap_DD.Value;
                    currentDeltaOK = e.TelemetryInfo.LapDeltaToBestLap_OK.Value;
                    break;

                case 1:
                    currentDelta = e.TelemetryInfo.LapDeltaToOptimalLap.Value;
                    currentDeltaDD = e.TelemetryInfo.LapDeltaToOptimalLap_DD.Value;
                    currentDeltaOK = e.TelemetryInfo.LapDeltaToOptimalLap_OK.Value;
                    break;

                case 2:
                    currentDelta = e.TelemetryInfo.LapDeltaToSessionBestLap.Value;
                    currentDeltaDD = e.TelemetryInfo.LapDeltaToSessionBestLap_DD.Value;
                    currentDeltaOK = e.TelemetryInfo.LapDeltaToSessionBestLap_OK.Value;
                    break;

                case 3:
                    currentDelta = e.TelemetryInfo.LapDeltaToSessionOptimalLap.Value;
                    currentDeltaDD = e.TelemetryInfo.LapDeltaToSessionOptimalLap_DD.Value;
                    currentDeltaOK = e.TelemetryInfo.LapDeltaToSessionOptimalLap_OK.Value;
                    break;
            }
            currentDelta = -currentDelta;
            currentDeltaDD = -currentDeltaDD;
            for (int i = 0; i < CurrentConfiguration.TMDisplaySettings.NumDisplayUnits; i++)
            {
                bool ledsSet = false;
                byte red = 0;
                byte green = 0;
                if (CurrentConfiguration.DisplayConfigurations[i].FFBClippingLights &&
                    CurrentScreen[i] == CurrentConfiguration.DisplayConfigurations[i].FFBClippingScreen - 1)
                {
                    //show ffb lights
                    LEDFunctions.GetShiftLights(Dictionarys, e.TelemetryInfo.SteeringWheelPctTorque.Value * 100, 0, 100, 95,
                                   ShiftStyleEnum.GreenProgressiveRedShift, false, out pastShiftPoint, out red, out green);
                    ledsSet = true;
                }
                if (DeltaLightsOn && CurrentConfiguration.DisplayConfigurations[i].DeltaLightsShow && !ledsSet)
                {
                    if (currentDeltaOK)
                    {
                        //delta lights please
                        switch (CurrentConfiguration.DisplayConfigurations[i].DeltaLightsPosition)
                        {
                            case DeltaLightsOptions.DualLeft: //left
                                if (currentDelta < 0)
                                {
                                    float place = (currentDelta) / (Convert.ToSingle(CurrentConfiguration.TMDisplaySettings.DeltaRange) / -2f) * 8f;
                                    int placeInt = Convert.ToInt32(Math.Round(place));
                                    if (placeInt > 8)
                                    {
                                        placeInt = 8;
                                    }
                                    if (CurrentConfiguration.TMDisplaySettings.ColourDeltaByDD)
                                    {
                                        if (currentDeltaDD >= 0)
                                        {
                                            red = 0;
                                            green = Convert.ToByte(this.ledScaleRightToLeft[placeInt]);
                                        }
                                        else
                                        {
                                            green = 0;
                                            red = Convert.ToByte(this.ledScaleRightToLeft[placeInt]);
                                        }
                                    }
                                    else
                                    {
                                        green = 0;
                                        red = Convert.ToByte(this.ledScaleRightToLeft[placeInt]);
                                    }
                                }
                                break;

                            case DeltaLightsOptions.DualRight: //right
                                if (currentDelta > 0)
                                {
                                    float place = (currentDelta) / (Convert.ToSingle(CurrentConfiguration.TMDisplaySettings.DeltaRange) / 2f) * 8f;
                                    int placeInt = Convert.ToInt32(Math.Round(place));
                                    if (placeInt > 8)
                                    {
                                        placeInt = 8;
                                    }
                                    if (CurrentConfiguration.TMDisplaySettings.ColourDeltaByDD)
                                    {
                                        if (currentDeltaDD >= 0)
                                        {
                                            red = 0;
                                            green = Convert.ToByte(this.ledScaleLeftToRight[placeInt]);
                                        }
                                        else
                                        {
                                            green = 0;
                                            red = Convert.ToByte(this.ledScaleLeftToRight[placeInt]);
                                        }
                                    }
                                    else
                                    {
                                        red = 0;
                                        green = Convert.ToByte(this.ledScaleLeftToRight[placeInt]);
                                    }
                                }
                                break;

                            case DeltaLightsOptions.Single: //single
                                float placeS = (currentDelta) / (Convert.ToSingle(CurrentConfiguration.TMDisplaySettings.DeltaRange) / 2f) * 4f;
                                int placeSInt = Convert.ToInt32(Math.Round(placeS));
                                if (placeSInt > 4)
                                {
                                    placeSInt = 4;
                                }
                                if (placeSInt < -4)
                                {
                                    placeSInt = -4;
                                }
                                if (CurrentConfiguration.TMDisplaySettings.ColourDeltaByDD)
                                {
                                    if (currentDeltaDD >= 0)
                                    {
                                        red = 0;
                                        green = Convert.ToByte(this.ledScaleLeftCenterRight[placeSInt + 4]);
                                    }
                                    else
                                    {
                                        green = 0;
                                        red = Convert.ToByte(this.ledScaleLeftCenterRight[placeSInt + 4]);
                                    }
                                }
                                else
                                {
                                    if (currentDelta < 0)
                                    {
                                        green = 0;
                                        red = Convert.ToByte(this.ledScaleLeftCenterRight[placeSInt + 4]);
                                    }
                                    else
                                    {
                                        red = 0;
                                        green = Convert.ToByte(this.ledScaleLeftCenterRight[placeSInt + 4]);
                                    }
                                }
                                break;
                        }
                    }
                    ledsSet = true;
                }
                if (!ledsSet)
                { //Normal Shift Lights
                    if (this.UseCustomShiftLights)
                    {
                        var rpms = this.GetCustomRPMS(e);
                        if (rpms != null)
                        {
                            LEDFunctions.GetShiftLights(
                                Dictionarys,
                                e.TelemetryInfo.RPM.Value,
                                rpms.First,
                                rpms.Last,
                                rpms.Shift,
                                this.MatchedShiftStyles[i],
                                this.UseTripleSegmentStyle[i],
                                out pastShiftPoint,
                                out red,
                                out green);
                        }
                        else
                        {
                            //copy of below code because rpm data was not loaded
                            LEDFunctions.GetShiftLights(
                            Dictionarys,
                            e.TelemetryInfo.RPM.Value,
                            this.shiftRPMs.ShiftStartRPM,
                            this.shiftRPMs.ShiftEndRPM,
                            this.shiftRPMs.ShiftPointRPM,
                            this.MatchedShiftStyles[i],
                            this.UseTripleSegmentStyle[i],
                            out pastShiftPoint,
                            out red,
                            out green);
                        }
                    }
                    else
                    {
                        LEDFunctions.GetShiftLights(
                            Dictionarys,
                            e.TelemetryInfo.RPM.Value,
                            this.shiftRPMs.ShiftStartRPM,
                            this.shiftRPMs.ShiftEndRPM,
                            this.shiftRPMs.ShiftPointRPM,
                            this.MatchedShiftStyles[i],
                            this.UseTripleSegmentStyle[i],
                            out pastShiftPoint,
                            out red,
                            out green);
                    }
                }

                //check for led overrides which are pit limiter, rev limiter TO ADD: Brake Lock, Car Alongside?
                if (CurrentConfiguration.DisplayConfigurations[i].RevLimiterLights ||
                    CurrentConfiguration.DisplayConfigurations[i].PitLights)
                {
                    EngineWarning engine = e.TelemetryInfo.EngineWarnings.Value;
                    SetEngineWarningsLEDS(engine, ref red, ref green, i);
                }
                RequestedTMLEDVariables[i].GreenLEDS = green;
                RequestedTMLEDVariables[i].RedLEDS = red;
            }
        }
        #endregion Telemetry Update Functions

        #region Session Info String Functions

        /// <summary>
        ///     Method called for everytime that the SessionInfo String is Updated
        /// </summary>
        /// <param name="e">SessionInfo Argument</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2241:Provide correct arguments to formatting methods")]
        internal void SessionUpdate(SdkWrapper.SessionInfoUpdatedEventArgs e)
        {
            if (this.newSession)
            {
                this.shiftLightsReady = false;
            }

            // Let's just dump the session info:
            // Parse the Drivers section of the session info into a list of drivers
            if (e.SessionInfo == null) return;
            ParseDrivers(e.SessionInfo);

            //Parse the ResultsPositions section of the session info to get the positions and times of drivers
            ParseTimes(e.SessionInfo);
            UpdateTimes();
            SessionQuickInfoSave();
            if (!Int32.TryParse(

                // ReSharper disable FormatStringProblem
                    YamlParser.Parse(e.SessionInfo,
                                     String.Format("SessionInfo:Sessions:SessionNum:{{0}}SessionLaps:",
                                                   this.SavedTelemetry.CurrentSessionNum)), out this.SavedTelemetry.TotalLaps))

            // ReSharper restore FormatStringProblem
            {
                this.SavedTelemetry.TotalLaps = -1;
            }
            if (this.shiftLightsReady != true)
            {
                SetShiftLightVariables(e);
                this.shiftLightsReady = true;
            }
        }

        //Class times and position stuff
        /// <summary>
        ///     Saves additional variables needed for some of the quick info displays
        /// </summary>
        private void SessionQuickInfoSave()
        {
            float classBestTemp = 0f;
            bool bestSet = false;
            var classPositions = new List<int>();
            foreach (
                Driver driver in this.SavedTelemetry.Drivers.Where(driver => driver.ClassId == this.SavedTelemetry.MyClassID))
            {
                if (bestSet == false)
                {
                    if (driver.FastestLapTime > 1)
                    {
                        classBestTemp = driver.FastestLapTime;
                        classPositions.Add(driver.Position);
                        bestSet = true;
                    }
                }
                else
                {
                    classPositions.Add(driver.Position);
                    if (driver.FastestLapTime < classBestTemp && driver.FastestLapTime > 1)
                    {
                        classBestTemp = driver.FastestLapTime;
                    }
                }
            }
            classPositions.Sort();
            for (int i = 0; i < classPositions.Count; i++)
            {
                if (classPositions[i] == this.SavedTelemetry.Position)
                {
                    this.SavedTelemetry.ClassPosition = i + 1;
                }
            }
            this.SavedTelemetry.ClassBestLap = classBestTemp;
        }

        private void SetShiftLightVariables(SdkWrapper.SessionInfoUpdatedEventArgs e)
        {
            SetShiftLights(e.SessionInfo);
            int driverID;
            Int32.TryParse(YamlParser.Parse(e.SessionInfo, "DriverInfo:DriverCarIdx:"), out driverID);
            Int32.TryParse(YamlParser.Parse(e.SessionInfo, String.Format("DriverInfo:Drivers:CarIdx:{{{0}}}{1}:", driverID, "CarID")), out this.SavedTelemetry.MyCarID);
            this.UseCustomShiftLights = false;
            this.CurrentCarShiftStyle = CarShiftStyles.None;
            var foundStyle = false;
            foreach (var style in ShiftLightData.CarShiftStyles)
            {
                if (style.CarID == this.SavedTelemetry.MyCarID)
                {
                    this.UseCustomShiftLights = style.UseCustomRPMS;
                    this.CurrentCarShiftStyle = style.ShiftStyle;
                    foundStyle = true;
                }
            }
            if (this.UseCustomShiftLights)
            {
                var found = false;
                foreach (var carShiftRPMData in this.ShiftLightData.CarShiftRPMs.Where(carShiftRPMData => carShiftRPMData.CarID == this.SavedTelemetry.MyCarID))
                {
                    found = true;
                    this.CurrentShiftRPMData = carShiftRPMData;
                }
                if (!found)
                {
                    this.UseCustomShiftLights = false;
                }
            }
            MatchedShiftStyles = new List<ShiftStyleEnum>();
            UseTripleSegmentStyle = new List<bool>();
            UseTripleSegmentStyle.Clear();
            MatchedShiftStyles.Clear();
            foreach (var unit in CurrentConfiguration.DisplayConfigurations)
            {
                if (foundStyle)
                {
                    this.MatchedShiftStyles.Add(
                        unit.MatchCarShiftStyle
                            ? LEDs.MatchCarShiftStyle(unit.MatchCarShiftStyleOption, this.CurrentCarShiftStyle, unit.MatchRedShift)
                            : unit.ShiftLightStyle);
                    this.UseTripleSegmentStyle.Add(unit.MatchCarShiftStyle ? this.CurrentCarShiftStyle == CarShiftStyles.LeftToRight3Segments : unit.ShiftClumps);
                }
                else
                {
                    this.MatchedShiftStyles.Add(unit.ShiftLightStyle);
                    this.UseTripleSegmentStyle.Add(unit.ShiftClumps);
                }
            }
        }
        /// <summary>
        ///     Updates lap times variables from Driver Array (from SessionString - ParseDrivers & ParseTimes)
        /// </summary>
        private void UpdateTimes()
        {
            bool bestSet = false;
            int myID = Wrapper.DriverId;
            float overallbestlapLoop = 0f;
            Driver me = null;

            //me = drivers.FirstOrDefault(d => d.Id == wrapper.DriverId);
            foreach (Driver driver in this.SavedTelemetry.Drivers)
            {
                if (bestSet == false)
                {
                    if (driver.FastestLapTime > 1)
                    {
                        overallbestlapLoop = driver.FastestLapTime;
                        bestSet = true;
                    }
                }
                else
                {
                    if (driver.FastestLapTime < overallbestlapLoop && driver.FastestLapTime > 1)
                    {
                        overallbestlapLoop = driver.FastestLapTime;
                    }
                }
                if (driver.Id == myID)
                {
                    //found yourself
                    me = driver;
                }
            }
            this.SavedTelemetry.Overallbestlap = overallbestlapLoop;
            if (me == null) return;

            //update lap times
            //this.SavedTelemetry.PersonalBestLap = me.FastestLapTime;
            //this.SavedTelemetry.LastLapTimeYAML = me.LastLapTime;
            this.SavedTelemetry.Position = me.Position;
            this.SavedTelemetry.MyClassID = me.ClassId;
        }

        #endregion Session Info String Functions

        #region YAML Parse Functions

        // Parse the YAML DriverInfo section that contains information such as driver id, name, license, car number, etc.
        private void ParseDrivers(string sessionInfo)
        {
            // This string is used for every property of the driver
            // {0} is replaced by the driver ID
            // {1} is replaced by the property key
            // The result is a string like:         DriverInfo:Drivers:CarIdx:{17}CarNumber:
            // which is the correct way to request the property 'CarNumber' from the driver with id 17.
            const string DriverYamlPath = "DriverInfo:Drivers:CarIdx:{{{0}}}{1}:";
            int id = 0;
            Driver driver;

            var newDrivers = new List<Driver>();

            // Loop through drivers until none are found
            do
            {
                driver = null;

                // Try to get the UserName of the driver (because its the first value given)
                // If the UserName value is not found (name == null) then we found all drivers
                string name = YamlParser.Parse(sessionInfo, string.Format(DriverYamlPath, id, "UserName"));
                if (name == null) continue;

                // Find this driver in the list
                // This strange " => " syntax is called a lambda expression and is short for a loop through all drivers
                // Read as: select the first driver 'd', if any, whose Name is equal to name.
                //if (drivers != null)
                //{
                //    driver = drivers.FirstOrDefault(d => d.Name == name);
                //}
                driver = this.SavedTelemetry.Drivers.FirstOrDefault(d => d.Name == name) ?? new Driver
                    {
                        Id = id,
                        Name = name,

                        //CustomerId = //
                        //    int.Parse(YamlParser.Parse(sessionInfo, string.Format(DriverYamlPath, id, "UserID"))),//
                        Number = YamlParser.Parse(sessionInfo, string.Format(DriverYamlPath, id, "CarNumber")),

                        //ClassId = //
                        //    int.Parse(YamlParser.Parse(sessionInfo, //
                        //                               string.Format(DriverYamlPath, id, "CarClassID"))), //
                        CarPath = YamlParser.Parse(sessionInfo, string.Format(DriverYamlPath, id, "CarPath")),

                        //CarClassRelSpeed = int.Parse(YamlParser.Parse(sessionInfo, //
                        //                                              string.Format(DriverYamlPath, id, //
                        //                                                            "CarClassRelSpeed"))),//
                        //Rating =//
                        //    int.Parse(YamlParser.Parse(sessionInfo, string.Format(DriverYamlPath, id, "IRating")))//
                    };
                int tempInt;
                if (Int32.TryParse(YamlParser.Parse(sessionInfo, string.Format(DriverYamlPath, id, "UserID")), out tempInt))
                {
                    driver.CustomerId = tempInt;
                }
                else
                {
                    driver.CustomerId = -1;
                }
                if (Int32.TryParse(YamlParser.Parse(sessionInfo, string.Format(DriverYamlPath, id, "CarClassID")), out tempInt))
                {
                    driver.ClassId = tempInt;
                }
                else
                {
                    driver.ClassId = -1;
                }

                if (Int32.TryParse(YamlParser.Parse(sessionInfo, string.Format(DriverYamlPath, id, "CarClassRelSpeed")), out tempInt))
                {
                    driver.CarClassRelSpeed = tempInt;
                }
                else
                {
                    driver.CarClassRelSpeed = -1;
                }
                if (Int32.TryParse(YamlParser.Parse(sessionInfo, string.Format(DriverYamlPath, id, "IRating")), out tempInt))
                {
                    driver.Rating = tempInt;
                }
                else
                {
                    driver.Rating = -1;
                }

                newDrivers.Add(driver);

                id++;
            } while (driver != null);

            // Replace old list of drivers with new list of drivers and update the grid
            this.SavedTelemetry.Drivers.Clear();
            this.SavedTelemetry.Drivers.AddRange(newDrivers);
        }

        // Parse the YAML SessionInfo section that contains information such as lap times, position, etc.
        private void ParseTimes(string sessionInfo)
        {
            // This string is used for every property of the driver
            // {0} is replaced by the current session number
            // {1} is replaced by the driver position
            // {2} is replaced by the property key
            // The result is a string like:         SessionInfo:Sessions:SessionNum:{1}ResultsPositions:Positin:{13}FastestTime:
            // which is the correct way to request the property 'FastestTime' from the driver in position 13.
            const string ResultsYamlPath =
                "SessionInfo:Sessions:SessionNum:{{{0}}}ResultsPositions:Position:{{{1}}}{2}:";

            int position = 1;
            Driver driver;

            // Loop through positions starting at 1 until no more are found
            do
            {
                driver = null;

                // Find the car id belonging to the current position
                string idString = YamlParser.Parse(sessionInfo,
                                                   string.Format(ResultsYamlPath, this.SavedTelemetry.CurrentSessionNum,
                                                                 position, "CarIdx"));
                if (idString == null) continue;
                int id = int.Parse(idString);

                // Find the corresponding driver from the list
                // This strange " => " syntax is called a lambda expression and is short for a loop through all drivers
                // Read as: select the first driver 'd', if any, whose Id is equal to id.
                driver = this.SavedTelemetry.Drivers.FirstOrDefault(d => d.Id == id);

                if (driver != null)
                {
                    driver.Position = position;
                    float temp;
                    driver.FastestLapTime = float.TryParse(YamlParser.Parse(sessionInfo,
                            string.Format(ResultsYamlPath, this.SavedTelemetry.CurrentSessionNum,
                                    position,
                                    "FastestTime")), out temp) ? temp : 0;

                    //driver.FastestLapTime =
                    //    float.Parse(
                    //        YamlParser.Parse(sessionInfo,
                    //                         string.Format(ResultsYamlPath, this.SavedTelemetry.CurrentSessionNum,
                    //                                       position,
                    //                                       "FastestTime")), CultureInfo.InvariantCulture);
                    driver.LastLapTime = float.TryParse(YamlParser.Parse(sessionInfo,
                            string.Format(ResultsYamlPath, this.SavedTelemetry.CurrentSessionNum,
                                    position,
                                    "LastTime")), out temp) ? temp : 0;

                    //driver.LastLapTime =
                    //    float.Parse(
                    //        YamlParser.Parse(sessionInfo,
                    //                         string.Format(ResultsYamlPath, this.SavedTelemetry.CurrentSessionNum,
                    //                                       position, "LastTime")),
                    //        CultureInfo.InvariantCulture);
                }

                position++;
            } while (driver != null);
        }

        /// <summary>
        ///     Updates Shift Light Variables from Session String
        /// </summary>
        /// <param name="sessionInfo"></param>
        private void SetShiftLights(string sessionInfo)
        {
            if (!float.TryParse(YamlParser.Parse(sessionInfo, "DriverInfo:DriverCarSLFirstRPM:"), out this.shiftRPMs.ShiftStartRPM))
            {
                this.shiftRPMs.ShiftStartRPM = 0;
            }
            if (!float.TryParse(YamlParser.Parse(sessionInfo, "DriverInfo:DriverCarSLLastRPM:"), out this.shiftRPMs.ShiftEndRPM))
            {
                this.shiftRPMs.ShiftEndRPM = 0;
            }
            if (!float.TryParse(YamlParser.Parse(sessionInfo, "DriverInfo:DriverCarSLShiftRPM:"), out this.shiftRPMs.ShiftPointRPM))
            {
                this.shiftRPMs.ShiftPointRPM = 0;
            }
            if (!float.TryParse(YamlParser.Parse(sessionInfo, "DriverInfo:DriverCarSLBlinkRPM:"), out this.shiftRPMs.ShiftBlinkRPM))
            {
                this.shiftRPMs.ShiftBlinkRPM = 0;
            }

            //this.shiftRPMs.ShiftStartRPM = float.Parse(YamlParser.Parse(sessionInfo, "DriverInfo:DriverCarSLFirstRPM:"));
            //this.shiftRPMs.ShiftEndRPM = float.Parse(YamlParser.Parse(sessionInfo, "DriverInfo:DriverCarSLLastRPM:"));
            //this.shiftRPMs.ShiftPointRPM = float.Parse(YamlParser.Parse(sessionInfo, "DriverInfo:DriverCarSLShiftRPM:"));
            //this.shiftRPMs.ShiftBlinkRPM = float.Parse(YamlParser.Parse(sessionInfo, "DriverInfo:DriverCarSLBlinkRPM:"));
        }
        #endregion YAML Parse Functions
    }

    public class FuelValues
    {
        public float CurrentFuelLevel = 0f;

        public Stack<float> FuelHistory;

        private readonly int maxLapsInHistory;
        public float LastLapDistPct = 0f;
        private float burnRate;
        private float lapsLeft;

        private readonly Boolean useWeightedCalculation;

        public FuelValues(int laps, Boolean useWeighted)
        {
            maxLapsInHistory = laps;
            FuelHistory = new Stack<float>(laps * 4 + 1);
            useWeightedCalculation = useWeighted;
        }

        public float BurnRate
        {
            get
            {
                return burnRate;
            }
        }

        public float LapsLeft
        {
            get
            {
                return lapsLeft;
            }
        }
        public void ResetFuel()
        {
            FuelHistory = new Stack<float>(maxLapsInHistory * 4 + 1);
            lapsLeft = 0f;
            burnRate = 0f;
        }

        public void UpdateCalculatedFuelValues()
        {
            int numberFuelMeasure = FuelHistory.Count;
            if (numberFuelMeasure < 6)
            {
                lapsLeft = 0f;
                burnRate = 0f;
                return;
            }
            if (useWeightedCalculation)
            {
                //copy out array
                var fuelHistoryCopy = new float[FuelHistory.Count];
                FuelHistory.CopyTo(fuelHistoryCopy, 0);
                float burn;
                if (numberFuelMeasure >= 14)
                {
                    //three laps or more laps
                    float burntemp = ((fuelHistoryCopy[0] - fuelHistoryCopy[4]) * 3
                            + (fuelHistoryCopy[4] - fuelHistoryCopy[8]) * 2
                            + (fuelHistoryCopy[8] - fuelHistoryCopy[12]) * 2); // / 7;
                    int place = 16;
                    int divisionCount = 7;
                    while (place <= fuelHistoryCopy.Count() - 1)
                    {
                        burntemp += fuelHistoryCopy[place - 4] - fuelHistoryCopy[place];
                        divisionCount++;
                        place += 4;
                    }
                    burn = burntemp / Convert.ToSingle(divisionCount);
                }
                else if (numberFuelMeasure >= 10)
                {
                    //two laps
                    burn = ((fuelHistoryCopy[0] - fuelHistoryCopy[4]) * 3
                            + (fuelHistoryCopy[4] - fuelHistoryCopy[8]) * 2) / 5;
                }
                else
                {
                    //one lap
                    burn = (fuelHistoryCopy[0] - fuelHistoryCopy[4]);
                }
                burnRate = Math.Abs(burn);
                lapsLeft = Math.Abs(CurrentFuelLevel / burn);
            }
            else //Don't Use Weighted Fuel Calc.
            {
                var fuelHistoryCopy = new float[FuelHistory.Count];
                int count = fuelHistoryCopy.Count();
                float burn = (fuelHistoryCopy[count - 1] - fuelHistoryCopy[0]) / (count / 4f);
                burnRate = Math.Abs(burn);
                lapsLeft = Math.Abs(CurrentFuelLevel / burn);
            }
        }
    }

    /// <summary>
    ///     Storage of telemetry values
    /// </summary>
    public class SavedTelemetryValues
    {
        public float ClassBestLap = 0f;
        public int ClassPosition = 0;
        public float CurrentFuelPCT = 0f;
        public int CurrentLap = 0;
        public int CurrentSessionNum = -5;
        public float CurrentSessionTime = 0f;
        public float DeltaBest = 0f;
        public bool DeltaBestOK = false;
        public float DeltaOpt = 0f;
        public bool DeltaOptOK = false;
        public float DeltaSesBest = 0f;
        public bool DeltaSesBestOK = false;
        public float DeltaSesOpt = 0f;
        public bool DeltaSesOptOK = false;
        public List<Driver> Drivers = new List<Driver>();
        public FuelValues Fuel;
        public double LastLapTimeMeasured; // measured using SDK now
        public float LastMeasuredCurrentLapTime = 0f;
        public double LastTelemetryUpdate;
        public int MyCarID;
        public int MyClassID = 0;
        public bool OnTrack = false;
        public float Overallbestlap = 0f;
        public float PersonalBestLap = 0f;  //set using e.Telemetry now
        public int Position = 0;
        public int SessionLapsRemaining = 0;
        public float SessionTimeRemaining = 0f;
        public int TotalLaps = -1;
        public List<Stack<float>> DeltaHistory;
        public int ExpectedDeltaHistoryLength;
        public float LastLapTimeAPI;

        public SavedTelemetryValues(int fuelLaps, Boolean fuelWeightedCalculation, int refreshRate)
        {
            Fuel = new FuelValues(fuelLaps,fuelWeightedCalculation);
            DeltaHistory = new List<Stack<float>>();
            for (var i = 0; i < 4; i++)
            {
                DeltaHistory.Add(new Stack<float>(refreshRate * 5 + 1)); //keeps for 5 seconds
            }
            ExpectedDeltaHistoryLength = refreshRate * 5 + 1;
        }
    }

    /// <summary>
    ///     Storage of variables used to send to display unit
    /// </summary>

    public class TMDisplayVariables
    {
        public string Display = "";
        public byte[] Dots = { 0, 0 };
        public string WaitString = ""; //Not used in FinalDisplayVariables
    }

    public class TMLEDVariables
    {
        public byte GreenLEDS = 0;
        public byte RedLEDS = 0;
        public byte WaitGreen = 0;
        public byte WaitRed = 0;
        public bool WaitPassThrough = true;
    }
}