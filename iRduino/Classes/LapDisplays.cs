//========================================//
// iRduino - Created by Mark Silverwood  //
//======================================//

namespace iRduino.Classes
{
    using System;
    using System.Collections.Generic;
    
    public enum LapDisplayStylesEnum
    {
        LapTimeDeltaPersonal,
        LapTimeDeltaOverall,
        LapTimeDeltaClassOverall,
        FullLapTime,
        DualDeltas
    }
    
    class LapDisplays
    {
        public static Dictionary<string, LapDisplayStylesEnum> LapDisplayStylesFill()
        {
            var lapDisplayStyles = new Dictionary<string, LapDisplayStylesEnum> {
                    {"Lap Time (12.34) and Personal Delta (-0.12)", LapDisplayStylesEnum.LapTimeDeltaPersonal},
                    {
                        "Lap Time (12.34) and Overall Session Best Delta (+0.12)",
                        LapDisplayStylesEnum.LapTimeDeltaOverall
                    },
                    {
                        "Lap Time (12.34) and Class Session Best Delta (+0.12)",
                        LapDisplayStylesEnum.LapTimeDeltaClassOverall
                    },
                    {"Lap Time (1.23.45)", LapDisplayStylesEnum.FullLapTime},
                    {"Personal Delta (-0.12) and Overall Delta (+0.12)", LapDisplayStylesEnum.DualDeltas}
                };
            return lapDisplayStyles;
        }

        public static string BuildLapDisplayString(LapDisplayStylesEnum lapstyle, SavedTelemetryValues savedTelemetry)
        {
            string displayString = "";
            switch (lapstyle)
            {
                case LapDisplayStylesEnum.LapTimeDeltaPersonal:
                    if (savedTelemetry.PersonalBestLap > 1)
                    {
                        displayString = String.Format(
                            "{0}{1}",
                            LapTimeParserShort(Convert.ToSingle(savedTelemetry.LastLapTimeMeasured)),
                            (savedTelemetry.LastLapTimeMeasured - savedTelemetry.PersonalBestLap).ToString(
                                " 0.00;-0.00; 0.00"));
                    }
                    else
                    {
                        displayString = String.Format(
                            "{0}{1}",
                            LapTimeParserShort(Convert.ToSingle(savedTelemetry.LastLapTimeMeasured)),
                            "nrEF");
                    }
                    break;
                case LapDisplayStylesEnum.LapTimeDeltaOverall:
                    if (savedTelemetry.Overallbestlap > 1)
                    {
                        displayString = String.Format(
                            "{0}{1}",
                            LapTimeParserShort(Convert.ToSingle(savedTelemetry.LastLapTimeMeasured)),
                            (savedTelemetry.LastLapTimeMeasured - savedTelemetry.Overallbestlap).ToString(
                                " 0.00;-0.00; 0.00"));
                    }
                    else
                    {
                        displayString = String.Format(
                            "{0}{1}",
                            LapTimeParserShort(Convert.ToSingle(savedTelemetry.LastLapTimeMeasured)),
                            "nrEF");
                    }
                    break;
                case LapDisplayStylesEnum.LapTimeDeltaClassOverall:
                    if (savedTelemetry.ClassBestLap > 1)
                    {
                        displayString = String.Format(
                            "{0}{1}",
                            LapTimeParserShort(Convert.ToSingle(savedTelemetry.LastLapTimeMeasured)),
                            (savedTelemetry.LastLapTimeMeasured - savedTelemetry.ClassBestLap).ToString(
                                " 0.00;-0.00; 0.00"));
                    }
                    else
                    {
                        displayString = String.Format(
                            "{0}{1}",
                            LapTimeParserShort(Convert.ToSingle(savedTelemetry.LastLapTimeMeasured)),
                            "nrEF");
                    }
                    break;
                case LapDisplayStylesEnum.FullLapTime:
                    displayString = String.Format(
                        "  {0}", LapTimeParser(Convert.ToSingle(savedTelemetry.LastLapTimeMeasured)));
                    break;
                case LapDisplayStylesEnum.DualDeltas: //personal overall
                    if (savedTelemetry.PersonalBestLap > 1 && savedTelemetry.Overallbestlap > 1)
                    {
                        displayString = String.Format(
                            "{0}{1}",
                            (savedTelemetry.LastLapTimeMeasured - savedTelemetry.PersonalBestLap).ToString(
                                " 0.00;-0.00; 0.00"),
                            (savedTelemetry.LastLapTimeMeasured - savedTelemetry.Overallbestlap).ToString(
                                " 0.00;-0.00; 0.00"));
                    }
                    else if (savedTelemetry.PersonalBestLap > 1)
                    {
                        //show only my personal delta
                        displayString = String.Format(
                            "{0}{1}",
                            (savedTelemetry.LastLapTimeMeasured - savedTelemetry.PersonalBestLap).ToString(
                                " 0.00;-0.00; 0.00"),
                            "nrEF");
                    }
                    else if (savedTelemetry.Overallbestlap > 1)
                    {
                        // show only overall delta
                        displayString = String.Format(
                            "{0}{1}",
                            "nrEF",
                            (savedTelemetry.LastLapTimeMeasured - savedTelemetry.Overallbestlap).ToString(
                                " 0.00;-0.00; 0.00"));
                    }
                    break;
            }
            return displayString;
        }

        /// <summary>
        ///     Converts lap time float into a string - long format (1.12.34)
        /// </summary>
        /// <param name="time">Float - Time in seconds</param>
        /// <returns>Formatted String</returns>
        private static string LapTimeParser(float time)
        {
            TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(time));
            return t.ToString("m\\.ss\\.fff");
        }

        /// <summary>
        ///     Converts lap time float into a string - short format (12.34)
        /// </summary>
        /// <param name="time">Float - Time in seconds</param>
        /// <returns>Formatted String</returns>
        private static string LapTimeParserShort(float time)
        {
            TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(time));
            return t.ToString("ss\\.ff");
        }
    }
}
