//========================================//
// iRduino - Created by Mark Silverwood  //
//======================================//

namespace ArduinoInterfaces
{

    public class Constants
    {
        public const int MaxNumberTM1638Units = 6;

        public const int NumberButtonsOnTm1638 = 8;

        public const int NumberPinsArduinoBoard = 54;

        public const int MaxNumberTm1640Units = 3;

        public const int MaxNumberJoystickButtons = 32;

        public const byte MessageStartByte1 = 255;

        //public const byte MessageStartByte2 = 37;

        public const byte MessageEndByte = 180;

        public const int MaxIntensityTM = 7;

        public const int MaxDisplayLengthTM1638 = 8;

        public const int MaxDisplayLengthTM1640 = 16;

        public const int MaxNumberScreensPerUnit = 20;

        public const int MaxDCDisplayLength = 5;

        public const int MessageHeaderLength = 3; //start bytes and intensity

        public const int MessageFooterLength = 3; //checksum & end byte

        public const int TM1638MessageLength = 8;

        public const int TM1638LEDMessageLength = 2;

        public const int TM1640MessageLength = TM1638MessageLength + 8;

// ReSharper disable InconsistentNaming
        public const byte MessageID_TMString = 37;

        public const byte MessageID_TMLED = 42;

        public const byte MessageID_Clear = 170;

        public const byte MessageID_TM1638Buttons = 55;

// ReSharper restore InconsistentNaming
    }
}
