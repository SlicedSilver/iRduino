//========================================//
// iRduino - Created by Mark Silverwood  //
//======================================//

namespace iRduino.Windows
{
    using ArduinoInterfaces;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Threading;
    using Microsoft.Win32;
    using iRduino.Classes;

    public class MyTreeViewItem : TreeViewItem
    {
        public PageTypes Page;
        public int ScreenNumber = -1;
        public int UnitNumber = -1;
    }


    /// <summary>
    ///     Interaction logic for OptionsWindowV2.xaml
    /// </summary>
    public partial class OptionsWindow
    {
        private readonly MainWindow hostApp;
        private readonly Random rand = new Random();
        public bool ChangingPages = false;

        public PageTypes PageToSet;
        public int ScreenToSet;
        public int UnitToSet;
        private ConfigurationOptions configurationOptions; //= new ConfigurationOptions();
        private int currentScreen;
        private int currentUnit;
        private string previewExample;
        private string previewHeader;
        private int screenCount;
        private bool screenPreview;
        private DispatcherTimer screenPreviewTimer = new DispatcherTimer();
        private int shiftPreviewRpm = 6000;
        private DispatcherTimer shiftPreviewTimer = new DispatcherTimer();
        private int treeBuilderUnitCount;
        private int treeControllerCount;
        private DateTime waitTime;
        private List<bool> tm1640UnitsTree;

        private string currentBranch = "";

        #region Window Logic
        
        public OptionsWindow(MainWindow host)
        {
            InitializeComponent();
            this.hostApp = host;
        }

        private void OptionsWindowClosing(object sender, CancelEventArgs e)
        {
            this.hostApp.OptionsButton.IsEnabled = true;
            this.hostApp.OptionsWindowOpen = false;
        }

        internal void DisableChanges()
        {
            IsEnabled = false;
        }

        internal void EnableChanges()
        {
            IsEnabled = true;
        }

        private void OptionsWindowLoaded(object sender, RoutedEventArgs e)
        {
            BuildTreeViewNodes(0, OptionsTreeView.Items);
            PageFrame.Source = new Uri("Pages/ConfigurationPage.xaml", UriKind.Relative);
            PageFrame.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(RoutedButtonClickHandler));
            PageFrame.AddHandler(Selector.SelectionChangedEvent, new RoutedEventHandler(RoutedSelectionChangedHandler));
            PageFrame.AddHandler(ToggleButton.CheckedEvent, new RoutedEventHandler(RoutedCheckChangeHandler));
            PageFrame.AddHandler(ToggleButton.UncheckedEvent, new RoutedEventHandler(RoutedCheckChangeHandler));
            this.waitTime = DateTime.Now;
            this.screenPreview = false;
            this.screenPreviewTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, 250) };
            this.screenPreviewTimer.Tick += this.ScreenPreviewTimerTick;
            this.screenPreviewTimer.Start();
            //////////LoadCurrentConf into ConfOptions
            if (this.hostApp.DisplayMngr.CurrentConfiguration != null)
            {
                //try load configuration
                ReloadConfList();
            }
        }

        private void RoutedCheckChangeHandler(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            var temp = e.OriginalSource as CheckBox;
            if (temp == null) return;
            if (temp.Name == "UseDXUnitsCheck")
            {
                if (temp.IsChecked == true)
                {
                    //add some units
                    if (configurationOptions.DisplayUnitConfigurations.Count == 0)
                    {
                        configurationOptions.DisplayUnitConfigurations.Add(new DisplayUnitConfiguration(true));
                    }
                    SetPage(PageTypes.Blank);
                    RebuildTree();
                }
                else if (temp.IsChecked == false)
                {
                    //remove all units
                    configurationOptions.DisplayUnitConfigurations.Clear();
                    SetPage(PageTypes.Blank);
                    RebuildTree();
                }
            }
            if (temp.Name == "TM1640Check")
            {
                if (temp.IsChecked != this.tm1640UnitsTree[this.currentUnit])
                {
                    SetPage(PageTypes.Blank);
                    RebuildTree();
                }
            }
        }

        private void RoutedSelectionChangedHandler(object sender, RoutedEventArgs e)
        {
            var temp = e.OriginalSource as ComboBox;
            if (temp == null) return;
            if (temp.Name == "NumberDisplayUnitsCBox")
            {
                if (temp.SelectedIndex != this.treeBuilderUnitCount && temp.SelectedIndex != -1)
                {
                    ChangedNumberOfUnits(temp.SelectedIndex);
                }
            }
            if (temp.Name == "ControllerNumCBox")
            {
                if (temp.SelectedIndex != this.treeControllerCount && temp.SelectedIndex != -1)
                {
                    ChangedNumberOfControllers(temp.SelectedIndex);
                }
            }
            if (temp.Name != "NumberScreensCBox" || ChangingPages) return;
            if (temp.SelectedIndex == this.configurationOptions.DisplayUnitConfigurations[this.currentUnit].Screens.Count - 1 ||
                temp.SelectedIndex == -1) return;
            ChangedNumberOfScreens(this.currentUnit, temp.SelectedIndex);
        }

        private void RoutedButtonClickHandler(object sender, RoutedEventArgs e)
        {
            var temp = e.OriginalSource as Button;
            if (temp == null) return;
            switch (temp.Name)
            {
                case "LoadButton":
                    this.LoadFileButtonClick(e);
                    break;
                case "NewButton":
                    CreateNewConfiguration();
                    e.Handled = true;
                    break;
                case "RemoveButton":
                    this.RemoveConfButtonClick();
                    e.Handled = true;
                    break;
                case "SaveButton":
                    SaveNewFile();
                    e.Handled = true;
                    break;
                case "DuplicateButton":
                    this.DuplicateConfClick();
                    break;
                case "PreviewShiftLightsButton":
                    this.PreviewShiftUnitButtonClick();
                    e.Handled = true;
                    break;
                case "PreviewOnUnitButton":
                    this.PreviewScreenButtonClick();
                    e.Handled = true;
                    break;
                case "GenerateArduinoSketchButton":
                    GenerateArduinoSketch();
                    e.Handled = true;
                    break;
            }
        }

        private void GenerateArduinoSketch()
        {
            //open new window and send _configurationOptions
            if (this.configurationOptions == null) return;
            if (this.configurationOptions.DisplayUnitConfigurations.Count == 0) return;
            var arduinoWindow = new GenerateArduinoWizard(this.configurationOptions);
            arduinoWindow.ShowDialog();
        }

        #endregion

        #region Loading Saving Configurations

        private void SaveNewFile()
        {
            // Configure save file dialog box
            var dlg = new SaveFileDialog
            {
                FileName = this.configurationOptions.Name,
                InitialDirectory = hostApp.DocumentsPath,
                DefaultExt = ".scft",
                Filter = "SLI Configuration File (.scft)|*.scft"
            };

            // Show save file dialog box
            bool? result = dlg.ShowDialog();

            // Process save file dialog box results 
            if (result != true) return;
            // Save document 
            string filename = dlg.FileName;
            this.configurationOptions.FileLocation = filename;
            Configuration.SaveConfigurationToFile(filename,
                                                  this.configurationOptions.SaveConfiguration(
                                                      this.hostApp.DisplayMngr.Dictionarys));
            this.hostApp.DisplayMngr.CurrentConfiguration.FileLocation = filename;
            //write new current.opt
            string path = hostApp.DocumentsPath + "current.opt";
            using (var outfile = new StreamWriter(path))
            {
                outfile.Write(this.hostApp.DisplayMngr.CurrentConfiguration.FileLocation);
            }
        }

        private void CurrentConfigurationCBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CurrentConfigurationCBox.SelectedIndex >= 0)
            {
                Configuration temp = null;
                string confName = CurrentConfigurationCBox.SelectedItem.ToString();
                foreach (Configuration conf in this.hostApp.DisplayMngr.Configurations.Where(conf => confName == conf.Name))
                {
                    temp = conf;
                }
                this.hostApp.DisplayMngr.CurrentConfiguration = temp;
                if (temp != null)
                {
                    this.hostApp.CurrentConfigurationLabel.Content = temp.Name;
                    this.hostApp.TrySetComPort(temp.PreferredComPort);
                }
                else
                {
                    this.hostApp.CurrentConfigurationLabel.Content = "None Loaded!";
                }
                if (this.hostApp.DisplayMngr.CurrentConfiguration != null)
                    this.hostApp.DisplayMngr.Intensity = this.hostApp.DisplayMngr.CurrentConfiguration.Intensity;
                this.hostApp.CheckCurrentConf();
                if (this.hostApp.DisplayMngr.CurrentConfiguration != null)
                {
                    if (!string.IsNullOrEmpty(this.hostApp.DisplayMngr.CurrentConfiguration.FileLocation))
                    {
                        //write new current.opt
                        string path = hostApp.DocumentsPath + "current.opt";
                        using (var outfile = new StreamWriter(path))
                        {
                            outfile.Write(this.hostApp.DisplayMngr.CurrentConfiguration.FileLocation);
                        }
                    }
                    this.configurationOptions = new ConfigurationOptions();
                    this.configurationOptions.LoadConfiguration(this.hostApp.DisplayMngr.CurrentConfiguration,
                                                            this.hostApp.DisplayMngr.Dictionarys);
                }
                SetPage(PageTypes.Configuration, UnitToSet);
                RebuildTree();
            }
            if (this.CurrentConfigurationCBox.SelectedIndex != -1)
            {
                return;
            }
            this.hostApp.DisplayMngr.CurrentConfiguration = null;
            this.hostApp.CurrentConfigurationLabel.Content = "None Loaded";
            this.hostApp.CheckCurrentConf();
        }

        private void DuplicateConfClick()
        {
            var temp = new ConfigurationOptions();
            var random = new Random();
            temp.LoadConfiguration(this.hostApp.DisplayMngr.CurrentConfiguration, this.hostApp.DisplayMngr.Dictionarys);
            temp.FileLocation = "";
            temp.Name += "-Duplicate" + random.Next(0, 100).ToString(CultureInfo.InvariantCulture);
            Configuration temp2 = temp.SaveConfiguration(this.hostApp.DisplayMngr.Dictionarys);
            this.hostApp.DisplayMngr.Configurations.Add(temp2);
            this.hostApp.DisplayMngr.CurrentConfiguration = temp2;
            this.hostApp.CurrentConfigurationLabel.Content = temp2.Name;
            this.hostApp.TrySetComPort(temp2.PreferredComPort);
            ReloadConfList();
        }

        private void ApplySaveConfiguration()
        {
            Configuration temp = null;
            foreach (Configuration conf in this.hostApp.DisplayMngr.Configurations)
            {
                if (conf.Name == this.configurationOptions.Name || conf.FileLocation == this.configurationOptions.FileLocation)
                {
                    //found current conf
                    temp = conf;
                }
            }
            this.hostApp.DisplayMngr.Configurations.Remove(temp);
            this.hostApp.DisplayMngr.CurrentConfiguration =
                this.configurationOptions.SaveConfiguration(this.hostApp.DisplayMngr.Dictionarys);
            this.hostApp.DisplayMngr.Configurations.Add(this.hostApp.DisplayMngr.CurrentConfiguration);
            if (!string.IsNullOrEmpty(this.configurationOptions.FileLocation))
            {
                Configuration.SaveConfigurationToFile(this.configurationOptions.FileLocation,
                                                      this.hostApp.DisplayMngr.CurrentConfiguration);
            }
            else
            {
                SaveNewFile();
                //remove
            }

            ReloadConfList();
        }

        private void RemoveConfButtonClick()
        {
            if (this.hostApp.DisplayMngr.CurrentConfiguration == null) return;
            if (!string.IsNullOrEmpty(this.hostApp.DisplayMngr.CurrentConfiguration.FileLocation))
            {
                MessageBoxResult result =
                    MessageBox.Show(
                        "Are you sure that you want to remove the current configuration? (This will also delete the file on your Hard-Drive)",
                        "Are you sure?", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    string nameToDelete = this.hostApp.DisplayMngr.CurrentConfiguration.Name;
                    string fileLocation = this.hostApp.DisplayMngr.CurrentConfiguration.FileLocation;
                    Configuration temp = null;
                    foreach (Configuration conf in this.hostApp.DisplayMngr.Configurations)
                    {
                        if (conf.Name == nameToDelete)
                        {
                            //found
                            temp = conf;
                        }
                    }
                    this.hostApp.DisplayMngr.Configurations.Remove(temp);
                    ReloadConfList();
                    try
                    {
                        File.Delete(fileLocation);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                this.hostApp.DisplayMngr.Configurations.Remove(this.hostApp.DisplayMngr.CurrentConfiguration);
                this.hostApp.DisplayMngr.CurrentConfiguration = null;
            }
            CurrentConfigurationCBox.SelectedIndex = -1;
            ReloadConfList();
        }

        private void CreateNewConfiguration()
        {
            var temp = new Configuration(true)
            {
                Name = "Unnamed" + this.rand.Next(1, 100).ToString(CultureInfo.InvariantCulture)
            };
            this.hostApp.DisplayMngr.Configurations.Add(temp);
            CurrentConfigurationCBox.Items.Add(temp.Name);
            CurrentConfigurationCBox.SelectedIndex = CurrentConfigurationCBox.Items.Count - 1;
        }

        private void LoadFileButtonClick(RoutedEventArgs e)
        {
            e.Handled = true;
            string loadedConf = null;
            var dlg = new OpenFileDialog
            {
                FileName = "SLIConfiguration",
                DefaultExt = ".scft",
                Filter = "SLI Configuration File (.scft)|*.scft",
                InitialDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase)
            };

            // Show open file dialog box 
            bool? result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                loadedConf = this.hostApp.LoadNewConfFile(filename);
            }
            if (!string.IsNullOrEmpty(loadedConf))
            {
                ReloadConfList();
                CurrentConfigurationCBox.SelectedItem = loadedConf;
            }
        }

        private void ReloadConfList()
        {
            string current = null;
            if (this.hostApp.DisplayMngr.CurrentConfiguration != null)
            {
                current = this.hostApp.DisplayMngr.CurrentConfiguration.Name;
            }
            CurrentConfigurationCBox.Items.Clear();
            foreach (Configuration conf in this.hostApp.DisplayMngr.Configurations)
            {
                CurrentConfigurationCBox.Items.Add(conf.Name);
            }
            if (CurrentConfigurationCBox.Items.Count > 0) // && _hostApp.DisplayMngr.CurrentConfiguration != null)
            {
                if (current != null)
                {
                    CurrentConfigurationCBox.SelectedItem = current;
                }
            }
        }

        private void ApplySaveButtonClick(object sender, RoutedEventArgs e)
        {
            ApplySaveConfiguration();
        }

        #endregion

        #region Screen Previews

        private void ScreenPreviewTimerTick(object sender, EventArgs e)
        {
            //_configurationOptions
            if (!this.screenPreview) return;
            if (this.waitTime > DateTime.Now) return;
            switch (this.screenCount)
            {
                case 0:
                    this.hostApp.DisplayMngr.ShowStringTimed(this.previewHeader, this.configurationOptions.HeaderDisplayTime + 1,
                                                         this.currentUnit);
                    this.screenCount++;
                    this.waitTime = DateTime.Now.Add(new TimeSpan(0, 0, this.configurationOptions.HeaderDisplayTime + 1));
                    break;
                case 1:
                    this.hostApp.DisplayMngr.ShowStringTimed(this.previewExample, 3, this.currentUnit);
                    this.screenCount++;
                    this.waitTime = DateTime.Now.Add(new TimeSpan(0, 0, 3));
                    this.screenPreview = false;
                    this.screenCount = 0;
                    break;
                default:
                    this.screenPreview = false;
                    this.screenCount = 0;
                    break;
            }
        }

        private void PreviewScreenButtonClick()
        {
            if (this.hostApp.ArduinoConnection.Running)
            {
                var header = "";
                var example = "";
                foreach (
                    string dvString in
                        this.configurationOptions.DisplayUnitConfigurations[this.currentUnit].Screens[this.currentScreen].Variables)
                {
                    DisplayVarsEnum dv;
                    if (Enum.TryParse(dvString, out dv))
                    {
                        header += this.hostApp.DisplayMngr.Dictionarys.DisplayVariables[dv].DisplayName;
                        if (dv != DisplayVarsEnum.Space && dv != DisplayVarsEnum.DoubleSpace)
                        {
                            header += ".";
                        }
                        example += this.hostApp.DisplayMngr.Dictionarys.DisplayVariables[dv].Example;
                    }
                }
                this.screenPreview = true;
                this.previewHeader = header;
                this.previewExample = example;
                if (this.configurationOptions.DisplayUnitConfigurations[this.currentUnit].Screens[this.currentScreen].UseCustomHeader)
                {
                    this.previewHeader =
                            this.configurationOptions.DisplayUnitConfigurations[this.currentUnit].Screens[
                                    this.currentScreen].CustomHeader;
                }
            }
            else
            {
                MessageBox.Show("Start the Connection to the Unit First!");               
            }
        }

        #endregion

        #region Shift Previews

        private void PreviewShiftLights()
        {
            this.hostApp.DisplayMngr.Previewing = true;
            this.shiftPreviewRpm = 6000;
            this.shiftPreviewTimer = new DispatcherTimer();
            this.shiftPreviewTimer.Tick += ShiftPreviewTick;
            this.shiftPreviewTimer.Interval = new TimeSpan(0, 0, 0, 0, 150);
            this.shiftPreviewTimer.Start();
        }

        private void ShiftPreviewTick(object sender, EventArgs e)
        {
            if (this.shiftPreviewRpm <= 7000)
            {
                var displays = new List<string>();
                var dots = new List<byte[]>();
                var greens = new List<byte>();
                var reds = new List<byte>();
                var pastShift = false;
                foreach (DisplayUnitConfiguration t in this.configurationOptions.DisplayUnitConfigurations)
                {
                    displays.Add(this.shiftPreviewRpm.ToString(CultureInfo.InvariantCulture));
                    dots.Add(t.IsTM1640 ? new byte[] { 0, 0 } : new byte[] { 0 });
                    if (t.LEDsConfigurations.ShowShiftLights)
                    {
                        byte red;
                        byte green;
                        ShiftStyleEnum temp =
                            this.hostApp.DisplayMngr.Dictionarys.ShiftStyles[t.LEDsConfigurations.ShiftLightStyle];
                        LEDFunctions.GetShiftLights(this.hostApp.DisplayMngr.Dictionarys, this.shiftPreviewRpm, 6000, 7000, 6800,
                                                            temp, t.LEDsConfigurations.ShiftClumps, out pastShift, out red,
                                                            out green);
                        greens.Add(green);
                        reds.Add(red);
                    }
                    else
                    {
                        greens.Add(0);
                        reds.Add(0);
                    }
                }
                int newInt = this.configurationOptions.Intensity;
                if (pastShift && this.configurationOptions.ShiftIntensity)
                {
                    if (this.configurationOptions.ShiftIntensityType)
                    {
                        //relative
                        newInt += this.configurationOptions.ShiftIntensityAmount + 1;
                    }
                    else
                    {
                        newInt += this.configurationOptions.ShiftIntensityAmount;
                    }
                }
                var dxMessage = new DxMessage
                {
                    DisplayList = displays,
                    Intensity = newInt,
                    GreenLEDSList = greens,
                    RedLEDSList = reds,
                    DotsList = dots
                };
                this.hostApp.ArduinoConnection.SendStringMulti(dxMessage);
                this.shiftPreviewRpm += 50;
            }
            else
            {
                this.hostApp.ArduinoConnection.Clear();
                this.hostApp.DisplayMngr.Previewing = false;
                this.shiftPreviewTimer.Stop();
            }
        }

        private void PreviewShiftUnitButtonClick()
        {
            if (this.hostApp.ArduinoConnection.Running)
            {
                PreviewShiftLights();
            }
            else
            {
                MessageBox.Show("Start the Connection to the Unit First!");
            }
        }

        #endregion

        #region Tree Menu

        //Clear Tree before calling from external code. make sure to update _confOptions before!! 
        private void BuildTreeViewNodes(int level, IList itemCollection)
        {
            if (this.configurationOptions == null)
            {
                MyTreeViewItem item0 = GetMyTreeView("Configurations", "wrench_orange.png", PageTypes.Configuration);
                item0.Selected += MyTreeItemSelected;
                itemCollection.Add(item0);
            }
            else
            {
                if (level == 6)
                {
                    return;
                }
                switch (level)
                {
                    case 0:
                        MyTreeViewItem item0 = GetMyTreeView("Configurations", "wrench_orange.png",
                                                             PageTypes.Configuration);
                        item0.Selected += MyTreeItemSelected;
                        item0.IsExpanded = true;
                        BuildTreeViewNodes(level + 1, item0.Items);
                        itemCollection.Add(item0);
                        break;
                    case 1:
                        MyTreeViewItem item1 = GetMyTreeView(this.configurationOptions.Name, "script_gear.png",
                                                             PageTypes.CurrentConfiguration);
                        item1.Selected += MyTreeItemSelected;
                        item1.IsExpanded = true;
                        BuildTreeViewNodes(level + 1, item1.Items);
                        itemCollection.Add(item1);
                        break;
                    case 2:
                        if (this.configurationOptions.DisplayUnitConfigurations != null
                            && this.configurationOptions.DisplayUnitConfigurations.Count > 0)
                        {
                            MyTreeViewItem itemTMUnits = GetMyTreeView("TM Display Units","bricks.png",PageTypes.TMUnits);
                            currentBranch = "TMUnits";
                            itemTMUnits.Selected += MyTreeItemSelected;
                            BuildTreeViewNodes(level + 1, itemTMUnits.Items);
                            itemCollection.Add(itemTMUnits);
                        }
                        if (this.configurationOptions.ControllerConfigurations != null
                            && this.configurationOptions.ControllerConfigurations.Count > 0)
                        {
                            MyTreeViewItem itemControllers = GetMyTreeView("Controllers", "controller.png", PageTypes.None);
                            currentBranch = "Controllers";
                            itemControllers.Focusable = false;
                            itemControllers.Selected += MyTreeItemSelected;
                            BuildTreeViewNodes(level + 1, itemControllers.Items);
                            itemCollection.Add(itemControllers);
                        }
                        MyTreeViewItem itemAdOpts = GetMyTreeView("Advanced Options", "page_gear.png",
                                                             PageTypes.AdvancedOptions);
                        itemAdOpts.Selected += MyTreeItemSelected;
                        itemCollection.Add(itemAdOpts);
                        break;
                    case 3:
                        if (currentBranch == "TMUnits")
                        {
                            if (this.configurationOptions.DisplayUnitConfigurations != null
                                && this.configurationOptions.DisplayUnitConfigurations.Count > 0)
                            {
                                for (int i = 0; i < this.configurationOptions.DisplayUnitConfigurations.Count; i++)
                                {
                                    this.treeBuilderUnitCount = i;
                                    MyTreeViewItem item2;
                                    if (this.configurationOptions.DisplayUnitConfigurations[i].IsTM1640)
                                    {
                                        item2 = GetMyTreeView(
                                            String.Format("Unit {0} (TM1640)", i + 1),
                                            "brickBlue.png",
                                            PageTypes.Unit,
                                            i);
                                        this.tm1640UnitsTree.Add(true);
                                    }
                                    else
                                    {
                                        item2 = GetMyTreeView(
                                            String.Format("Unit {0}", i + 1), "brick.png", PageTypes.Unit, i);
                                        this.tm1640UnitsTree.Add(false);
                                    }
                                    item2.Selected += MyTreeItemSelected;
                                    BuildTreeViewNodes(level + 1, item2.Items);
                                    int count = this.tm1640UnitsTree.Count(flag => flag);
                                    if (count > Constants.MaxNumberTm1640Units)
                                    {
                                        MessageBox.Show(
                                            "Only Three or less TM1640 display units is supported currently");
                                    }
                                    itemCollection.Add(item2);
                                }
                            }
                        }
                        if (currentBranch == "Controllers")
                        {
                            if (this.configurationOptions.ControllerConfigurations != null)
                            {
                                if (this.configurationOptions.ControllerConfigurations.Count > 0)
                                {
                                    for (int i = 0; i < this.configurationOptions.ControllerConfigurations.Count; i++)
                                    {
                                        MyTreeViewItem itemJoy = GetMyTreeView(
                                            String.Format("Controller {0}", i + 1),
                                            "controller.png",
                                            PageTypes.JoystickButtons,
                                            i);
                                        itemJoy.Selected += MyTreeItemSelected;
                                        itemCollection.Add(itemJoy);
                                        this.treeControllerCount = i + 1;
                                    }
                                }
                            }
                        }
                        break;
                    case 4:
                        if (currentBranch == "TMUnits")
                        {
                            if (!this.configurationOptions.DisplayUnitConfigurations[this.treeBuilderUnitCount].IsTM1640) //not TM1640
                            {
                                MyTreeViewItem item3 = GetMyTreeView(
                                    "Buttons", "joystick.png", PageTypes.Buttons, this.treeBuilderUnitCount);
                                item3.Selected += MyTreeItemSelected;
                                itemCollection.Add(item3);
                                MyTreeViewItem item4 = GetMyTreeView(
                                    "LEDs", "lightbulb.png", PageTypes.LEDs, this.treeBuilderUnitCount);
                                item4.Selected += MyTreeItemSelected;
                                itemCollection.Add(item4);
                            }
                            MyTreeViewItem item5 = GetMyTreeView(
                                "Screens", "layers.png", PageTypes.None, this.treeBuilderUnitCount);
                            item5.Focusable = false;
                            BuildTreeViewNodes(level + 1, item5.Items);
                            itemCollection.Add(item5);
                        }
                        break;
                    case 5:
                        if (currentBranch == "TMUnits")
                        {
                            for (int j = 0;
                                 j
                                 < this.configurationOptions.DisplayUnitConfigurations[this.treeBuilderUnitCount]
                                       .Screens.Count;
                                 j++)
                            {
                                MyTreeViewItem item6 = GetMyTreeView(
                                    string.Format("Screen {0}", j + 1),
                                    "picture_empty.png",
                                    this.configurationOptions.DisplayUnitConfigurations[this.treeBuilderUnitCount]
                                        .IsTM1640
                                        ? PageTypes.TM1640Screen
                                        : PageTypes.Screen,
                                    this.treeBuilderUnitCount,
                                    j);
                                item6.Selected += MyTreeItemSelected;
                                BuildTreeViewNodes(level + 1, item6.Items);
                                itemCollection.Add(item6);
                            }
                        }
                        break;
                }
            }
        }

        private static MyTreeViewItem GetMyTreeView(string text, string imagePath, PageTypes page, int unitNumber = -1,
                                                    int screenNumber = -1)
        {
            var item = new MyTreeViewItem
            {
                Page = page,
                UnitNumber = unitNumber,
                ScreenNumber = screenNumber,
                IsExpanded = false
            };
            // create stack panel
            var stack = new StackPanel { Orientation = Orientation.Horizontal };
            // create Image
            var image = new Image
            {
                Source = new BitmapImage(new Uri("/iRduino;component/Resources/" + imagePath, UriKind.Relative))
            };
            // Label
            var lbl = new Label { Content = text };
            image.Margin = new Thickness(4,0,0,0);
            lbl.Margin = new Thickness(0);
            lbl.VerticalAlignment = VerticalAlignment.Center;
            stack.Margin = new Thickness(0);
            // Add into stack
            stack.Children.Add(image);
            stack.Children.Add(lbl);
            // assign stack to header
            item.Header = stack;
            if (page != PageTypes.Configuration)
            {
                item.Margin = new Thickness(-10, 0, 0, 0);
            }
            return item;
        }

        private void MyTreeItemSelected(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            var myTreeItem = (MyTreeViewItem)sender;
            SetPage(myTreeItem.Page, myTreeItem.UnitNumber, myTreeItem.ScreenNumber);
            this.currentUnit = myTreeItem.UnitNumber;
            this.currentScreen = myTreeItem.ScreenNumber;
        }

        private void RebuildTree()
        {
            UpdateConfOptionsVars();
            var temp = OptionsTreeView.SelectedItem as MyTreeViewItem;
            if (temp != null)
            {
                if (temp.Page != PageTypes.CurrentConfiguration && temp.Page != PageTypes.Unit)
                {
                    temp.Page = PageTypes.TMUnits;
                }
            } 
            OptionsTreeView.Items.Clear();
            this.treeBuilderUnitCount = 0;
            this.treeControllerCount = 0;
            this.tm1640UnitsTree = new List<bool>();
            this.configurationOptions.NumberDisplays = this.configurationOptions.DisplayUnitConfigurations.Count - 1;
            BuildTreeViewNodes(0, OptionsTreeView.Items);
            if (temp == null) return;
            var queue = new Queue<MyTreeViewItem>();
            queue.Enqueue(OptionsTreeView.Items[0] as MyTreeViewItem);
            while (queue.Count > 0)
            {
                // Take the next node from the front of the queue
                MyTreeViewItem node = queue.Dequeue();
                if (node.Page == PageTypes.TMUnits && temp.Page == PageTypes.Unit)
                {
                    node.IsExpanded = true;
                }
                if (node.Page == temp.Page && node.UnitNumber == temp.UnitNumber)
                {
                    node.IsSelected = true;
                    node.IsExpanded = true;
                    continue;
                }

                // Add the node’s children to the back of the queue
                foreach (object child in node.Items)
                    queue.Enqueue(child as MyTreeViewItem);
            }
        }

        private void UpdateConfOptionsVars()
        {
            this.configurationOptions.NumberDisplays = this.configurationOptions.DisplayUnitConfigurations.Count;
            foreach (DisplayUnitConfiguration dUnit in this.configurationOptions.DisplayUnitConfigurations)
            {
                dUnit.TotalUnits = this.configurationOptions.DisplayUnitConfigurations.Count;
                dUnit.NumScreens = dUnit.Screens.Count - 1;
            }
        }


        private void ChangedNumberOfScreens(int currentUnitParameter, int p)
        {
            int newCount = p + 1;
            while (this.configurationOptions.DisplayUnitConfigurations[currentUnitParameter].Screens.Count != newCount)
            {
                if (this.configurationOptions.DisplayUnitConfigurations[currentUnitParameter].Screens.Count < newCount)
                {
                    //add
                    this.configurationOptions.DisplayUnitConfigurations[currentUnitParameter].Screens.Add(new Screen());
                }
                else
                {
                    this.configurationOptions.DisplayUnitConfigurations[currentUnitParameter].Screens.RemoveAt(
                        this.configurationOptions.DisplayUnitConfigurations[this.currentUnit].Screens.Count - 1);
                }
            }
            SetPage(PageTypes.Blank);
            RebuildTree();
        }

        private void ChangedNumberOfControllers(int index)
        {
            if (index == 0)
            {
                this.configurationOptions.ControllerConfigurations.Clear();
                this.configurationOptions.NumberControllers = 0;
            }
            else
            {
                while (this.configurationOptions.ControllerConfigurations.Count != index)
                {
                    if (this.configurationOptions.ControllerConfigurations.Count < index)
                    {
                        //add
                        this.configurationOptions.ControllerConfigurations.Add(new ControllerButtonConfiguration());
                    }
                    else
                    {
                        //remove
                        this.configurationOptions.ControllerConfigurations.RemoveAt(
                            this.configurationOptions.ControllerConfigurations.Count - 1);
                    }
                }
                this.configurationOptions.NumberControllers = this.configurationOptions.ControllerConfigurations.Count;
            }
            SetPage(PageTypes.Blank);
            RebuildTree();
        }

        private void ChangedNumberOfUnits(int index)
        {
            int newCount = index + 1;
            while (this.configurationOptions.DisplayUnitConfigurations.Count != newCount)
            {
                if (this.configurationOptions.DisplayUnitConfigurations.Count < newCount)
                {
                    //add
                    this.configurationOptions.DisplayUnitConfigurations.Add(new DisplayUnitConfiguration(true));
                }
                else
                {
                    //remove
                    this.configurationOptions.DisplayUnitConfigurations.RemoveAt(
                        this.configurationOptions.DisplayUnitConfigurations.Count - 1);
                }
            }
            this.configurationOptions.NumberDisplays = this.configurationOptions.DisplayUnitConfigurations.Count;
            SetPage(PageTypes.Blank);
            RebuildTree();
        }

        #endregion

        #region Pages

        private void SetPage(PageTypes pageType, int unit = -1, int screen = -1)
        {
            PageFrame.LoadCompleted += this.PageFrameLoaded;
            PageToSet = pageType;
            UnitToSet = unit;
            ScreenToSet = screen;
            ChangingPages = true;
            PageFrame.Source = new Uri("Pages/BlankPage.xaml", UriKind.Relative);
        }

        private void PageFrameLoaded(object sender, NavigationEventArgs e)
        {
            PageFrame.LoadCompleted -= this.PageFrameLoaded;
            SetPage2(PageToSet, UnitToSet, ScreenToSet);
        }

        private void SetPage2(PageTypes pageType, int unit = -1, int screen = -1)
        {
            switch (pageType)
            {
                case PageTypes.Configuration:
                    PageFrame.Source = new Uri("Pages/ConfigurationPage.xaml", UriKind.Relative);
                    break;
                case PageTypes.CurrentConfiguration:
                    this.configurationOptions.Dictionarys = this.hostApp.DisplayMngr.Dictionarys;
                    PageFrame.DataContext = this.configurationOptions;
                    PageFrame.Source = new Uri("Pages/CurrentConfiguration.xaml", UriKind.Relative);
                    break;
                case PageTypes.AdvancedOptions:
                    PageFrame.DataContext = this.configurationOptions;
                    PageFrame.Source = new Uri("Pages/AdvancedOptions.xaml", UriKind.Relative);
                    break;
                case PageTypes.TMUnits:
                    this.configurationOptions.Dictionarys = this.hostApp.DisplayMngr.Dictionarys;
                    PageFrame.DataContext = this.configurationOptions;
                    PageFrame.Source = new Uri("Pages/TMUnits.xaml", UriKind.Relative);
                    break;
                case PageTypes.Unit:
                    this.configurationOptions.DisplayUnitConfigurations[unit].HostApp = this.hostApp;
                    this.configurationOptions.DisplayUnitConfigurations[unit].UnitNumber = unit;
                    PageFrame.DataContext = this.configurationOptions.DisplayUnitConfigurations[unit];
                    PageFrame.Source = new Uri("Pages/UnitPage.xaml", UriKind.Relative);
                    this.currentUnit = unit;
                    break;
                case PageTypes.Buttons:
                    this.configurationOptions.DisplayUnitConfigurations[unit].HostApp = this.hostApp;
                    this.configurationOptions.DisplayUnitConfigurations[unit].TotalUnits =
                        this.configurationOptions.NumberDisplays;
                    PageFrame.DataContext = this.configurationOptions.DisplayUnitConfigurations[unit];
                    PageFrame.Source = new Uri("Pages/ButtonsPage.xaml", UriKind.Relative);
                    this.currentUnit = unit;
                    break;
                case PageTypes.LEDs:
                    this.configurationOptions.DisplayUnitConfigurations[unit].HostApp = this.hostApp;
                    this.configurationOptions.DisplayUnitConfigurations[unit].NumScreens =
                        this.configurationOptions.DisplayUnitConfigurations[unit].Screens.Count - 1;
                    PageFrame.DataContext = this.configurationOptions.DisplayUnitConfigurations[unit];
                    PageFrame.Source = new Uri("Pages/LEDsPage.xaml", UriKind.Relative);
                    this.currentUnit = unit;
                    break;
                case PageTypes.Screen:
                    this.configurationOptions.DisplayUnitConfigurations[unit].HostApp = this.hostApp;
                    this.configurationOptions.DisplayUnitConfigurations[unit].ScreenToEdit = screen;
                    PageFrame.DataContext = this.configurationOptions.DisplayUnitConfigurations[unit];
                    PageFrame.Source = new Uri("Pages/ScreenPage.xaml", UriKind.Relative);
                    this.currentUnit = unit;
                    break;
                case PageTypes.TM1640Screen:
                    this.configurationOptions.DisplayUnitConfigurations[unit].HostApp = this.hostApp;
                    this.configurationOptions.DisplayUnitConfigurations[unit].ScreenToEdit = screen;
                    PageFrame.DataContext = this.configurationOptions.DisplayUnitConfigurations[unit];
                    PageFrame.Source = new Uri("Pages/TM1640ScreenPage.xaml", UriKind.Relative);
                    this.currentUnit = unit;
                    break;
                case PageTypes.JoystickButtons:
                    this.configurationOptions.Dictionarys = this.hostApp.DisplayMngr.Dictionarys;
                    this.configurationOptions.EditNumber = unit;
                    PageFrame.DataContext = this.configurationOptions;
                    PageFrame.Source = new Uri("Pages/JoystickButtonsPage.xaml", UriKind.Relative);
                    break;
                case PageTypes.Blank:
                    PageFrame.Source = new Uri("Pages/BlankPage.xaml", UriKind.Relative);
                    break;
                case PageTypes.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("pageType");
            }
            ChangingPages = false;
        }

        private void UpdateFrameDataContext()
        {
            var content = PageFrame.Content as FrameworkElement;
            if (content == null)
                return;
            content.DataContext = PageFrame.DataContext;
        }

        private void PageFrameDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateFrameDataContext();
        }

        private void PageFrameLoadCompleted(object sender, NavigationEventArgs e)
        {
            UpdateFrameDataContext();
        }

        #endregion
    }
}