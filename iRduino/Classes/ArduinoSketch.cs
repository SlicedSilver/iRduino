//========================================//
// iRduino - Created by Mark Silverwood  //
//======================================//
namespace iRduino.Classes
{
    using System.IO;
    using System.Windows;
    using System;
    using System.Collections.Generic;
    using Microsoft.Win32;

    using iRduino.ArduinoTemplates;

    public class ArduinoPins
    {
        public int DataPin;

        public int ClockPin;

        public List<int> UnitStrobePins;

        public List<int> TM1640DataPins;

        public List<int> TM1640ClockPins;
    }

    public class ArduinoSketch
    {
 
        public static void GenerateSketch(ConfigurationOptions configurationOptions, ArduinoPins pins)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                                               + "\\iRduino\\";
            // Configure save file dialog box
            var dlg = new SaveFileDialog
            {
                FileName = configurationOptions.Name,
                DefaultExt = ".ino",
                InitialDirectory = documentsPath,
                Filter = "Arduino Sketch (.ino)|*.ino"
            };

            // Show save file dialog box
            bool? result = dlg.ShowDialog();

            // Process save file dialog box results 
            if (result != true) return;
            // Save document 
            string fileLocation = dlg.FileName;
            int startOfName = fileLocation.LastIndexOf("\\", StringComparison.Ordinal);
            int startOfExtension = fileLocation.LastIndexOf('.');
            if (startOfName < startOfExtension)
            {
                string name = fileLocation.Substring(startOfName+1,startOfExtension-startOfName-1);
                if (name.Length < startOfName - 1)
                {
                    if (fileLocation.Substring(startOfName - name.Length, name.Length) != name)
                    {
                        //directory not valid
                        fileLocation = fileLocation.Insert(startOfName + 1, String.Format("{0}\\",name));
                    }

                }
            }

            FileInfo file = new FileInfo(fileLocation);
            if (file.Directory != null)
            {
                file.Directory.Create(); // If the directory already exists, this method does nothing.
            }
            
            ArduinoSketchT4 sketch = new ArduinoSketchT4(configurationOptions, pins);
            String sketchContent = sketch.TransformText();
            File.WriteAllText(fileLocation, sketchContent);

            MessageBox.Show("Finished Saving Arduino Sketch");
        }
    }
}
