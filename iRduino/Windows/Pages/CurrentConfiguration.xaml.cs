//========================================//
// iRduino - Created by Mark Silverwood  //
//======================================//

namespace iRduino.Windows.Pages
{
    using ArduinoInterfaces;

    using iRduino.Classes;

    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;

    /// <summary>
    ///     Interaction logic for CurrentConfiguration.xaml
    /// </summary>
    public partial class CurrentConfiguration
    {
        private bool startingUp = true;
        
        public CurrentConfiguration()
        {
            this.InitializeComponent();
        }

        private void PageLoaded1(object sender, RoutedEventArgs e)
        {
            this.NumberControllersLabel.IsEnabled = false;
            for (int i = 0; i <= Constants.MaxIntensityTM; i++)
            {
                this.ControllerNumCBox.Items.Add(i.ToString(CultureInfo.InvariantCulture));
            }
            var temp = (ConfigurationOptions) this.DataContext;
            foreach (var com in temp.Dictionarys.ComPorts)
            {
                this.PrefComPortCBox.Items.Add(com.Value);
            }
            this.UseDXUnitsCheck.IsChecked = temp.DisplayUnitConfigurations.Count != 0;
            //Setup Data Binding
            var nameBinding = new Binding("Name") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(this.NameTextBox, TextBox.TextProperty, nameBinding);
            var prefComBinding = new Binding("PreferredComPort") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(this.PrefComPortCBox, Selector.SelectedIndexProperty, prefComBinding);
            var contNumBinding = new Binding("NumberControllers") {Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(this.ControllerNumCBox, Selector.SelectedIndexProperty, contNumBinding);
            this.UseControllersCheck.IsChecked = this.ControllerNumCBox.SelectedIndex > 0;
            this.startingUp = false;
        }

        private void UseDxUnitsCheckChecked(object sender, RoutedEventArgs e)
        {
            if (this.startingUp) //prevents event from travelling further
            {
                e.Handled = true;
            }
        }

        private void UseDxUnitsCheckUnchecked(object sender, RoutedEventArgs e)
        {
            if (this.startingUp)
            {
                e.Handled = true;
            }
        }

        private void UseControllersCheckUnchecked(object sender, RoutedEventArgs e)
        {
            if (!this.startingUp)
            {
                this.ControllerNumCBox.SelectedIndex = 0;
                //e.Handled = true;
            }
            this.ControllerNumCBox.IsEnabled = false;
            this.NumberControllersLabel.IsEnabled = false;
        }

        private void UseControllersCheckChecked(object sender, RoutedEventArgs e)
        {
            this.ControllerNumCBox.IsEnabled = true;
            this.NumberControllersLabel.IsEnabled = true;
            if (!this.startingUp)
            {
                this.ControllerNumCBox.SelectedIndex = 0;
                //e.Handled = true;
            }
        }

        private void ControllerNumCBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.startingUp)
            {
                e.Handled = true;
            }
        }

    }
}