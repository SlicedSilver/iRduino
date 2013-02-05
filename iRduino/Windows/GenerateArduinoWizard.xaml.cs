//========================================//
// iRduino - Created by Mark Silverwood  //
//======================================//

namespace iRduino.Windows
{
    using System.Linq;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using ArduinoInterfaces;
    using iRduino.Classes;

    /// <summary>
    /// Interaction logic for GenerateArduinoWizard.xaml
    /// </summary>
    public partial class GenerateArduinoWizard
    {
        private readonly ConfigurationOptions configurationOptions;
        private List<ComboBox> units;
        private List<Label> unitsLabels;
        private List<ComboBox> tm1640Clocks;
        private List<Label> tm1640ClockLabels;
        private List<ComboBox> tm1640Datas;
        private List<Label> tm1640DataLabels;
        private int numberTm1640S;
        private List<int> tm1640UnitNumbers;

        public GenerateArduinoWizard(ConfigurationOptions confOpts)
        {
            InitializeComponent();
            this.configurationOptions = confOpts;
        }

        private void ArduinoSketchWindowLoaded(object sender, RoutedEventArgs e)
        {
            this.units = new List<ComboBox>
                {
                    Unit1PinCBox,
                    Unit2PinCBox,
                    Unit3PinCBox,
                    Unit4PinCBox,
                    Unit5PinCBox,
                    Unit6PinCBox
                };
            this.unitsLabels = new List<Label> {Unit1Label, Unit2Label, Unit3Label, Unit4Label, Unit5Label, Unit6Label};
            this.numberTm1640S = 0;
            this.tm1640UnitNumbers = new List<int>();
            for (var m = 0; m < this.configurationOptions.DisplayUnitConfigurations.Count; m++)
            {
                if (this.configurationOptions.DisplayUnitConfigurations[m].IsTM1640)
                {
                    this.tm1640UnitNumbers.Add(m);
                    this.numberTm1640S += 1;
                }
            }
            ConfSelectedLabel.Content = this.configurationOptions.Name;
            NumberUnitsLabel.Content =
                (this.configurationOptions.DisplayUnitConfigurations.Count - this.numberTm1640S).ToString(
                    CultureInfo.InvariantCulture);
            ConfSelectedLabel2.Content = this.configurationOptions.Name;
            NumberTM1640UnitsLabel.Content = this.numberTm1640S.ToString(CultureInfo.InvariantCulture);

            if (this.configurationOptions.DisplayUnitConfigurations.Count - this.numberTm1640S <= 0)
            {
                DataPinCBox.Visibility = Visibility.Hidden;
                ClockPinCBox.Visibility = Visibility.Hidden;
                DataPinLabel.Visibility = Visibility.Hidden;
                ClockPinLabel.Visibility = Visibility.Hidden;
            }

            for (var i = 0; i < Constants.NumberPinsArduinoBoard; i++)
            {
                DataPinCBox.Items.Add(i.ToString(CultureInfo.InvariantCulture));
                ClockPinCBox.Items.Add(i.ToString(CultureInfo.InvariantCulture));
                foreach (var cb in this.units)
                {
                    cb.Items.Add(i.ToString(CultureInfo.InvariantCulture));
                }
            }
            for (var k = 0; k < this.configurationOptions.DisplayUnitConfigurations.Count; k++)
            {
                if (this.configurationOptions.DisplayUnitConfigurations[k].IsTM1640)
                {
                    this.units[k].Visibility = Visibility.Hidden;
                    this.unitsLabels[k].Visibility = Visibility.Hidden;
                }
            }
            for (var j = this.configurationOptions.DisplayUnitConfigurations.Count; j < Constants.MaxNumberTM1638Units; j++)
            {
                this.units[j].Visibility = Visibility.Hidden;
                this.unitsLabels[j].Visibility = Visibility.Hidden;
            }
            Grid2.Visibility = Visibility.Hidden;
            PreviousButton1.IsEnabled = false;
            NextButton1.IsEnabled = false;
            GenerateButton.IsEnabled = false;
            Checker();
        }

        private void CboxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Checker();
        }

        private void Checker()
        {
            if ((ClockPinCBox.SelectedIndex >= 0 && DataPinCBox.SelectedIndex >= 0) || this.configurationOptions.DisplayUnitConfigurations.Count - this.numberTm1640S <= 0)
            {
                bool allset = true;
                for (var u = 0; u < this.configurationOptions.DisplayUnitConfigurations.Count; u++)
                {
                    if (!this.configurationOptions.DisplayUnitConfigurations[u].IsTM1640)
                    {
                        if (this.units[u].SelectedIndex == -1)
                        {
                            allset = false;
                        }
                    }
                }
                if (allset)
                {
                    if (this.numberTm1640S == 0)
                    {
                        GenerateButton.IsEnabled = true;
                    }
                    else
                    {
                        NextButton1.IsEnabled = true;
                        //check next level
                        if (Grid2.Visibility == Visibility.Visible)
                        {
                            for (var u = 0; u < this.numberTm1640S; u++)
                            {
                                if (this.tm1640Clocks[u].SelectedIndex == -1)
                                {
                                    allset = false;
                                }
                                if (this.tm1640Datas[u].SelectedIndex == -1)
                                {
                                    allset = false;
                                }
                            }
                            if (allset)
                            {
                                GenerateButton.IsEnabled = true;
                                GenerateButton2.IsEnabled = true;
                            }
                        }
                    }
                }
            }
        }

        private void GenerateButtonClick(object sender, RoutedEventArgs e)
        {
            GenerateSketch();
        }

        private void NextButton1Click(object sender, RoutedEventArgs e)
        {
            //go to second screen (TM1640)
            Grid1.Visibility = Visibility.Hidden;
            Grid2.Visibility = Visibility.Visible;
            PreviousButton2.IsEnabled = true;
            this.tm1640ClockLabels = new List<Label> {this.TM1640Unit1ClockLabel, TM1640Unit2ClockLabel, TM1640Unit3ClockLabel};
            this.tm1640DataLabels = new List<Label> {this.TM1640Unit1DataLabel, TM1640Unit2DataLabel, TM1640Unit3DataLabel};
            this.tm1640Clocks = new List<ComboBox> {TM1640ClockPin1CBox, TM1640ClockPin2CBox, TM1640ClockPin3CBox};
            this.tm1640Datas = new List<ComboBox> {TM1640DataPin1CBox, TM1640DataPin2CBox, TM1640DataPin3CBox};

            //populate cboxs
            for (var i = 0; i < Constants.NumberPinsArduinoBoard; i++)
            {
                foreach (var cb in this.tm1640Clocks)
                {
                    cb.Items.Add(i.ToString(CultureInfo.InvariantCulture));
                }
                foreach (var cb in this.tm1640Datas)
                {
                    cb.Items.Add(i.ToString(CultureInfo.InvariantCulture));
                }
            }
            //show / hide relevant parts
            for (var p = 0; p < Constants.MaxNumberTm1640Units; p++)
            {
                if (p < this.numberTm1640S)
                {
                    //set labels
                    this.tm1640ClockLabels[p].Content = string.Format("Unit {0} Clock Pin: ", this.tm1640UnitNumbers[p]+1);
                    this.tm1640DataLabels[p].Content = string.Format("Unit {0} Data Pin: ", this.tm1640UnitNumbers[p]+1);
                }
                else
                {
                    this.tm1640Datas[p].Visibility = Visibility.Hidden;
                    this.tm1640Clocks[p].Visibility = Visibility.Hidden;
                    this.tm1640DataLabels[p].Visibility = Visibility.Hidden;
                    this.tm1640ClockLabels[p].Visibility = Visibility.Hidden;
                }
            }
            //check system for weather data is all supplied and then enable generate button
        }
        private void GenerateSketch()
        {
            int numberTM1640 = 0;
            int numberTM1638 = 0;
            foreach (var unit in configurationOptions.DisplayUnitConfigurations)
            {
                if (unit.IsTM1640)
                {
                    numberTM1640++;
                }
                else
                {
                    numberTM1638++;
                }
            }
            var pins = new ArduinoPins
                           {
                               ClockPin = numberTM1638 > 0 ? this.ClockPinCBox.SelectedIndex : -1,
                               DataPin = numberTM1638 > 0 ? this.DataPinCBox.SelectedIndex : -1,
                               UnitStrobePins = numberTM1638 > 0 ? this.units.Select(item => item.SelectedIndex).ToList() : new List<int> { -1 },
                               TM1640DataPins = numberTM1640 > 0 ? this.tm1640Datas.Select(item => item.SelectedIndex).ToList() : new List<int> { -1 },
                               TM1640ClockPins = numberTM1640 > 0 ?
                                   this.tm1640Clocks.Select(item => item.SelectedIndex).ToList() : new List<int> { -1 }
                           };
            ArduinoSketch.GenerateSketch(this.configurationOptions, pins);
        }

        private void PreviousButton2Click(object sender, RoutedEventArgs e)
        {
            Grid2.Visibility = Visibility.Hidden;
            Grid1.Visibility = Visibility.Visible;
        }
    }
}