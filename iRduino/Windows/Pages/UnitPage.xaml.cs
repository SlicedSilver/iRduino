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
    ///     Interaction logic for UnitPage.xaml
    /// </summary>
    public partial class UnitPage
    {
        //public MyPropertyPathHelper.Dummy temp = new MyPropertyPathHelper.Dummy();
        public Binding NumberScreensBinding;

        public UnitPage()
        {
            InitializeComponent();
        }

        private void ShowLapCheckChecked(object sender, RoutedEventArgs e)
        {
            LapDisplayStyleCBox.IsEnabled = true;
            LapDisplayStyleLabel.IsEnabled = true;
        }

        private void ShowLapCheckUnchecked(object sender, RoutedEventArgs e)
        {
            LapDisplayStyleCBox.IsEnabled = false;
            LapDisplayStyleLabel.IsEnabled = false;
        }

        private void PageLoaded1(object sender, RoutedEventArgs e)
        {
            DeltaLightsOptionsLabel.IsEnabled = false;
            LapDisplayStyleLabel.IsEnabled = false;
            DCDisplayTimeLabel.IsEnabled = false;

            for (int i = 1; i <= Constants.MaxNumberScreensPerUnit; i++)
            {
                NumberScreensCBox.Items.Add(i.ToString(CultureInfo.InvariantCulture));
            }
            for (int i = 1; i <= Constants.MaxDCDisplayLength; i++)
            {
                DCDisplayTimeCBox.Items.Add(i.ToString(CultureInfo.InvariantCulture));
            }
            var temp = (DisplayUnitConfiguration) DataContext;
            foreach (var lapStyle in temp.HostApp.DisplayMngr.Dictionarys.LapDisplayStyles)
            {
                LapDisplayStyleCBox.Items.Add(lapStyle.Key);
            }
            foreach (var item in temp.HostApp.DisplayMngr.Dictionarys.DeltaLightsPositionOptions)
            {
                DeltaLightsOptionCBox.Items.Add(item.Key);
            }
            //Setup Data Binding
            NumberScreensBinding = new Binding("NumScreens") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(NumberScreensCBox, Selector.SelectedIndexProperty, NumberScreensBinding);

            var showLapBinding = new Binding("ShowLap") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(ShowLapCheck, ToggleButton.IsCheckedProperty, showLapBinding);
            var lapDisplayStyleBinding = new Binding("LapStyle") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(LapDisplayStyleCBox, Selector.SelectedItemProperty, lapDisplayStyleBinding);
            LapDisplayStyleCBox.SelectedItem = temp.LapStyle;
            var invertedBinding = new Binding("Inverted") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(InvertedCheck, ToggleButton.IsCheckedProperty, invertedBinding);
            var showDCBinding = new Binding("ShowDC") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(ShowDCCheck, ToggleButton.IsCheckedProperty, showDCBinding);
            var dcDisplayTimeBinding = new Binding("DCDisplayTime") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(DCDisplayTimeCBox, Selector.SelectedIndexProperty, dcDisplayTimeBinding);
            var switchBinding = new Binding("SwitchLEDs") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(SwitchLEDsCheck, ToggleButton.IsCheckedProperty, switchBinding);
            var tm1640Binding = new Binding("IsTM1640") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(TM1640Check, ToggleButton.IsCheckedProperty, tm1640Binding);
            var deltaLightsUnitBinding = new Binding("LEDsConfigurations.DeltaLightsShow") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(UseDeltaLightsCheck, ToggleButton.IsCheckedProperty, deltaLightsUnitBinding);
            var deltaOptionsBinding = new Binding("LEDsConfigurations.DeltaLightsPosition") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(DeltaLightsOptionCBox, Selector.SelectedItemProperty, deltaOptionsBinding);
        }

        private void ShowDCCheckChecked(object sender, RoutedEventArgs e)
        {
            DCDisplayTimeCBox.IsEnabled = true;
            DCDisplayTimeLabel.IsEnabled = true;
        }

        private void ShowDCCheckUnchecked(object sender, RoutedEventArgs e)
        {
            DCDisplayTimeCBox.IsEnabled = false;
            DCDisplayTimeLabel.IsEnabled = false;
        }

        private void TM1640CheckChecked(object sender, RoutedEventArgs e)
        {
            InvertedCheck.IsChecked = false;
            InvertedCheck.IsEnabled = false;
            SwitchLEDsCheck.IsChecked = false;
            SwitchLEDsCheck.IsEnabled = false;
        }

        private void TM1640CheckUnchecked(object sender, RoutedEventArgs e)
        {
            InvertedCheck.IsEnabled = false;
            SwitchLEDsCheck.IsEnabled = false;
        }

        private void UseDeltaLightsCheckChecked(object sender, RoutedEventArgs e)
        {
            DeltaLightsOptionCBox.IsEnabled = true;
            DeltaLightsOptionsLabel.IsEnabled = true;
        }

        private void UseDeltaLightsCheckUnchecked(object sender, RoutedEventArgs e)
        {
            DeltaLightsOptionCBox.IsEnabled = false;
            DeltaLightsOptionsLabel.IsEnabled = false;
        }
    }
}