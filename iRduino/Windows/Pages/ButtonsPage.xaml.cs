//========================================//
// iRduino - Created by Mark Silverwood  //
//======================================//

namespace iRduino.Windows.Pages
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
            this.InitializeComponent();
        }

        private void PageLoaded1(object sender, RoutedEventArgs e)
        {
            this.Temp = (DisplayUnitConfiguration) this.DataContext;
            this.ButtonFunctionsCBoxs = new List<ComboBox>
                {
                    this.ButtonFunctionCBox1,
                    this.ButtonFunctionCBox2,
                    this.ButtonFunctionCBox3,
                    this.ButtonFunctionCBox4,
                    this.ButtonFunctionCBox5,
                    this.ButtonFunctionCBox6,
                    this.ButtonFunctionCBox7,
                    this.ButtonFunctionCBox8
                };
            this.ButtonOptionsCBoxs = new List<ComboBox>
                {
                    this.ButtonOptionCBox1,
                    this.ButtonOptionCBox2,
                    this.ButtonOptionCBox3,
                    this.ButtonOptionCBox4,
                    this.ButtonOptionCBox5,
                    this.ButtonOptionCBox6,
                    this.ButtonOptionCBox7,
                    this.ButtonOptionCBox8
                };
            this.ButtonScreensCBoxs = new List<ComboBox>
                {
                    this.ButtonScreenCBox1,
                    this.ButtonScreenCBox2,
                    this.ButtonScreenCBox3,
                    this.ButtonScreenCBox4,
                    this.ButtonScreenCBox5,
                    this.ButtonScreenCBox6,
                    this.ButtonScreenCBox7,
                    this.ButtonScreenCBox8
                };
            //populate combo boxs
            foreach (var butFunc in this.Temp.HostApp.DisplayMngr.Dictionarys.ButtonFunctions)
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
                cBox.Items.Add("Current");
                for (int i = 1; i <= this.Temp.TotalUnits + 1; i++)
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
                BindingOperations.SetBinding(this.ButtonFunctionsCBoxs[x], Selector.SelectedValueProperty, buttonFuncBinding);
                var buttonOptBinding = new Binding(String.Format("ButtonConfigurations.ButtonOptions[{0}]", x))
                    {
                        Mode = BindingMode.TwoWay
                    };
                BindingOperations.SetBinding(this.ButtonOptionsCBoxs[x], Selector.SelectedValueProperty, buttonOptBinding);
                var buttonScreenBinding = new Binding(String.Format("ButtonConfigurations.ButtonOptionsScreens[{0}]", x))
                    {
                        Mode = BindingMode.TwoWay
                    };
                BindingOperations.SetBinding(this.ButtonScreensCBoxs[x], Selector.SelectedIndexProperty, buttonScreenBinding);
            }
        }

        private void ButtonFunctionSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //foreach (var butFuncCBOX in bf)
            for (int x = 0; x < Constants.NumberButtonsOnTm1638; x++)
            {
                if (this.ButtonFunctionsCBoxs[x].SelectedIndex >= 0)
                {
                    foreach (var butFunc in this.Temp.HostApp.DisplayMngr.Dictionarys.ButtonFunctions)
                    {
                        if (this.ButtonFunctionsCBoxs[x].SelectedItem.ToString() == butFunc.Value.Name)
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
            }
        }
    }
}