//========================================//
// iRduino - Created by Mark Silverwood  //
//======================================//

namespace iRduino.Windows
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    
    /// <summary>
    /// Interaction logic for UploaderDialog.xaml
    /// </summary>
    public partial class UploaderDialog
    {
        private readonly string fileLocation;

        public UploaderDialog(string fileLocationIn)
        {
            InitializeComponent();
            fileLocation = fileLocationIn;
        }

        private void WindowLoaded1(object sender, RoutedEventArgs e)
        {
            fileLocationLabel.Content = fileLocation;
            this.Height = 175;

            arduinoBoardCBox.Items.Add("Arduino Uno");
            arduinoBoardCBox.Items.Add("Arduino Leonardo");
            arduinoBoardCBox.Items.Add("Arduino Duemilanove (328)");
            arduinoBoardCBox.Items.Add("Arduino Duemilanove (168)");
            arduinoBoardCBox.Items.Add("Arduino Nano (328)");
            arduinoBoardCBox.Items.Add("Arduino Nano (168)");
            arduinoBoardCBox.Items.Add("Arduino Mini (328)");
            arduinoBoardCBox.Items.Add("Arduino Mini (168)");
            arduinoBoardCBox.Items.Add("Arduino Pro Mini (328)");
            arduinoBoardCBox.Items.Add("Arduino Pro Mini (168)");
            arduinoBoardCBox.Items.Add("Arduino Mega 2560/ADK");
            arduinoBoardCBox.Items.Add("Arduino Mega");

            string[] ports = ArduinoInterfaces.ArduinoLink.CheckComPorts();
            foreach (var p in ports)
            {
                comPortCBox.Items.Add(p);
            }
        }

        public void Checker()
        {
            if (arduinoBoardCBox.SelectedIndex >= 0 && comPortCBox.SelectedIndex >= 0)
            {
                uploadButton.IsEnabled = true;
            }
            else
            {
                uploadButton.IsEnabled = false;
            }
        }

        private void YesButtonClick(object sender, RoutedEventArgs e)
        {
            this.Height = 300;
        }

        private void NoButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ArduinoBoardCBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Checker();
        }

        private void ComPortCBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Checker();
        }

        private void UploadButtonClick(object sender, RoutedEventArgs e)
        {
            string arguments = "";
            arguments += "\"" + fileLocation + "\" ";
            arguments += (arduinoBoardCBox.SelectedIndex + 1).ToString(CultureInfo.InvariantCulture);
            arguments += " ";
            arguments += comPortCBox.SelectedValue.ToString();
            string uploaderLocation = AppDomain.CurrentDomain.BaseDirectory + "ArduinoUploader\\ArduinoUploader.exe";
            MessageBox.Show("cmd /K \"" + uploaderLocation + "\" " + arguments);
            Process.Start("cmd", "/K \"" + uploaderLocation + "\" " + arguments);
            //Process.Start(
            //System.Diagnostics.Process process = new System.Diagnostics.Process();
            //System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            //startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            //startInfo.FileName = "cmd.exe";
            //startInfo.Arguments = "/K /t:09 \"" + uploaderLocation + "\" " + arguments;
            //MessageBox.Show(startInfo.Arguments);
            //process.StartInfo = startInfo;
            //process.Start();
            
            this.Close();
        }
    }
}