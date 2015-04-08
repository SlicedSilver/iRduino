//========================================//
// iRduino - Created by Mark Silverwood  //
//======================================//

namespace iRduino.Classes
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Xml;

    public enum PageTypes
    {
        Configuration,
        CurrentConfiguration,
        AdvancedOptions,
        TMUnits,
        Unit,
        Buttons,
        LEDs,
        Screen,
        TM1640Screen,
        JoystickButtons,
        Arduino,
        FergoTech,
        DigitalInputs,
        DigitalOutputs,
        Expander,
        Blank,
        None
    }
    
    class OptionPages
    {
        public static Version GetPublishedVersion()
        {
            XmlDocument xmlDoc = new XmlDocument();
            Assembly asmCurrent = Assembly.GetExecutingAssembly();
            string executePath = new Uri(asmCurrent.GetName().CodeBase).LocalPath;

            xmlDoc.Load(executePath + ".manifest");
            string retval = string.Empty;
            if (xmlDoc.HasChildNodes)
            {
                var attributes = xmlDoc.ChildNodes[1].ChildNodes[0].Attributes;
                if (attributes != null)
                {
                    retval = attributes.GetNamedItem("version").Value.ToString(CultureInfo.InvariantCulture);
                }
            }
            return new Version(retval);
        }
    }
}
