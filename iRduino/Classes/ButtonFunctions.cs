//========================================//
// iRduino - Created by Mark Silverwood  //
//======================================//

namespace iRduino.Classes
{
    using System.Windows.Forms;
    using System;
    using System.Collections.Generic;
    using ArduinoInterfaces;

    public enum ButtonFunctionsEnum
    {
        None,
        IncreaseIntensity,
        DecreaseIntensity,
        NextScreen,
        PreviousScreen,
        SpecificScreen,
        DisplayQuickInfo,
        CycleLapDeltaType,
        LapDeltaLEDsSwitch,
        TestSerialLink,
        LEDsOnOff,
        KeyboardButtonPress
    }

    public class ButtonFunction
    {
        public string Name;
        public List<string> Options = new List<string>();

        public ButtonFunction(string name)
        {
            Name = name;
        }
    }

    public class ButtonFunctionsClass
    {
        public static Dictionary<ButtonFunctionsEnum, ButtonFunction> FillButtonFunctions()
        {
            var buttonFunctions = new Dictionary<ButtonFunctionsEnum, ButtonFunction>
                {
                    {ButtonFunctionsEnum.None, new ButtonFunction("None")},
                    {ButtonFunctionsEnum.IncreaseIntensity, new ButtonFunction("Increase Intensity")},
                    {ButtonFunctionsEnum.DecreaseIntensity, new ButtonFunction("Decrease Intensity")},
                    {ButtonFunctionsEnum.NextScreen, new ButtonFunction("Next Screen")},
                    {ButtonFunctionsEnum.PreviousScreen, new ButtonFunction("Previous Screen")},
                    {ButtonFunctionsEnum.SpecificScreen, new ButtonFunction("Specific Screen")}
                };
            var quick = new ButtonFunction("Display Quick Info");
            quick.Options.Add("Current Time");
            quick.Options.Add("Position");
            quick.Options.Add("Class Position");
            quick.Options.Add("Lap ### of ###");
            quick.Options.Add("Sessions Fastest Lap");
            quick.Options.Add("Personal Best Lap");
            quick.Options.Add("Personal Last Lap");
            quick.Options.Add("Lap Delta to Best Lap");
            quick.Options.Add("Lap Delta to Optimal Lap");
            quick.Options.Add("Lap Delta to Session Best Lap");
            quick.Options.Add("Lap Delta to Session Optimal Lap");
            quick.Options.Add("Current Selected Lap Delta Type (for Selectable Lap Delta Variable)");
            quick.Options.Add("Change in delta for last 5 seconds (for Selectable Lap Delta Variable)");
            quick.Options.Add("Class Sessions Fastest Lap");
            quick.Options.Add("Fuel Percentage");
            quick.Options.Add("Session Time");
            quick.Options.Add("Session Time Remaining");
            quick.Options.Add("Session Laps Remaining");
            quick.Options.Add("Laps of Fuel Remaining");
            quick.Options.Add("Fuel Burn Rate (Litres/Lap)");
            quick.Options.Add("Fuel Burn Rate (Gallons/Lap)");
            buttonFunctions.Add(ButtonFunctionsEnum.DisplayQuickInfo, quick);
            buttonFunctions.Add(ButtonFunctionsEnum.CycleLapDeltaType, new ButtonFunction("Cycle Next Lap Delta Type"));
            buttonFunctions.Add(ButtonFunctionsEnum.LapDeltaLEDsSwitch, new ButtonFunction("Lap Delta LEDs On/Off"));
            buttonFunctions.Add(ButtonFunctionsEnum.TestSerialLink, new ButtonFunction("Test Serial Link"));
            buttonFunctions.Add(ButtonFunctionsEnum.LEDsOnOff, new ButtonFunction("LEDs on/off toggle"));
            var keyPress = new ButtonFunction("Keyboard Button Press");
            keyPress.Options.Add("a");
            keyPress.Options.Add("b");
            keyPress.Options.Add("c");
            keyPress.Options.Add("d");
            keyPress.Options.Add("e");
            keyPress.Options.Add("f");
            keyPress.Options.Add("g");
            keyPress.Options.Add("h");
            keyPress.Options.Add("i");
            keyPress.Options.Add("j");
            keyPress.Options.Add("k");
            keyPress.Options.Add("l");
            keyPress.Options.Add("m");
            keyPress.Options.Add("n");
            keyPress.Options.Add("o");
            keyPress.Options.Add("p");
            keyPress.Options.Add("q");
            keyPress.Options.Add("r");
            keyPress.Options.Add("s");
            keyPress.Options.Add("t");
            keyPress.Options.Add("u");
            keyPress.Options.Add("v");
            keyPress.Options.Add("w");
            keyPress.Options.Add("x");
            keyPress.Options.Add("y");
            keyPress.Options.Add("z");
            keyPress.Options.Add("0");
            keyPress.Options.Add("1");
            keyPress.Options.Add("2");
            keyPress.Options.Add("3");
            keyPress.Options.Add("4");
            keyPress.Options.Add("5");
            keyPress.Options.Add("6");
            keyPress.Options.Add("7");
            keyPress.Options.Add("8");
            keyPress.Options.Add("9");
            keyPress.Options.Add("0");
            keyPress.Options.Add("-");
            keyPress.Options.Add("=");
            keyPress.Options.Add("`");
            keyPress.Options.Add("[");
            keyPress.Options.Add("]");
            keyPress.Options.Add(";");
            keyPress.Options.Add("'");
            keyPress.Options.Add(",");
            keyPress.Options.Add(".");
            keyPress.Options.Add("{F1}");
            keyPress.Options.Add("{F2}");
            keyPress.Options.Add("{F3}");
            keyPress.Options.Add("{F4}");
            keyPress.Options.Add("{F5}");
            keyPress.Options.Add("{F6}");
            keyPress.Options.Add("{F7}");
            keyPress.Options.Add("{F8}");
            keyPress.Options.Add("{F9}");
            keyPress.Options.Add("{F10}");
            keyPress.Options.Add("{F11}");
            keyPress.Options.Add("{F12}");
            keyPress.Options.Add("{F13}");
            keyPress.Options.Add("{F14}");
            keyPress.Options.Add("{F15}");
            keyPress.Options.Add("{F16}");
            keyPress.Options.Add("{BACKSPACE}");
            keyPress.Options.Add("{BREAK}");
            keyPress.Options.Add("{CAPSLOCK}");
            keyPress.Options.Add("{DELETE}");
            keyPress.Options.Add("{DOWN}");
            keyPress.Options.Add("{END}");
            keyPress.Options.Add("{ENTER}");
            keyPress.Options.Add("{ESC}");
            keyPress.Options.Add("{HELP}");
            keyPress.Options.Add("{HOME}");
            keyPress.Options.Add("{INSERT}");
            keyPress.Options.Add("{LEFT}");
            keyPress.Options.Add("{NUMLOCK}");
            keyPress.Options.Add("{PGDN}");
            keyPress.Options.Add("{PGUP}");
            keyPress.Options.Add("{RIGHT}");
            keyPress.Options.Add("{SCROLLLOCK}");
            keyPress.Options.Add("{TAB}");
            keyPress.Options.Add("{UP}");
            buttonFunctions.Add(ButtonFunctionsEnum.KeyboardButtonPress, keyPress);

            return buttonFunctions;
        }

        public static void ButtonPress(DisplayManager disp, int unit, int num, bool control)
        {
            var butFuncEnum = ButtonFunctionsEnum.None;
            int newUnit = unit;
            int newNum = num;
            if (control)
            {
                for (int b = 0; b < Constants.MaxNumberJoystickButtons; b++)
                {
                    if (disp.CurrentConfiguration.ControllerConfigurations[unit].ButtonNumbers[b] == num)
                    {
                        butFuncEnum =
                            disp.CurrentConfiguration.ControllerConfigurations[unit].ButtonFunctions[b];
                        newNum = b;
                        newUnit = 0;
                    }
                }
            }
            else
            {
                butFuncEnum =
                    disp.CurrentConfiguration.DisplayConfigurations[unit].ButtonFunctions[num - 1];
            }
            switch (butFuncEnum)
            {
                case ButtonFunctionsEnum.None:
                    break;
                case ButtonFunctionsEnum.IncreaseIntensity:
                    if (disp.Intensity < Constants.MaxIntensityTM)
                    {
                        disp.Intensity++;
                        if (disp.CurrentConfiguration.TMDisplaySettings.ShowHeaders)
                        {
                            disp.ShowStringTimed(String.Format("Int {0}", disp.Intensity), disp.CurrentConfiguration.TMDisplaySettings.HeaderDisplayTime,
                                            newUnit);
                        }
                    }
                    break;
                case ButtonFunctionsEnum.DecreaseIntensity:
                    if (disp.Intensity > 0)
                    {
                        disp.Intensity--;
                        if (disp.CurrentConfiguration.TMDisplaySettings.ShowHeaders)
                        {
                            disp.ShowStringTimed(String.Format("Int {0}", disp.Intensity), disp.CurrentConfiguration.TMDisplaySettings.HeaderDisplayTime,
                                            newUnit);
                        }
                    }
                    break;
                case ButtonFunctionsEnum.NextScreen:
                    int which = control
                                    ? disp.CurrentConfiguration.ControllerConfigurations[unit].ButtonOptionsScreens[newNum]
                                    : disp.CurrentConfiguration.DisplayConfigurations[unit].ButtonOptionsScreens[num - 1];
                    if (which >= 0)
                    {
                        switch (which)
                        {
                            case 0: //All
                                for (int d = 0; d < disp.CurrentConfiguration.DisplayConfigurations.Count; d++)
                                {
                                    ChangeScreen(disp, d, true);
                                }
                                break;
                            case 1: //Current
                                ChangeScreen(disp, unit, true);
                                break;
                            default: //Specific
                                ChangeScreen(disp, which - 2, true);
                                break;
                        }
                    }
                    break;
                case ButtonFunctionsEnum.PreviousScreen:
                    int which2 = control
                                     ? disp.CurrentConfiguration.ControllerConfigurations[unit].ButtonOptionsScreens[newNum]
                                     : disp.CurrentConfiguration.DisplayConfigurations[unit].ButtonOptionsScreens[num - 1];
                    if (which2 >= 0)
                    {
                        switch (which2)
                        {
                            case 0: //All
                                for (int d = 0; d < disp.CurrentConfiguration.DisplayConfigurations.Count; d++)
                                {
                                    ChangeScreen(disp, d, false);
                                }
                                break;
                            case 1: //Current
                                ChangeScreen(disp, unit, false);
                                break;
                            default: //Specific
                                ChangeScreen(disp, which2 - 2, false);
                                break;
                        }
                    }
                    break;

                case ButtonFunctionsEnum.SpecificScreen:
                    int which4 = control
                                     ? disp.CurrentConfiguration.ControllerConfigurations[unit].ButtonOptionsScreens[newNum]
                                     : disp.CurrentConfiguration.DisplayConfigurations[unit].ButtonOptionsScreens[num - 1];
                    string screenString = control
                                              ? disp.CurrentConfiguration.ControllerConfigurations[unit].ButtonOptions[newNum
                                                    ]
                                              : disp.CurrentConfiguration.DisplayConfigurations[unit].ButtonOptions[num - 1];
                    int newScreen;
                    Int32.TryParse(screenString, out newScreen);
                    if (which4 >= 0)
                    {
                        switch (which4)
                        {
                            case 0: //All
                                for (int d = 0; d < disp.CurrentConfiguration.DisplayConfigurations.Count; d++)
                                {
                                    SelectScreen(disp, d, newScreen);
                                }
                                break;
                            case 1: //Current
                                SelectScreen(disp, unit, newScreen);
                                break;
                            default: //Specific
                                SelectScreen(disp, which4 - 2, newScreen);
                                break;
                        }
                    }
                    break;

                case ButtonFunctionsEnum.CycleLapDeltaType:
                    if (disp.CurrentDeltaType < 3) //0 to 3 for 4 types
                    {
                        disp.CurrentDeltaType++;
                    }
                    else
                    {
                        disp.CurrentDeltaType = 0;
                    }
                    DisplayQuickInfoNow(disp, "Current Selected Lap Delta Type (for Selectable Lap Delta Variable)", disp.CurrentConfiguration.TMDisplaySettings.DeltaMessageScreen);
                    break;
                case ButtonFunctionsEnum.LapDeltaLEDsSwitch:
                    if (disp.DeltaLightsOn)
                    {
                        disp.DeltaLightsOn = false;
                        if (disp.CurrentConfiguration.TMDisplaySettings.ShowHeaders)
                        {
                            disp.ShowStringTimed("dLt oFF", disp.CurrentConfiguration.TMDisplaySettings.HeaderDisplayTime, newUnit);
                        }
                    }
                    else
                    {
                        disp.DeltaLightsOn = true;
                        if (disp.CurrentConfiguration.TMDisplaySettings.ShowHeaders)
                        {
                            disp.ShowStringTimed("dLt on", disp.CurrentConfiguration.TMDisplaySettings.HeaderDisplayTime, newUnit);
                        }
                    }
                    break;

                case ButtonFunctionsEnum.DisplayQuickInfo:
                    int which3;
                    string quick;
                    if (control)
                    {
                        which3 = disp.CurrentConfiguration.ControllerConfigurations[unit].ButtonOptionsScreens[newNum];
                        if (which3 > 0)
                        {
                            which3 += 1;
                        }
                        quick = disp.CurrentConfiguration.ControllerConfigurations[unit].ButtonOptions[newNum];
                    }
                    else
                    {
                        which3 = disp.CurrentConfiguration.DisplayConfigurations[unit].ButtonOptionsScreens[num - 1];
                        quick = disp.CurrentConfiguration.DisplayConfigurations[unit].ButtonOptions[num - 1];
                    }
                    switch (which3)
                    {
                        case 0: //All
                            for (int d = 0; d < disp.CurrentConfiguration.DisplayConfigurations.Count; d++)
                            {
                                DisplayQuickInfoNow(disp,
                                    quick, d);
                                //ChangeScreen(d, false);
                            }
                            break;
                        case 1: //Current
                            DisplayQuickInfoNow(disp,
                                quick, unit);
                            //ChangeScreen(unit, false);
                            break;
                        default: //Specific
                            DisplayQuickInfoNow(disp, 
                                quick, which3 - 2);
                            //ChangeScreen(which3, false);
                            break;
                    }
                    break;
                case ButtonFunctionsEnum.TestSerialLink:
                    disp.Test = true;
                    disp.WaitTime[0] = DateTime.Now.AddSeconds(1.7);
                    disp.HostApp.TMDisplayTest();
                    break;
                case ButtonFunctionsEnum.LEDsOnOff:
                    if (disp.LEDSOn)
                    {
                        disp.LEDSOn = false;
                        if (disp.CurrentConfiguration.TMDisplaySettings.ShowHeaders)
                        {
                            disp.ShowStringTimed("LEdS oFF", disp.CurrentConfiguration.TMDisplaySettings.HeaderDisplayTime, newUnit);
                        }
                    }
                    else
                    {
                        disp.LEDSOn = true;
                        if (disp.CurrentConfiguration.TMDisplaySettings.ShowHeaders)
                        {
                            disp.ShowStringTimed("LEdS on", disp.CurrentConfiguration.TMDisplaySettings.HeaderDisplayTime, newUnit);
                        }
                    }
                    break;
                case ButtonFunctionsEnum.KeyboardButtonPress:
                    if (control)
                    {
                        if (disp.CurrentConfiguration.ControllerConfigurations[unit].ButtonOptions[newNum] != "" &&
                            disp.CurrentConfiguration.ControllerConfigurations[unit].ButtonOptions[newNum] != null)
                        {
                            SendKeys.SendWait(disp.CurrentConfiguration.ControllerConfigurations[unit].ButtonOptions[newNum]);
                        }
                    }
                    else
                    {
                        if (disp.CurrentConfiguration.DisplayConfigurations[unit].ButtonOptions[num - 1] != "" &&
                            disp.CurrentConfiguration.DisplayConfigurations[unit].ButtonOptions[num - 1] != null)
                        {
                            SendKeys.SendWait(disp.CurrentConfiguration.DisplayConfigurations[unit].ButtonOptions[num - 1]);
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Change Display Screen
        /// </summary>
        /// <param name="disp">DisplayManager Reference</param>
        /// <param name="unit">Unit Number</param>
        /// <param name="next">Whether to go to next or previous screen</param>
        private static void ChangeScreen(DisplayManager disp, int unit, bool next)
        {
            //unit = unit - 1;
            if (next)
            {
                if (disp.CurrentScreen[unit] < disp.CurrentConfiguration.DisplayConfigurations[unit].NumScreens - 1)
                {
                    disp.CurrentScreen[unit]++;
                    if (disp.CurrentConfiguration.TMDisplaySettings.ShowHeaders)
                    {
                        string header = disp.GetHeader(unit);
                        disp.ShowStringTimed(header,
                                        disp.CurrentConfiguration.TMDisplaySettings.HeaderDisplayTime, unit);
                    }
                }
            }
            else
            {
                if (disp.CurrentScreen[unit] > 0)
                {
                    disp.CurrentScreen[unit]--;
                    if (disp.CurrentConfiguration.TMDisplaySettings.ShowHeaders)
                    {
                        string header = disp.GetHeader(unit);
                        disp.ShowStringTimed(header,
                                        disp.CurrentConfiguration.TMDisplaySettings.HeaderDisplayTime, unit);
                    }
                }
            }
        }

        /// <summary>
        /// Select a specific screen on a specific unit
        /// </summary>
        /// <param name="disp">DisplayManager Reference</param>
        /// <param name="unit"></param>
        /// <param name="number">Zero-based please</param>
        private static void SelectScreen(DisplayManager disp, int unit, int number)
        {
            int newScreen;
            if (number <= 0)
            {
                //first
                newScreen = 0;
            }
            else if (number >= disp.CurrentConfiguration.DisplayConfigurations[unit].NumScreens)
            {
                //last
                newScreen = disp.CurrentConfiguration.DisplayConfigurations[unit].Screens.Count - 1;
            }
            else
            {
                //in the middle
                newScreen = number;
            }
            disp.CurrentScreen[unit] = newScreen;
            if (disp.CurrentConfiguration.TMDisplaySettings.ShowHeaders)
            {
                string header = disp.GetHeader(unit);
                disp.ShowStringTimed(header,
                                disp.CurrentConfiguration.TMDisplaySettings.HeaderDisplayTime, unit);
            }
        }

        /// <summary>
        ///     Quick info string builder.
        /// </summary>
        /// <param name="disp">DisplayManager Reference</param>
        /// <param name="option">Option string for quick info</param>
        /// <param name="unit"></param>
        private static void DisplayQuickInfoNow(DisplayManager disp, string option, int unit)
        {
            switch (option)
            {
                case "Current Time":
                    disp.ShowStringTimed(
                        String.Format(" {0}-{1}", DateTime.Now.Hour.ToString("00"), DateTime.Now.Minute.ToString("00")),
                        disp.CurrentConfiguration.TMDisplaySettings.QuickInfoDisplayTime, unit);
                    break;
                case "Position":
                    if (disp.Wrapper.IsConnected)
                    {
                        disp.ShowStringTimed(String.Format("Pos {0}", disp.SavedTelemetry.Position),
                                        disp.CurrentConfiguration.TMDisplaySettings.QuickInfoDisplayTime, unit);
                    }
                    break;
                case "Class Position":
                    if (disp.Wrapper.IsConnected)
                    {
                        disp.ShowStringTimed(String.Format("C Pos {0}", disp.SavedTelemetry.ClassPosition),
                                        disp.CurrentConfiguration.TMDisplaySettings.QuickInfoDisplayTime, unit);
                    }
                    break;
                case "Lap ### of ###":
                    if (disp.Wrapper.IsConnected)
                    {
                        disp.ShowStringTimed(
                            disp.SavedTelemetry.TotalLaps > 0
                                ? String.Format("L{0}-{1}", disp.SavedTelemetry.CurrentLap, disp.SavedTelemetry.TotalLaps)
                                : String.Format("L {0}", disp.SavedTelemetry.CurrentLap),
                            disp.CurrentConfiguration.TMDisplaySettings.QuickInfoDisplayTime, unit);
                    }
                    break;
                case "Sessions Fastest Lap":
                    if (disp.Wrapper.IsConnected)
                    {
                        if (disp.SavedTelemetry.Overallbestlap > 1)
                        {
                            TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(disp.SavedTelemetry.Overallbestlap));
                            disp.ShowStringTimed(t.ToString("m\\-ss\\.fff"), disp.CurrentConfiguration.TMDisplaySettings.QuickInfoDisplayTime, unit);
                        }
                        else
                        {
                            disp.ShowStringTimed("  none  ", disp.CurrentConfiguration.TMDisplaySettings.QuickInfoDisplayTime, unit);
                        }
                    }
                    break;
                case "Personal Best Lap":
                    if (disp.Wrapper.IsConnected)
                    {
                        if (disp.SavedTelemetry.PersonalBestLap > 1)
                        {
                            TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(disp.SavedTelemetry.PersonalBestLap));
                            disp.ShowStringTimed(t.ToString("m\\-ss\\.fff"), disp.CurrentConfiguration.TMDisplaySettings.QuickInfoDisplayTime, unit);
                        }
                        else
                        {
                            disp.ShowStringTimed("  none  ", disp.CurrentConfiguration.TMDisplaySettings.QuickInfoDisplayTime, unit);
                        }
                    }
                    break;
                case "Personal Last Lap":
                    if (disp.Wrapper.IsConnected)
                    {
                        if (disp.SavedTelemetry.LastLapTimeMeasured < 5) return;
                        disp.ShowStringTimed(
                            LapDisplays.BuildLapDisplayString(
                                LapDisplayStylesEnum.FullLapTime, disp.SavedTelemetry),
                            disp.CurrentConfiguration.TMDisplaySettings.LapDisplayTime,
                            unit);
                    }
                    break;
                case "Class Sessions Fastest Lap":
                    if (disp.Wrapper.IsConnected)
                    {
                        if (disp.SavedTelemetry.ClassBestLap > 1)
                        {
                            TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(disp.SavedTelemetry.ClassBestLap));
                            disp.ShowStringTimed(t.ToString("m\\-ss\\.fff"), disp.CurrentConfiguration.TMDisplaySettings.QuickInfoDisplayTime, unit);
                        }
                        else
                        {
                            disp.ShowStringTimed("  none  ", disp.CurrentConfiguration.TMDisplaySettings.QuickInfoDisplayTime, unit);
                        }
                    }
                    break;
                case "Fuel Percentage":
                    if (disp.Wrapper.IsConnected)
                    {
                        disp.ShowStringTimed(String.Format("FPct{0}", (disp.SavedTelemetry.CurrentFuelPCT * 100).ToString("0.0")),
                                        disp.CurrentConfiguration.TMDisplaySettings.QuickInfoDisplayTime, unit);
                    }
                    break;
                case "Session Time":
                    if (disp.Wrapper.IsConnected)
                    {
                        var tempSt = new TimeSpan(0, 0, Convert.ToInt32(disp.SavedTelemetry.CurrentSessionTime));
                        disp.ShowStringTimed(
                            String.Format("{0}-{1}.{2}", tempSt.Hours.ToString("0"), tempSt.Minutes.ToString("00"),
                                          tempSt.Seconds.ToString("00")),
                            disp.CurrentConfiguration.TMDisplaySettings.QuickInfoDisplayTime, unit);
                    }
                    break;
                case "Session Time Remaining":
                    if (disp.Wrapper.IsConnected && disp.SavedTelemetry.SessionTimeRemaining > 0)
                    {
                        var tempStr = new TimeSpan(0, 0, Convert.ToInt32(disp.SavedTelemetry.SessionTimeRemaining));
                        disp.ShowStringTimed(
                            String.Format("{0}-{1}.{2}", tempStr.Hours.ToString("0"), tempStr.Minutes.ToString("00"),
                                          tempStr.Seconds.ToString("00")),
                            disp.CurrentConfiguration.TMDisplaySettings.QuickInfoDisplayTime, unit);
                    }
                    break;
                case "Session Laps Remaining":
                    if (disp.Wrapper.IsConnected && disp.SavedTelemetry.SessionLapsRemaining > 0)
                    {
                        disp.ShowStringTimed(
                            String.Format("{0} LaPS", (disp.SavedTelemetry.SessionLapsRemaining).ToString("000")),  //why was I multiplying this value by 100?
                            disp.CurrentConfiguration.TMDisplaySettings.QuickInfoDisplayTime, unit);
                    }
                    break;
                    case "Laps of Fuel Remaining":
                    if (disp.Wrapper.IsConnected && disp.SavedTelemetry.Fuel.LapsLeft > 100)
                    {
                        disp.ShowStringTimed(
                            String.Format("{0} LaPS", (disp.SavedTelemetry.Fuel.LapsLeft).ToString("0")),
                            disp.CurrentConfiguration.TMDisplaySettings.QuickInfoDisplayTime,
                            unit);
                    }
                    else if (disp.Wrapper.IsConnected && disp.SavedTelemetry.Fuel.LapsLeft > 0.09f)
                    {
                        disp.ShowStringTimed(
                            String.Format("{0} LaPS", (disp.SavedTelemetry.Fuel.LapsLeft).ToString("0.0")),
                            disp.CurrentConfiguration.TMDisplaySettings.QuickInfoDisplayTime,
                            unit);
                    }
                    else
                    {
                        disp.ShowStringTimed("__._ LaPS", disp.CurrentConfiguration.TMDisplaySettings.QuickInfoDisplayTime, unit);
                    }
                    break;
                    case "Fuel Burn Rate (Litres/Lap)":
                    if (disp.Wrapper.IsConnected && disp.SavedTelemetry.Fuel.BurnRate > 0.09f)
                    {
                        disp.ShowStringTimed(
                            String.Format("{0} burn", (disp.SavedTelemetry.Fuel.BurnRate).ToString("0.00")),
                            disp.CurrentConfiguration.TMDisplaySettings.QuickInfoDisplayTime,
                            unit);
                    }
                    else
                    {
                        disp.ShowStringTimed("_.__ burn", disp.CurrentConfiguration.TMDisplaySettings.QuickInfoDisplayTime, unit);
                    }
                    break;
                    case "Fuel Burn Rate (Gallons/Lap)":
                    if (disp.Wrapper.IsConnected && disp.SavedTelemetry.Fuel.BurnRate > 0.09f)
                    {
                        disp.ShowStringTimed(
                            String.Format("{0} burn", (disp.SavedTelemetry.Fuel.BurnRate * 0.26417f).ToString("0.00")),
                            disp.CurrentConfiguration.TMDisplaySettings.QuickInfoDisplayTime,
                            unit);
                    }
                    else
                    {
                        disp.ShowStringTimed("_.__ burn", disp.CurrentConfiguration.TMDisplaySettings.QuickInfoDisplayTime, unit);
                    }
                    break;
                    case "Lap Delta to Best Lap":
                    if (disp.Wrapper.IsConnected && disp.SavedTelemetry.DeltaBestOK)
                    {
                        disp.ShowStringTimed(String.Format("bt {0}", disp.SavedTelemetry.DeltaBest.ToString(" 0.00;-0.00; 0.00")), disp.CurrentConfiguration.TMDisplaySettings.QuickInfoDisplayTime, unit);
                    }
                    else
                    {
                        disp.ShowStringTimed("bt  _.__", disp.CurrentConfiguration.TMDisplaySettings.QuickInfoDisplayTime, unit);
                    }
                    break;
                    case "Lap Delta to Optimal Lap":
                    if (disp.Wrapper.IsConnected && disp.SavedTelemetry.DeltaOptOK)
                    {
                        disp.ShowStringTimed(String.Format("op {0}", disp.SavedTelemetry.DeltaOpt.ToString(" 0.00;-0.00; 0.00")), disp.CurrentConfiguration.TMDisplaySettings.QuickInfoDisplayTime, unit);
                    }
                    else
                    {
                        disp.ShowStringTimed("op  _.__", disp.CurrentConfiguration.TMDisplaySettings.QuickInfoDisplayTime, unit);
                    }
                    break;
                    case "Lap Delta to Session Best Lap":
                    if (disp.Wrapper.IsConnected && disp.SavedTelemetry.DeltaSesBestOK)
                    {
                        disp.ShowStringTimed(String.Format("sb {0}", disp.SavedTelemetry.DeltaSesBest.ToString(" 0.00;-0.00; 0.00")), disp.CurrentConfiguration.TMDisplaySettings.QuickInfoDisplayTime, unit);
                    }
                    else
                    {
                        disp.ShowStringTimed("sb  _.__", disp.CurrentConfiguration.TMDisplaySettings.QuickInfoDisplayTime, unit);
                    }
                    break;
                    case "Lap Delta to Session Optimal Lap":
                    if (disp.Wrapper.IsConnected && disp.SavedTelemetry.DeltaSesOptOK)
                    {
                        disp.ShowStringTimed(String.Format("so {0}", disp.SavedTelemetry.DeltaSesOpt.ToString(" 0.00;-0.00; 0.00")), disp.CurrentConfiguration.TMDisplaySettings.QuickInfoDisplayTime, unit);
                    }
                    else
                    {
                        disp.ShowStringTimed("so  _.__", disp.CurrentConfiguration.TMDisplaySettings.QuickInfoDisplayTime, unit);
                    }
                    break;
                    case "Current Selected Lap Delta Type (for Selectable Lap Delta Variable)":
                    switch (disp.CurrentDeltaType)
                    {
                        case 0:
                            disp.ShowStringTimed("bEst", disp.CurrentConfiguration.TMDisplaySettings.QuickInfoDisplayTime, unit);
                            break;
                        case 1:
                            disp.ShowStringTimed("opt", disp.CurrentConfiguration.TMDisplaySettings.QuickInfoDisplayTime, unit);
                            break;
                        case 2:
                            disp.ShowStringTimed("ses bEst", disp.CurrentConfiguration.TMDisplaySettings.QuickInfoDisplayTime, unit);
                            break;
                        case 3:
                            disp.ShowStringTimed("ses opt", disp.CurrentConfiguration.TMDisplaySettings.QuickInfoDisplayTime, unit);
                            break;
                    }
                    break;
                case "Change in delta for last 5 seconds (for Selectable Lap Delta Variable)":
                    string result;
                    float[] array = disp.SavedTelemetry.DeltaHistory[disp.CurrentDeltaType].ToArray(); 
                    bool dataOk = true;
                    int limit = Math.Min(disp.SavedTelemetry.ExpectedDeltaHistoryLength, array.Length);
                    for (int x = 0; x < limit; x++)
                    {
                        if (array[x] > 500 - 10)
                        {
                            dataOk = false;
                        }
                    }
                    if (disp.Wrapper.IsConnected && dataOk)
                    {
                        float answer = array[0] - array[disp.SavedTelemetry.ExpectedDeltaHistoryLength - 2];
                        result = String.Format("{0}", answer.ToString(" 0.00;-0.00; 0.00"));
                    }
                    else
                    {
                        result = " _.__";
                    }
                    disp.ShowStringTimed(
                            String.Format("Ld5 {0}", result), disp.CurrentConfiguration.TMDisplaySettings.QuickInfoDisplayTime, unit);
                    break;
            }
        }

    }

}