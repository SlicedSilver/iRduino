//========================================//
// iRduino - Created by Mark Silverwood  //
//======================================//

namespace iRduino.Windows.Pages
{
    using iRduino.Classes;

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Forms;
    using System.Windows.Threading;

    using Binding = System.Windows.Data.Binding;
    using ComboBox = System.Windows.Controls.ComboBox;

    /// <summary>
    ///     Interaction logic for ButtonsPage.xaml
    /// </summary>
    public partial class JoystickButtonsPage
    {
        public List<ComboBox> ButtonFunctionsCBoxs;
        public List<ComboBox> ButtonOptionsCBoxs;
        public List<ComboBox> ButtonScreensCBoxs;
        public List<ComboBox> JoyButtonCBoxs;
        public ConfigurationOptions Temp;
        private IList<ControllerDevice> joysticks;
        private ControllerDevice selectedJoystick;
        private DispatcherTimer updateUiTimer;
        private int currentPage;
        private bool changingPages;

        private const int NumberButtonsPerPage = 4;


        public JoystickButtonsPage()
        {
            this.InitializeComponent();
        }

        private void PageLoaded1(object sender, RoutedEventArgs e)
        {
            this.updateUiTimer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 0, 0, 200)};
            this.updateUiTimer.Tick += this.UpdateUiTimerTick;
            this.joysticks = ControllerDevice.Available();
            foreach (ControllerDevice joystickDevice in this.joysticks)
            {
                this.JoyDevicesCBox.Items.Add(joystickDevice.Name);
            }
            this.Temp = (ConfigurationOptions) this.DataContext;
            this.JoyButtonCBoxs = new List<ComboBox>
                {
                    this.JoyButtonCBox1,
                    this.JoyButtonCBox2,
                    this.JoyButtonCBox3,
                    this.JoyButtonCBox4
                };
            this.ButtonFunctionsCBoxs = new List<ComboBox>
                {
                    this.ButtonFunctionCBox1,
                    this.ButtonFunctionCBox2,
                    this.ButtonFunctionCBox3,
                    this.ButtonFunctionCBox4
                };
            this.ButtonOptionsCBoxs = new List<ComboBox>
                {
                    this.ButtonOptionCBox1,
                    this.ButtonOptionCBox2,
                    this.ButtonOptionCBox3,
                    this.ButtonOptionCBox4
                };
            this.ButtonScreensCBoxs = new List<ComboBox>
                {
                    this.ButtonScreenCBox1,
                    this.ButtonScreenCBox2,
                    this.ButtonScreenCBox3,
                    this.ButtonScreenCBox4
                };
            //populate combo boxs
            foreach (var butFunc in this.Temp.Dictionarys.ButtonFunctions)
            {
                foreach (ComboBox cbox in this.ButtonFunctionsCBoxs)
                {
                    cbox.Items.Add(butFunc.Value.Name);
                }
            }
            foreach (ComboBox cBox in this.ButtonScreensCBoxs)
            {
                cBox.IsEnabled = true;
                cBox.Items.Add("All");
                for (int i = 1; i <= this.Temp.NumberDisplays + 1; i++)
                {
                    cBox.Items.Add(i.ToString(CultureInfo.InvariantCulture));
                }
            }
            //data binding here!!!
            for (int x = 0; x < NumberButtonsPerPage; x++)
            {
                var buttonFuncBinding =
                    new Binding(String.Format("ControllerConfigurations[{0}].ButtonFunctions[{1}]", this.Temp.EditNumber, x))
                        {
                            Mode = BindingMode.TwoWay
                        };
                BindingOperations.SetBinding(this.ButtonFunctionsCBoxs[x], Selector.SelectedValueProperty, buttonFuncBinding);
                var buttonOptBinding =
                    new Binding(String.Format("ControllerConfigurations[{0}].ButtonOptions[{1}]", this.Temp.EditNumber, x))
                        {
                            Mode = BindingMode.TwoWay
                        };
                BindingOperations.SetBinding(this.ButtonOptionsCBoxs[x], Selector.SelectedValueProperty, buttonOptBinding);
                var buttonScreenBinding =
                    new Binding(String.Format("ControllerConfigurations[{0}].ButtonOptionsScreens[{1}]", this.Temp.EditNumber,
                                              x))
                        {
                            Mode = BindingMode.TwoWay
                        };
                BindingOperations.SetBinding(this.ButtonScreensCBoxs[x], Selector.SelectedIndexProperty, buttonScreenBinding);
                var buttonNumberBinding =
                    new Binding(String.Format("ControllerConfigurations[{0}].ButtonNumbers[{1}]", this.Temp.EditNumber, x))
                        {
                            Mode = BindingMode.TwoWay
                        };
                BindingOperations.SetBinding(this.JoyButtonCBoxs[x], Selector.SelectedIndexProperty, buttonNumberBinding);
            }


            if (this.Temp.ControllerConfigurations[this.Temp.EditNumber].Selected)
            {
                //try select correct device in dropdown menu
                for (int p = 0; p < this.joysticks.Count; p++)
                {
                    if (this.joysticks[p].Guid == this.Temp.ControllerConfigurations[this.Temp.EditNumber].DeviceGuid)
                    {
                        //found
                        this.JoyDevicesCBox.SelectedIndex = p;
                    }
                }
            }
        }

        private void UpdateUiTimerTick(object sender, EventArgs e)
        {
            string temp = this.selectedJoystick.GetButtons()
                                           .Aggregate("", (current, item) => current + item.ToString(" 00"));
            this.ButtonPressLabel.Content = temp;
        }

        private void ButtonFunctionSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.changingPages) return;
            //foreach (var butFuncCBOX in bf)
            for (int x = 0; x < NumberButtonsPerPage; x++)
            {
                if (this.ButtonFunctionsCBoxs[x].SelectedIndex < 0) continue;
                int x1 = x;
                foreach (
                    var butFunc in
                        this.Temp.Dictionarys.ButtonFunctions.Where(
                            butFunc => this.ButtonFunctionsCBoxs[x1].SelectedItem.ToString() == butFunc.Value.Name))
                {
                    if (butFunc.Key == ButtonFunctionsEnum.NextScreen ||
                        butFunc.Key == ButtonFunctionsEnum.PreviousScreen ||
                        butFunc.Key == ButtonFunctionsEnum.SpecificScreen ||
                        butFunc.Key == ButtonFunctionsEnum.DisplayQuickInfo)
                    {
                        this.ButtonScreensCBoxs[x].IsEnabled = true;
                        if (this.ButtonScreensCBoxs[x].SelectedIndex == -1)
                        {
                            this.ButtonScreensCBoxs[x].SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        this.ButtonScreensCBoxs[x].IsEnabled = false;
                        this.ButtonScreensCBoxs[x].SelectedIndex = -1;
                    }
                    //found
                    if (butFunc.Value.Options.Count > 0)
                    {
                        this.ButtonOptionsCBoxs[x].IsEnabled = true;
                        int currentSelection = this.ButtonOptionsCBoxs[x].SelectedIndex;
                        this.ButtonOptionsCBoxs[x].Items.Clear();
                        foreach (string option in butFunc.Value.Options)
                        {
                            this.ButtonOptionsCBoxs[x].Items.Add(option);
                        }
                        if (currentSelection <= this.ButtonOptionsCBoxs[x].Items.Count - 1)
                        {
                            this.ButtonOptionsCBoxs[x].SelectedIndex = currentSelection;
                        }
                        if (this.ButtonOptionsCBoxs[x].SelectedIndex == -1)
                        {
                            this.ButtonOptionsCBoxs[x].SelectedIndex = 0;
                        }
                    }
                    else if (butFunc.Key == ButtonFunctionsEnum.SpecificScreen)
                    {
                        this.ButtonOptionsCBoxs[x].IsEnabled = true;
                        int currentSelection = this.ButtonOptionsCBoxs[x].SelectedIndex;
                        this.ButtonOptionsCBoxs[x].Items.Clear();
                        for (int sc = 1; sc <= 16; sc++)
                            //not using Screens.Count on purpose, so that configurations can be changed without needing to reset button configs
                        {
                            this.ButtonOptionsCBoxs[x].Items.Add(sc.ToString(CultureInfo.InvariantCulture));
                        }
                        if (currentSelection <= this.ButtonOptionsCBoxs[x].Items.Count - 1)
                        {
                            this.ButtonOptionsCBoxs[x].SelectedIndex = currentSelection;
                        }
                        if (this.ButtonOptionsCBoxs[x].SelectedIndex == -1)
                        {
                            this.ButtonOptionsCBoxs[x].SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        this.ButtonOptionsCBoxs[x].SelectedIndex = -1;
                        this.ButtonOptionsCBoxs[x].IsEnabled = false;
                    }
                }
            }
        }

        private void JoyDevicesCBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.JoyDevicesCBox.SelectedIndex >= 0)
            {
                this.selectedJoystick = this.joysticks[this.JoyDevicesCBox.SelectedIndex];
                this.selectedJoystick.Acquire(new Form());
                this.Temp.ControllerConfigurations[this.Temp.EditNumber].DeviceGuid = this.selectedJoystick.Guid;
                this.Temp.ControllerConfigurations[this.Temp.EditNumber].Selected = true;
                foreach (ComboBox joyButtonCBox in this.JoyButtonCBoxs)
                {
                    for (int y = 0; y < this.selectedJoystick.ButtonCount; y++)
                    {
                        joyButtonCBox.Items.Add(y.ToString(CultureInfo.InvariantCulture));
                    }
                }
                this.updateUiTimer.Start();
                foreach (ComboBox item in this.JoyButtonCBoxs)
                {
                    item.IsEnabled = true;
                }
                foreach (ComboBox item2 in this.ButtonFunctionsCBoxs)
                {
                    item2.IsEnabled = true;
                }
            }
            else
            {
                this.updateUiTimer.Stop();
                this.selectedJoystick = null;
                this.Temp.ControllerConfigurations[this.Temp.EditNumber].Selected = false;
                foreach (ComboBox item in this.JoyButtonCBoxs)
                {
                    item.IsEnabled = false;
                }
                foreach (ComboBox item2 in this.ButtonFunctionsCBoxs)
                {
                    item2.IsEnabled = false;
                }
            }
        }

        private void PageChange(int currentPageParameter)
        {
            // decouple data binding
            for (int x = 0; x < NumberButtonsPerPage; x++)
            {
                BindingOperations.ClearBinding(this.ButtonFunctionsCBoxs[x], Selector.SelectedValueProperty);
                BindingOperations.ClearBinding(this.ButtonOptionsCBoxs[x], Selector.SelectedValueProperty);
                BindingOperations.ClearBinding(this.ButtonScreensCBoxs[x], Selector.SelectedIndexProperty);
                BindingOperations.ClearBinding(this.JoyButtonCBoxs[x], Selector.SelectedIndexProperty);
                this.ButtonFunctionsCBoxs[x].SelectedIndex = -1;
                this.ButtonOptionsCBoxs[x].SelectedIndex = -1;
                this.ButtonScreensCBoxs[x].SelectedIndex = -1;
                this.JoyButtonCBoxs[x].SelectedIndex = -1;
            }
            // change page label
            int startNumber = currentPageParameter * NumberButtonsPerPage;
            this.PageLabel.Content = String.Format("Buttons {0} to {1}", startNumber + 1, startNumber + NumberButtonsPerPage);
            // recouple data binding
            this.changingPages = false;
            //data binding here!!!
            for (int x = 0; x < NumberButtonsPerPage; x++)
            {
                var buttonFuncBinding =
                    new Binding(String.Format("ControllerConfigurations[{0}].ButtonFunctions[{1}]", this.Temp.EditNumber,
                                              startNumber + x))
                        {
                            Mode = BindingMode.TwoWay
                        };
                BindingOperations.SetBinding(this.ButtonFunctionsCBoxs[x], Selector.SelectedValueProperty, buttonFuncBinding);
                var buttonOptBinding =
                    new Binding(String.Format("ControllerConfigurations[{0}].ButtonOptions[{1}]", this.Temp.EditNumber,
                                              startNumber + x))
                        {
                            Mode = BindingMode.TwoWay
                        };
                BindingOperations.SetBinding(this.ButtonOptionsCBoxs[x], Selector.SelectedValueProperty, buttonOptBinding);
                var buttonScreenBinding =
                    new Binding(String.Format("ControllerConfigurations[{0}].ButtonOptionsScreens[{1}]", this.Temp.EditNumber,
                                              startNumber + x))
                        {
                            Mode = BindingMode.TwoWay
                        };
                BindingOperations.SetBinding(this.ButtonScreensCBoxs[x], Selector.SelectedIndexProperty, buttonScreenBinding);
                var buttonNumberBinding =
                    new Binding(String.Format("ControllerConfigurations[{0}].ButtonNumbers[{1}]", this.Temp.EditNumber,
                                              startNumber + x))
                        {
                            Mode = BindingMode.TwoWay
                        };
                BindingOperations.SetBinding(this.JoyButtonCBoxs[x], Selector.SelectedIndexProperty, buttonNumberBinding);
            }
        }


        private void ChangeNextPageButtonClick(object sender, RoutedEventArgs e)
        {
            this.ChangePage(true);
            e.Handled = true;
        }

        private void ChangePreviousPageButtonClick(object sender, RoutedEventArgs e)
        {
            this.ChangePage(false);
            e.Handled = true;
        }

        private void ChangePage(bool direction)
        {
            this.NextPageButton.IsEnabled = false;
            this.PreviousPageButton.IsEnabled = false;
            this.changingPages = true;
            if (direction)
            {
                this.currentPage++;
            }
            else
            {
                this.currentPage--;
            }
            this.PageChange(this.currentPage);
            switch (this.currentPage)
            {
                case 0:
                    this.NextPageButton.IsEnabled = true;
                    break;
                case 1:
                    this.NextPageButton.IsEnabled = true;
                    this.PreviousPageButton.IsEnabled = true;
                    break;
                case 2:
                    this.NextPageButton.IsEnabled = true;
                    this.PreviousPageButton.IsEnabled = true;
                    break;
                case 3:
                    this.NextPageButton.IsEnabled = true;
                    this.PreviousPageButton.IsEnabled = true;
                    break;
                case 4:
                    this.NextPageButton.IsEnabled = true;
                    this.PreviousPageButton.IsEnabled = true;
                    break;
                case 5:
                    this.NextPageButton.IsEnabled = true;
                    this.PreviousPageButton.IsEnabled = true;
                    break;
                case 6:
                    this.NextPageButton.IsEnabled = true;
                    this.PreviousPageButton.IsEnabled = true;
                    break;
                case 7:
                    this.PreviousPageButton.IsEnabled = true;
                    break;
            }
            //_changingPages = false;
        }

        private void ClearButtonClick(object sender, RoutedEventArgs e)
        {
            for (int x = 0; x < NumberButtonsPerPage; x++)
            {
                this.ButtonFunctionsCBoxs[x].SelectedIndex = -1;
                this.ButtonOptionsCBoxs[x].SelectedIndex = -1;
                this.ButtonScreensCBoxs[x].SelectedIndex = -1;
                this.JoyButtonCBoxs[x].SelectedIndex = -1;
            }
        }
    }
}