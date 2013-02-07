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
            foreach (ComboBox cbox in this.scv)
            {
                cbox.SelectedIndex = -1;
                cbox.IsEnabled = false;
            }
            this.scv[0].IsEnabled = true;
            this.screen = this.Temp.ScreenToEdit;
            if (this.Temp.Screens[this.screen].Variables.Count > 0)
            {
                for (int i = 0; i < this.Temp.Screens[this.screen].Variables.Count; i++)
                {
                    this.scv[i].IsEnabled = true;
                    DisplayVarsEnum temp222;
                    if (Enum.TryParse(this.Temp.Screens[this.screen].Variables[i],
                                      out temp222))
                    {
                        this.scv[i].SelectedItem = this.Temp.HostApp.DisplayMngr.Dictionarys.DisplayVariables[temp222].Name;
                    }
                    else
                    {
                        this.scv[i].SelectedIndex = -1;
                    }
                }
            }
            else
            {
                this.scv[0].IsEnabled = true;
                this.scv[0].SelectedIndex = -1;
            }
            this.UseCustomHeaderCheck.IsChecked = this.Temp.Screens[this.screen].UseCustomHeader;
            this.HeaderTextBox.Text = this.Temp.Screens[this.screen].CustomHeader;
            this.firstLoad = false;
        }

        private void ScreenVariablesUpdated(object sender, SelectionChangedEventArgs e)
        {
            this.ScreenVariablesUpdatedExtracted(Constants.MaxDisplayLengthTM1638, "{0} / 8", 6, this.scv, this.Temp, this.SpaceUsedLabel, this.firstLoad);
        }

        protected void ScreenVariablesUpdatedExtracted(int maxDisplayLengthTM1638, string format, int param, List<ComboBox> scvIn, DisplayUnitConfiguration temp, Label spaceUsedLabel, bool firstLoadIn)
        {
            int count = 0;
            int i = 0;
            int stop = 0;
            while (count < maxDisplayLengthTM1638 && i < scvIn.Count)
            {
                stop = i + 1;
                if (scvIn[i].SelectedIndex >= 0)
                {
                    count += temp.HostApp.DisplayMngr.Dictionarys.DisplayVariables.Where(item => item.Value.Name == scvIn[i].SelectedItem.ToString()).Sum(item => item.Value.Length);
                    spaceUsedLabel.Content = String.Format(format, count);
                    if (count > maxDisplayLengthTM1638)
                    {
                        stop = i;
                        i = scvIn.Count;
                    }
                    i++;
                }
                else
                {
                    scvIn[i].IsEnabled = true;
                    scvIn[i].Items.Clear();
                    foreach (var variable in temp.HostApp.DisplayMngr.Dictionarys.DisplayVariables)
                    {
                        if (variable.Value.Length <= maxDisplayLengthTM1638 - count)
                        {
                            scvIn[i].Items.Add(variable.Value.Name);
                        }
                    }
                    i = scvIn.Count;
                }
            }
            for (int j = stop; j < scvIn.Count; j++)
            {
                scvIn[j].Items.Clear();
                scvIn[j].SelectedIndex = -1;
                scvIn[j].IsEnabled = false;
            }
            //save to config
            if (!firstLoadIn)
            {
                for (int p = 0; p < param; p++)
                {
                    if (scvIn[p].SelectedValue != null)
                    {
                        string tempV = scvIn[p].SelectedValue.ToString();
                        var temp2 = new DisplayVarsEnum();
                        bool found = false;
                        foreach (var disVar in temp.HostApp.DisplayMngr.Dictionarys.DisplayVariables)
                        {
                            if (disVar.Value.Name == tempV)
                            {
                                temp2 = disVar.Key;
                                found = true;
                            }
                        }
                        if (found)
                        {
                            if (temp.Screens[temp.ScreenToEdit].Variables.Count - 1 < p)
                            {
                                temp.Screens[temp.ScreenToEdit].Variables.Add("");
                            }
                            temp.Screens[temp.ScreenToEdit].Variables[p] = temp2.ToString();
                        }
                    }
                }
            }
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
                cb.SelectedValue = "Space";
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