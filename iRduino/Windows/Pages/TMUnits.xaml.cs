//========================================//
// iRduino - Created by Mark Silverwood  //
//======================================//

namespace iRduino.Pages
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
            InitializeComponent();
        }

        private void ShowHeaderCheckChecked(object sender, RoutedEventArgs e)
        {
            HeaderDisplayTimeCBox.IsEnabled = true;
            HeaderDisplayTimeLabel.IsEnabled = true;
        }

        private void ShowHeaderCheckUnchecked(object sender, RoutedEventArgs e)
        {
            HeaderDisplayTimeCBox.IsEnabled = false;
            HeaderDisplayTimeLabel.IsEnabled = false;
        }

        private void PageLoaded1(object sender, RoutedEventArgs e)
        {
            HeaderDisplayTimeLabel.IsEnabled = false;
            ShiftIntensityAmountLabel.IsEnabled = false;


            for (int i = 1; i <= Constants.MaxNumberTM1638Units; i++)
            {
                NumberDisplayUnitsCBox.Items.Add(i.ToString(CultureInfo.InvariantCulture));
                LapDisplayTimeCBox.Items.Add(i.ToString(CultureInfo.InvariantCulture));
                QuickInfoDisplayTimeCBox.Items.Add(i.ToString(CultureInfo.InvariantCulture));
                HeaderDisplayTimeCBox.Items.Add(i.ToString(CultureInfo.InvariantCulture));
            }
            for (int i = 0; i <= Constants.MaxIntensityTM; i++)
            {
                IntensityCBox.Items.Add(i.ToString(CultureInfo.InvariantCulture));
                ShiftIntensityAmountCBox.Items.Add(i.ToString(CultureInfo.InvariantCulture));
            }
            
            var temp = (ConfigurationOptions)DataContext;
            for (int i = 0; i <= temp.DisplayUnitConfigurations.Count; i++)
            {
                deltaMessageScreenCBox.Items.Add((i + 1).ToString(CultureInfo.InvariantCulture));
            }
            deltaRangeCBox.Items.Add("-0.5 / +0.5 seconds");
            deltaRangeCBox.Items.Add("-1.0 / +1.0 seconds");
            deltaRangeCBox.Items.Add("-1.5 / +1.5 seconds");
            deltaRangeCBox.Items.Add("-2.0 / +2.0 seconds");
            //Setup Data Binding
            var numberDisplayBinding = new Binding("NumberDisplays") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(NumberDisplayUnitsCBox, Selector.SelectedIndexProperty, numberDisplayBinding);
            var lapDisplayTimeBinding = new Binding("LapTimeDisplayTime") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(LapDisplayTimeCBox, Selector.SelectedIndexProperty, lapDisplayTimeBinding);
            var quickInfoTimeBinding = new Binding("QuickInfoDisplayTime") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(QuickInfoDisplayTimeCBox, Selector.SelectedIndexProperty, quickInfoTimeBinding);
            var intensityBinding = new Binding("Intensity") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(IntensityCBox, Selector.SelectedIndexProperty, intensityBinding);
            var headerDisplayTimeBinding = new Binding("HeaderDisplayTime") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(HeaderDisplayTimeCBox, Selector.SelectedIndexProperty, headerDisplayTimeBinding);
            var showHeaderBinding = new Binding("ShowHeader") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(ShowHeaderCheck, ToggleButton.IsCheckedProperty, showHeaderBinding);

            var shiftIntensityBinding = new Binding("ShiftIntensity") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(ShiftIntensityCheck, ToggleButton.IsCheckedProperty, shiftIntensityBinding);
            var shiftIntensityTypeBinding = new Binding("ShiftIntensityType") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(ShiftIntensityTypeCheck, ToggleButton.IsCheckedProperty,
                                         shiftIntensityTypeBinding);
            var shiftIntensityAmountBinding = new Binding("ShiftIntensityAmount") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(ShiftIntensityAmountCBox, Selector.SelectedIndexProperty,
                                         shiftIntensityAmountBinding);

            var deltaDefaultBinding = new Binding("DeltaLightsOnDefault") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(deltaDefaultOnCheck, ToggleButton.IsCheckedProperty, deltaDefaultBinding);
            var deltaColourBinding = new Binding("ColourDeltaByDD") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(deltaColourCheck, ToggleButton.IsCheckedProperty, deltaColourBinding);
            var deltaRangeBinding = new Binding("DeltaRange") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(deltaRangeCBox, Selector.SelectedIndexProperty, deltaRangeBinding);
            var deltaMessageScreenBinding = new Binding("DeltaMessageScreen") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(deltaMessageScreenCBox, Selector.SelectedIndexProperty, deltaMessageScreenBinding);
        }

        private void ShiftIntensityCheckChecked(object sender, RoutedEventArgs e)
        {
            ShiftIntensityAmountCBox.IsEnabled = true;
            ShiftIntensityTypeCheck.IsEnabled = true;
            ShiftIntensityAmountLabel.IsEnabled = true;
        }

        private void ShiftIntensityCheckUnchecked(object sender, RoutedEventArgs e)
        {
            ShiftIntensityAmountCBox.IsEnabled = false;
            ShiftIntensityTypeCheck.IsEnabled = false;
            ShiftIntensityAmountLabel.IsEnabled = false;
        }

        private void ShiftIntensityTypeCheckUnchecked(object sender, RoutedEventArgs e)
        {
            ShiftIntensityAmountCBox.Items.Clear();
            for (int i = 0; i <= Constants.MaxIntensityTM; i++)
            {
                ShiftIntensityAmountCBox.Items.Add(i.ToString(CultureInfo.InvariantCulture));
            }
        }

        private void ShiftIntensityTypeCheckChecked(object sender, RoutedEventArgs e)
        {
            ShiftIntensityAmountCBox.Items.Clear();
            for (int i = 1; i <= Constants.MaxIntensityTM; i++)
            {
                ShiftIntensityAmountCBox.Items.Add("+" + i.ToString(CultureInfo.InvariantCulture));
            }
        }
    }
}