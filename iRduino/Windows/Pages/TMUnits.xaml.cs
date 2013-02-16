//========================================//
// iRduino - Created by Mark Silverwood  //
//======================================//

namespace iRduino.Windows.Pages
{
    using ArduinoInterfaces;

    using iRduino.Classes;

    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;

    /// <summary>
    ///     Interaction logic for CurrentConfiguration.xaml
    /// </summary>
    public partial class TMUnits
    {
        //private ConfigurationOptions temp;
        
        public TMUnits()
        {
            this.InitializeComponent();
        }

        private void ShowHeaderCheckChecked(object sender, RoutedEventArgs e)
        {
            this.HeaderDisplayTimeCBox.IsEnabled = true;
            this.HeaderDisplayTimeLabel.IsEnabled = true;
        }

        private void ShowHeaderCheckUnchecked(object sender, RoutedEventArgs e)
        {
            this.HeaderDisplayTimeCBox.IsEnabled = false;
            this.HeaderDisplayTimeLabel.IsEnabled = false;
        }

        private void PageLoaded1(object sender, RoutedEventArgs e)
        {
            this.HeaderDisplayTimeLabel.IsEnabled = false;
            this.ShiftIntensityAmountLabel.IsEnabled = false;


            for (int i = 1; i <= Constants.MaxNumberTM1638Units; i++)
            {
                this.NumberDisplayUnitsCBox.Items.Add(i.ToString(CultureInfo.InvariantCulture));
                this.LapDisplayTimeCBox.Items.Add(i.ToString(CultureInfo.InvariantCulture));
                this.QuickInfoDisplayTimeCBox.Items.Add(i.ToString(CultureInfo.InvariantCulture));
                this.HeaderDisplayTimeCBox.Items.Add(i.ToString(CultureInfo.InvariantCulture));
            }
            for (int i = 0; i <= Constants.MaxIntensityTM; i++)
            {
                this.IntensityCBox.Items.Add(i.ToString(CultureInfo.InvariantCulture));
                this.ShiftIntensityAmountCBox.Items.Add(i.ToString(CultureInfo.InvariantCulture));
            }
            
            var temp = (ConfigurationOptions)this.DataContext;
            for (int i = 0; i < temp.DisplayUnitConfigurations.Count; i++)
            {
                this.DeltaMessageScreenCBox.Items.Add((i + 1).ToString(CultureInfo.InvariantCulture));
            }

            foreach (var item in Classes.AdvancedOptions.RefreshRates)
            {
                this.DisplayRefreshCBox.Items.Add(item.ToString(CultureInfo.InvariantCulture));
                this.LEDRefreshCBox.Items.Add(item.ToString(CultureInfo.InvariantCulture));
            }
            this.DeltaRangeCBox.Items.Add("-0.5 / +0.5 seconds");
            this.DeltaRangeCBox.Items.Add("-1.0 / +1.0 seconds");
            this.DeltaRangeCBox.Items.Add("-1.5 / +1.5 seconds");
            this.DeltaRangeCBox.Items.Add("-2.0 / +2.0 seconds");
            //Setup Data Binding
            var displayRateBinding = new Binding("DisplayRefreshRate") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(this.DisplayRefreshCBox, Selector.SelectedValueProperty, displayRateBinding);
            var ledRateBinding = new Binding("LEDRefreshRate") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(this.LEDRefreshCBox, Selector.SelectedValueProperty, ledRateBinding);
            var numberDisplayBinding = new Binding("NumberDisplays") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(this.NumberDisplayUnitsCBox, Selector.SelectedIndexProperty, numberDisplayBinding);
            var lapDisplayTimeBinding = new Binding("LapTimeDisplayTime") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(this.LapDisplayTimeCBox, Selector.SelectedIndexProperty, lapDisplayTimeBinding);
            var quickInfoTimeBinding = new Binding("QuickInfoDisplayTime") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(this.QuickInfoDisplayTimeCBox, Selector.SelectedIndexProperty, quickInfoTimeBinding);
            var intensityBinding = new Binding("Intensity") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(this.IntensityCBox, Selector.SelectedIndexProperty, intensityBinding);
            var headerDisplayTimeBinding = new Binding("HeaderDisplayTime") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(this.HeaderDisplayTimeCBox, Selector.SelectedIndexProperty, headerDisplayTimeBinding);
            var showHeaderBinding = new Binding("ShowHeader") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(this.ShowHeaderCheck, ToggleButton.IsCheckedProperty, showHeaderBinding);

            var shiftIntensityBinding = new Binding("ShiftIntensity") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(this.ShiftIntensityCheck, ToggleButton.IsCheckedProperty, shiftIntensityBinding);
            var shiftIntensityTypeBinding = new Binding("ShiftIntensityType") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(this.ShiftIntensityTypeCheck, ToggleButton.IsCheckedProperty,
                                         shiftIntensityTypeBinding);
            var shiftIntensityAmountBinding = new Binding("ShiftIntensityAmount") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(this.ShiftIntensityAmountCBox, Selector.SelectedIndexProperty,
                                         shiftIntensityAmountBinding);

            var deltaDefaultBinding = new Binding("DeltaLightsOnDefault") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(this.DeltaDefaultOnCheck, ToggleButton.IsCheckedProperty, deltaDefaultBinding);
            var deltaColourBinding = new Binding("ColourDeltaByDD") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(this.DeltaColourCheck, ToggleButton.IsCheckedProperty, deltaColourBinding);
            var deltaRangeBinding = new Binding("DeltaRange") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(this.DeltaRangeCBox, Selector.SelectedIndexProperty, deltaRangeBinding);
            var deltaMessageScreenBinding = new Binding("DeltaMessageScreen") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(this.DeltaMessageScreenCBox, Selector.SelectedIndexProperty, deltaMessageScreenBinding);
        }

        private void ShiftIntensityCheckChecked(object sender, RoutedEventArgs e)
        {
            this.ShiftIntensityAmountCBox.IsEnabled = true;
            this.ShiftIntensityTypeCheck.IsEnabled = true;
            this.ShiftIntensityAmountLabel.IsEnabled = true;
        }

        private void ShiftIntensityCheckUnchecked(object sender, RoutedEventArgs e)
        {
            this.ShiftIntensityAmountCBox.IsEnabled = false;
            this.ShiftIntensityTypeCheck.IsEnabled = false;
            this.ShiftIntensityAmountLabel.IsEnabled = false;
        }

        private void ShiftIntensityTypeCheckUnchecked(object sender, RoutedEventArgs e)
        {
            this.ShiftIntensityAmountCBox.Items.Clear();
            for (int i = 0; i <= Constants.MaxIntensityTM; i++)
            {
                this.ShiftIntensityAmountCBox.Items.Add(i.ToString(CultureInfo.InvariantCulture));
            }
        }

        private void ShiftIntensityTypeCheckChecked(object sender, RoutedEventArgs e)
        {
            this.ShiftIntensityAmountCBox.Items.Clear();
            for (int i = 1; i <= Constants.MaxIntensityTM; i++)
            {
                this.ShiftIntensityAmountCBox.Items.Add("+" + i.ToString(CultureInfo.InvariantCulture));
            }
        }
    }
}