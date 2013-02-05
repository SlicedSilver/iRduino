//========================================//
// iRduino - Created by Mark Silverwood  //
//======================================//

namespace iRduino.Windows
{
    using System.Reflection;
    using System;

    /// <summary>
    /// Interaction logic for AboutBox.xaml
    /// </summary>
    public partial class AboutBox
    {
        public AboutBox()
        {
            InitializeComponent();
            VersionNumberLabel.Content = AssemblyVersion;
            BuildDateLabel.Content = RetrieveLinkerTimestamp().ToLongDateString();
        }

        public string AssemblyVersion
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
        }

        private static DateTime RetrieveLinkerTimestamp()
        {
            string filePath = Assembly.GetCallingAssembly().Location;
            const int CPeHeaderOffset = 60;
            const int CLinkerTimestampOffset = 8;
            var b = new byte[2048];
            System.IO.Stream s = null;

            try
            {
                s = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                s.Read(b, 0, 2048);
            }
            finally
            {
                if (s != null)
                {
                    s.Close();
                }
            }

            int i = BitConverter.ToInt32(b, CPeHeaderOffset);
            int secondsSince1970 = BitConverter.ToInt32(b, i + CLinkerTimestampOffset);
            var dt = new DateTime(1970, 1, 1, 0, 0, 0);
            dt = dt.AddSeconds(secondsSince1970);
            dt = dt.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours);
            return dt;
        }
    }
}
