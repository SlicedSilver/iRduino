//========================================//
// iRduino - Created by Mark Silverwood  //
//======================================//

namespace iRduino.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using iRacingSdkWrapper;

    public enum DisplayVarsEnum
    {
        Speed,
        SpeedMiles,
        Gear,
        RPM5,
        RPM4,
        RPM3,
        Throttle,
        Brake,
        Clutch,
        RaceLaps3,
        RaceLaps2,
        Laps3,
        Laps2,
        CurrentLapTime,
        SelectableLapDelta,
        LapDeltaBest,
        LapDeltaSessionBest,
        LapDeltaOptimal,
        LapDeltaSessionOptimal,
        ForceFeedBackPCT,
        FuelLitres,
        FuelGallons,
        FuelPCT,
        FuelBurnRate,
        FuelBurnRateGallons,
        FuelLapsLeft,
        LongG,
        LatG,
        TotalG,
        Position,
        FuelPressure,
        FuelPressurePSi,
        Pitch,
        PitchRate,
        Roll,
        RollRate,
        SessionLapsRemaining,
        SessionTime,
        SessionTimeRemaining,
        SteeringWheelAngle,
        EngineVoltage,
        EngineWaterLevel,
        EngineWaterLevelGallons,
        EngineWaterTemp,
        EngineWaterTempFahrenheit,
        OilTemp,
        OilTempFahrenheit,
        OilPressure,
        OilPressurePSi,
        OilLevel,
        OilLevelGallons,
        ManifoldPressure,
        ManifoldPressurePSi,
        Yaw,
        YawRate,
        Space,
        DoubleSpace,
        Underscore
    }

    public class DisplayVariablesClass
    {
        public static Dictionary<DisplayVarsEnum, DisplayVariable> FillDisplayVariables()
        {
            var displayVariables = new Dictionary<DisplayVarsEnum, DisplayVariable> {
                    {DisplayVarsEnum.Space, new DisplayVariable("Space", 1, " ", " ")},
                    {DisplayVarsEnum.DoubleSpace, new DisplayVariable("Double Space", 2, "  ", "  ")},
                    {DisplayVarsEnum.Underscore, new DisplayVariable("Underscore", 1, "_", "_")},
                    {DisplayVarsEnum.Speed, new DisplayVariable("Speed", 3, "SPd", "148")},
                    {DisplayVarsEnum.SpeedMiles, new DisplayVariable("Speed Miles/Hour", 3, "SPd", "102")},
                    {DisplayVarsEnum.Gear, new DisplayVariable("Gear", 1, "g", "3")},
                    {DisplayVarsEnum.RPM5, new DisplayVariable("RPM 5 Digits", 5, "rPm  ", "16842")},
                    {DisplayVarsEnum.RPM4, new DisplayVariable("RPM 4 Digits", 4, "rPm ", "7685")},
                    {DisplayVarsEnum.RPM3, new DisplayVariable("RPM 3 Digits", 3, "rPm", "16.5")},
                    {DisplayVarsEnum.Throttle, new DisplayVariable("Throttle", 3, "thr", "100")},
                    {DisplayVarsEnum.Brake, new DisplayVariable("Brake", 3, "brk", "100")},
                    {DisplayVarsEnum.Clutch, new DisplayVariable("Clutch", 3, "clt", "100")},
                    {DisplayVarsEnum.RaceLaps3, new DisplayVariable("Race Laps 3 Digits", 3, "rlp", "123")},
                    {DisplayVarsEnum.RaceLaps2, new DisplayVariable("Race Laps 2 Digits", 2, "rl", "43")},
                    {DisplayVarsEnum.Laps3, new DisplayVariable("Lap 3 Digits", 3, "Lap", "123")},
                    {DisplayVarsEnum.Laps2, new DisplayVariable("Lap 2 Digits", 2, "Lp", "43")},

                    {DisplayVarsEnum.CurrentLapTime, new DisplayVariable("Current Lap Time", 6, "crt lt", "1-12.43")},
                    {DisplayVarsEnum.SelectableLapDelta, new DisplayVariable("Selectable Lap Delta Time", 3, "SLdt", "-.43")},
                    {DisplayVarsEnum.LapDeltaBest, new DisplayVariable("Lap Delta (Best)", 3, "ldtb", "-.43")},
                    {DisplayVarsEnum.LapDeltaOptimal, new DisplayVariable("Lap Delta (Optimal)", 3, "ldto", "-.43")},
                    {DisplayVarsEnum.LapDeltaSessionBest, new DisplayVariable("Lap Delta (Session Best)", 3, "ldsb", "-.43")},
                    {DisplayVarsEnum.LapDeltaSessionOptimal, new DisplayVariable("Lap Delta (Session Optimal)", 3, "ldso", "-.43")},

                    {DisplayVarsEnum.ForceFeedBackPCT, new DisplayVariable("Force Feedback %", 3, "FFb", "100")},
                    {DisplayVarsEnum.FuelLitres, new DisplayVariable("Fuel Level Litres", 3, "Flv", "12.5")},
                    {DisplayVarsEnum.FuelGallons, new DisplayVariable("Fuel Level Gallons (US)", 3, "Flv", "12.5")},
                    {DisplayVarsEnum.FuelPCT, new DisplayVariable("Fuel Percentage", 3, "Fp ", "45.7")},
                    {DisplayVarsEnum.FuelBurnRate, new DisplayVariable("Fuel Burn Rate (Litres / Lap)", 3, "brn", "1.25")},
                    {DisplayVarsEnum.FuelBurnRateGallons, new DisplayVariable("Fuel Burn Rate (Gallons (US) / Lap)", 3, "brn", "1.25")},
                    {DisplayVarsEnum.FuelLapsLeft, new DisplayVariable("Laps Left on Fuel", 3, "Flp", "15.7")},
                    {DisplayVarsEnum.LatG, new DisplayVariable("Lateral G", 3, "lat", "-1.8")},
                    {DisplayVarsEnum.LongG, new DisplayVariable("Longitudinal G", 3, "lon", "-1.4")},
                    {DisplayVarsEnum.TotalG, new DisplayVariable("Total G", 3, "tot", "-2.8")},
                    {DisplayVarsEnum.Position, new DisplayVariable("Position", 2, "Po", "31")},
                    {DisplayVarsEnum.FuelPressure, new DisplayVariable("Fuel Pressure (bar)", 3, "FPr", "10.8")},
                    {DisplayVarsEnum.FuelPressurePSi, new DisplayVariable("Fuel Pressure (PSi)", 3, "FPr", "10.8")},
                    {DisplayVarsEnum.Pitch, new DisplayVariable("Pitch (degrees)", 3, "Ptc", "-2.8")},
                    {DisplayVarsEnum.PitchRate, new DisplayVariable("Pitch Rate (degrees/s)", 3, "Ptr", "-1.5")},
                    {DisplayVarsEnum.Roll, new DisplayVariable("Roll (degrees)", 3, "roL", "-1.0")},
                    {DisplayVarsEnum.RollRate, new DisplayVariable("Roll Rate (degrees/s)", 3, "tot", "-1.2")},
                    {
                        DisplayVarsEnum.SessionLapsRemaining,
                        new DisplayVariable("Session Laps Remaining", 3, "SLr", "109")
                    },
                    {DisplayVarsEnum.SessionTime, new DisplayVariable("Session Time", 6, "SES-T", "0-12.34")},
                    {
                        DisplayVarsEnum.SessionTimeRemaining,
                        new DisplayVariable("Session Time Remaining", 6, "SEStr", "1-15.34")
                    },
                    {
                        DisplayVarsEnum.SteeringWheelAngle,
                        new DisplayVariable("Steering Wheel Angle (degrees)", 4, "Stan", "-145")
                    },
                    {DisplayVarsEnum.EngineVoltage, new DisplayVariable("Engine Voltage", 3, "vlt", "10.5")},
                    {DisplayVarsEnum.EngineWaterLevel, new DisplayVariable("Engine Water Level (Litres)", 2, "wL", "5.8")},
                    {DisplayVarsEnum.EngineWaterLevelGallons, new DisplayVariable("Engine Water Level (Gallons US)", 2, "wL", "5.8")},                    
                    {DisplayVarsEnum.EngineWaterTemp, new DisplayVariable("Water Temp (C)", 3, "wtP", "102")},
                    {DisplayVarsEnum.EngineWaterTempFahrenheit, new DisplayVariable("Water Temp (F)", 3, "wtP", "102")},
                    {DisplayVarsEnum.OilLevel, new DisplayVariable("Oil Level (litres)", 2, "oLL", "1.2")},
                    {DisplayVarsEnum.OilLevelGallons, new DisplayVariable("Oil Level (Gallons US)", 2, "oLL", "1.2")},
                    {DisplayVarsEnum.OilPressure, new DisplayVariable("Oil Pressure (bar)", 2, "oLP", "1.2")},
                    {DisplayVarsEnum.OilPressurePSi, new DisplayVariable("Oil Pressure (PSi)", 2, "oLP", "1.2")},
                    {DisplayVarsEnum.OilTemp, new DisplayVariable("Oil Temp (C)", 3, "oLt", "102")},
                    {DisplayVarsEnum.OilTempFahrenheit, new DisplayVariable("Oil Temp (F)", 3, "oLt", "102")}, 
                    {DisplayVarsEnum.ManifoldPressure, new DisplayVariable("Manifold Pressure (bar)", 3, "EPr", " 1.2")},                   
                    {DisplayVarsEnum.ManifoldPressurePSi, new DisplayVariable("Manifold Pressure (PSi)", 3, "EPr", " 1.2")},
                    {DisplayVarsEnum.Yaw, new DisplayVariable("Yaw (degrees)", 3, "yaw", "-10")},
                    {DisplayVarsEnum.YawRate, new DisplayVariable("Yaw Rate (degrees/s)", 3, "ywr", "-13")}
                };
            return displayVariables;
        }

        public static string BuildDisplayString(SdkWrapper.TelemetryUpdatedEventArgs e, List<string> variables, Dictionarys dictionarys, SavedTelemetryValues savedTelemetry, int currentDeltaType)
        {
            var displayString = "";
            foreach (string dvString in variables)
            {
                DisplayVarsEnum dv;
                if (!Enum.TryParse(dvString, out dv)) continue;
                switch (dv)
                {
                    case DisplayVarsEnum.Speed:
                        float speedtemp = e.TelemetryInfo.Speed.Value * 3.6f;
                        if (speedtemp > 1)
                        {
                            displayString += RightJustify((e.TelemetryInfo.Speed.Value * 3.6f).ToString("0"),
                                                          dictionarys.DisplayVariables[dv].Length);
                        }
                        else
                        {
                            displayString += RightJustify("0", dictionarys.DisplayVariables[dv].Length);
                        }
                        break;
                    case DisplayVarsEnum.SpeedMiles:
                        float speedtemp2 = e.TelemetryInfo.Speed.Value * 2.236f;
                        if (speedtemp2 > 1)
                        {
                            displayString += RightJustify((e.TelemetryInfo.Speed.Value * 2.236f).ToString("0"),
                                                          dictionarys.DisplayVariables[dv].Length);
                        }
                        else
                        {
                            displayString += RightJustify("0", dictionarys.DisplayVariables[dv].Length);
                        }
                        break;
                    case DisplayVarsEnum.Gear:
                        if (e.TelemetryInfo.Gear.Value > 0)
                        {
                            displayString += e.TelemetryInfo.Gear.Value.ToString(CultureInfo.InvariantCulture);
                        }
                        else if (e.TelemetryInfo.Gear.Value == 0)
                        {
                            displayString += "n";
                        }
                        else
                        {
                            displayString += "r";
                        }
                        break;
                    case DisplayVarsEnum.RPM5:
                        displayString += RightJustify(e.TelemetryInfo.RPM.Value.ToString("0"),
                                                      dictionarys.DisplayVariables[dv].Length);
                        break;
                    case DisplayVarsEnum.RPM4:
                        if (e.TelemetryInfo.RPM.Value >= 10000)
                        {
                            displayString += RightJustify((e.TelemetryInfo.RPM.Value / 1000f).ToString("0.00"),
                                                          dictionarys.DisplayVariables[dv].Length + 1);
                        }
                        else
                        {
                            displayString += RightJustify(e.TelemetryInfo.RPM.Value.ToString("0"),
                                                          dictionarys.DisplayVariables[dv].Length);
                        }
                        break;
                    case DisplayVarsEnum.RPM3:
                        if (e.TelemetryInfo.RPM.Value >= 10000)
                        {
                            displayString += RightJustify((e.TelemetryInfo.RPM.Value / 1000f).ToString("0.0"),
                                                          dictionarys.DisplayVariables[dv].Length + 1);
                        }
                        else
                        {
                            displayString += RightJustify((e.TelemetryInfo.RPM.Value / 1000f).ToString("0.00"),
                                                          dictionarys.DisplayVariables[dv].Length + 1);
                        }
                        break;
                    case DisplayVarsEnum.Throttle:
                        displayString += RightJustify((e.TelemetryInfo.Throttle.Value * 100).ToString("0"),
                                                      dictionarys.DisplayVariables[dv].Length);
                        break;
                    case DisplayVarsEnum.Brake:
                        displayString += RightJustify((e.TelemetryInfo.Brake.Value * 100).ToString("0"),
                                                      dictionarys.DisplayVariables[dv].Length);
                        break;
                    case DisplayVarsEnum.Clutch:
                        displayString += RightJustify((e.TelemetryInfo.Clutch.Value * 100).ToString("0"),
                                                      dictionarys.DisplayVariables[dv].Length);
                        break;
                    case DisplayVarsEnum.RaceLaps3:
                        displayString +=
                            RightJustify(e.TelemetryInfo.RaceLaps.Value.ToString(CultureInfo.InvariantCulture),
                                         dictionarys.DisplayVariables[dv].Length);
                        break;
                    case DisplayVarsEnum.RaceLaps2:
                        displayString +=
                            RightJustify(e.TelemetryInfo.RaceLaps.Value.ToString(CultureInfo.InvariantCulture),
                                         dictionarys.DisplayVariables[dv].Length);
                        break;
                    case DisplayVarsEnum.Laps3:
                        displayString +=
                            RightJustify(e.TelemetryInfo.Lap.Value.ToString(CultureInfo.InvariantCulture),
                                         dictionarys.DisplayVariables[dv].Length);
                        break;
                    case DisplayVarsEnum.Laps2:
                        displayString +=
                            RightJustify(e.TelemetryInfo.Lap.Value.ToString(CultureInfo.InvariantCulture),
                                         dictionarys.DisplayVariables[dv].Length);
                        break;

                    case DisplayVarsEnum.CurrentLapTime:
                        if (e.TelemetryInfo.LapCurrentLapTime.Value > 1)
                        {
                            TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(e.TelemetryInfo.LapCurrentLapTime.Value));
                            displayString += t.ToString("m\\-ss\\.ff");
                        }
                        else
                        {
                            displayString += "_-__.__";
                        }
                        break;
                    case DisplayVarsEnum.SelectableLapDelta:
                        switch (currentDeltaType)
                        {
                            case 0:
                                if (e.TelemetryInfo.LapDeltaToBestLap_OK.Value)
                                {
                                    displayString += DeltaDisplay(e.TelemetryInfo.LapDeltaToBestLap.Value);
                                }
                                else
                                {
                                    displayString += " _._";
                                }
                                break;
                            case 1:
                                if (e.TelemetryInfo.LapDeltaToOptimalLap_OK.Value)
                                {
                                    displayString += DeltaDisplay(e.TelemetryInfo.LapDeltaToOptimalLap.Value);
                                }
                                else
                                {
                                    displayString += " _._";
                                }

                                break;
                            case 2:
                                if (e.TelemetryInfo.LapDeltaToSessionBestLap_OK.Value)
                                {
                                    displayString += DeltaDisplay(e.TelemetryInfo.LapDeltaToSessionBestLap.Value);
                                }
                                else
                                {
                                    displayString += " _._";
                                }

                                break;
                            case 3:
                                if (e.TelemetryInfo.LapDeltaToSessionOptimalLap_OK.Value)
                                {
                                    displayString += DeltaDisplay(e.TelemetryInfo.LapDeltaToSessionOptimalLap.Value);
                                }
                                else
                                {
                                    displayString += " _._";
                                }
                                break;
                            default:
                                displayString += " _._";
                                break;
                        }
                        break;
                    case DisplayVarsEnum.LapDeltaBest:
                        if (e.TelemetryInfo.LapDeltaToBestLap_OK.Value)
                        {
                            displayString += DeltaDisplay(e.TelemetryInfo.LapDeltaToBestLap.Value);
                        }
                        else
                        {
                            displayString += " _._";
                        }
                        break;
                    case DisplayVarsEnum.LapDeltaOptimal:
                        if (e.TelemetryInfo.LapDeltaToOptimalLap_OK.Value)
                        {
                            displayString += DeltaDisplay(e.TelemetryInfo.LapDeltaToOptimalLap.Value);
                        }
                        else
                        {
                            displayString += " _._";
                        }
                        break;
                    case DisplayVarsEnum.LapDeltaSessionBest:
                        if (e.TelemetryInfo.LapDeltaToSessionBestLap_OK.Value)
                        {
                            displayString += DeltaDisplay(e.TelemetryInfo.LapDeltaToSessionBestLap.Value);
                        }
                        else
                        {
                            displayString += " _._";
                        }
                        break;
                    case DisplayVarsEnum.LapDeltaSessionOptimal:
                        if (e.TelemetryInfo.LapDeltaToSessionOptimalLap_OK.Value)
                        {
                            displayString += DeltaDisplay(e.TelemetryInfo.LapDeltaToSessionOptimalLap.Value);
                        }
                        else
                        {
                            displayString += " _._";
                        }
                        break;


                    case DisplayVarsEnum.ForceFeedBackPCT:
                        displayString += RightJustify(
                            (e.TelemetryInfo.SteeringWheelPctTorque.Value * 100).ToString("0"),
                            dictionarys.DisplayVariables[dv].Length);
                        break;
                    case DisplayVarsEnum.FuelLitres:
                        if (e.TelemetryInfo.FuelLevel.Value < 100)
                        {
                            displayString += RightJustify(e.TelemetryInfo.FuelLevel.Value.ToString("0.0"),
                                                          dictionarys.DisplayVariables[dv].Length + 1);
                        }
                        else
                        {
                            displayString += RightJustify(e.TelemetryInfo.FuelLevel.Value.ToString("0"),
                                                          dictionarys.DisplayVariables[dv].Length);
                        }
                        break;
                    case DisplayVarsEnum.FuelGallons:
                        float fuelGallons = e.TelemetryInfo.FuelLevel.Value * 0.26417f;
                        if (fuelGallons < 100)
                        {
                            displayString += RightJustify(fuelGallons.ToString("0.0"),
                                                          dictionarys.DisplayVariables[dv].Length + 1);
                        }
                        else
                        {
                            displayString += RightJustify(fuelGallons.ToString("0"),
                                                          dictionarys.DisplayVariables[dv].Length);
                        }
                        break;
                    case DisplayVarsEnum.FuelPCT:
                        if (e.TelemetryInfo.FuelLevelPct.Value >= 100)
                        {
                            displayString += "100";
                        }
                        else
                        {
                            displayString += RightJustify((e.TelemetryInfo.FuelLevelPct.Value * 100).ToString("0.0"),
                                                          dictionarys.DisplayVariables[dv].Length + 1);
                        }
                        break;
                    case DisplayVarsEnum.FuelBurnRate:
                        if (savedTelemetry.Fuel.BurnRate >= 0.01f)
                        {
                            displayString += RightJustify(savedTelemetry.Fuel.BurnRate.ToString("0.00"), dictionarys.DisplayVariables[dv].Length + 1);
                        }
                        else
                        {
                            displayString += "_.__";
                        }
                        break;
                    case DisplayVarsEnum.FuelBurnRateGallons:
                        //System.Diagnostics.Debug.WriteLine(String.Format("FuelBurnGallons"));
                        float burnRateGallons = savedTelemetry.Fuel.BurnRate * 0.26417f;
                        //System.Diagnostics.Debug.WriteLine(String.Format("          Value: {0}",burnRateGallons));
                        if (burnRateGallons >= 0.01f)
                        {
                            displayString += RightJustify(burnRateGallons.ToString("0.00"), dictionarys.DisplayVariables[dv].Length + 1);
                        }
                        else
                        {
                            displayString += "_.__";
                        }
                        break;
                    case DisplayVarsEnum.FuelLapsLeft:
                        if (savedTelemetry.Fuel.LapsLeft > 100)
                        {
                            displayString += RightJustify(savedTelemetry.Fuel.LapsLeft.ToString("0"), dictionarys.DisplayVariables[dv].Length);
                        }
                        else if (savedTelemetry.Fuel.LapsLeft >= 0.1f)
                        {
                            displayString += RightJustify(savedTelemetry.Fuel.LapsLeft.ToString("0.0"), dictionarys.DisplayVariables[dv].Length + 1);
                        }
                        else
                        {
                            displayString += "_._";
                        }
                        break;
                    case DisplayVarsEnum.LongG:
                        displayString += RightJustify(
                            (e.TelemetryInfo.LongAccel.Value / 10).ToString(" 0.0;-0.0; 0.0"),
                            dictionarys.DisplayVariables[dv].Length + 1);
                        break;
                    case DisplayVarsEnum.LatG:
                        displayString += RightJustify(
                            (e.TelemetryInfo.LatAccel.Value / 10).ToString(" 0.0;-0.0; 0.0"),
                            dictionarys.DisplayVariables[dv].Length + 1);
                        break;
                    case DisplayVarsEnum.TotalG:
                        displayString +=
                            RightJustify(
                                (Math.Sqrt(e.TelemetryInfo.LongAccel.Value * e.TelemetryInfo.LongAccel.Value +
                                           e.TelemetryInfo.LatAccel.Value * e.TelemetryInfo.LatAccel.Value) / 10)
                                    .ToString(" 0.0;-0.0; 0.0"), dictionarys.DisplayVariables[dv].Length + 1);
                        break;
                    case DisplayVarsEnum.Position:
                        displayString +=
                            RightJustify(savedTelemetry.Position.ToString(CultureInfo.InvariantCulture),
                                         dictionarys.DisplayVariables[dv].Length);
                        break;
                    case DisplayVarsEnum.Space:
                        displayString += " ";
                        break;
                    case DisplayVarsEnum.DoubleSpace:
                        displayString += "  ";
                        break;
                    case DisplayVarsEnum.Underscore:
                        displayString += "_";
                        break;
                    case DisplayVarsEnum.FuelPressure:
                        displayString += RightJustify(
                            (e.TelemetryInfo.FuelPress.Value).ToString(" 0.0;-0.0; 0.0"),
                            dictionarys.DisplayVariables[dv].Length + 1);
                        break;
                    case DisplayVarsEnum.FuelPressurePSi:
                        displayString += RightJustify(
                            (e.TelemetryInfo.FuelPress.Value * 14.5f).ToString(" 0.0;-0.0; 0.0"),
                            dictionarys.DisplayVariables[dv].Length + 1);
                        break;
                    case DisplayVarsEnum.ManifoldPressure:
                        displayString += RightJustify(
                            (e.TelemetryInfo.ManifoldPressure.Value).ToString(" 0.0;-0.0; 0.0"),
                            dictionarys.DisplayVariables[dv].Length + 1);
                        break;
                    case DisplayVarsEnum.ManifoldPressurePSi:
                        displayString += RightJustify(
                            (e.TelemetryInfo.ManifoldPressure.Value * 14.5f).ToString(" 0.0;-0.0; 0.0"),
                            dictionarys.DisplayVariables[dv].Length + 1);
                        break;
                    case DisplayVarsEnum.OilLevel:
                        displayString += RightJustify(
                            (e.TelemetryInfo.OilLevel.Value).ToString("0.0"),
                            dictionarys.DisplayVariables[dv].Length + 1);
                        break;
                    case DisplayVarsEnum.OilPressure:
                        displayString += RightJustify(
                            (e.TelemetryInfo.OilPress.Value).ToString("0.0"),
                            dictionarys.DisplayVariables[dv].Length + 1);
                        break;
                    case DisplayVarsEnum.OilTemp:
                        displayString += RightJustify(
                            (e.TelemetryInfo.OilTemp.Value).ToString("000"),
                            dictionarys.DisplayVariables[dv].Length);
                        break;
                    case DisplayVarsEnum.OilLevelGallons:
                        displayString += RightJustify(
                            (e.TelemetryInfo.OilLevel.Value * 0.26417f).ToString("0.0"),
                            dictionarys.DisplayVariables[dv].Length + 1);
                        break;
                    case DisplayVarsEnum.OilPressurePSi:
                        displayString += RightJustify(
                            (e.TelemetryInfo.OilPress.Value * 14.5f).ToString("0.0"),
                            dictionarys.DisplayVariables[dv].Length + 1);
                        break;
                    case DisplayVarsEnum.OilTempFahrenheit:
                        displayString += RightJustify(
                            ((e.TelemetryInfo.OilTemp.Value) * 9f/5f + 32).ToString("000"),
                            dictionarys.DisplayVariables[dv].Length);
                        break;
                    case DisplayVarsEnum.Pitch:
                        displayString += RightJustify(
                            (e.TelemetryInfo.Pitch.Value * 57.2957795).ToString(" 0.0;-0.0; 0.0"),
                            dictionarys.DisplayVariables[dv].Length + 1);
                        break;
                    case DisplayVarsEnum.PitchRate:
                        displayString += RightJustify(
                            (e.TelemetryInfo.PitchRate.Value * 57.2957795).ToString(" 0.0;-0.0; 0.0"),
                            dictionarys.DisplayVariables[dv].Length + 1);
                        break;
                    case DisplayVarsEnum.Roll:
                        displayString += RightJustify(
                            (e.TelemetryInfo.Roll.Value * 57.2957795).ToString(" 0.0;-0.0; 0.0"),
                            dictionarys.DisplayVariables[dv].Length + 1);
                        break;
                    case DisplayVarsEnum.RollRate:
                        displayString += RightJustify(
                            (e.TelemetryInfo.RollRate.Value * 57.2957795).ToString(" 0.0;-0.0; 0.0"),
                            dictionarys.DisplayVariables[dv].Length + 1);
                        break;
                    case DisplayVarsEnum.Yaw:
                        displayString += RightJustify(
                            (e.TelemetryInfo.Yaw.Value * 57.2957795).ToString(" 00;-00; 00"),
                            dictionarys.DisplayVariables[dv].Length);
                        break;
                    case DisplayVarsEnum.YawRate:
                        displayString += RightJustify(
                            (e.TelemetryInfo.YawRate.Value * 57.2957795).ToString(" 00;-00; 00"),
                            dictionarys.DisplayVariables[dv].Length);
                        break;
                    case DisplayVarsEnum.SteeringWheelAngle:
                        displayString += RightJustify(
                            (e.TelemetryInfo.SteeringWheelAngle.Value * 57.2957795).ToString(" 000;-000; 000"),
                            dictionarys.DisplayVariables[dv].Length);
                        break;
                    case DisplayVarsEnum.EngineVoltage:
                        displayString += RightJustify(
                            (e.TelemetryInfo.Voltage.Value).ToString("00.0"),
                            dictionarys.DisplayVariables[dv].Length + 1);
                        break;
                    case DisplayVarsEnum.EngineWaterLevel:
                        displayString += RightJustify(
                            (e.TelemetryInfo.WaterLevel.Value).ToString("0.0"),
                            dictionarys.DisplayVariables[dv].Length + 1);
                        break;
                    case DisplayVarsEnum.EngineWaterTemp:
                        displayString += RightJustify(
                            (e.TelemetryInfo.WaterTemp.Value).ToString("000"),
                            dictionarys.DisplayVariables[dv].Length);
                        break;
                    case DisplayVarsEnum.EngineWaterLevelGallons:
                        displayString += RightJustify(
                            (e.TelemetryInfo.WaterLevel.Value * 0.26417).ToString("0.0"),
                            dictionarys.DisplayVariables[dv].Length + 1);
                        break;
                    case DisplayVarsEnum.EngineWaterTempFahrenheit:
                        displayString += RightJustify(
                            ((e.TelemetryInfo.WaterTemp.Value) * 9f/5f + 32).ToString("000"),
                            dictionarys.DisplayVariables[dv].Length);
                        break;
                    case DisplayVarsEnum.SessionLapsRemaining:
                        if (e.TelemetryInfo.SessionLapsRemaining != null)
                        {
                            displayString += RightJustify(
                                (e.TelemetryInfo.SessionLapsRemaining.Value).ToString("000"),
                                dictionarys.DisplayVariables[dv].Length);
                        }
                        break;
                    case DisplayVarsEnum.SessionTime:
                        if (e.TelemetryInfo.SessionTime != null)
                        {
                            var tempST = new TimeSpan(0, 0, Convert.ToInt32(e.TelemetryInfo.SessionTime.Value));
                            displayString += RightJustify(
                                String.Format("{0}-{1}.{2}", tempST.Hours.ToString("0"),
                                              tempST.Minutes.ToString("00"), tempST.Seconds.ToString("00")),
                                dictionarys.DisplayVariables[dv].Length + 1);
                        }
                        break;
                    case DisplayVarsEnum.SessionTimeRemaining:
                        if (e.TelemetryInfo.SessionTimeRemaining != null)
                        {
                            var tempStr = new TimeSpan(0, 0,
                                                       Convert.ToInt32(e.TelemetryInfo.SessionTimeRemaining.Value));
                            displayString += RightJustify(
                                String.Format("{0}-{1}.{2}", tempStr.Hours.ToString("0"),
                                              tempStr.Minutes.ToString("00"), tempStr.Seconds.ToString("00")),
                                dictionarys.DisplayVariables[dv].Length + 1);
                        }
                        break;
                }
            }
            return displayString;
        }

        private static string DeltaDisplay(float dtime)
        {
            string final;
            string display = dtime.ToString(" 0.00;-0.00; 0.00");
            if (dtime <= 0.99f && dtime >= -0.99f)
            {
                display = dtime.ToString(" 0.00;-0.00; 0.00");
                final = display[0] + display.Substring(2, 3);
            }
            else
            {
                final = display.Substring(0, 4);
            }
            return final;
        }

        /// <summary>
        ///     Reformats a string so that it is right justified to a certain length.
        /// </summary>
        /// <param name="input">Input String</param>
        /// <param name="num">Length of required string to be outputted</param>
        /// <returns>Right justified String</returns>
        private static string RightJustify(string input, int num)
        {
            if (input.Length < num)
            {
                string temp = input;
                while (temp.Length < num)
                {
                    temp = " " + temp;
                }
                return temp;
            }
            return input.Length > num ? input.Substring(input.Length - num, num) : input;
        }
    }
    
    public class DisplayVariable
    {
        public string DisplayName;
        public string Example;
        public int Length;
        public string Name;

        public DisplayVariable(string name, int length, string displayName, string example) //, string _uniqueID)
        {
            Name = name;
            Length = length;
            DisplayName = displayName;
            Example = example;
        }
    }

    public class Screen
    {
        public Screen()
        {
            Variables = new List<string>();
            CustomHeader = "";
            Example = "";
        }

        public bool UseCustomHeader { get; set; }

        public string Example { get; set; }

        public string CustomHeader { get; set; }

        public List<string> Variables { get; set; }
    }

}
