using System;

namespace iRduino.Classes
{
    using iRduino.Windows;

    class ErrorReporting
    {
        public static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            if (e.Source != "iRacingSdkWrapper")
            {
                #if !DEBUG
                string error =
                        String.Format(
                                "{0}\n{1}\n{2}\n{3}",
                                e.Message,
                                e.Source,
                                e.TargetSite,
                                e.StackTrace);
                ErrorWindow window = new ErrorWindow(error);
                window.ShowDialog();
                #endif
            }
        }

    }
}
