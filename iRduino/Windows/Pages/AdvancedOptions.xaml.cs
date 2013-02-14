//========================================//
// iRduino - Created by Mark Silverwood  //
//======================================//

namespace iRduino.Windows.Pages
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
            this.InitializeComponent();
        }

        private void SerialSpeedCheckBoxUnchecked1(object sender, RoutedEventArgs e)
        {
            this.SerialSpeedCBox.IsEnabled = false;
            this.SerialPortSpeedLabel.IsEnabled = false;
        }

        private void SerialSpeedCheckBoxChecked1(object sender, RoutedEventArgs e)
        {
            this.SerialSpeedCBox.IsEnabled = true;
            this.SerialPortSpeedLabel.IsEnabled = true;
        }

        private void PageLoaded1(object sender, RoutedEventArgs e)
        {
            this.SerialPortSpeedLabel.IsEnabled = false;
            this.temp = (ConfigurationOptions)this.DataContext;
            this.numberTM1640 = 0;
            this.numberTM1638 = 0;
            for (var x = 2; x <= 50; x++)
            {
                this.FuelLapsCBox.Items.Add(x.ToString(CultureInfo.InvariantCulture));
            }
            foreach (var item in Classes.AdvancedOptions.DisplayRefreshRates)
            {
                this.TMRefreshRateCBox.Items.Add(item.ToString(CultureInfo.InvariantCulture));
            }
            foreach (var item in Classes.AdvancedOptions.SerialSpeeds)
            {
                this.SerialSpeedCBox.Items.Add(item.ToString(CultureInfo.InvariantCulture));
            }
            foreach (var unit in this.temp.DisplayUnitConfigurations)
            {
                if (unit.IsTM1640)
                {
                    this.numberTM1640++;
                }
                else
                {
                    this.numberTM1638++;
                }
            }
            this.refreshString = this.TMRefreshRateCBox.SelectedValue == null ? "" : this.TMRefreshRateCBox.SelectedValue.ToString();
            this.RecommendSpeed.Content = Classes.AdvancedOptions.CalculateRecommendSerialSpeed(Classes.AdvancedOptions.ParseDisplayRefreshRatesString(this.refreshString),
                this.numberTM1638, this.numberTM1640);
            //databinding
            var displayRateBinding = new Binding("DisplayRefreshRate") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(this.TMRefreshRateCBox, Selector.SelectedValueProperty, displayRateBinding);
            var useCustomSpeedBinding = new Binding("UseCustomSerialSpeed") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(this.UseCustomSpeedCheck, ToggleButton.IsCheckedProperty, useCustomSpeedBinding);
            var serialSpeedBinding = new Binding("SerialPortSpeed") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(this.SerialSpeedCBox, Selector.SelectedValueProperty, serialSpeedBinding);
            var logMessagesBinding = new Binding("LogArduinoMessages") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(this.LogMessagesCheck, ToggleButton.IsCheckedProperty, logMessagesBinding);
            var useCustomFuelOptionsBinding = new Binding("UseCustomFuelCalculationOptions") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(this.UseCustomFuelOptionsCheck, ToggleButton.IsCheckedProperty, useCustomFuelOptionsBinding);
            var useWeightedFuelBinding = new Binding("UseWeightedFuelCalculations") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(this.WeightedFuelCheck, ToggleButton.IsCheckedProperty, useWeightedFuelBinding);
            var fuelLapsBinding = new Binding("FuelCalculationLaps") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(this.FuelLapsCBox, Selector.SelectedIndexProperty, fuelLapsBinding);
        }

        private void TMRefreshRateCBoxSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            this.refreshString = this.TMRefreshRateCBox.SelectedValue == null ? "" : this.TMRefreshRateCBox.SelectedValue.ToString();
            this.RecommendSpeed.Content = Classes.AdvancedOptions.CalculateRecommendSerialSpeed(Classes.AdvancedOptions.ParseDisplayRefreshRatesString(this.refreshString),
                this.numberTM1638, this.numberTM1640);
            
        }

        private void UseCustomFuelOptionsCheckChecked(object sender, RoutedEventArgs e)
        {
            FuelLapsCBox.IsEnabled = true;
            WeightedFuelCheck.IsEnabled = true;
        }

        private void UseCustomFuelOptionsCheckUnchecked(object sender, RoutedEventArgs e)
        {
            FuelLapsCBox.SelectedIndex = 1;
            FuelLapsCBox.IsEnabled = false;
            WeightedFuelCheck.IsChecked = true;
            WeightedFuelCheck.IsEnabled = false;
        }
    }
}
