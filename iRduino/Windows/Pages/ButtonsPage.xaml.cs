//========================================//
// iRduino - Created by Mark Silverwood  //
//======================================//

namespace iRduino.Pages
{
    using ArduinoInterfaces;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using iRduino.Classes;

    /// <summary>
    ///     Interaction logic for ButtonsPage.xaml
    /// </summary>
    public partial class ButtonsPage
    {
        public List<ComboBox> ButtonFunctionsCBoxs;
        public List<ComboBox> ButtonOptionsCBoxs;
        public List<ComboBox> ButtonScreensCBoxs;
        public DisplayUnitConfiguration Temp;

        public ButtonsPage()
        {
            InitializeComponent();
        }

        private void PageLoaded1(object sender, RoutedEventArgs e)
        {
            Temp = (DisplayUnitConfiguration) DataContext;
            ButtonFunctionsCBoxs = new List<ComboBox>
                {
                    ButtonFunctionCBox1,
                    ButtonFunctionCBox2,
                    ButtonFunctionCBox3,
                    ButtonFunctionCBox4,
                    ButtonFunctionCBox5,
                    ButtonFunctionCBox6,
                    ButtonFunctionCBox7,
                    ButtonFunctionCBox8
                };
            ButtonOptionsCBoxs = new List<ComboBox>
                {
                    ButtonOptionCBox1,
                    ButtonOptionCBox2,
                    ButtonOptionCBox3,
                    ButtonOptionCBox4,
                    ButtonOptionCBox5,
                    ButtonOptionCBox6,
                    ButtonOptionCBox7,
                    ButtonOptionCBox8
                };
            ButtonScreensCBoxs = new List<ComboBox>
                {
                    ButtonScreenCBox1,
                    ButtonScreenCBox2,
                    ButtonScreenCBox3,
                    ButtonScreenCBox4,
                    ButtonScreenCBox5,
                    ButtonScreenCBox6,
                    ButtonScreenCBox7,
                    ButtonScreenCBox8
                };
            //populate combo boxs
            foreach (var butFunc in Temp.HostApp.DisplayMngr.Dictionarys.ButtonFunctions)
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
                cBox.Items.Add("Current");
                for (int i = 1; i <= Temp.TotalUnits + 1; i++)
                {
                    cBox.Items.Add(i.ToString(CultureInfo.InvariantCulture));
                }
            }


            //data bind
            for (int x = 0; x < Constants.NumberButtonsOnTm1638; x++)
            {
                var buttonFuncBinding = new Binding(String.Format("ButtonConfigurations.ButtonFunctions[{0}]", x))
                    {
                        Mode = BindingMode.TwoWay
                    };
                BindingOperations.SetBinding(ButtonFunctionsCBoxs[x], Selector.SelectedValueProperty, buttonFuncBinding);
                var buttonOptBinding = new Binding(String.Format("ButtonConfigurations.ButtonOptions[{0}]", x))
                    {
                        Mode = BindingMode.TwoWay
                    };
                BindingOperations.SetBinding(ButtonOptionsCBoxs[x], Selector.SelectedValueProperty, buttonOptBinding);
                var buttonScreenBinding = new Binding(String.Format("ButtonConfigurations.ButtonOptionsScreens[{0}]", x))
                    {
                        Mode = BindingMode.TwoWay
                    };
                BindingOperations.SetBinding(ButtonScreensCBoxs[x], Selector.SelectedIndexProperty, buttonScreenBinding);
            }
        }

        private void ButtonFunctionSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //foreach (var butFuncCBOX in bf)
            for (int x = 0; x < Constants.NumberButtonsOnTm1638; x++)
            {
                if (ButtonFunctionsCBoxs[x].SelectedIndex >= 0)
                {
                    foreach (var butFunc in Temp.HostApp.DisplayMngr.Dictionarys.ButtonFunctions)
                    {
                        if (ButtonFunctionsCBoxs[x].SelectedItem.ToString() == butFunc.Value.Name)
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
            }
        }
    }
}