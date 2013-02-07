﻿//========================================//
// iRduino - Created by Mark Silverwood  //
//======================================//

namespace iRduino.Windows
{
    using System.Windows.Media.Animation;

    using ArduinoInterfaces;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using iRacingSdkWrapper;
    using iRduino.Classes;
    using System.Windows.Media.Imaging;

    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public DisplayManager DisplayMngr = new DisplayManager();
        public bool OptionsWindowOpen = false;
        public ArduinoLink ArduinoConnection;
        private OptionsWindow optionsWindow;
        private SdkWrapper wrapper;
        private readonly BitmapImage startImage;
        private readonly BitmapImage stopImage;

        // Constructor
        public MainWindow()
        {
            InitializeComponent();
            System.Threading.Thread.CurrentThread.CurrentCulture =
                CultureInfo.CreateSpecificCulture("en-US");
            MainWindowBackground.Opacity = 0.6;
            this.startImage = new BitmapImage();
            this.startImage.BeginInit();
            this.startImage.UriSource = new Uri("pack://application:,,,/iRduino;component/Resources/appbar.control.play.png");
            this.startImage.EndInit();
            this.stopImage = new BitmapImage();
            this.stopImage.BeginInit();
            this.stopImage.UriSource = new Uri("pack://application:,,,/iRduino;component/Resources/appbar.control.stop.png");
            this.stopImage.EndInit();
            this.Height -= 30; //Fix height for Metro Styling without Frame
            FadeAnimationBackground(0.1, 3);
        }

        private void FadeAnimationBackground(double endOpacity, double time)
        {
            // Create a duration of 2 seconds.
            var duration = new Duration(TimeSpan.FromSeconds(time));

            // Create two DoubleAnimations and set their properties.
            var myDoubleAnimation1 = new DoubleAnimation { Duration = duration };

            Storyboard sb = new Storyboard { Duration = duration };

            sb.Children.Add(myDoubleAnimation1);

            Storyboard.SetTarget(myDoubleAnimation1, this.MainWindowBackground);

            // Set the attached properties of Canvas.Left and Canvas.Top
            // to be the target properties of the two respective DoubleAnimations
            Storyboard.SetTargetProperty(myDoubleAnimation1, new PropertyPath(OpacityProperty));

            myDoubleAnimation1.To = endOpacity;

            // Begin the animation.
            sb.Begin();
        }

        ////////

        // Event handler called when the session info is updated
        // This typically happens when a car crosses the finish line for example
        private void WrapperSessionInfoUpdated(object sender, SdkWrapper.SessionInfoUpdatedEventArgs e)
        {
            SdkWrapper.SessionInfoUpdatedEventArgs newSessionInfo = e; //may fix a problem (b u g)??
            DisplayMngr.SessionUpdate(newSessionInfo);
        }


        // Event handler called when the telemetry is updated
        // This happens (max) 60 times per second
        private void WrapperTelemetryUpdated(object sender, SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            try
            {
                DisplayMngr.TelemetryUpdate(e);
            }
            catch (Exception ex)
            {
                if (ex.Source != "iRacingSdkWrapper")
                {
                    #if !DEBUG
                        MessageBox.Show(ex.Message);
                    #endif
                }
            }
        }

        /// <summary>
        /// Sets the status message, status light and disables options window button if connected.
        /// </summary>
        private void StatusChanged()
        {
            if (this.wrapper.IsConnected)
            {
                if (this.wrapper.IsRunning)
                {
                    ConnectionStatusLabel.Content = "Connected!";
                    StatusLight.Stroke = new SolidColorBrush {Color = Color.FromArgb(255, 53, 255, 43)};
                    var myHorizontalGradient = new LinearGradientBrush
                        {
                            StartPoint = new Point(0.5, 0),
                            EndPoint = new Point(0.5, 1)
                        };
                    myHorizontalGradient.GradientStops.Add(new GradientStop(Color.FromArgb(255, 11, 156, 4), 1.0));
                    myHorizontalGradient.GradientStops.Add(new GradientStop(Color.FromArgb(255, 24, 220, 5), 0.0));
                    StatusLight.Fill = myHorizontalGradient;
                    OptionsButton.IsEnabled = false;
                    if (OptionsWindowOpen)
                    {
                        this.optionsWindow.DisableChanges();
                    }
                }
                else
                {
                    ConnectionStatusLabel.Content = "Disconnected.";
                    StatusLight.Stroke = new SolidColorBrush {Color = Color.FromArgb(255, 255, 43, 43)};
                    var myHorizontalGradient = new LinearGradientBrush
                        {
                            StartPoint = new Point(0.5, 0),
                            EndPoint = new Point(0.5, 1)
                        };
                    myHorizontalGradient.GradientStops.Add(new GradientStop(Color.FromArgb(255, 126, 4, 4), 1.0));
                    myHorizontalGradient.GradientStops.Add(new GradientStop(Color.FromArgb(255, 226, 13, 13), 0.0));
                    StatusLight.Fill = myHorizontalGradient;
                    if (!OptionsWindowOpen)
                    {
                        OptionsButton.IsEnabled = true;
                    }
                    else
                    {
                        this.optionsWindow.EnableChanges();
                    }
                }
            }
            else
            {
                if (this.wrapper.IsRunning)
                {
                    ConnectionStatusLabel.Content = "Disconnected, waiting for sim...";
                    StatusLight.Stroke = new SolidColorBrush {Color = Color.FromArgb(255, 255, 207, 43)};
                    var myHorizontalGradient = new LinearGradientBrush
                        {
                            StartPoint = new Point(0.5, 0),
                            EndPoint = new Point(0.5, 1)
                        };
                    myHorizontalGradient.GradientStops.Add(new GradientStop(Color.FromArgb(255, 224, 125, 6), 1.0));
                    myHorizontalGradient.GradientStops.Add(new GradientStop(Color.FromArgb(255, 253, 253, 3), 0.0));
                    StatusLight.Fill = myHorizontalGradient;
                    if (!OptionsWindowOpen)
                    {
                        OptionsButton.IsEnabled = true;
                    }
                    else
                    {
                        this.optionsWindow.EnableChanges();
                    }
                }
                else
                {
                    ConnectionStatusLabel.Content = "Disconnected.";
                    StatusLight.Stroke = new SolidColorBrush {Color = Color.FromArgb(255, 255, 43, 43)};
                    var myHorizontalGradient = new LinearGradientBrush
                        {
                            StartPoint = new Point(0.5, 0),
                            EndPoint = new Point(0.5, 1)
                        };
                    myHorizontalGradient.GradientStops.Add(new GradientStop(Color.FromArgb(255, 126, 4, 4), 1.0));
                    myHorizontalGradient.GradientStops.Add(new GradientStop(Color.FromArgb(255, 226, 13, 13), 0.0));
                    StatusLight.Fill = myHorizontalGradient;
                    if (!OptionsWindowOpen)
                    {
                        OptionsButton.IsEnabled = true;
                    }
                    else
                    {
                        this.optionsWindow.EnableChanges();
                    }
                }
            }
        }

        // Event handler called when the sdk wrapper connects (eg, you start it, or the sim is started)
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

        private void MainWindowClosing(object sender, CancelEventArgs e)
        {
            if (this.wrapper.IsRunning)
            {
                this.wrapper.Stop();
            }

            if (this.ArduinoConnection.Running)
            {
                this.ArduinoConnection.Stop();
            }
        }

        private void StartButtonClick(object sender, RoutedEventArgs e)
        {
            // If the wrapper is running, stop it. Otherwise, start it.
            if (this.wrapper.IsRunning)
            {
                this.wrapper.Stop();
                this.ArduinoConnection.Stop();
                StartButtonLabel.Content = "Start";
                StartButtonImage.Source = this.startImage;
                FadeAnimationBackground(0.1, 2);
                //StartButtonImage.Source = "/iRduino;component/Resources/appbar.control.play.png";
                ComPortBox.IsEnabled = true;
            }
            else
            {
                this.wrapper.Start();
                DisplayMngr.CurrentConfiguration.NumDisplayUnits =
                    DisplayMngr.CurrentConfiguration.DisplayConfigurations.Count;
                var tm1640Units =
                    DisplayMngr.CurrentConfiguration.DisplayConfigurations.Select(item => item.IsTM1640).ToList();
                bool useDx = this.DisplayMngr.CurrentConfiguration.DisplayConfigurations.Count > 0;
                this.ArduinoConnection.Start(ComPortBox.SelectedValue.ToString(),
                          DisplayMngr.CurrentConfiguration.SerialPortSpeed,
                          DisplayMngr.CurrentConfiguration.NumDisplayUnits, tm1640Units, useDx, DisplayMngr.CurrentConfiguration.LogArduinoMessages);
                DisplayMngr.SetupDisplayMngr();
                DisplayMngr.Intensity = DisplayMngr.CurrentConfiguration.Intensity;
                DisplayMngr.ControllerCheckTimer.Start();
                StartButtonLabel.Content = "Stop";
                StartButtonImage.Source = this.stopImage;
                FadeAnimationBackground(0.4, 2);
                ComPortBox.IsEnabled = false;
            }
            StatusChanged();
        }

        private void OptionsButtonClick(object sender, RoutedEventArgs e)
        {
            this.optionsWindow = new OptionsWindow(this);
            this.optionsWindow.Show();
            OptionsWindowOpen = true;
            OptionsButton.IsEnabled = false;
        }

        private void StatusLightMouseDown(object sender, MouseButtonEventArgs e)
        {
            DisplayMngr.Test = true;
            this.ArduinoConnection.Test();
        }

        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            String[] ports = ArduinoLink.CheckComPorts();
            foreach (var item in ports)
            {
                ComPortBox.Items.Add(item);
            }
            ComPortBox.SelectedIndex = 0;
            StartButtonLabel.Content = "Start";
            StartButtonImage.Source = this.startImage;
            this.ArduinoConnection = new ArduinoLink();

            this.ArduinoConnection.ButtonPress += this.ArduinoSLIButtonPress;
            this.ArduinoConnection.TestFinished += this.ArduinoSLITestFinished;

            // Create a new instance of the SdkWrapper object
            this.wrapper = new SdkWrapper
                {
                    EventRaiseType = SdkWrapper.EventRaiseTypes.CurrentThread,
                    TelemetryUpdateFrequency = 60//60  //NEWBUILD
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
                    StartButton.IsEnabled = true;
                }
            }
            else
            {
                StartButton.IsEnabled = false;
            }
            LoadConfsinDirectory();
            DisplayMngr.ShiftLightData = ShiftLightData.LoadShiftData(AppDomain.CurrentDomain.BaseDirectory + "ShiftLightData.xml"); //Load ShiftLightData.xml
        }

        private void ArduinoSLIButtonPress(int unit, int button)
        {
            DisplayMngr.SLIButtonPress(unit, button);
        }

        private void ArduinoSLITestFinished()
        {
            DisplayMngr.Test = false;
        }


        private void ComPortBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.ComPortBox.SelectedIndex <= 0)
            {
                return;
            }
            if (this.DisplayMngr.ConfSet)
            {
                this.StartButton.IsEnabled = true;
            }
        }

        // loads all confs in the application directory
        private void LoadConfsinDirectory()
        {
            string currentConf = null;
            string currentConfLocationFile = null;
            string path = AppDomain.CurrentDomain.BaseDirectory + "current.opt";
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
            IEnumerable<string> files = Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory, "*.scft");
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
                    CurrentConfigurationLabel.Content = conf.Name;
                    TrySetComPort(conf.PreferredComPort);
                }
            }
            CheckCurrentConf();
        }

        public void TrySetComPort(int port)
        {
            //find string
            ComPortBox.SelectedItem = DisplayMngr.Dictionarys.ComPorts[port];
        }

        // check if current conf is loaded and thus can start the connection to the SLI and iracing
        public void CheckCurrentConf()
        {
            if (DisplayMngr.CurrentConfiguration != null)
            {
                if (ComPortBox.SelectedIndex >= 0)
                {
                    DisplayMngr.ConfSet = true;
                    StartButton.IsEnabled = true;
                }
                //
            }
            else
            {
                DisplayMngr.ConfSet = false;
                StartButton.IsEnabled = false;
            }
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
                MessageBox.Show("Configuration already Loaded");
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
                            string newName = newLoadedConf.Name +
                                             rand.Next(0, 100).ToString(CultureInfo.InvariantCulture);
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


        private void MainWindowClosed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void CloseButtonMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
            e.Handled = true;
        }

        private void MinimizeButtonMouseEnter(object sender, MouseEventArgs e)
        {
            MinimizeButton.Opacity = 1;
        }

        private void MinimizeButtonMouseLeave(object sender, MouseEventArgs e)
        {
            MinimizeButton.Opacity = 0.5;
        }

        private void CloseButtonMouseEnter(object sender, MouseEventArgs e)
        {
            CloseButton.Opacity = 1;
        }

        private void CloseButtonMouseLeave(object sender, MouseEventArgs e)
        {
            CloseButton.Opacity = 0.5;
        }

        private void HelpButtonMouseEnter(object sender, MouseEventArgs e)
        {
            HelpButton.Opacity = 1;
        }

        private void HelpButtonMouseLeave(object sender, MouseEventArgs e)
        {
            HelpButton.Opacity = 0.5;
        }

        private void AboutBoxMouseEnter(object sender, MouseEventArgs e)
        {
            AboutBox.Opacity = 1;
        }

        private void AboutBoxMouseDown(object sender, MouseButtonEventArgs e)
        {
            var aboutWindow = new AboutBox();
            aboutWindow.Show();
            e.Handled = true;
        }

        private void AboutBoxMouseLeave(object sender, MouseEventArgs e)
        {
            AboutBox.Opacity = 0.5;
        }

        private void HelpButtonMouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("http://irduino.codeplex.com/documentation");
            e.Handled = true;
        }

        private void MinimizeButtonMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
            e.Handled = true;
        }

        private void TitleBarMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }


    }
}