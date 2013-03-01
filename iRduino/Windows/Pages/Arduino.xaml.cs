namespace iRduino.Windows.Pages
{
    using System.Globalization;

    using ArduinoInterfaces;

    using iRduino.Classes;

    /// <summary>
    /// Interaction logic for Arduino.xaml
    /// </summary>
    public partial class Arduino
    {
        private bool startingUp = true;
        
        public Arduino()
        {
            InitializeComponent();
        }

        private void PageLoaded1(object sender, System.Windows.RoutedEventArgs e)
        {
            var temp = (ConfigurationOptions) this.DataContext;
            for (int i = 0; i <= Constants.MaxIntensityTM; i++)
            {
                this.ExpanderNumCBox.Items.Add(i.ToString(CultureInfo.InvariantCulture));
            }
            this.UseExpandersCheck.IsChecked = temp.ArduinoIOConfiguration.UseExpanders;
            this.ExpanderNumCBox.SelectedIndex = temp.ArduinoIOConfiguration.NumberExpanders;
            this.UseInputsCheck.IsChecked = temp.ArduinoIOConfiguration.UseInputs;
            this.UseOutputsCheck.IsChecked = temp.ArduinoIOConfiguration.UseOutputs;
            this.startingUp = false;
        }

        private void Checker(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.startingUp) //prevents event from travelling further
            {
                e.Handled = true;
            }
        }

        private void UseExpandersCheckChecked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.ExpanderNumCBox.IsEnabled = true;
            this.NumberExpandersLabel.IsEnabled = true;
            if (!this.startingUp)
            {
                this.ExpanderNumCBox.SelectedIndex = 0;
                //e.Handled = true;
            }
        }

        private void UseExpandersCheckUnchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!this.startingUp)
            {
                this.ExpanderNumCBox.SelectedIndex = 0;
                //e.Handled = true;
            }
            this.NumberExpandersLabel.IsEnabled = false;
            this.ExpanderNumCBox.IsEnabled = false;
        }

    }
}
