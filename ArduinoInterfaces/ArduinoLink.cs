//========================================//
// iRduino - Created by Mark Silverwood  //
//======================================//

namespace ArduinoInterfaces
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.IO.Ports;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Windows;
    using System.Windows.Threading;

    public class Message
    {
        public byte[] SerialMessage;
        public int Length;
    }

    public class DxMessage
    {
        public List<string> DisplayList;

        public int Intensity;

        public List<byte> GreenLEDSList;

        public List<byte> RedLEDSList;

        public List<byte[]> DotsList;
    }

    public class ArduinoLink
    {
        #region FontBytes

        /*
         The bits are displayed by mapping bellow
             -- 0 --
            |       |
            5       1
             -- 6 --
            4       2
            |       |
             -- 3 --  .7 
         */

        private static readonly byte[] FontBytes =
            {
                0, // (32)  <space>
                134, // (33)	!
                34, // (34)	"
                126, // (35)	#
                109, // (36)	$
                0, // (37)	%
                0, // (38)	&
                2, // (39)	'
                48, // (40)	(
                6, // (41)	)
                99, // (42)	*
                0, // (43)	+
                4, // (44)	,
                64, // (45)	-
                128, // (46)	.
                82, // (47)	/
                63, // (48)	0
                6, // (49)	1
                91, // (50)	2
                79, // (51)	3
                102, // (52)	4
                109, // (53)	5
                125, // (54)	6
                39, // (55)	7
                127, // (56)	8
                111, // (57)	9
                0, // (58)	:
                0, // (59)	;
                0, // (60)	<
                72, // (61)	=
                0, // (62)	>
                83, // (63)	?
                95, // (64)	@
                119, // (65)	A
                127, // (66)	B
                57, // (67)	C
                63, // (68)	D
                121, // (69)	E
                113, // (70)	F
                61, // (71)	G
                118, // (72)	H
                6, // (73)	I
                31, // (74)	J
                105, // (75)	K
                56, // (76)	L
                21, // (77)	M
                55, // (78)	N
                63, // (79)	O
                115, // (80)	P
                103, // (81)	Q
                49, // (82)	R
                109, // (83)	S
                120, // (84)	T
                62, // (85)	U
                42, // (86)	V
                29, // (87)	W
                118, // (88)	X
                110, // (89)	Y
                91, // (90)	Z
                57, // (91)	[
                100, // (92)	\ (this can't be the last char on a line, even in comment or it'll concat)
                15, // (93)	]
                0, // (94)	^
                8, // (95)	_
                32, // (96)	`
                95, // (97)	a
                124, // (98)	b
                88, // (99)	c
                94, // (100)	d
                123, // (101)	e
                49, // (102)	f
                111, // (103)	g
                116, // (104)	h
                4, // (105)	i
                14, // (106)	j
                117, // (107)	k
                48, // (108)	l
                85, // (109)	m
                84, // (110)	n
                92, // (111)	o
                115, // (112)	p
                103, // (113)	q
                80, // (114)	r
                109, // (115)	s
                120, // (116)	t
                28, // (117)	u
                42, // (118)	v
                29, // (119)	w
                118, // (120)	x
                110, // (121)	y
                71, // (122)	z
                70, // (123)	{
                6, // (124)	|
                112, // (125)	}
                1 // (126)	~                                   
            };

        #endregion Font

        #region Fields_Properties_Events

        public delegate void ButtonPressEventHandler(int unit, int button);
        public delegate void TestEventhandler();

        private static SerialPort sp; //Serial Communication Port
        private readonly byte[] butByte = new byte[1];
        private readonly List<int> messageLengths = new List<int> {16, 27, 38, 49, 60, 71};
        private readonly DispatcherTimer testTimer;
        private readonly DispatcherTimer timer;
        private BitArray buttons;
        private int buttonsRead;
        private byte[] newSerialData;
        private int numberUnits = 1;

        private bool logArduinoMessagesToFile;
        private bool sendDxMessage;

        private int testCounter;
        private List<bool> tm1640Units;
        private int numberTM1640Units;
        private int currentMessageLength;
        public StringBuilder Sb;
        
        public bool Running { get; private set; }

        public event ButtonPressEventHandler ButtonPress;
        public event TestEventhandler TestFinished;
#endregion

        public ArduinoLink()
        {
            this.timer = new DispatcherTimer();
            this.timer.Tick += this.TimerTick;
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, 17);
            this.testTimer = new DispatcherTimer();
            this.testTimer.Tick += this.TestTimerTick;
            this.testTimer.Interval = new TimeSpan(0, 0, 0, 0, 150);
        }

        #region Serial Port Handling

        /// <summary>
        ///     Gets a list of the available COM Ports on the System
        /// </summary>
        /// <returns>A string array of port names</returns>
        public static String[] CheckComPorts()
        {
            return SerialPort.GetPortNames();
        }

        /// <summary>
        ///     Starts serial communication on specified port
        /// </summary>
        /// <param name="comPort">COM Port to use</param>
        /// <param name="speed">Connection Speed</param>
        /// <param name="numberUnitsIn">Number of Display Units</param>
        /// <param name="tm1640UnitsIn">List of bool showing which units are TM1640s</param>
        /// <param name="sendDxSLIMessage"></param>
        /// <param name="logArduinoMessages"></param>
        public void Start(string comPort, int speed, int numberUnitsIn, List<bool> tm1640UnitsIn, bool sendDxSLIMessage, bool logArduinoMessages)
        {
            try
            {
                sp = new SerialPort(comPort, speed, Parity.None, 8);
                sp.Open();
                this.sendDxMessage = sendDxSLIMessage;
                this.timer.Start();
                this.numberUnits = numberUnitsIn;
                this.tm1640Units = tm1640UnitsIn;
                this.logArduinoMessagesToFile = logArduinoMessages;
                int count = tm1640UnitsIn.Count(item => item);
                this.numberTM1640Units = count;
                this.currentMessageLength = this.messageLengths[this.numberUnits - 1] + (Constants.ExtraMessageLengthTM1640 * this.numberTM1640Units);
                this.Running = true;
                if (this.logArduinoMessagesToFile)
                {
                    this.Sb = new StringBuilder("");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        ///     Stops serial communication
        /// </summary>
        public void Stop()
        {
            //send blank to display
            this.Clear();
            this.timer.Stop();
            if (sp.IsOpen)
            {
                sp.Close();
            }
            this.Running = false;
            if (this.logArduinoMessagesToFile)
            {
                if (Sb.ToString() != "")
                {
                    using (var outfile = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\iRduino\\log.txt"))
                    {
                        outfile.Write(this.Sb.ToString());
                    }
                }
            }
        }

        #endregion

        /// <summary>
        ///     Checks and corrects integer values for being either too large or too small
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="min">Minimum possible value</param>
        /// <param name="max">Maximum possible value</param>
        /// <returns></returns>
        private static int IntValueCheck(int value, int min, int max)
        {
            if (value > max)
            {
                value = max;
            }
            else if (value < min)
            {
                value = min;
            }
            return value;
        }

        #region Button Receiving from Arduino

        /// <summary>
        ///     Use to check for button presses sent from the Arduino
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerTick(object sender, EventArgs e)
        {
            //this.ReadSerialFromArduino();
            //newEdit
            var t = new Thread(ReadSerialFromArduino);
            t.Start();
        }

        private void ReadSerialFromArduino()
        {
            if (sp.BytesToRead < 1 + this.numberUnits - this.numberTM1640Units)
            {
                return;
            }
            if (sp.ReadByte() != Constants.MessageStartByte1)
            {
                return;
            }
            for (int u = 1; u <= this.numberUnits; u++)
            {
                if (this.tm1640Units[u - 1])
                {
                    continue;
                }
                this.buttonsRead = sp.ReadByte();
                if (this.buttonsRead != -1)
                {
                    this.butByte[0] = Convert.ToByte(this.buttonsRead);
                    this.buttons = new BitArray(this.butByte);
                    for (var i = 0; i < Constants.NumberButtonsOnTm1638; i++)
                    {
                        if (this.buttons[i])
                        {
                            Invoke(ref this.ButtonPress, u, i + 1);
                        }
                    }
                }
            }
        }

        private static void Invoke(ref ButtonPressEventHandler ev, int unit, int button)
        {
            if (ev == null) throw new ArgumentNullException("ev");
            ButtonPressEventHandler temp = ev;
            if (temp != null)
            {
                temp(unit, button);
            }
        }

        #endregion

        #region Test Sequence

        public void Test()
        {
            if (!this.Running)
            {
                return;
            }
            this.testCounter = 0;
            this.testTimer.Start();
        }

        private void TestTimerTick(object sender, EventArgs e)
        {
            this.testCounter++;
            var displays = new List<string>(this.numberUnits);
            var greens = new List<byte>(this.numberUnits);
            var reds = new List<byte>(this.numberUnits);
            var dotsList = new List<byte[]>(this.numberUnits);

            byte green;
            byte red;
            byte dots;

            for (int t = 0; t < this.numberUnits; t++)
            {
                displays.Add("");
                greens.Add(0);
                reds.Add(0);
                dotsList.Add(this.tm1640Units[t] ? new Byte[] { 0, 0 } : new Byte[] { 0 });
            }
            
            string display = this.TestSequence(this.testCounter, out green, out red, out dots);

            this.DuplicateDisplayAcrossAllUnits(display, green, red, dots, displays, greens, reds, dotsList, this.tm1640Units);

            var dxMessage = new DxMessage
            {
                DisplayList = displays,
                Intensity = 3,
                GreenLEDSList = greens,
                RedLEDSList = reds,
                DotsList = dotsList
            };
            this.SendStringMulti(dxMessage);
        }

        private void DuplicateDisplayAcrossAllUnits(string display, byte green, byte red, byte dots, List<string> displays, List<byte> greens, List<byte> reds, List<byte[]> dotsList, List<bool> tm1640UnitsIn)
        {
            for (int i = 0; i < this.numberUnits; i++)
            {
                if (tm1640UnitsIn[i])
                {
                    displays[i] = display + display;
                    greens[i] = green;
                    reds[i] = red;
                    dotsList[i][0] = dots;
                    dotsList[i][1] = dots;
                }
                else
                {
                    displays[i] = display;
                    greens[i] = green;
                    reds[i] = red;
                    dotsList[i][0] = dots;
                }
            }
        }

        private string TestSequence(int count, out byte green, out byte red, out byte dots)
        {
            string display;
            switch (count)
            {
                case 1:
                    display = "*";
                    green = 0;
                    red = 0;
                    dots = 128;
                    break;
                case 2:
                    display = " o";
                    green = 0;
                    red = 0;
                    dots = 64;
                    break;
                case 3:
                    display = "  *";
                    green = 0;
                    red = 0;
                    dots = 32;
                    break;
                case 4:
                    display = "   o";
                    green = 0;
                    red = 0;
                    dots = 16;
                    break;
                case 5:
                    display = "    *";
                    green = 0;
                    red = 0;
                    dots = 8;
                    break;
                case 6:
                    display = "     o";
                    green = 0;
                    red = 0;
                    dots = 4;
                    break;
                case 7:
                    display = "      *";
                    green = 0;
                    red = 0;
                    dots = 2;
                    break;
                case 8:
                    display = "       o";
                    green = 0;
                    red = 0;
                    dots = 1;
                    break;
                case 9:
                    display = "88888888";
                    green = 255;
                    red = 0;
                    dots = 255;
                    break;
                case 10:
                    display = "88888888";
                    green = 0;
                    red = 255;
                    dots = 255;
                    break;
                case 11:
                    display = "        ";
                    green = 0;
                    red = 0;
                    dots = 0;
                    this.testTimer.Stop();
                    TestEventhandler temp = this.TestFinished;
                    if (temp != null)
                    {
                        temp();
                    }
                    break;
                default:
                    display = "        ";
                    green = 0;
                    red = 0;
                    dots = 0;
                    break;
            }
            return display;
        }

        #endregion

        #region Building and Sending Serial Messages 

        /// <summary>
        ///     Sends to SLI unit for Multiple Units. Please ensure that all Lists are equal length
        /// </summary>
        public void SendStringMulti(DxMessage dxMessage)
        {
            var displayList = dxMessage.DisplayList;
            var intensity = dxMessage.Intensity;
            var greenLEDSList = dxMessage.GreenLEDSList;
            var redLEDSList = dxMessage.RedLEDSList;
            var dotsList = dxMessage.DotsList;
            this.newSerialData = new byte[this.currentMessageLength];
            this.newSerialData[0] = Constants.MessageStartByte1; // starting check
            this.newSerialData[1] = Constants.MessageStartByte2; // starting check
            this.newSerialData[2] = Convert.ToByte(IntValueCheck(intensity, 0, Constants.MaxIntensityTM));
            int serialCount = 2;

            if (sendDxMessage)
            {
                //Units Message Builder
                serialCount = this.SendMultiUnits(displayList, greenLEDSList, redLEDSList, dotsList, serialCount);
                    //Refactored Method containing Logic to build unit messages
            }

            this.newSerialData[++serialCount] = Constants.MessageEndByte; //end of message
            this.Checksumer(serialCount); // Refactored Method for determining the checksum of the message and adding it to the serial message
            var message = new Message { Length = this.currentMessageLength, SerialMessage = this.newSerialData };
            var t = new Thread(SerialWrite);
            t.Start(message);
            this.WriteToFile(this.newSerialData);
        }

        private static void SerialWrite(object messageIn)
        {
            if (messageIn == null)
            {
                return;
            }
            var message = messageIn as Message;
            if (sp.IsOpen)
            {
                if (message != null)
                {
                    sp.Write(message.SerialMessage, 0, message.Length);
                }
            }
        }

        private void Checksumer(int serialCount)
        {
            byte sum = 0;
            unchecked // Let overflow occur without exceptions
            {
                for (var p = 2; p < this.currentMessageLength - 2; p++)
                {
                    sum += this.newSerialData[p];
                }
            }
            this.newSerialData[++serialCount] = sum; //message checksum
            //return serialCount;
        }

        /// <summary>
        /// Constructs the serial message part for TM1638/TM1640 Units
        /// </summary>
        /// <param name="displayList"></param>
        /// <param name="greenLEDSList"></param>
        /// <param name="redLEDSList"></param>
        /// <param name="dotsList"></param>
        /// <param name="serialCount"></param>
        /// <returns>Serial Count</returns>
        private int SendMultiUnits(IList<string> displayList, IList<byte> greenLEDSList, IList<byte> redLEDSList, IList<byte[]> dotsList, int serialCount)
        {
            for (var u = 0; u < this.numberUnits; u++)
            {
                this.newSerialData[++serialCount] = Convert.ToByte(u + 1);
                if (!this.tm1640Units[u])
                {
                    this.newSerialData[++serialCount] = greenLEDSList[u];
                    this.newSerialData[++serialCount] = redLEDSList[u];
                }
                //string conversion
                var display = this.DisplayStringConversion(displayList, u);
                //add dots
                int textLength;
                var dotsArray = this.DisplayDotsAdd(dotsList, u, out textLength);
                //final building of display string
                for (var i = 0; i < textLength; i++)
                {
                    if (display.Length - 1 < i)
                    {
                        this.newSerialData[++serialCount] = 0;
                    }
                    else
                    {
                        this.newSerialData[++serialCount] = display[i];
                    }
                    if (dotsArray[i])
                    {
                        this.newSerialData[serialCount] += 128; //doesn't increment serialCount because it alters last byte
                    }
                }
            }
            return serialCount;
        }

        private BitArray DisplayDotsAdd(IList<byte[]> dotsList, int u, out int textLength)
        {
            byte[] tempDots;
            if (this.tm1640Units[u])
            {
                textLength = 16;
                tempDots = new byte[2];
                tempDots[0] = Convert.ToByte(dotsList[u][0]);
                if (dotsList[u].Length < 2)
                {
                    tempDots[1] = Convert.ToByte(0);
                }
                else
                {
                    tempDots[1] = Convert.ToByte(dotsList[u][1]);
                }
            }
            else
            {
                tempDots = new byte[1];
                tempDots[0] = Convert.ToByte(dotsList[u][0]);
                textLength = 8;
            }
            var dotsArray = new BitArray(tempDots);
            return dotsArray;
        }

        private byte[] DisplayStringConversion(IList<string> displayList, int u)
        {
            byte[] display;
            if (this.tm1640Units[u])
            {
                display = displayList[u].Length > 0
                                  ? Encoding.ASCII.GetBytes(displayList[u])
                                  : new byte[] { 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32 };
            }
            else
            {
                display = displayList[u].Length > 0
                                  ? Encoding.ASCII.GetBytes(displayList[u])
                                  : new byte[] { 32, 32, 32, 32, 32, 32, 32, 32 };
            }

            for (var i = 0; i < display.Length; i++)
            {
                display[i] = FontBytes[display[i] - 32];
            }
            return display;
        }

        public void WriteToFile(byte[] message)
        {
            if (this.logArduinoMessagesToFile)
            {
                for (var x = 0; x < message.Length; x++)
                {
                    this.Sb.Append(String.Format("{0},", message[x].ToString(CultureInfo.InvariantCulture)));
                }
                this.Sb.AppendLine("END");
            }
        }

        public void Clear()
        {
            var displays = new List<string>();
            var bytes = new List<byte>();
            var dotBytes = new List<byte[]>();

            for (var i = 1; i <= this.numberUnits; i++)
            {
                if (this.tm1640Units[i - 1])
                {
                    displays.Add("                ");
                    dotBytes.Add(new byte[] {0, 0});
                    bytes.Add(0);
                }
                else
                {
                    displays.Add("        ");
                    dotBytes.Add(new byte[] {0});
                    bytes.Add(0);
                }
            }
            var dxMessage = new DxMessage
                                {
                                    DisplayList = displays,
                                    Intensity = 0,
                                    GreenLEDSList = bytes,
                                    RedLEDSList = bytes,
                                    DotsList = dotBytes
                                };
            this.SendStringMulti(dxMessage);
        }

        #endregion
    }
}