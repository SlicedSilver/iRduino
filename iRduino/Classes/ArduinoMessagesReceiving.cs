using System;
using System.Collections.Generic;

namespace iRduino.Classes
{
    using System.Collections;

    using ArduinoInterfaces;

    public class ArduinoMessagesReceiving
    {
        private BitArray buttons;
        private int buttonsRead;
        public int NumberUnits = 1;
        private readonly byte[] butByte = new byte[1];
        public List<bool> TM1640Units;

        public event ButtonPressEventHandler ButtonPress;  
        public delegate void ButtonPressEventHandler(int unit, int button);

        public void SerialMessageReceiver(int[] message)
        {
            switch (message[0])
            {
                case 55: TM1638ButtonsMessageReader(message);
                    break;
            }
        }

        private void TM1638ButtonsMessageReader(IList<int> messageData)
        {
            int readPos = 1;
            for (int u = 1; u <= this.NumberUnits; u++)
            {
                if (this.TM1640Units[u - 1])
                {
                    continue;
                }
                this.buttonsRead = messageData[readPos++];
                if (this.buttonsRead != -1)
                {
                    this.butByte[0] = Convert.ToByte(this.buttonsRead);
                    this.buttons = new BitArray(this.butByte);
                    for (var i = 0; i < Constants.NumberButtonsOnTm1638; i++)
                    {
                        if (this.buttons[i])
                        {
                            if (this.ButtonPress == null) throw new ArgumentNullException();
                            ButtonPressEventHandler temp = this.ButtonPress;
                            if (temp != null)
                            {
                                temp(u, i+1);
                            }
                        }
                    }
                }
            }
        }

    }
}
