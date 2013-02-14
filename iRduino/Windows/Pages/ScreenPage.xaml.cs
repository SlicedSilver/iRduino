//========================================//
// iRduino - Created by Mark Silverwood  //
//======================================//

namespace iRduino.Windows.Pages
{
    using ArduinoInterfaces;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    using iRduino.Classes;

    /// <summary>
    ///     Interaction logic for ScreenPage.xaml
    /// </summary>
    public partial class ScreenPage
    {
        public DisplayUnitConfiguration Temp;
        private bool firstLoad = true;
        private List<ComboBox> scv;

        private int screen;

        public ScreenPage()
        {
            this.InitializeComponent();
        }

        private void StartLoading()
        {
            this.firstLoad = true;
            this.Temp = (DisplayUnitConfiguration) this.DataContext;
            this.scv = new List<ComboBox>
                {
                    this.ScreenVariable1CBox,
                    this.ScreenVariable2CBox,
                    this.ScreenVariable3CBox,
                    this.ScreenVariable4CBox,
                    this.ScreenVariable5CBox,
                    this.ScreenVariable6CBox
                };
            foreach (var variable in this.Temp.HostApp.DisplayMngr.Dictionarys.DisplayVariables)
            {
                foreach (ComboBox comboBox in this.scv)
                {
                    comboBox.Items.Add(variable.Value.Name);
                }
            }
            PageHelper.StartLoading1(this.scv, ref this.screen, this.Temp, this.UseCustomHeaderCheck, this.HeaderTextBox, ref this.firstLoad);
        }

        private void ScreenVariablesUpdated(object sender, SelectionChangedEventArgs e)
        {
            PageHelper.ScreenVariablesUpdatedExtracted(Constants.MaxDisplayLengthTM1638, "{0} / 8", 6, this.scv, this.Temp, this.SpaceUsedLabel, this.firstLoad);
        }

        private void PageDataContextChanged1(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.DataContext != null)
            {
                this.StartLoading();
            }
            else
            {
                MessageBox.Show("Error Loading... Please Try Again.");
            }
        }

        private void ClearButtonClick(object sender, RoutedEventArgs e)
        {
            foreach (var cb in this.scv)
            {
                cb.SelectedIndex = -1;
            }
        }

        private void UseCustomHeaderCheckUnchecked(object sender, RoutedEventArgs e)
        {
            this.HeaderTextBox.Text = "";
            this.HeaderTextBox.IsEnabled = false;
            this.Temp.Screens[this.screen].UseCustomHeader = false;
        }

        private void UseCustomHeaderCheckChecked(object sender, RoutedEventArgs e)
        {
            this.HeaderTextBox.IsEnabled = true;
            this.Temp.Screens[this.screen].UseCustomHeader = true;
        }

        private void HeaderTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            this.Temp.Screens[this.screen].CustomHeader = this.HeaderTextBox.Text;
        }
    }
}