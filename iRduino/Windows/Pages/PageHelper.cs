using iRduino.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace iRduino.Windows.Pages
{
    public class PageHelper
    {
        public static void StartLoading1(List<ComboBox> scv, ref int screen, DisplayUnitConfiguration temp, CheckBox useCustomHeaderCheck, TextBox headerTextBox, ref bool firstLoad)
        {
            foreach (ComboBox cbox in scv)
            {
                cbox.SelectedIndex = -1;
                cbox.IsEnabled = false;
            }
            scv[0].IsEnabled = true;
            screen = temp.ScreenToEdit;
            if (temp.Screens[screen].Variables.Count > 0)
            {
                for (int i = 0; i < temp.Screens[screen].Variables.Count; i++)
                {
                    scv[i].IsEnabled = true;
                    DisplayVarsEnum temp222;
                    if (Enum.TryParse(temp.Screens[screen].Variables[i], out temp222))
                    {
                        scv[i].SelectedItem = temp.HostApp.DisplayMngr.Dictionarys.DisplayVariables[temp222].Name;
                    }
                    else
                    {
                        scv[i].SelectedIndex = -1;
                    }
                }
            }
            else
            {
                scv[0].IsEnabled = true;
                scv[0].SelectedIndex = -1;
            }
            useCustomHeaderCheck.IsChecked = temp.Screens[screen].UseCustomHeader;
            headerTextBox.Text = temp.Screens[screen].CustomHeader;
            firstLoad = false;
        }

        public static void ScreenVariablesUpdatedExtracted(int maxDisplayLengthTM1638, string format, int param, List<ComboBox> scvIn, DisplayUnitConfiguration temp, Label spaceUsedLabel, bool firstLoadIn)
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
    }
}
