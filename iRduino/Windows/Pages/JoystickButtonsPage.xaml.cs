//========================================//
// iRduino - Created by Mark Silverwood  //
//======================================//

namespace iRduino.Pages
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
            InitializeComponent();
        }

        private void PageLoaded1(object sender, RoutedEventArgs e)
        {
            this.updateUiTimer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 0, 0, 200)};
            this.updateUiTimer.Tick += this.UpdateUiTimerTick;
            this.joysticks = ControllerDevice.Available();
            foreach (ControllerDevice joystickDevice in this.joysticks)
            {
                JoyDevicesCBox.Items.Add(joystickDevice.Name);
            }
            Temp = (ConfigurationOptions) DataContext;
            JoyButtonCBoxs = new List<ComboBox>
                {
                    JoyButtonCBox1,
                    JoyButtonCBox2,
                    JoyButtonCBox3,
                    JoyButtonCBox4
                };
            ButtonFunctionsCBoxs = new List<ComboBox>
                {
                    ButtonFunctionCBox1,
                    ButtonFunctionCBox2,
                    ButtonFunctionCBox3,
                    ButtonFunctionCBox4
                };
            ButtonOptionsCBoxs = new List<ComboBox>
                {
                    ButtonOptionCBox1,
                    ButtonOptionCBox2,
                    ButtonOptionCBox3,
                    ButtonOptionCBox4
                };
            ButtonScreensCBoxs = new List<ComboBox>
                {
                    ButtonScreenCBox1,
                    ButtonScreenCBox2,
                    ButtonScreenCBox3,
                    ButtonScreenCBox4
                };
            //populate combo boxs
            foreach (var butFunc in Temp.Dictionarys.ButtonFunctions)
            {
                foreach (ComboBox cbox in ButtonFunctionsCBoxs)
                {
                    cbox.Items.Add(butFunc.Value.Name);
                }
            }
            foreach (ComboBox cBox in ButtonScreensCBoxs)
            {
                cBox.IsEnabled = true;
                cBox.Items.Add("All");
                for (int i = 1; i <= Temp.NumberDisplays + 1; i++)
                {
                    cBox.Items.Add(i.ToString(CultureInfo.InvariantCulture));
                }
            }
            //data binding here!!!
            for (int x = 0; x < NumberButtonsPerPage; x++)
            {
                var buttonFuncBinding =
                    new Binding(String.Format("ControllerConfigurations[{0}].ButtonFunctions[{1}]", Temp.EditNumber, x))
                        {
                            Mode = BindingMode.TwoWay
                        };
                BindingOperations.SetBinding(ButtonFunctionsCBoxs[x], Selector.SelectedValueProperty, buttonFuncBinding);
                var buttonOptBinding =
                    new Binding(String.Format("ControllerConfigurations[{0}].ButtonOptions[{1}]", Temp.EditNumber, x))
                        {
                            Mode = BindingMode.TwoWay
                        };
                BindingOperations.SetBinding(ButtonOptionsCBoxs[x], Selector.SelectedValueProperty, buttonOptBinding);
                var buttonScreenBinding =
                    new Binding(String.Format("ControllerConfigurations[{0}].ButtonOptionsScreens[{1}]", Temp.EditNumber,
                                              x))
                        {
                            Mode = BindingMode.TwoWay
                        };
                BindingOperations.SetBinding(ButtonScreensCBoxs[x], Selector.SelectedIndexProperty, buttonScreenBinding);
                var buttonNumberBinding =
                    new Binding(String.Format("ControllerConfigurations[{0}].ButtonNumbers[{1}]", Temp.EditNumber, x))
                        {
                            Mode = BindingMode.TwoWay
                        };
                BindingOperations.SetBinding(JoyButtonCBoxs[x], Selector.SelectedIndexProperty, buttonNumberBinding);
            }


            if (Temp.ControllerConfigurations[Temp.EditNumber].Selected)
            {
                //try select correct device in dropdown menu
                for (int p = 0; p < this.joysticks.Count; p++)
                {
                    if (this.joysticks[p].Guid == Temp.ControllerConfigurations[Temp.EditNumber].DeviceGuid)
                    {
                        //found
                        JoyDevicesCBox.SelectedIndex = p;
                    }
                }
            }
        }

        private void UpdateUiTimerTick(object sender, EventArgs e)
        {
            string temp = this.selectedJoystick.GetButtons()
                                           .Aggregate("", (current, item) => current + item.ToString(" 00"));
            ButtonPressLabel.Content = temp;
        }

        private void ButtonFunctionSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.changingPages) return;
            //foreach (var butFuncCBOX in bf)
            for (int x = 0; x < NumberButtonsPerPage; x++)
            {
                if (ButtonFunctionsCBoxs[x].SelectedIndex < 0) continue;
                int x1 = x;
                foreach (
                    var butFunc in
                        Temp.Dictionarys.ButtonFunctions.Where(
                            butFunc => ButtonFunctionsCBoxs[x1].SelectedItem.ToString() == butFunc.Value.Name))
                {
                    if (butFunc.Key == ButtonFunctionsEnum.NextScreen ||
                        butFunc.Key == ButtonFunctionsEnum.PreviousScreen ||
                        butFunc.Key == ButtonFunctionsEnum.SpecificScreen ||
                        butFunc.Key == ButtonFunctionsEnum.DisplayQuickInfo)
                    {
                        ButtonScreensCBoxs[x].IsEnabled = true;
                        if (ButtonScreensCBoxs[x].SelectedIndex == -1)
                        {
                            ButtonScreensCBoxs[x].SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        ButtonScreensCBoxs[x].IsEnabled = false;
                        ButtonScreensCBoxs[x].SelectedIndex = -1;
                    }
                    //found
                    if (butFunc.Value.Options.Count > 0)
                    {
                        ButtonOptionsCBoxs[x].IsEnabled = true;
                        int currentSelection = ButtonOptionsCBoxs[x].SelectedIndex;
                        ButtonOptionsCBoxs[x].Items.Clear();
                        foreach (string option in butFunc.Value.Options)
                        {
                            ButtonOptionsCBoxs[x].Items.Add(option);
                        }
                        if (currentSelection <= ButtonOptionsCBoxs[x].Items.Count - 1)
                        {
                            ButtonOptionsCBoxs[x].SelectedIndex = currentSelection;
                        }
                        if (ButtonOptionsCBoxs[x].SelectedIndex == -1)
                        {
                            ButtonOptionsCBoxs[x].SelectedIndex = 0;
                        }
                    }
                    else if (butFunc.Key == ButtonFunctionsEnum.SpecificScreen)
                    {
                        ButtonOptionsCBoxs[x].IsEnabled = true;
                        int currentSelection = ButtonOptionsCBoxs[x].SelectedIndex;
                        ButtonOptionsCBoxs[x].Items.Clear();
                        for (int sc = 1; sc <= 16; sc++)
                            //not using Screens.Count on purpose, so that configurations can be changed without needing to reset button configs
                        {
                            ButtonOptionsCBoxs[x].Items.Add(sc.ToString(CultureInfo.InvariantCulture));
                        }
                        if (currentSelection <= ButtonOptionsCBoxs[x].Items.Count - 1)
                        {
                            ButtonOptionsCBoxs[x].SelectedIndex = currentSelection;
                        }
                        if (ButtonOptionsCBoxs[x].SelectedIndex == -1)
                        {
                            ButtonOptionsCBoxs[x].SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        ButtonOptionsCBoxs[x].SelectedIndex = -1;
                        ButtonOptionsCBoxs[x].IsEnabled = false;
                    }
                }
            }
        }

        private void JoyDevicesCBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (JoyDevicesCBox.SelectedIndex >= 0)
            {
                this.selectedJoystick = this.joysticks[JoyDevicesCBox.SelectedIndex];
                this.selectedJoystick.Acquire(new Form());
                Temp.ControllerConfigurations[Temp.EditNumber].DeviceGuid = this.selectedJoystick.Guid;
                Temp.ControllerConfigurations[Temp.EditNumber].Selected = true;
                foreach (ComboBox joyButtonCBox in JoyButtonCBoxs)
                {
                    for (int y = 0; y < this.selectedJoystick.ButtonCount; y++)
                    {
                        joyButtonCBox.Items.Add(y.ToString(CultureInfo.InvariantCulture));
                    }
                }
                this.updateUiTimer.Start();
                foreach (ComboBox item in JoyButtonCBoxs)
                {
                    item.IsEnabled = true;
                }
                foreach (ComboBox item2 in ButtonFunctionsCBoxs)
                {
                    item2.IsEnabled = true;
                }
            }
            else
            {
                this.updateUiTimer.Stop();
                this.selectedJoystick = null;
                Temp.ControllerConfigurations[Temp.EditNumber].Selected = false;
                foreach (ComboBox item in JoyButtonCBoxs)
                {
                    item.IsEnabled = false;
                }
                foreach (ComboBox item2 in ButtonFunctionsCBoxs)
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
                BindingOperations.ClearBinding(ButtonFunctionsCBoxs[x], Selector.SelectedValueProperty);
                BindingOperations.ClearBinding(ButtonOptionsCBoxs[x], Selector.SelectedValueProperty);
                BindingOperations.ClearBinding(ButtonScreensCBoxs[x], Selector.SelectedIndexProperty);
                BindingOperations.ClearBinding(JoyButtonCBoxs[x], Selector.SelectedIndexProperty);
                ButtonFunctionsCBoxs[x].SelectedIndex = -1;
                ButtonOptionsCBoxs[x].SelectedIndex = -1;
                ButtonScreensCBoxs[x].SelectedIndex = -1;
                JoyButtonCBoxs[x].SelectedIndex = -1;
            }
            // change page label
            int startNumber = currentPageParameter * NumberButtonsPerPage;
            PageLabel.Content = String.Format("Buttons {0} to {1}", startNumber + 1, startNumber + NumberButtonsPerPage);
            // recouple data binding
            this.changingPages = false;
            //data binding here!!!
            for (int x = 0; x < NumberButtonsPerPage; x++)
            {
                var buttonFuncBinding =
                    new Binding(String.Format("ControllerConfigurations[{0}].ButtonFunctions[{1}]", Temp.EditNumber,
                                              startNumber + x))
                        {
                            Mode = BindingMode.TwoWay
                        };
                BindingOperations.SetBinding(ButtonFunctionsCBoxs[x], Selector.SelectedValueProperty, buttonFuncBinding);
                var buttonOptBinding =
                    new Binding(String.Format("ControllerConfigurations[{0}].ButtonOptions[{1}]", Temp.EditNumber,
                                              startNumber + x))
                        {
                            Mode = BindingMode.TwoWay
                        };
                BindingOperations.SetBinding(ButtonOptionsCBoxs[x], Selector.SelectedValueProperty, buttonOptBinding);
                var buttonScreenBinding =
                    new Binding(String.Format("ControllerConfigurations[{0}].ButtonOptionsScreens[{1}]", Temp.EditNumber,
                                              startNumber + x))
                        {
                            Mode = BindingMode.TwoWay
                        };
                BindingOperations.SetBinding(ButtonScreensCBoxs[x], Selector.SelectedIndexProperty, buttonScreenBinding);
                var buttonNumberBinding =
                    new Binding(String.Format("ControllerConfigurations[{0}].ButtonNumbers[{1}]", Temp.EditNumber,
                                              startNumber + x))
                        {
                            Mode = BindingMode.TwoWay
                        };
                BindingOperations.SetBinding(JoyButtonCBoxs[x], Selector.SelectedIndexProperty, buttonNumberBinding);
            }
        }


        private void ChangeNextPageButtonClick(object sender, RoutedEventArgs e)
        {
            ChangePage(true);
            e.Handled = true;
        }

        private void ChangePreviousPageButtonClick(object sender, RoutedEventArgs e)
        {
            ChangePage(false);
            e.Handled = true;
        }

        private void ChangePage(bool direction)
        {
            NextPageButton.IsEnabled = false;
            PreviousPageButton.IsEnabled = false;
            this.changingPages = true;
            if (direction)
            {
                this.currentPage++;
            }
            else
            {
                this.currentPage--;
            }
            PageChange(this.currentPage);
            switch (this.currentPage)
            {
                case 0:
                    NextPageButton.IsEnabled = true;
                    break;
                case 1:
                    NextPageButton.IsEnabled = true;
                    PreviousPageButton.IsEnabled = true;
                    break;
                case 2:
                    NextPageButton.IsEnabled = true;
                    PreviousPageButton.IsEnabled = true;
                    break;
                case 3:
                    NextPageButton.IsEnabled = true;
                    PreviousPageButton.IsEnabled = true;
                    break;
                case 4:
                    NextPageButton.IsEnabled = true;
                    PreviousPageButton.IsEnabled = true;
                    break;
                case 5:
                    NextPageButton.IsEnabled = true;
                    PreviousPageButton.IsEnabled = true;
                    break;
                case 6:
                    NextPageButton.IsEnabled = true;
                    PreviousPageButton.IsEnabled = true;
                    break;
                case 7:
                    PreviousPageButton.IsEnabled = true;
                    break;
            }
            //_changingPages = false;
        }

        private void ClearButtonClick(object sender, RoutedEventArgs e)
        {
            for (int x = 0; x < NumberButtonsPerPage; x++)
            {
                ButtonFunctionsCBoxs[x].SelectedIndex = -1;
                ButtonOptionsCBoxs[x].SelectedIndex = -1;
                ButtonScreensCBoxs[x].SelectedIndex = -1;
                JoyButtonCBoxs[x].SelectedIndex = -1;
            }
        }
    }
}