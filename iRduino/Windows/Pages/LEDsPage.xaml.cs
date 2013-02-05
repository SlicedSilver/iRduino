//========================================//
// iRduino - Created by Mark Silverwood  //
//======================================//

namespace iRduino.Pages
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
            InitializeComponent();
        }


        private void ShowShiftLightsCheckUnchecked(object sender, RoutedEventArgs e)
        {
            ShiftLightStyleCBox.IsEnabled = false;
            ShiftLightStyleLabel.IsEnabled = false;
            ShiftClumpsCheck.IsEnabled = false;
            MatchShiftLightsCheck.IsEnabled = false;
            MatchStyleOptionLabel.IsEnabled = false;
            MatchShiftLightsCheck.IsChecked = false;
            MatchShiftLightsOptionCBox.IsEnabled = false;
            UseRedShiftCheck.IsEnabled = false;
            UseRedShiftCheck.IsChecked = false;
        }

        private void ShowPitLightsCheckUnchecked(object sender, RoutedEventArgs e)
        {
            PitLightSpeedCBox.IsEnabled = false;
            PitLimiterFlashSpeedLabel.IsEnabled = false;
            PitLimiterStyleLabel.IsEnabled = false;
            PitLightStyleCBox.IsEnabled = false;
        }

        private void ShowRevLightsCheckUnchecked(object sender, RoutedEventArgs e)
        {
            RevLightStyleCBox.IsEnabled = false;
            RevLimiterStyleLabel.IsEnabled = false;
        }

        private void ShowFFBCheckUnchecked(object sender, RoutedEventArgs e)
        {
            FFBClipScreenCBox.IsEnabled = false;
            FFBWhichScreenLabel.IsEnabled = false;
        }

        private void ShowShiftLightsCheckChecked(object sender, RoutedEventArgs e)
        {
            ShiftLightStyleCBox.IsEnabled = true;
            ShiftLightStyleLabel.IsEnabled = true;
            ShiftClumpsCheck.IsEnabled = true;
            if (ShiftLightStyleCBox.SelectedIndex == -1)
            {
                ShiftLightStyleCBox.SelectedIndex = 0;
            }
            MatchShiftLightsCheck.IsEnabled = true;
            //MatchShiftLightsOptionCBox.IsEnabled = true;
            //UseRedShiftCheck.IsEnabled = true;
        }

        private void ShowPitLightsCheckChecked(object sender, RoutedEventArgs e)
        {
            PitLightStyleCBox.IsEnabled = true;
            PitLimiterStyleLabel.IsEnabled = true;
            PitLimiterFlashSpeedLabel.IsEnabled = true;
            PitLightSpeedCBox.IsEnabled = true;
            if (PitLightStyleCBox.SelectedIndex == -1)
            {
                PitLightStyleCBox.SelectedIndex = 0;
            }
            if (PitLightSpeedCBox.SelectedIndex == -1)
            {
                PitLightSpeedCBox.SelectedIndex = 0;
            }
        }

        private void ShowRevLightsCheckChecked(object sender, RoutedEventArgs e)
        {
            RevLightStyleCBox.IsEnabled = true;
            RevLimiterStyleLabel.IsEnabled = true;
            if (RevLightStyleCBox.SelectedIndex == -1)
            {
                RevLightStyleCBox.SelectedIndex = 0;
            }
        }

        private void ShowFFBCheckChecked(object sender, RoutedEventArgs e)
        {
            FFBClipScreenCBox.IsEnabled = true;
            FFBWhichScreenLabel.IsEnabled = true;
            if (FFBClipScreenCBox.SelectedIndex == -1)
            {
                FFBClipScreenCBox.SelectedIndex = 0;
            }
        }

        private void PageLoaded1(object sender, RoutedEventArgs e)
        {
            FFBWhichScreenLabel.IsEnabled = false;
            RevLimiterStyleLabel.IsEnabled = false;
            PitLimiterFlashSpeedLabel.IsEnabled = false;
            PitLimiterStyleLabel.IsEnabled = false;
            ShiftLightStyleLabel.IsEnabled = false;
            MatchStyleOptionLabel.IsEnabled = false;
            
            //Populate CBOXs
            Temp = (DisplayUnitConfiguration) DataContext;
            foreach (var item in Temp.HostApp.DisplayMngr.Dictionarys.ShiftStyles)
            {
                ShiftLightStyleCBox.Items.Add(item.Key);
            }
            foreach (var item in Temp.HostApp.DisplayMngr.Dictionarys.RevFlashStyles)
            {
                RevLightStyleCBox.Items.Add(item.Key);
            }
            foreach (var item in Temp.HostApp.DisplayMngr.Dictionarys.PitFlashStyles)
            {
                PitLightStyleCBox.Items.Add(item.Key);
            }
            foreach (var item in Temp.HostApp.DisplayMngr.Dictionarys.PitFlashSpeeds)
            {
                PitLightSpeedCBox.Items.Add(item.Key);
            }
            for (int i = 1; i <= Temp.NumScreens + 1; i++)
            {
                FFBClipScreenCBox.Items.Add(i.ToString(CultureInfo.InvariantCulture));
            }
            foreach (var item in Temp.HostApp.DisplayMngr.Dictionarys.MatchCarShiftOptions)
            {
                MatchShiftLightsOptionCBox.Items.Add(item.Key);
            }
            //Data Bind
            var showShiftLightsBinding = new Binding("LEDsConfigurations.ShowShiftLights") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(ShowShiftLightsCheck, ToggleButton.IsCheckedProperty, showShiftLightsBinding);
            var matchShiftLightsBinding = new Binding("LEDsConfigurations.MatchCarShiftLights") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(MatchShiftLightsCheck, ToggleButton.IsCheckedProperty, matchShiftLightsBinding);
            var matchShiftOptionBinding = new Binding("LEDsConfigurations.MatchCarShiftOptions") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(
                MatchShiftLightsOptionCBox, Selector.SelectedValueProperty, matchShiftOptionBinding);
            var useMatchRedShiftBinding = new Binding("LEDsConfigurations.MatchRedShift") { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(UseRedShiftCheck, ToggleButton.IsCheckedProperty, useMatchRedShiftBinding);
            var showPitLightsBinding = new Binding("LEDsConfigurations.PitLights") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(ShowPitLightsCheck, ToggleButton.IsCheckedProperty, showPitLightsBinding);
            var showRevLightsBinding = new Binding("LEDsConfigurations.RevLimiterLights") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(ShowRevLightsCheck, ToggleButton.IsCheckedProperty, showRevLightsBinding);
            var ffbClippingBinding = new Binding("LEDsConfigurations.FFBClippingLights") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(ShowFFBCheck, ToggleButton.IsCheckedProperty, ffbClippingBinding);
            var ffbClipScreenBinding = new Binding("LEDsConfigurations.FFBClippingScreen") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(FFBClipScreenCBox, Selector.SelectedIndexProperty, ffbClipScreenBinding);
            var shiftLightStyleBinding = new Binding("LEDsConfigurations.ShiftLightStyle") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(ShiftLightStyleCBox, Selector.SelectedItemProperty, shiftLightStyleBinding);
            var pitLightStyleBinding = new Binding("LEDsConfigurations.PitLimiterStyle") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(PitLightStyleCBox, Selector.SelectedItemProperty, pitLightStyleBinding);
            var pitLightSpeedBinding = new Binding("LEDsConfigurations.PitLimiterSpeed") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(PitLightSpeedCBox, Selector.SelectedItemProperty, pitLightSpeedBinding);
            var revLightStyleBinding = new Binding("LEDsConfigurations.RevLimiterStyle") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(RevLightStyleCBox, Selector.SelectedItemProperty, revLightStyleBinding);
            var shiftClumpsBinding = new Binding("LEDsConfigurations.ShiftClumps") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(ShiftClumpsCheck, ToggleButton.IsCheckedProperty, shiftClumpsBinding);
        }

        private void MatchShiftLightsCheckChecked(object sender, RoutedEventArgs e)
        {
            MatchShiftLightsOptionCBox.IsEnabled = true;
            MatchStyleOptionLabel.IsEnabled = true;
            UseRedShiftCheck.IsEnabled = true;
        }

        private void MatchShiftLightsCheckUnchecked(object sender, RoutedEventArgs e)
        {
            MatchShiftLightsOptionCBox.IsEnabled = false;
            MatchStyleOptionLabel.IsEnabled = false;
            UseRedShiftCheck.IsChecked = false;
            UseRedShiftCheck.IsEnabled = false;
        }
    }
}