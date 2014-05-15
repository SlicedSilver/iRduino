using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Text;

using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

using ArduinoInterfaces;
using iRduino.Classes;
using iRacingSdkWrapper;

namespace Gui2
{
    public partial class main : Form
    {

        public DisplayManager DisplayMngr;
        public bool OptionsWindowOpen = false;
        public ArduinoLink ArduinoConnection;
        public ArduinoMessagesSending ArduinoMessagesSendingMngr;
        public ArduinoMessagesReceiving ArduinoMessagesReceivingMngr;
        private SdkWrapper wrapper;
        public readonly string DocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\iRduino2\\";

        private bool allowClose = false;
        private bool allowShow = false;

        public main()
        {
            InitializeComponent();

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

            AppDomain currentDomain = AppDomain.CurrentDomain;
            //currentDomain.UnhandledException += ErrorReporting.MyHandler;

        }


        public void connect()
        {

            // If the wrapper is running, stop it. Otherwise, start it.
            if (this.wrapper.IsRunning)
            {
                this.stop();
                this.allowClose = true;
            }
            else
            {

                this.Enabled = false;


                DisplayMngr.CurrentConfiguration.TMDisplaySettings.NumDisplayUnits = DisplayMngr.CurrentConfiguration.DisplayConfigurations.Count;

                var tm1640Units = DisplayMngr.CurrentConfiguration.DisplayConfigurations.Select(item => item.IsTM1640).ToList();
                this.ArduinoConnection.Start(ComPortBox.SelectedItem.ToString(), DisplayMngr.CurrentConfiguration.SerialPortSettings.SerialPortSpeed, DisplayMngr.CurrentConfiguration.AdvancedSettings.LogArduinoMessages);

                ArduinoMessagesReceivingMngr.NumberUnits = DisplayMngr.CurrentConfiguration.TMDisplaySettings.NumDisplayUnits;
                ArduinoMessagesReceivingMngr.TM1640Units = tm1640Units;

                DisplayMngr.SetupDisplayMngr(this.wrapper.TelemetryUpdateFrequency, tm1640Units);
                DisplayMngr.Intensity = DisplayMngr.CurrentConfiguration.TMDisplaySettings.Intensity;
                DisplayMngr.ControllerCheckTimer.Start();

                StartButtonLabel.Image = iRduino.Properties.Resources.appbar_control_stop;
                StartButtonLabel.Text = "Parar";
                ComPortBox.Enabled = false;

                this.wrapper.Start();

                this.TMDisplayTest();

                this.allowClose = false;

                this.Enabled = true;

            }

            this.StatusChanged();

        }

        public void stop()
        {
            StartButtonLabel.Enabled = false;
            DisplayMngr.SendTMDisplay();
            System.Threading.Thread.Sleep(1000);
            this.wrapper.Stop();
            this.ArduinoConnection.Stop();
            StartButtonLabel.Text = "Iniciar";
            StartButtonLabel.Image = iRduino.Properties.Resources.appbar_control_play;
            ComPortBox.Enabled = true;
            StartButtonLabel.Enabled = true;
        }

        private void StatusChanged()
        {
            if (this.wrapper.IsConnected)
            {
                if (this.wrapper.IsRunning)
                {
                    GUIStatusUpdate("Connected!", true);
                    ConnectionStatusLabel.BackColor = Color.Green;
                    DisplayMngr.RequestedTMDisplayVariables[0].Display = "CONECTADO";
                    DisplayMngr.SendTMDisplay();
                }
                else
                {
                    GUIStatusUpdate("Disconnected.", false);
                    ConnectionStatusLabel.BackColor = Color.Red;
                }

                testarComunicaoToolStripMenuItem.Enabled = true;

            }
            else
            {
                if (this.wrapper.IsRunning)
                {
                    GUIStatusUpdate("Disconnected, waiting for sim...", false);
                    testarComunicaoToolStripMenuItem.Enabled = true;
                    ConnectionStatusLabel.BackColor = Color.Yellow;
                }
                else
                {
                    testarComunicaoToolStripMenuItem.Enabled = false;
                    ConnectionStatusLabel.BackColor = Color.Red;
                }
            }
        }

        private void GUIStatusUpdate(string text, bool connected)
        {
            ConnectionStatusLabel.Text = text;
        }

        private void WrapperConnected(object sender, EventArgs e)
        {
            StatusChanged();
            DisplayMngr.ResetSavedTelemetryValues(); //set all saved Tele values to defaults.
        }

        // Event handler called when the sdk wrapper disconnects (eg, the sim closes)
        private void WrapperDisconnected(object sender, EventArgs e)
        {
            StatusChanged();
        }

        public void TMDisplayTest()
        {
            if (!this.ArduinoConnection.Running)
            {
                return;
            }
            var unitTypes = this.DisplayMngr.CurrentConfiguration.DisplayConfigurations.Select(disp => disp.IsTM1640).ToList();
            this.ArduinoMessagesSendingMngr.TMDisplayTest(unitTypes, this.ArduinoConnection);
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (e.CloseReason == CloseReason.UserClosing)
            {

                if (this.allowClose)
                {
                    this._closing();
                    return;
                }
                else
                {
                    e.Cancel = true;
                    notifyIcon1.BalloonTipText = "Continuamos rodando";
                    notifyIcon1.ShowBalloonTip(1);
                    this.Hide();
                }

            }
            else
            {

                this._closing();
                return;

            }

        }

        private void _closing()
        {

            if (this.wrapper.IsRunning)
            {
                DisplayMngr.SendTMDisplay();
                DisplayMngr.ShowStringTimed("        ", 300, 0);

                this.wrapper.Stop();
            }

            if (this.ArduinoConnection.Running)
            {
                this.ArduinoConnection.Stop();
            }

        }


        private void StartButtonLabel_Click(object sender, EventArgs e)
        {
            StartButtonLabel.Enabled = false;
            this.connect();
            StartButtonLabel.Enabled = true;
        }

        private void testButton_Click(object sender, EventArgs e)
        {
            DisplayMngr.Test = true;
            this.TMDisplayTest();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            DisplayMngr = new DisplayManager();
            String[] ports = ArduinoLink.CheckComPorts();
            foreach (var item in ports)
            {
                ComPortBox.Items.Add(item);

            }
            ComPortBox.SelectedIndex = 0;
            StartButtonLabel.Text = "Start";

            this.ArduinoConnection = new ArduinoLink();
            this.ArduinoMessagesReceivingMngr = new ArduinoMessagesReceiving();
            this.ArduinoMessagesSendingMngr = new ArduinoMessagesSending();
            this.ArduinoConnection.SerialMessageReceived += ArduinoMessagesReceivingMngr.SerialMessageReceiver;
            this.ArduinoMessagesReceivingMngr.ButtonPress += this.ArduinoSLIButtonPress;
            this.ArduinoMessagesSendingMngr.TestFinished += this.ArduinoSLITestFinished;


            // Create a new instance of the SdkWrapper object
            this.wrapper = new SdkWrapper
            {
                EventRaiseType = SdkWrapper.EventRaiseTypes.CurrentThread,
                TelemetryUpdateFrequency = 30//60  //NEWBUILD
            };
            // Tell it to raise events on the current thread (don't worry if you don't know what a thread is)
            // Only update telemetry 60 times per second
            // Attach some useful events so you can respond when they get raised
            this.wrapper.Connected += this.WrapperConnected;
            this.wrapper.Disconnected += this.WrapperDisconnected;
            this.wrapper.SessionInfoUpdated += this.WrapperSessionInfoUpdated;
            this.wrapper.TelemetryUpdated += this.WrapperTelemetryUpdated;

            DisplayMngr.ArduinoConnection = this.ArduinoConnection;
            DisplayMngr.Wrapper = this.wrapper;

            if (ComPortBox.SelectedIndex >= 0)
            {
                if (DisplayMngr.ConfSet)
                {
                    StartButtonLabel.Enabled = true;
                }
            }
            else
            {
                StartButtonLabel.Enabled = false;
            }
            LoadConfsinDirectory();
            DisplayMngr.ShiftLightData = ShiftLightData.LoadShiftData(AppDomain.CurrentDomain.BaseDirectory + "ShiftLightData.xml"); //Load ShiftLightData.xml

            if (StartButtonLabel.Enabled)
            {
                this.connect();
            }

        }

        private void ArduinoSLIButtonPress(int unit, int button)
        {
            DisplayMngr.SLIButtonPress(unit, button);
        }

        private void ArduinoSLITestFinished()
        {
            DisplayMngr.Test = false;
            this.Enabled = true;
        }


        public void TrySetComPort(int port)
        {
            //find string
            ComPortBox.SelectedItem = DisplayMngr.Dictionarys.ComPorts[port];
        }

        private void WrapperSessionInfoUpdated(object sender, SdkWrapper.SessionInfoUpdatedEventArgs e)
        {
            SdkWrapper.SessionInfoUpdatedEventArgs newSessionInfo = e; //may fix a problem (b u g)??
            DisplayMngr.SessionUpdate(newSessionInfo);
        }

        private void WrapperTelemetryUpdated(object sender, SdkWrapper.TelemetryUpdatedEventArgs e)
        {

            DisplayMngr.TelemetryUpdate(e);
        }

        private void LoadConfsinDirectory()
        {
            string currentConf = null;
            string currentConfLocationFile = null;
            string path = DocumentsPath + "current.opt";
            FileInfo file = new FileInfo(path);
            if (file.Directory != null)
            {
                file.Directory.Create(); // If the directory already exists, this method does nothing.
            }
            if (File.Exists(path))
            {
                using (var sr = new StreamReader(path))
                {
                    currentConfLocationFile = sr.ReadToEnd();
                }
            }
            //load that conf and save its name
            if (currentConfLocationFile != null)
            {
                if (File.Exists(currentConfLocationFile))
                {
                    currentConf = LoadNewConfFile(currentConfLocationFile);
                }
            }
            //get current directory
            IEnumerable<string> files = Directory.EnumerateFiles(DocumentsPath, "*.scft");
            string[] fileLocs = files as string[] ?? files.ToArray();
            if (fileLocs.Any())
            {
                foreach (
                    string fileLoc in
                        fileLocs.Where(File.Exists)
                                .Where(fileLoc => fileLoc != currentConfLocationFile))
                {
                    LoadNewConfFile(fileLoc);
                }
            }



            //get a list of files in directory
            // load all files with scft extension
            //set default conf from saved above
            foreach (Configuration conf in DisplayMngr.Configurations)
            {

                if (conf.Name == currentConf)
                {
                    DisplayMngr.CurrentConfiguration = conf;
                    CurrentConfigurationLabel.Text = conf.Name;
                    TrySetComPort(conf.SerialPortSettings.PreferredComPort);
                }

            }

            this._makeMenuConfiguracoes();

            CheckCurrentConf();
        }

        private void _makeMenuConfiguracoes()
        {

            configuracoesToolStripMenuItem.DropDownItems.Clear();

            foreach (Configuration conf in DisplayMngr.Configurations)
            {
                if (conf.Name == DisplayMngr.CurrentConfiguration.Name)
                {
                    configuracoesToolStripMenuItem.DropDownItems.Add(conf.Name, iRduino.Properties.Resources.appbar_control_play, this.configClick);
                }
                else
                {
                    configuracoesToolStripMenuItem.DropDownItems.Add(conf.Name, null, this.configClick);
                }

            }


        }

        private void configClick(object sender, EventArgs e)
        {

            foreach (Configuration conf in this.DisplayMngr.Configurations)
            {
                if (conf.Name == (sender as ToolStripMenuItem).Text)
                {
                    string path = DocumentsPath + "current.opt";
                    var sr = new StreamWriter(path);
                    sr.Write(conf.FileLocation);
                    sr.Close();

                    this.stop();
                    DisplayMngr.CurrentConfiguration = conf;
                    CurrentConfigurationLabel.Text = conf.Name;
                    TrySetComPort(conf.SerialPortSettings.PreferredComPort);
                    this.connect();
                }
            }

            this._makeMenuConfiguracoes();


        }

        private void removerConfig(object sender, EventArgs e)
        {

            Configuration toRemove = null;

            foreach (Configuration conf in this.DisplayMngr.Configurations)
            {
                if (conf.Name == (sender as ToolStripMenuItem).Text)
                {
                    var path = conf.FileLocation;
                    File.Delete(path);
                    MessageBox.Show("Removido com sucesso!");
                    toRemove = conf;
                }
            }

            if (toRemove != null)
            {
                DisplayMngr.Configurations.Remove(toRemove);
            }

            this.LoadConfsinDirectory();
            this._makeMenuConfiguracoes();

        }

        public string LoadNewConfFile(string filename)
        {
            //find if file already loaded
            string returnString = null;
            bool alreadyOpen = false;
            foreach (Configuration conf in DisplayMngr.Configurations)
            {
                if (conf.FileLocation == filename)
                {
                    alreadyOpen = true;
                }
            }
            if (alreadyOpen)
            {
                //mbox
                //MessageBox.Show("Configuration already Loaded");
            }
            else
            {
                Configuration newLoadedConf;
                bool result = DisplayMngr.LoadConfFile(filename, out newLoadedConf);
                //if reading was successful then leave as is. if not, then switch back to previous conf:remove this conf:show message.
                //set as current conf
                if (result)
                {
                    bool nameExists = false;
                    foreach (Configuration conf in DisplayMngr.Configurations)
                    {
                        if (conf.Name == newLoadedConf.Name)
                        {
                            nameExists = true;
                        }
                    }
                    if (nameExists)
                    {
                        //changename
                        bool nameOk = false;
                        while (!nameOk)
                        {
                            var rand = new Random();
                            string newName = newLoadedConf.Name + rand.Next(0, 100).ToString(CultureInfo.InvariantCulture);
                            bool nameExists1 = false;
                            foreach (Configuration conf in DisplayMngr.Configurations)
                            {
                                if (conf.Name == newName)
                                {
                                    nameExists1 = true;
                                }
                            }
                            if (!nameExists1)
                            {
                                nameOk = true;
                                newLoadedConf.Name = newName;
                            }
                        }
                    }
                    DisplayMngr.Configurations.Add(newLoadedConf);
                    returnString = newLoadedConf.Name;
                }
            }
            return returnString;
        }

        public void CheckCurrentConf()
        {
            if (DisplayMngr.CurrentConfiguration != null)
            {
                if (ComPortBox.SelectedIndex >= 0)
                {
                    DisplayMngr.ConfSet = true;
                    StartButtonLabel.Enabled = true;
                }
                //
            }
            else
            {
                DisplayMngr.ConfSet = false;
                StartButtonLabel.Enabled = false;
            }
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void testarComunicaoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            DisplayMngr.Test = true;
            this.TMDisplayTest();
        }

        private void ComPortBox_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (this.ComPortBox.SelectedIndex <= 0)
            {
                return;
            }
            if (this.DisplayMngr.ConfSet)
            {
                this.StartButtonLabel.Enabled = true;
                ComPortBox.Visible = false;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.allowShow = true;
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }


        private void Main_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.allowClose = true;
            this.Close();
        }

        private void teste1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.allowClose = true;
            this.Close();
        }

        private void Main_Shown(object sender, EventArgs e)
        {

            if (this.allowShow == false)
            {
                //this.Hide();
                this.Show();
                //this.WindowState = FormWindowState.Minimized;
                //this.notifyIcon1.BalloonTipText = "Iniciado..";
                //this.notifyIcon1.ShowBalloonTip(1);
                //configuraçõesToolStripMenuItem.PerformClick();

            }
        }


        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.allowShow = true;
            this.Show();
            this.WindowState = FormWindowState.Normal;

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }



    }
}
