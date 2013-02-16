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

        public static List<int> RefreshRates = new List<int> { 30, 15, 10, 5, 1 };

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

        public static int ParseRefreshRatesString(string input)
        {
            int temp;
            int.TryParse(input, out temp);
            foreach (var refresh in RefreshRates)
            {
                if (refresh == temp)
                {
                    return refresh;
                }
            }
            return RefreshRates[0];
        }

        public static int CalculateRecommendSerialSpeed(int displayRefreshRate, int ledRefreshRate, int numberTM1638, int numberTM1640)
        {
            //calculate message length
            var messageBytes = displayRefreshRate
                                * ((Constants.MessageFooterLength + Constants.MessageHeaderLength
                                    + Constants.TM1638MessageLength) * numberTM1638
                                   + (Constants.MessageFooterLength + Constants.MessageHeaderLength
                                      + Constants.TM1640MessageLength) * numberTM1640) + ledRefreshRate * ((Constants.MessageFooterLength + Constants.MessageHeaderLength
                                    + Constants.TM1638LEDMessageLength) * numberTM1638);
            //calculate mininum speed required
            double minSpeedDouble =  messageBytes * 1.2f * 8 * 2f; //100% extra overhead
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
