//========================================//
// iRduino - Created by Mark Silverwood  //
//======================================//

namespace iRduino.Classes
{
    using System;
    using System.Collections.Generic;

    public enum ShiftStyleEnum
    {
        GreenProgressive,
        RedProgressive,
        GreenRedProgressive,
        GreenRedProgressiveRedShift,
        GreenThenRedProgressive,
        GreenProgressiveRedShift,
        GreenProgressiveFirstHalf,
        RedProgressiveFirstHalf,
        GreenRedProgressiveFirstHalf,
        GreenRedProgressiveRedShiftFirstHalf,
        GreenThenRedProgressiveFirstHalf,
        GreenProgressiveRedShiftFirstHalf,
        GreenProgressiveSecondHalf,
        RedProgressiveSecondHalf,
        GreenRedProgressiveSecondHalf,
        GreenRedProgressiveRedShiftSecondHalf,
        GreenThenRedProgressiveSecondHalf,
        GreenProgressiveRedShiftSecondHalf,
        GreenConverging,
        RedConverging,
        GreenRedConverging,
        GreenConvergingRedShift,
        GreenRedConvergingRedShift,
        GreenConvergingFirstHalf,
        RedConvergingFirstHalf,
        GreenRedConvergingFirstHalf,
        GreenConvergingRedShiftFirstHalf,
        GreenRedConvergingRedShiftFirstHalf,
        GreenConvergingSecondHalf,
        RedConvergingSecondHalf,
        GreenRedConvergingSecondHalf,
        GreenConvergingRedShiftSecondHalf,
        GreenRedConvergingRedShiftSecondHalf,
        BasicShiftLight,
        None
    }

    public enum PitFlashStyleEnum
    {
        GreenFlash,
        RedFlash,
        GreenThenRedFlash,
        GreenRedAlternateFlash,
        TwinGreenRedAlternateFlash
    }

    public enum PitFlashSpeedsEnum
    {
        Full,
        Half,
        Quarter,
        Eighth
    }

    public enum RevFlashStyleEnum
    {
        StayRed,
        FlashRed
    }

    public enum MatchCarShiftOptions
    {
        Single,
        DualLeft,
        DualRight
    }

    public enum DeltaLightsOptions
    {
        Single,
        DualLeft,
        DualRight
    }

    class LEDs
    {
        /// <summary>
        /// Gets the LED bytes for a Quick Info Light
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="min">Mininum</param>
        /// <param name="max">Maximum</param>
        /// <returns>byte[0] = green, byte[1] = red</returns>
        public static byte[] QuickInfoLEDs(double value, double min, double max)
        {
            if (value < min)
            {
                return new byte[] { 0, 1 };
            }
            if (value > max)
            {
                return new byte[] { 255, 0 };
            }
            List<byte> ledScaleLeftToRight = new List<byte> { 1, 3, 7, 15, 31, 63, 127, 255 };
            double pos = (value / (max - min)) * 8;
            int posInt = Convert.ToInt32(Math.Round(pos));
            return posInt == 0 ? new byte[] { 0 , 1 } : new byte[] {ledScaleLeftToRight[posInt] , 0 };
        }

        /// <summary>
        /// Shift Light Styles
        /// </summary>
        /// <returns>A Dictionary used in the Dictionarys class</returns>
        public static Dictionary<ShiftStyleEnum, Rpmstyle> FillRPMStyles()
        {
            var rpmStyles = new Dictionary<ShiftStyleEnum, Rpmstyle>
                {
                    {
                        ShiftStyleEnum.GreenProgressive,
                        new Rpmstyle(8, false, new List<byte> {1, 3, 7, 15, 31, 63, 127, 255},
                                     new List<byte> {0, 0, 0, 0, 0, 0, 0, 0}, null, null)
                    },
                    {
                        ShiftStyleEnum.GreenProgressiveRedShift,
                        new Rpmstyle(8, true, new List<byte> {1, 3, 7, 15, 31, 63, 127, 255},
                                     new List<byte> {0, 0, 0, 0, 0, 0, 0, 0}, new List<byte> {0, 0, 0, 0, 0, 0, 0, 0},
                                     new List<byte> {1, 3, 7, 15, 31, 63, 127, 255})
                    },
                    {
                        ShiftStyleEnum.GreenRedProgressive,
                        new Rpmstyle(8, false, new List<byte> {1, 3, 7, 15, 31, 31, 31, 31},
                                     new List<byte> {0, 0, 0, 0, 0, 32, 96, 224}, null, null)
                    },
                    {
                        ShiftStyleEnum.GreenRedProgressiveRedShift,
                        new Rpmstyle(8, true, new List<byte> {1, 3, 7, 15, 31, 31, 31, 31},
                                     new List<byte> {0, 0, 0, 0, 0, 32, 96, 224},
                                     new List<byte> {0, 0, 0, 0, 0, 0, 0, 0},
                                     new List<byte> {1, 3, 7, 15, 31, 63, 127, 255})
                    },
                    {
                        ShiftStyleEnum.GreenThenRedProgressive,
                        new Rpmstyle(16, false, new List<byte> {1, 3, 7, 15, 31, 63, 127, 255, 0, 0, 0, 0, 0, 0, 0, 0},
                                     new List<byte> {0, 0, 0, 0, 0, 0, 0, 0, 1, 3, 7, 15, 31, 63, 127, 255}, null,
                                     null)
                    },
                    {
                        ShiftStyleEnum.RedProgressive, new Rpmstyle(8, false, new List<byte> {0, 0, 0, 0, 0, 0, 0, 0},
                                                                    new List<byte> {1, 3, 7, 15, 31, 63, 127, 255}, null,
                                                                    null)
                    },

                    //New Ones
                    {
                        ShiftStyleEnum.GreenProgressiveFirstHalf,
                        new Rpmstyle(16, false,
                                     new List<byte>
                                         {
                                             1,
                                             3,
                                             7,
                                             15,
                                             31,
                                             63,
                                             127,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255
                                         },
                                     new List<byte> {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, null, null)
                    },
                    {
                        ShiftStyleEnum.GreenProgressiveRedShiftFirstHalf,
                        new Rpmstyle(16, true,
                                     new List<byte>
                                         {
                                             1,
                                             3,
                                             7,
                                             15,
                                             31,
                                             63,
                                             127,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255
                                         },
                                     new List<byte> {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                     new List<byte> {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                     new List<byte>
                                         {
                                             1,
                                             3,
                                             7,
                                             15,
                                             31,
                                             63,
                                             127,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255
                                         })
                    },
                    {
                        ShiftStyleEnum.GreenRedProgressiveFirstHalf,
                        new Rpmstyle(16, false,
                                     new List<byte>
                                         {
                                             1,
                                             3,
                                             7,
                                             15,
                                             31,
                                             63,
                                             127,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255
                                         },
                                     new List<byte> {0, 0, 0, 0, 0, 0, 0, 0}, null, null)
                    },
                    {
                        ShiftStyleEnum.GreenRedProgressiveRedShiftFirstHalf,
                        new Rpmstyle(16, true,
                                     new List<byte>
                                         {
                                             1,
                                             3,
                                             7,
                                             15,
                                             31,
                                             63,
                                             127,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255
                                         },
                                     new List<byte> {0, 0, 0, 0, 0, 0, 0, 0},
                                     new List<byte> {0, 0, 0, 0, 0, 0, 0, 0},
                                     new List<byte>
                                         {
                                             1,
                                             3,
                                             7,
                                             15,
                                             31,
                                             63,
                                             127,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255
                                         })
                    },
                    {
                        ShiftStyleEnum.GreenThenRedProgressiveFirstHalf,
                        new Rpmstyle(32, false,
                                     new List<byte>
                                         {
                                             1,
                                             3,
                                             7,
                                             15,
                                             31,
                                             63,
                                             127,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0
                                         },
                                     new List<byte>
                                         {
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             1,
                                             3,
                                             7,
                                             15,
                                             31,
                                             63,
                                             127,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255
                                         }, null,
                                     null)
                    },
                    {
                        ShiftStyleEnum.RedProgressiveFirstHalf,
                        new Rpmstyle(16, false, new List<byte> {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                     new List<byte>
                                         {
                                             1,
                                             3,
                                             7,
                                             15,
                                             31,
                                             63,
                                             127,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255,
                                             255
                                         }, null,
                                     null)
                    },
                    //Second Halfs
                    {
                        ShiftStyleEnum.GreenProgressiveSecondHalf,
                        new Rpmstyle(16, false, new List<byte> {0, 0, 0, 0, 0, 0, 0, 0, 1, 3, 7, 15, 31, 63, 127, 255},
                                     new List<byte> {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, null, null)
                    },
                    {
                        ShiftStyleEnum.GreenProgressiveRedShiftSecondHalf,
                        new Rpmstyle(16, true, new List<byte> {0, 0, 0, 0, 0, 0, 0, 0, 1, 3, 7, 15, 31, 63, 127, 255},
                                     new List<byte> {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                     new List<byte> {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                     new List<byte> {0, 0, 0, 0, 0, 0, 0, 0, 1, 3, 7, 15, 31, 63, 127, 255})
                    },
                    {
                        ShiftStyleEnum.GreenRedProgressiveSecondHalf,
                        new Rpmstyle(16, false, new List<byte> {0, 0, 0, 0, 0, 0, 0, 0, 1, 3, 3, 3, 3, 3, 3, 3},
                                     new List<byte> {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 12, 28, 60, 124, 252}, null, null)
                    },
                    {
                        ShiftStyleEnum.GreenRedProgressiveRedShiftSecondHalf,
                        new Rpmstyle(16, true, new List<byte> {0, 0, 0, 0, 0, 0, 0, 0, 1, 3, 3, 3, 3, 3, 3, 3},
                                     new List<byte> {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 12, 28, 60, 124, 252},
                                     new List<byte> {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                     new List<byte> {0, 0, 0, 0, 0, 0, 0, 0, 1, 3, 7, 15, 31, 63, 127, 255})
                    },
                    {
                        ShiftStyleEnum.GreenThenRedProgressiveSecondHalf,
                        new Rpmstyle(32, false,
                                     new List<byte>
                                         {
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             1,
                                             3,
                                             7,
                                             15,
                                             31,
                                             63,
                                             127,
                                             255,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0
                                         },
                                     new List<byte>
                                         {
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             0,
                                             1,
                                             3,
                                             7,
                                             15,
                                             31,
                                             63,
                                             127,
                                             255
                                         }, null,
                                     null)
                    },
                    {
                        ShiftStyleEnum.RedProgressiveSecondHalf,
                        new Rpmstyle(16, false, new List<byte> {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                     new List<byte> {0, 0, 0, 0, 0, 0, 0, 0, 1, 3, 7, 15, 31, 63, 127, 255}, null,
                                     null)
                    },
                    {
                        ShiftStyleEnum.GreenConverging,
                        new Rpmstyle(4, false, new List<byte> {129, 195, 231, 255},
                                     new List<byte> {0, 0, 0, 0}, null, null)
                    },
                    {
                        ShiftStyleEnum.RedConverging,
                        new Rpmstyle(4, false, new List<byte> {0, 0, 0, 0},
                                     new List<byte> {129, 195, 231, 255}, null, null)
                    },
                    {
                        ShiftStyleEnum.GreenRedConverging,
                        new Rpmstyle(4, false, new List<byte> {129, 195, 195, 195},
                                     new List<byte> {0, 0, 36, 60}, null, null)
                    },
                    {
                        ShiftStyleEnum.GreenConvergingRedShift,
                        new Rpmstyle(4, true, new List<byte> {129, 195, 231, 255},
                                     new List<byte> {0, 0, 0, 0}, new List<byte> {0, 0, 0, 0},
                                     new List<byte> {129, 195, 231, 255})
                    },
                    {
                        ShiftStyleEnum.GreenRedConvergingRedShift,
                        new Rpmstyle(4, true, new List<byte> {129, 195, 195, 195},
                                     new List<byte> {0, 0, 36, 60}, new List<byte> {0, 0, 0, 0},
                                     new List<byte> {129, 195, 231, 255})
                    },
                    //First Halfs
                    {
                        ShiftStyleEnum.GreenConvergingFirstHalf,
                        new Rpmstyle(8, false, new List<byte> {1, 3, 7, 15, 31, 63, 127, 255},
                                     new List<byte> {0, 0, 0, 0, 0, 0, 0, 0}, null, null)
                    },
                    {
                        ShiftStyleEnum.RedConvergingFirstHalf,
                        new Rpmstyle(8, false, new List<byte> {0, 0, 0, 0, 0, 0, 0, 0},
                                     new List<byte> {1, 3, 7, 15, 31, 63, 127, 255}, null, null)
                    },
                    {
                        ShiftStyleEnum.GreenRedConvergingFirstHalf,
                        new Rpmstyle(8, false, new List<byte> {1, 3, 7, 15, 31, 31, 31, 31},
                                     new List<byte> {0, 0, 0, 0, 0, 32, 96, 224}, null, null)
                    },
                    {
                        ShiftStyleEnum.GreenConvergingRedShiftFirstHalf,
                        new Rpmstyle(8, true, new List<byte> {1, 3, 7, 15, 31, 63, 127, 255},
                                     new List<byte> {0, 0, 0, 0, 0, 0, 0, 0}, new List<byte> {0, 0, 0, 0, 0, 0, 0, 0},
                                     new List<byte> {1, 3, 7, 15, 31, 63, 127, 255})
                    },
                    {
                        ShiftStyleEnum.GreenRedConvergingRedShiftFirstHalf,
                        new Rpmstyle(8, true, new List<byte> {1, 3, 7, 15, 31, 31, 31, 31},
                                     new List<byte> {0, 0, 0, 0, 0, 32, 96, 224},
                                     new List<byte> {0, 0, 0, 0, 0, 0, 0, 0},
                                     new List<byte> {1, 3, 7, 15, 31, 63, 127, 255})
                    },
                    //Second Halfs
                    {
                        ShiftStyleEnum.GreenConvergingSecondHalf,
                        new Rpmstyle(8, false, new List<byte> {128, 192, 224, 240, 248, 252, 254, 255},
                                     new List<byte> {0, 0, 0, 0, 0, 0, 0, 0}, null, null)
                    },
                    {
                        ShiftStyleEnum.RedConvergingSecondHalf,
                        new Rpmstyle(8, false, new List<byte> {0, 0, 0, 0, 0, 0, 0, 0},
                                     new List<byte> {128, 192, 224, 240, 248, 252, 254, 255}, null, null)
                    },
                    {
                        ShiftStyleEnum.GreenRedConvergingSecondHalf,
                        new Rpmstyle(8, false, new List<byte> {128, 192, 224, 240, 248, 248, 248, 248},
                                     new List<byte> {0, 0, 0, 0, 0, 4, 6, 7}, null, null)
                    },
                    {
                        ShiftStyleEnum.GreenConvergingRedShiftSecondHalf,
                        new Rpmstyle(8, true, new List<byte> {128, 192, 224, 240, 248, 252, 254, 255},
                                     new List<byte> {0, 0, 0, 0, 0, 0, 0, 0}, new List<byte> {0, 0, 0, 0, 0, 0, 0, 0},
                                     new List<byte> {128, 192, 224, 240, 248, 252, 254, 255})
                    },
                    {
                        ShiftStyleEnum.GreenRedConvergingRedShiftSecondHalf,
                        new Rpmstyle(8, true, new List<byte> {128, 192, 224, 240, 248, 248, 248, 248},
                                     new List<byte> {0, 0, 0, 0, 0, 4, 6, 7}, new List<byte> {0, 0, 0, 0, 0, 0, 0, 0},
                                     new List<byte> {128, 192, 224, 240, 248, 252, 254, 255})
                    },
                    {
                        ShiftStyleEnum.BasicShiftLight,
                        new Rpmstyle(2, true, new List<byte> {0,0}, new List<byte> {0,0}, new List<byte> {0,0}, new List<byte> {255,255})
                    }
                };
            return rpmStyles;
        }

        /// <summary>
        /// Pit Limiter Styles
        /// </summary>
        /// <returns>A Dictionary used in the Dictionarys class</returns>
        public static Dictionary<PitFlashStyleEnum, Rpmstyle> FillPitStyles()
        {
            var pitStyles = new Dictionary<PitFlashStyleEnum, Rpmstyle>
                {
                    {
                        PitFlashStyleEnum.GreenFlash,
                        new Rpmstyle(2, false, new List<byte> {255, 0}, new List<byte> {0, 0}, null, null)
                    },
                    {
                        PitFlashStyleEnum.GreenRedAlternateFlash,
                        new Rpmstyle(2, false, new List<byte> {170, 85}, new List<byte> {85, 170}, null, null)
                    },
                    {
                        PitFlashStyleEnum.TwinGreenRedAlternateFlash,
                        new Rpmstyle(2, false, new List<byte> {204, 51}, new List<byte> {51, 204}, null, null)
                    },
                    {
                        PitFlashStyleEnum.GreenThenRedFlash,
                        new Rpmstyle(2, false, new List<byte> {255, 0}, new List<byte> {0, 255}, null, null)
                    },
                    {
                        PitFlashStyleEnum.RedFlash,
                        new Rpmstyle(2, false, new List<byte> {0, 0}, new List<byte> {255, 0}, null, null)
                    }
                };
            return pitStyles;
        }

        /// <summary>
        /// Rev Limiter Styles
        /// </summary>
        /// <returns>A Dictionary used in the Dictionarys class</returns>
        public static Dictionary<RevFlashStyleEnum, Rpmstyle> FillRevLimStyles()
        {
            var revLimStyles = new Dictionary<RevFlashStyleEnum, Rpmstyle>
                {
                    {
                        RevFlashStyleEnum.FlashRed,
                        new Rpmstyle(2, false, new List<byte> {0, 0}, new List<byte> {255, 0}, null, null)
                    },
                    {
                        RevFlashStyleEnum.StayRed,
                        new Rpmstyle(2, false, new List<byte> {0, 0}, new List<byte> {255, 255}, null, null)
                    }
                };
            return revLimStyles;
        }

        /// <summary>
        /// Sets the User Interface Display names for each Enumeration Above
        /// </summary>
        /// <param name="shiftStyles"></param>
        /// <param name="pitFlashStyles"></param>
        /// <param name="revFlashStyles"></param>
        /// <param name="pitFlashSpeeds"></param>
        /// <param name="matchStyleOptions"></param>
        /// <param name="deltaLightsPositionOptions"></param>
        public static void LEDUserInterfaceNames(out Dictionary<string, ShiftStyleEnum> shiftStyles, out Dictionary<string, PitFlashStyleEnum> pitFlashStyles, out Dictionary<string, RevFlashStyleEnum> revFlashStyles, out Dictionary<string, PitFlashSpeedsEnum> pitFlashSpeeds, out Dictionary<string, MatchCarShiftOptions> matchStyleOptions, out Dictionary<string, DeltaLightsOptions> deltaLightsPositionOptions)
        {
            shiftStyles = new Dictionary<string, ShiftStyleEnum>();
            pitFlashStyles = new Dictionary<string, PitFlashStyleEnum>();
            revFlashStyles = new Dictionary<string, RevFlashStyleEnum>();
            pitFlashSpeeds = new Dictionary<string, PitFlashSpeedsEnum>();
            matchStyleOptions = new Dictionary<string, MatchCarShiftOptions>();
            deltaLightsPositionOptions = new Dictionary<string, DeltaLightsOptions>();

            shiftStyles.Add("Green Progressive", ShiftStyleEnum.GreenProgressive);
            shiftStyles.Add("Green Progressive with Red Shift", ShiftStyleEnum.GreenProgressiveRedShift);
            shiftStyles.Add("Green and Red Progressive", ShiftStyleEnum.GreenRedProgressive);
            shiftStyles.Add("Green and Red Progressive with Red Shift", ShiftStyleEnum.GreenRedProgressiveRedShift);
            shiftStyles.Add("Green then Red Progressive", ShiftStyleEnum.GreenThenRedProgressive);
            shiftStyles.Add("Red Progressive", ShiftStyleEnum.RedProgressive);
            shiftStyles.Add("Green Progressive (First half)", ShiftStyleEnum.GreenProgressiveFirstHalf);
            shiftStyles.Add("Green Progressive with Red Shift (First half)",
                            ShiftStyleEnum.GreenProgressiveRedShiftFirstHalf);
            shiftStyles.Add("Green and Red Progressive (First half)", ShiftStyleEnum.GreenRedProgressiveFirstHalf);
            shiftStyles.Add("Green and Red Progressive with Red Shift (First half)",
                            ShiftStyleEnum.GreenRedProgressiveRedShiftFirstHalf);
            shiftStyles.Add("Green then Red Progressive (First half)", ShiftStyleEnum.GreenThenRedProgressiveFirstHalf);
            shiftStyles.Add("Red Progressive (First half)", ShiftStyleEnum.RedProgressiveFirstHalf);
            shiftStyles.Add("Green Progressive (Second half)", ShiftStyleEnum.GreenProgressiveSecondHalf);
            shiftStyles.Add("Green Progressive with Red Shift (Second half)",
                            ShiftStyleEnum.GreenProgressiveRedShiftSecondHalf);
            shiftStyles.Add("Green and Red Progressive (Second half)", ShiftStyleEnum.GreenRedProgressiveSecondHalf);
            shiftStyles.Add("Green and Red Progressive with Red Shift (Second half)",
                            ShiftStyleEnum.GreenRedProgressiveRedShiftSecondHalf);
            shiftStyles.Add("Green then Red Progressive (Second half)", ShiftStyleEnum.GreenThenRedProgressiveSecondHalf);
            shiftStyles.Add("Red Progressive (Second half)", ShiftStyleEnum.RedProgressiveSecondHalf);
            //new ones
            shiftStyles.Add("Green Converging", ShiftStyleEnum.GreenConverging);
            shiftStyles.Add("Red Converging", ShiftStyleEnum.RedConverging);
            shiftStyles.Add("Green and Red Converging", ShiftStyleEnum.GreenRedConverging);
            shiftStyles.Add("Green Converging with Red Shift", ShiftStyleEnum.GreenConvergingRedShift);
            shiftStyles.Add("Green and Red Converging with Red Shift", ShiftStyleEnum.GreenRedConvergingRedShift);

            shiftStyles.Add("Green Converging (First half)", ShiftStyleEnum.GreenConvergingFirstHalf);
            shiftStyles.Add("Red Converging (First half)", ShiftStyleEnum.RedConvergingFirstHalf);
            shiftStyles.Add("Green and Red Converging (First half)", ShiftStyleEnum.GreenRedConvergingFirstHalf);
            shiftStyles.Add("Green Converging with Red Shift (First half)",
                            ShiftStyleEnum.GreenConvergingRedShiftFirstHalf);
            shiftStyles.Add("Green and Red Converging with Red Shift (First half)",
                            ShiftStyleEnum.GreenRedConvergingRedShiftFirstHalf);

            shiftStyles.Add("Green Converging (Second half)", ShiftStyleEnum.GreenConvergingSecondHalf);
            shiftStyles.Add("Red Converging (Second half)", ShiftStyleEnum.RedConvergingSecondHalf);
            shiftStyles.Add("Green and Red Converging (Second half)", ShiftStyleEnum.GreenRedConvergingSecondHalf);
            shiftStyles.Add("Green Converging with Red Shift (Second half)",
                            ShiftStyleEnum.GreenConvergingRedShiftSecondHalf);
            shiftStyles.Add("Green and Red Converging with Red Shift (Second half)",
                            ShiftStyleEnum.GreenRedConvergingRedShiftSecondHalf);
            shiftStyles.Add("Basic Shift Light",
                            ShiftStyleEnum.BasicShiftLight);

            pitFlashStyles.Add("Green Flash", PitFlashStyleEnum.GreenFlash);
            pitFlashStyles.Add("Green & Red Alternate Switch", PitFlashStyleEnum.GreenRedAlternateFlash);
            pitFlashStyles.Add("Twin Green & Red Alternate Switch", PitFlashStyleEnum.TwinGreenRedAlternateFlash);
            pitFlashStyles.Add("Green then Red  Flash", PitFlashStyleEnum.GreenThenRedFlash);
            pitFlashStyles.Add("Red Flash", PitFlashStyleEnum.RedFlash);

            revFlashStyles.Add("Flash Red", RevFlashStyleEnum.FlashRed);
            revFlashStyles.Add("Stay Red", RevFlashStyleEnum.StayRed);

            pitFlashSpeeds.Add("Full Speed", PitFlashSpeedsEnum.Full);
            pitFlashSpeeds.Add("Half Speed", PitFlashSpeedsEnum.Half);
            pitFlashSpeeds.Add("Quarter Speed", PitFlashSpeedsEnum.Quarter);
            pitFlashSpeeds.Add("Eighth Speed", PitFlashSpeedsEnum.Eighth);

            matchStyleOptions.Add("Single Unit", MatchCarShiftOptions.Single);
            matchStyleOptions.Add("Dual Units (Left Half)", MatchCarShiftOptions.DualLeft);
            matchStyleOptions.Add("Dual Units (Right Half)", MatchCarShiftOptions.DualRight);


            deltaLightsPositionOptions.Add("Single Unit", DeltaLightsOptions.Single);
            deltaLightsPositionOptions.Add("Dual Units (Left Half)", DeltaLightsOptions.DualLeft);
            deltaLightsPositionOptions.Add("Dual Units (Right Half)", DeltaLightsOptions.DualRight);
        }

        public static ShiftStyleEnum MatchCarShiftStyle(MatchCarShiftOptions option, CarShiftStyles carStyle, bool useRedShift)
        {
            switch (carStyle)
            {
                case CarShiftStyles.LeftToRight:
                    switch (option)
                    {
                        case MatchCarShiftOptions.Single:
                            if (useRedShift)
                            {
                                return ShiftStyleEnum.GreenProgressiveRedShift;
                            }
                            return ShiftStyleEnum.GreenProgressive;
                        case MatchCarShiftOptions.DualLeft:
                            if (useRedShift)
                            {
                                return ShiftStyleEnum.GreenProgressiveRedShiftFirstHalf;
                            }
                            return ShiftStyleEnum.GreenProgressiveFirstHalf;
                        case MatchCarShiftOptions.DualRight:
                            if (useRedShift)
                            {
                                return ShiftStyleEnum.GreenProgressiveRedShiftSecondHalf;
                            }
                            return ShiftStyleEnum.GreenProgressiveSecondHalf;
                    }
                    break;
                case CarShiftStyles.LeftToRight3Segments:
                    switch (option)/////
                    {
                        case MatchCarShiftOptions.Single:
                            if (useRedShift)
                            {
                                return ShiftStyleEnum.GreenProgressiveRedShift;
                            }
                            return ShiftStyleEnum.GreenProgressive;
                        case MatchCarShiftOptions.DualLeft:
                            if (useRedShift)
                            {
                                return ShiftStyleEnum.GreenProgressiveRedShiftFirstHalf;
                            }
                            return ShiftStyleEnum.GreenProgressiveFirstHalf;
                        case MatchCarShiftOptions.DualRight: 
                            if (useRedShift)
                            {
                                return ShiftStyleEnum.GreenProgressiveRedShiftSecondHalf;
                            }
                            return ShiftStyleEnum.GreenProgressiveSecondHalf;
                    }
                    break;
                case CarShiftStyles.RightToLeft: ///////Not Implemented yet (Does this type Exist??)
                    switch (option)
                    {
                        case MatchCarShiftOptions.Single:
                            break;
                        case MatchCarShiftOptions.DualLeft:
                            break;
                        case MatchCarShiftOptions.DualRight:
                            break;
                    }
                    break;
                case CarShiftStyles.Converging:
                    switch (option)
                    {
                        case MatchCarShiftOptions.Single:
                            if (useRedShift)
                            {
                                return ShiftStyleEnum.GreenConvergingRedShift;
                            }
                            return ShiftStyleEnum.GreenConverging;
                        case MatchCarShiftOptions.DualLeft:
                            if (useRedShift)
                            {
                                return ShiftStyleEnum.GreenConvergingRedShiftFirstHalf;
                            }
                            return ShiftStyleEnum.GreenConvergingFirstHalf;
                        case MatchCarShiftOptions.DualRight:
                            if (useRedShift)
                            {
                                return ShiftStyleEnum.GreenConvergingRedShiftSecondHalf;
                            }
                            return ShiftStyleEnum.GreenConvergingSecondHalf;
                    }
                    break;
                case CarShiftStyles.SingleLight:
                    switch (option)
                    {
                        case MatchCarShiftOptions.Single:
                            if (useRedShift)
                            {
                                return ShiftStyleEnum.BasicShiftLight;
                            }
                            return ShiftStyleEnum.BasicShiftLight;
                        case MatchCarShiftOptions.DualLeft:
                            if (useRedShift)
                            {
                                return ShiftStyleEnum.BasicShiftLight;
                            }
                            return ShiftStyleEnum.BasicShiftLight;
                        case MatchCarShiftOptions.DualRight:
                            if (useRedShift)
                            {
                                return ShiftStyleEnum.BasicShiftLight;
                            }
                            return ShiftStyleEnum.BasicShiftLight;
                    }
                    break;
                case CarShiftStyles.NoLights:
                    switch (option)
                    {
                        case MatchCarShiftOptions.Single:
                            return ShiftStyleEnum.None;
                        case MatchCarShiftOptions.DualLeft:
                            return ShiftStyleEnum.None;
                        case MatchCarShiftOptions.DualRight:
                            return ShiftStyleEnum.None;
                    }
                    break;
                case CarShiftStyles.None:
                    return ShiftStyleEnum.None;
            }
            return ShiftStyleEnum.None;
        }
    }

    /// <summary>
    ///     Storage of Shift Light Parameters
    /// </summary>
    public class ShiftRPMS
    {
        public float ShiftBlinkRPM;
        public float ShiftEndRPM;
        public float ShiftPointRPM;
        public float ShiftStartRPM;
    }

    public class Lights
    {
        public byte Green;
        public byte Red;

        public Lights()
        {
        }

        public Lights(byte red, byte green)
        {
            Red = red;
            Green = green;
        }
    }

    public class Rpmstyle
    {
        public int Length; //number of lights in above lists
        public List<Lights> Normal;

        public bool SecondRevFall = true;
        //When used for REV Limiter Flashs then if true then second byte of array will allow other light settings to mix (use or operator on result)

        public List<Lights> Shifted;
        public bool UseShiftedArray = false;

        public Rpmstyle()
        {
        }

        private void RpmstyleExtracted(List<byte> Greens, List<byte> Reds, int longerCount, List<Lights> normalOrShifted)
        {
            for (int j = 0; j < longerCount; j++)
            {
                if (j < Reds.Count && j < Greens.Count)
                {
                    normalOrShifted.Add(new Lights(Reds[j], Greens[j]));
                }
                else
                    if (j < Reds.Count)
                    {
                        //reds left
                        normalOrShifted.Add(new Lights(Reds[j], 0));
                    }
                    else
                        if (j < Greens.Count)
                        {
                            //greens left
                            normalOrShifted.Add(new Lights(0, Greens[j]));
                        }
            }
        }
        public Rpmstyle(int length, bool useShiftArrayIn, List<byte> normalGreens, List<byte> normalReds,
                        List<byte> shiftedGreens, List<byte> shiftedReds)
        {
            Length = length;
            UseShiftedArray = useShiftArrayIn;
            Normal = new List<Lights>();
            int longerCountNormal = Math.Max(normalGreens.Count, normalReds.Count);
            RpmstyleExtracted(normalGreens, normalReds, longerCountNormal, Normal);
            if (UseShiftedArray)
            {
                Shifted = new List<Lights>();
                int longerCountShifted = Math.Max(shiftedGreens.Count, shiftedReds.Count);
                RpmstyleExtracted(shiftedGreens, shiftedReds, longerCountShifted, Shifted);
            }
        }
    }

    public class LEDFunctions
    {
        /// <summary>
        ///     Gets LED bytes using ShiftStyles
        /// </summary>
        /// <param name="dictionarys"></param>
        /// <param name="currentRPM">Current RPM</param>
        /// <param name="shiftStart">RPM of first shift light</param>
        /// <param name="shiftEnd">RPM of last Shift light</param>
        /// <param name="shiftPoint">RPM of optimal shift point</param>
        /// <param name="style">Shift Light Style</param>
        /// <param name="pastShiftPoint"></param>
        /// <param name="red">RED LEDs byte</param>
        /// <param name="green">Green LEDS byte</param>
        /// <param name="shiftClumps"></param>
        public static void GetShiftLights(Dictionarys dictionarys,float currentRPM, float shiftStart, float shiftEnd, float shiftPoint,
                                   ShiftStyleEnum style, bool shiftClumps,out bool pastShiftPoint, out byte red, out byte green)
        {
            pastShiftPoint = false;
            red = 0;
            green = 0;
            if (style == ShiftStyleEnum.None) return;
            if (shiftStart < 1 || shiftEnd < 1)
            {
                if (shiftPoint > 1)
                {
                    shiftStart = shiftPoint - 1;
                    shiftEnd = shiftPoint;
                }
                else
                {
                    return; // No Valid Shift values for this current gear/map/car
                }
            }
            if (shiftPoint < 1) shiftPoint = 40000;
            if (!(currentRPM >= shiftStart)) return;
            //get length of shift style
            int shiftStyleLength = dictionarys.RPMStyles[style].Length;
            //calc point in shift lists to use
            float calc = ((currentRPM - shiftStart) / (shiftEnd - shiftStart)) * Convert.ToSingle(shiftStyleLength - 1);
            int place = Convert.ToInt32(Math.Floor(calc));
            if (place >= shiftStyleLength)
            {
                place = shiftStyleLength - 1;
            }
            if (shiftClumps)
            {
                if (place >= shiftStyleLength - 1)
                {
                    //last seg, all lights on so leave as is
                }
                else if (place >= (shiftStyleLength / 2) - 1)
                {
                    //middle seg
                    switch (shiftStyleLength)
                    {
                        case 4:
                            place = 1;
                            break;
                        case 8:
                            place = 4;
                            break;
                        case 16:
                            place = 10;
                            break;
                        case 32:
                            place = 20;
                            break;
                    }
                }
                else
                {
                    //first seg
                    switch (shiftStyleLength)
                    {
                        case 4:
                            place = 0;
                            break;
                        case 8:
                            place = 1;
                            break;
                        case 16:
                            place = 4;
                            break;
                        case 32:
                            place = 9;
                            break;
                    }
                }
            } //end shiftClumps
            if (shiftPoint <= 1) // Shift Point RPM value not valid
            {
                shiftPoint = 50000;
            }
            if (currentRPM >= shiftPoint && dictionarys.RPMStyles[style].UseShiftedArray)
            {
                pastShiftPoint = true;
                Lights temp = dictionarys.RPMStyles[style].Shifted[place];
                red = temp.Red;
                green = temp.Green;
            }
            else
            {
                Lights temp = dictionarys.RPMStyles[style].Normal[place]; //-1?
                red = temp.Red;
                green = temp.Green;
            }
        }
    }
}
