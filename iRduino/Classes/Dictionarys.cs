//========================================//
// iRduino - Created by Mark Silverwood  //
//======================================//

namespace iRduino.Classes
{
    using System.Collections.Generic;

    /// <summary>
    /// Container class for all the Dictionaries used in the Application
    /// </summary>
    public class Dictionarys
    {
        public Dictionary<ButtonFunctionsEnum, ButtonFunction> ButtonFunctions;

        public Dictionary<int, string> ComPorts = new Dictionary<int, string>
            {
                {0, "COM1"},
                {1, "COM2"},
                {2, "COM3"},
                {3, "COM4"},
                {4, "COM5"},
                {5, "COM6"},
                {6, "COM7"},
                {7, "COM8"},
                {8, "COM9"},
                {9, "COM10"},
                {10, "COM11"},
                {11, "COM12"},
                {12, "COM13"},
                {13, "COM14"},
                {14, "COM15"},
                {15, "COM16"},
                {16, "COM17"},
                {17, "COM18"},
                {18, "COM19"},
                {19, "COM20"},
                {20, "COM21"},
                {21, "COM22"},
                {22, "COM23"},
                {23, "COM24"},
                {24, "COM25"},
                {25, "COM26"},
                {26, "COM27"},
                {27, "COM28"},
                {28, "COM29"},
                {29, "COM30"},
                {30, "COM31"},
                {31, "COM32"}
            };

        public Dictionary<string, WarningTypesEnum> WarningTypes = new Dictionary<string, WarningTypesEnum> 
            { 
                {"Text Only",WarningTypesEnum.Text},
                {"Lights Only",WarningTypesEnum.Lights},
                {"Both Lights and Text",WarningTypesEnum.Both}
            };

        public Dictionary<DisplayVarsEnum, DisplayVariable> DisplayVariables;
        public Dictionary<string, LapDisplayStylesEnum> LapDisplayStyles;
        public Dictionary<string, PitFlashSpeedsEnum> PitFlashSpeeds;
        public Dictionary<string, PitFlashStyleEnum> PitFlashStyles;
        public Dictionary<PitFlashStyleEnum, Rpmstyle> PitStyles;
        public Dictionary<ShiftStyleEnum, Rpmstyle> RPMStyles;
        public Dictionary<string, RevFlashStyleEnum> RevFlashStyles;
        public Dictionary<RevFlashStyleEnum, Rpmstyle> RevLimStyles;
        public Dictionary<string, ShiftStyleEnum> ShiftStyles;
        public Dictionary<string, MatchCarShiftOptions> MatchCarShiftOptions;
        public Dictionary<string, DeltaLightsOptions> DeltaLightsPositionOptions;

        public Dictionarys()
        {
            this.ButtonFunctions = ButtonFunctionsClass.FillButtonFunctions();
            this.DisplayVariables = DisplayVariablesClass.FillDisplayVariables();
            LEDs.LEDUserInterfaceNames(out this.ShiftStyles, out this.PitFlashStyles, out this.RevFlashStyles, out this.PitFlashSpeeds, out this.MatchCarShiftOptions, out this.DeltaLightsPositionOptions);
            this.RPMStyles = LEDs.FillRPMStyles();
            this.PitStyles = LEDs.FillPitStyles();
            this.RevLimStyles = LEDs.FillRevLimStyles();
            this.LapDisplayStyles = LapDisplays.LapDisplayStylesFill();
        }

    }
}