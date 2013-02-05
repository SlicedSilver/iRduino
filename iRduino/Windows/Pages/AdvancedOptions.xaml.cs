//========================================//
// iRduino - Created by Mark Silverwood  //
//======================================//

namespace iRduino.Pages
{
    using iRduino.Classes;
    using System.Globalization;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows;

    /// <summary>
    /// Interaction logic for AdvancedOptions.xaml
    /// </summary>
    public partial class AdvancedOptions
    {
        private ConfigurationOptions temp;

        private int numberTM1638;
        private string refreshString;
        private int numberTM1640;
        
        public AdvancedOptions()
        {
            InitializeComponent();
        }

        private void SerialSpeedCheckBoxUnchecked1(object sender, RoutedEventArgs e)
        {
            SerialSpeedCBox.IsEnabled = false;
            SerialPortSpeedLabel.IsEnabled = false;
        }

        private void SerialSpeedCheckBoxChecked1(object sender, RoutedEventArgs e)
        {
            SerialSpeedCBox.IsEnabled = true;
            SerialPortSpeedLabel.IsEnabled = true;
        }

        private void PageLoaded1(object sender, RoutedEventArgs e)
        {
            SerialPortSpeedLabel.IsEnabled = false;
            temp = (ConfigurationOptions)DataContext;
            numberTM1640 = 0;
            numberTM1638 = 0;
            foreach (var item in Classes.AdvancedOptions.DisplayRefreshRates)
            {
                TMRefreshRateCBox.Items.Add(item.ToString(CultureInfo.InvariantCulture));
            }
            foreach (var item in Classes.AdvancedOptions.SerialSpeeds)
            {
                SerialSpeedCBox.Items.Add(item.ToString(CultureInfo.InvariantCulture));
            }
            foreach (var unit in temp.DisplayUnitConfigurations)
            {
                if (unit.IsTM1640)
                {
                    numberTM1640++;
                }
                else
                {
                    numberTM1638++;
                }
            }
            refreshString = this.TMRefreshRateCBox.SelectedValue == null ? "" : this.TMRefreshRateCBox.SelectedValue.ToString();
            RecommendSpeed.Content = Classes.AdvancedOptions.CalculateRecommendSerialSpeed(Classes.AdvancedOptions.ParseDisplayRefreshRatesString(refreshString),
                numberTM1638, numberTM1640);
            //databinding
            var displayRateBinding = new Binding("DisplayRefreshRate") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(TMRefreshRateCBox, Selector.SelectedValueProperty, displayRateBinding);
            var useCustomSpeedBinding = new Binding("UseCustomSerialSpeed") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(UseCustomSpeedCheck, ToggleButton.IsCheckedProperty, useCustomSpeedBinding);
            var serialSpeedBinding = new Binding("SerialPortSpeed") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(SerialSpeedCBox, Selector.SelectedValueProperty, serialSpeedBinding);
            var logMessagesBinding = new Binding("LogArduinoMessages") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(LogMessagesCheck, ToggleButton.IsCheckedProperty, logMessagesBinding);
        }

        private void TMRefreshRateCBoxSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            refreshString = this.TMRefreshRateCBox.SelectedValue == null ? "" : this.TMRefreshRateCBox.SelectedValue.ToString();
            RecommendSpeed.Content = Classes.AdvancedOptions.CalculateRecommendSerialSpeed(Classes.AdvancedOptions.ParseDisplayRefreshRatesString(refreshString),
                numberTM1638, numberTM1640);
            
        }
    }
}
