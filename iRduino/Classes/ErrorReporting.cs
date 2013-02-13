using System;

namespace iRduino.Classes
{

    class ErrorReporting
    {
        public static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
        }

    }
}
