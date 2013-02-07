//========================================//
// iRduino - Created by Mark Silverwood  //
//======================================//

namespace iRduino.Windows.Pages
{
    using iRduino.Classes;

    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;

    /// <summary>
    ///     Interaction logic for LEDsPage.xaml
    /// </summary>
    public partial class LEDsPage
    {
        public DisplayUnitConfiguration Temp;

        public LEDsPage()
        {
            this.InitializeComponent();
        }


        private void ShowShiftLightsCheckUnchecked(object sender, RoutedEventArgs e)
        {
            this.ShiftLightStyleCBox.IsEnabled = false;
            this.ShiftLightStyleLabel.IsEnabled = false;
            this.ShiftClumpsCheck.IsEnabled = false;
            this.MatchShiftLightsCheck.IsEnabled = false;
            this.MatchStyleOptionLabel.IsEnabled = false;
            this.MatchShiftLightsCheck.IsChecked = false;
            this.MatchShiftLightsOptionCBox.IsEnabled = false;
            this.UseRedShiftCheck.IsEnabled = false;
            this.UseRedShiftCheck.IsChecked = false;
        }

        private void ShowPitLightsCheckUnchecked(object sender, RoutedEventArgs e)
        {
            this.PitLightSpeedCBox.IsEnabled = false;
            this.PitLimiterFlashSpeedLabel.IsEnabled = false;
            this.PitLimiterStyleLabel.IsEnabled = false;
            this.PitLightStyleCBox.IsEnabled = false;
        }

        private void ShowRevLightsCheckUnchecked(object sender, RoutedEventArgs e)
        {
            this.RevLightStyleCBox.IsEnabled = false;
            this.RevLimiterStyleLabel.IsEnabled = false;
        }

        private void ShowFFBCheckUnchecked(object sender, RoutedEventArgs e)
        {
            this.FFBClipScreenCBox.IsEnabled = false;
            this.FFBWhichScreenLabel.IsEnabled = false;
        }

        private void ShowShiftLightsCheckChecked(object sender, RoutedEventArgs e)
        {
            this.ShiftLightStyleCBox.IsEnabled = true;
            this.ShiftLightStyleLabel.IsEnabled = true;
            this.ShiftClumpsCheck.IsEnabled = true;
            if (this.ShiftLightStyleCBox.SelectedIndex == -1)
            {
                this.ShiftLightStyleCBox.SelectedIndex = 0;
            }
            this.MatchShiftLightsCheck.IsEnabled = true;
            //MatchShiftLightsOptionCBox.IsEnabled = true;
            //UseRedShiftCheck.IsEnabled = true;
        }

        private void ShowPitLightsCheckChecked(object sender, RoutedEventArgs e)
        {
            this.PitLightStyleCBox.IsEnabled = true;
            this.PitLimiterStyleLabel.IsEnabled = true;
            this.PitLimiterFlashSpeedLabel.IsEnabled = true;
            this.PitLightSpeedCBox.IsEnabled = true;
            if (this.PitLightStyleCBox.SelectedIndex == -1)
            {
                this.PitLightStyleCBox.SelectedIndex = 0;
            }
            if (this.PitLightSpeedCBox.SelectedIndex == -1)
            {
                this.PitLightSpeedCBox.SelectedIndex = 0;
            }
        }

        private void ShowRevLightsCheckChecked(object sender, RoutedEventArgs e)
        {
            this.RevLightStyleCBox.IsEnabled = true;
            this.RevLimiterStyleLabel.IsEnabled = true;
            if (this.RevLightStyleCBox.SelectedIndex == -1)
            {
                this.RevLightStyleCBox.SelectedIndex = 0;
            }
        }

        private void ShowFFBCheckChecked(object sender, RoutedEventArgs e)
        {
            this.FFBClipScreenCBox.IsEnabled = true;
            this.FFBWhichScreenLabel.IsEnabled = true;
            if (this.FFBClipScreenCBox.SelectedIndex == -1)
            {
                this.FFBClipScreenCBox.SelectedIndex = 0;
            }
        }

        private void PageLoaded1(object sender, RoutedEventArgs e)
        {
            this.FFBWhichScreenLabel.IsEnabled = false;
            this.RevLimiterStyleLabel.IsEnabled = false;
            this.PitLimiterFlashSpeedLabel.IsEnabled = false;
            this.PitLimiterStyleLabel.IsEnabled = false;
            this.ShiftLightStyleLabel.IsEnabled = false;
            this.MatchStyleOptionLabel.IsEnabled = false;
            
            //Populate CBOXs
            this.Temp = (DisplayUnitConfiguration) this.DataContext;
            foreach (var item in this.Temp.HostApp.DisplayMngr.Dictionarys.ShiftStyles)
            {
                this.ShiftLightStyleCBox.Items.Add(item.Key);
            }
            foreach (var item in this.Temp.HostApp.DisplayMngr.Dictionarys.RevFlashStyles)
            {
                this.RevLightStyleCBox.Items.Add(item.Key);
            }
            foreach (var item in this.Temp.HostApp.DisplayMngr.Dictionarys.PitFlashStyles)
            {
                this.PitLightStyleCBox.Items.Add(item.Key);
            }
            foreach (var item in this.Temp.HostApp.DisplayMngr.Dictionarys.PitFlashSpeeds)
            {
                this.PitLightSpeedCBox.Items.Add(item.Key);
            }
            for (int i = 1; i <= this.Temp.NumScreens + 1; i++)
            {
                this.FFBClipScreenCBox.Items.Add(i.ToString(CultureInfo.InvariantCulture));
            }
            foreach (var item in this.Temp.HostApp.DisplayMngr.Dictionarys.MatchCarShiftOptions)
            {
                this.MatchShiftLightsOptionCBox.Items.Add(item.Key);
            }
            //Data Bind
            var showShiftLightsBinding = new Binding("LEDsConfigurations.ShowShiftLights") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(this.ShowShiftLightsCheck, ToggleButton.IsCheckedProperty, showShiftLightsBinding);
            var matchShiftLightsBinding = new Binding("LEDsConfigurations.MatchCarShiftLights") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(this.MatchShiftLightsCheck, ToggleButton.IsCheckedProperty, matchShiftLightsBinding);
            var matchShiftOptionBinding = new Binding("LEDsConfigurations.MatchCarShiftOptions") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(
                this.MatchShiftLightsOptionCBox, Selector.SelectedValueProperty, matchShiftOptionBinding);
            var useMatchRedShiftBinding = new Binding("LEDsConfigurations.MatchRedShift") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(this.UseRedShiftCheck, ToggleButton.IsCheckedProperty, useMatchRedShiftBinding);
            var showPitLightsBinding = new Binding("LEDsConfigurations.PitLights") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(this.ShowPitLightsCheck, ToggleButton.IsCheckedProperty, showPitLightsBinding);
            var showRevLightsBinding = new Binding("LEDsConfigurations.RevLimiterLights") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(this.ShowRevLightsCheck, ToggleButton.IsCheckedProperty, showRevLightsBinding);
            var ffbClippingBinding = new Binding("LEDsConfigurations.FFBClippingLights") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(this.ShowFFBCheck, ToggleButton.IsCheckedProperty, ffbClippingBinding);
            var ffbClipScreenBinding = new Binding("LEDsConfigurations.FFBClippingScreen") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(this.FFBClipScreenCBox, Selector.SelectedIndexProperty, ffbClipScreenBinding);
            var shiftLightStyleBinding = new Binding("LEDsConfigurations.ShiftLightStyle") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(this.ShiftLightStyleCBox, Selector.SelectedItemProperty, shiftLightStyleBinding);
            var pitLightStyleBinding = new Binding("LEDsConfigurations.PitLimiterStyle") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(this.PitLightStyleCBox, Selector.SelectedItemProperty, pitLightStyleBinding);
            var pitLightSpeedBinding = new Binding("LEDsConfigurations.PitLimiterSpeed") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(this.PitLightSpeedCBox, Selector.SelectedItemProperty, pitLightSpeedBinding);
            var revLightStyleBinding = new Binding("LEDsConfigurations.RevLimiterStyle") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(this.RevLightStyleCBox, Selector.SelectedItemProperty, revLightStyleBinding);
            var shiftClumpsBinding = new Binding("LEDsConfigurations.ShiftClumps") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(this.ShiftClumpsCheck, ToggleButton.IsCheckedProperty, shiftClumpsBinding);
        }

        private void MatchShiftLightsCheckChecked(object sender, RoutedEventArgs e)
        {
            this.MatchShiftLightsOptionCBox.IsEnabled = true;
            this.MatchStyleOptionLabel.IsEnabled = true;
            this.UseRedShiftCheck.IsEnabled = true;
        }

        private void MatchShiftLightsCheckUnchecked(object sender, RoutedEventArgs e)
        {
            this.MatchShiftLightsOptionCBox.IsEnabled = false;
            this.MatchStyleOptionLabel.IsEnabled = false;
            this.UseRedShiftCheck.IsChecked = false;
            this.UseRedShiftCheck.IsEnabled = false;
        }
    }
}