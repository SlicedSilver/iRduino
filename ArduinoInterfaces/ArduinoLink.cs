//========================================//
// iRduino - Created by Mark Silverwood  //
//======================================//

namespace ArduinoInterfaces
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.IO.Ports;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Threading;

    public class Message
    {
        public byte[] ArduinoSerialMessage;
        public int Length;
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

        public static readonly byte[] FontBytes =
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

        
        public delegate void SerialMessageReceiver(int[] message);
        public event SerialMessageReceiver SerialMessageReceived;
        

        private static SerialPort sp; //Serial Communication Port
        
        private readonly DispatcherTimer timer;
        

        private bool logArduinoMessagesToFile;

        private StringBuilder sb;

        //Serial Reading Thread variables
        private int[] serialReadHolder;
        private int messagePosition;
        private int checkerPosition;

        public bool Running { get; private set; }


        private readonly BlockingCollection<Message> messageQueue = new BlockingCollection<Message>(30);
        private bool messageConsumerActive;

        
#endregion

        public ArduinoLink()
        {
            this.timer = new DispatcherTimer();
            this.timer.Tick += this.TimerTick;
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, 17);
            
        }

        #region Serial Port Handling

        private void StartMessageConsumer()
        {
            messageConsumerActive = true;
            Task.Factory.StartNew(() =>
            {
                while (messageConsumerActive)
                {
                    Message message;
                    if (messageQueue.TryTake(out message))
                    {
                        if (message != null)
                        {
                            if (sp.IsOpen)
                            {
                                sp.Write(message.ArduinoSerialMessage, 0, message.Length);
                            }
                        }
                    }
                    if (messageQueue.Count == 0)
                    {
                        Thread.Sleep(10);
                    }
                }
            });
        }


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
        /// <param name="logArduinoMessages"></param>
        public void Start(string comPort, int speed, bool logArduinoMessages)
        {
            try
            {
                sp = new SerialPort(comPort, speed, Parity.None, 8);
                sp.Open();
                StartMessageConsumer(); //Starts serial message task consumer
                this.timer.Start();
                this.logArduinoMessagesToFile = logArduinoMessages;
                this.Running = true;
                if (this.logArduinoMessagesToFile)
                {
                    this.sb = new StringBuilder("");
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
            messageConsumerActive = false; //Stops message consumer task
            if (this.logArduinoMessagesToFile)
            {
                if (this.sb.ToString() != "")
                {
                    using (var outfile = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\iRduino\\log.txt"))
                    {
                        outfile.Write(this.sb.ToString());
                    }
                }
            }
        }

        #endregion


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
            var t = new Thread(ReadSerialMessages);
            t.Start();
        }


        private void ReadSerialMessages()
        {
            if (serialReadHolder == null)
            {
                serialReadHolder = new int[128];
                messagePosition = new Int32();
                messagePosition = -1;
            }
            if (sp.BytesToRead > 0)
            {
                messagePosition++;
                if (messagePosition == 128) messagePosition = 0;
                serialReadHolder[messagePosition] = sp.ReadByte();

                if (serialReadHolder[messagePosition] != Constants.MessageEndByte) return; //end byte
                checkerPosition = (messagePosition == 0) ? 128 - 1 : messagePosition - 1;
                int messageLength = serialReadHolder[checkerPosition]; //messageLength
                if (checkerPosition >= messageLength + 3)
                {
                    checkerPosition = checkerPosition - messageLength - 3;
                }
                else
                {
                    checkerPosition = checkerPosition + 128 - messageLength - 3;
                }
                if (serialReadHolder[checkerPosition] != Constants.MessageStartByte1) return; //start byte1
                checkerPosition++;

                byte sum = 0;
                int readCount = 0;
                int[] md = new int[messageLength+1];
                while (readCount <= messageLength)
                {
                    if (checkerPosition == 128) checkerPosition = 0;
                    md[readCount] = serialReadHolder[checkerPosition];
                    unchecked
                    {
                        sum += Convert.ToByte(serialReadHolder[checkerPosition]);
                    }
                    readCount++;
                    checkerPosition++;
                }
                if (checkerPosition == 128) checkerPosition = 0;
                if (sum == serialReadHolder[checkerPosition])
                {
                    if (this.SerialMessageReceived == null) throw new ArgumentNullException();
                    SerialMessageReceiver temp = this.SerialMessageReceived;
                    if (temp != null)
                    {
                        temp(md);
                    }
                }

            }

        }

        #endregion

        #region Building and Sending Serial Messages 

        /// <summary>
        /// Packages the serial message (start bytes, end bytes, checksum, etc...) and passes it along to the serial communication method.
        /// </summary>
        /// <param name="messageID">Message ID</param>
        /// <param name="messageData">Message Data</param>
        public void SendSerialMessage(byte messageID, byte[] messageData)
        {
            int messageDataLength = messageData.Length;
            if (messageDataLength == 0) return; //can't send empty messages
            byte[] serialMessage = new byte[messageDataLength + 5]; // increase length to allow packet frame
            serialMessage[0] = Constants.MessageStartByte1;
            serialMessage[1] = messageID;
            int serialCount = 1;
            foreach (var b in messageData)
            {
                serialMessage[++serialCount] = b;
            }
            serialMessage[++serialCount] = ChecksumCalculator(messageID,messageData); //checksum
            serialMessage[++serialCount] = Convert.ToByte(messageDataLength);
            serialMessage[++serialCount] = Constants.MessageEndByte;
            //add to serial messages queue.
            var message = new Message { Length = serialMessage.Length, ArduinoSerialMessage = serialMessage };
            if (messageQueue.TryAdd(message))
            {
                this.WriteToFile(serialMessage);
            }
        }

        /// <summary>
        /// Performs the checksum for serial message.
        /// The checksum considers only the messagedata and the messageID
        /// </summary>
        /// <param name="messageID">Message ID</param>
        /// <param name="messageData">Message Data</param>
        /// <returns></returns>
        private byte ChecksumCalculator(byte messageID, IEnumerable<byte> messageData)
        {
            byte sum = messageID;
            unchecked // Let overflow occur without exceptions
            {
                sum = messageData.Aggregate(sum, (current, b) => (byte)(current + b));
            }
            return sum;
        }

        public void WriteToFile(byte[] message)
        {
            if (this.logArduinoMessagesToFile)
            {
                for (var x = 0; x < message.Length; x++)
                {
                    this.sb.Append(String.Format("{0},", message[x].ToString(CultureInfo.InvariantCulture)));
                }
                this.sb.AppendLine("END");
            }
        }

        public void Clear()  //sends command to arduino, need arduino function to handle the rest.
        {
            SendSerialMessage(Constants.MessageID_Clear, new byte[] { 170, 170 });
        }

        #endregion
    }
}