using System;

namespace iRduino.Classes
{
    using CrashReporterDotNET;

    class ErrorReporting
    {
        public static void ApplicationThreadException(Exception e)
        {
            var reportCrash = new ReportCrash
            {
                FromEmail = "irduinoerrors@gmail.com",
                ToEmail = "msilverwood@gmail.com",
                SMTPHost = "smtp.gmail.com",
                Port = 587,
                UserName = "irduinoerrors",
                Password = "passwordGoesHere",
                EnableSSL = true,
            };

            reportCrash.Send(e);
        }
    }
}
