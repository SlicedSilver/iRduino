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
    ///     Interaction logic for UnitPage.xaml
    /// </summary>
    public partial class UnitPage
    {
        //public MyPropertyPathHelper.Dummy temp = new MyPropertyPathHelper.Dummy();
        public Binding NumberScreensBinding;

        public UnitPage()
        {
            this.InitializeComponent();
        }

        private void ShowLapCheckChecked(object sender, RoutedEventArgs e)
        {
            this.LapDisplayStyleCBox.IsEnabled = true;
            this.LapDisplayStyleLabel.IsEnabled = true;
        }

        private void ShowLapCheckUnchecked(object sender, RoutedEventArgs e)
        {
            this.LapDisplayStyleCBox.IsEnabled = false;
            this.LapDisplayStyleLabel.IsEnabled = false;
        }

        private void PageLoaded1(object sender, RoutedEventArgs e)
        {
            this.DeltaLightsOptionsLabel.IsEnabled = false;
            this.LapDisplayStyleLabel.IsEnabled = false;
            this.DCDisplayTimeLabel.IsEnabled = false;

            for (int i = 1; i <= Constants.MaxNumberScreensPerUnit; i++)
            {
                this.NumberScreensCBox.Items.Add(i.ToString(CultureInfo.InvariantCulture));
            }
            for (int i = 1; i <= Constants.MaxDCDisplayLength; i++)
            {
                this.DCDisplayTimeCBox.Items.Add(i.ToString(CultureInfo.InvariantCulture));
            }
            var temp = (DisplayUnitConfiguration) this.DataContext;
            foreach (var lapStyle in temp.HostApp.DisplayMngr.Dictionarys.LapDisplayStyles)
            {
                this.LapDisplayStyleCBox.Items.Add(lapStyle.Key);
            }
            foreach (var item in temp.HostApp.DisplayMngr.Dictionarys.DeltaLightsPositionOptions)
            {
                this.DeltaLightsOptionCBox.Items.Add(item.Key);
            }
            foreach (var item in temp.HostApp.DisplayMngr.Dictionarys.WarningTypes)
            {
                this.EngineWarningsTypeCBox.Items.Add(item.Key);
            }
            //Setup Data Binding
            this.NumberScreensBinding = new Binding("NumScreens") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(this.NumberScreensCBox, Selector.SelectedIndexProperty, this.NumberScreensBinding);

            var showLapBinding = new Binding("ShowLap") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(this.ShowLapCheck, ToggleButton.IsCheckedProperty, showLapBinding);
            var lapDisplayStyleBinding = new Binding("LapStyle") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(this.LapDisplayStyleCBox, Selector.SelectedItemProperty, lapDisplayStyleBinding);
            this.LapDisplayStyleCBox.SelectedItem = temp.LapStyle;
            var invertedBinding = new Binding("Inverted") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(this.InvertedCheck, ToggleButton.IsCheckedProperty, invertedBinding);
            var showDCBinding = new Binding("ShowDC") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(this.ShowDCCheck, ToggleButton.IsCheckedProperty, showDCBinding);
            var dcDisplayTimeBinding = new Binding("DCDisplayTime") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(this.DCDisplayTimeCBox, Selector.SelectedIndexProperty, dcDisplayTimeBinding);
            var switchBinding = new Binding("SwitchLEDs") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(this.SwitchLEDsCheck, ToggleButton.IsCheckedProperty, switchBinding);
            var tm1640Binding = new Binding("IsTM1640") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(this.TM1640Check, ToggleButton.IsCheckedProperty, tm1640Binding);
            var deltaLightsUnitBinding = new Binding("LEDsConfigurations.DeltaLightsShow") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(this.UseDeltaLightsCheck, ToggleButton.IsCheckedProperty, deltaLightsUnitBinding);
            var deltaOptionsBinding = new Binding("LEDsConfigurations.DeltaLightsPosition") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(this.DeltaLightsOptionCBox, Selector.SelectedItemProperty, deltaOptionsBinding);
            var showEngineWarningsBinding = new Binding("ShowEngineWarnings") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(this.UseEngineWarningsCheck, ToggleButton.IsCheckedProperty, showEngineWarningsBinding);
            var engineWarningsOptionBinding = new Binding("WarningType") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(this.EngineWarningsTypeCBox, Selector.SelectedItemProperty, engineWarningsOptionBinding);
  }

        private void ShowDCCheckChecked(object sender, RoutedEventArgs e)
        {
            this.DCDisplayTimeCBox.IsEnabled = true;
            this.DCDisplayTimeLabel.IsEnabled = true;
        }

        private void ShowDCCheckUnchecked(object sender, RoutedEventArgs e)
        {
            this.DCDisplayTimeCBox.IsEnabled = false;
            this.DCDisplayTimeLabel.IsEnabled = false;
        }

        private void TM1640CheckChecked(object sender, RoutedEventArgs e)
        {
            this.InvertedCheck.IsChecked = false;
            this.InvertedCheck.IsEnabled = false;
            this.SwitchLEDsCheck.IsChecked = false;
            this.SwitchLEDsCheck.IsEnabled = false;
        }

        private void TM1640CheckUnchecked(object sender, RoutedEventArgs e)
        {
            this.InvertedCheck.IsEnabled = false;
            this.SwitchLEDsCheck.IsEnabled = false;
        }

        private void UseDeltaLightsCheckChecked(object sender, RoutedEventArgs e)
        {
            this.DeltaLightsOptionCBox.IsEnabled = true;
            this.DeltaLightsOptionsLabel.IsEnabled = true;
        }

        private void UseDeltaLightsCheckUnchecked(object sender, RoutedEventArgs e)
        {
            this.DeltaLightsOptionCBox.IsEnabled = false;
            this.DeltaLightsOptionsLabel.IsEnabled = false;
        }

        private void UseEngineWarningsCheckChecked(object sender, RoutedEventArgs e)
        {
            this.EngineWarningsTypeCBox.IsEnabled = true;
            this.EngineWarningsTypeLabel.IsEnabled = true;
        }

        private void UseEngineWarningsCheckUnchecked(object sender, RoutedEventArgs e)
        {
            this.EngineWarningsTypeCBox.IsEnabled = false;
            this.EngineWarningsTypeLabel.IsEnabled = false;
        }
    }
}