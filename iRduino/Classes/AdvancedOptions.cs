//========================================//
// iRduino - Created by Mark Silverwood  //
//======================================//
namespace iRduino.Classes
{
    using System;
    using System.Collections.Generic;
    using ArduinoInterfaces;

    class AdvancedOptions
    {
        public static List<int> SerialSpeeds = new List<int> {9600, 14400, 19200, 28800, 38400, 57600, 115200 };

        public static List<int> DisplayRefreshRates = new List<int> { 30, 15, 10, 5, 1 };

        public static int ParseSerialSpeedString(string input)
        {
            int temp;
            int.TryParse(input, out temp);
            foreach (var speed in SerialSpeeds)
            {
                if (speed == temp)
                {
                    return speed;
                }
            }
            return SerialSpeeds[SerialSpeeds.Count - 1];
        }

        public static int ParseDisplayRefreshRatesString(string input)
        {
            int temp;
            int.TryParse(input, out temp);
            foreach (var refresh in DisplayRefreshRates)
            {
                if (refresh == temp)
                {
                    return refresh;
                }
            }
            return DisplayRefreshRates[0];
        }

        public static int CalculateRecommendSerialSpeed(int displayRefreshRate, int numberTM1638, int numberTM1640)
        {
            //calculate message length
            var messageLength = Constants.MessageHeaderLength + (numberTM1638 * Constants.TM1638MessageLength)
                                + (numberTM1640 * Constants.TM1640MessageLength) + Constants.MessageFooterLength;
            //calculate mininum speed required
            double minSpeedDouble = messageLength * displayRefreshRate * 1.2f * 8 * 2.5f; //150% extra overhead
            var minSpeed = Convert.ToInt32(minSpeedDouble);
            //select from SerialSpeeds
            foreach (var speed in SerialSpeeds)
            {
                if (speed >= minSpeed)
                {
                    return speed;
                }
            }
            return SerialSpeeds[SerialSpeeds.Count - 1];
        }
    }
}
