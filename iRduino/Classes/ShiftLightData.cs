//========================================//
// iRduino - Created by Mark Silverwood  //
//======================================//

namespace iRduino.Classes
{
    using System.IO;
    using System.Xml.Serialization;

    class ShiftLightData
    {
        public static ShiftData LoadShiftData(string path)
        {
            var xRoot = new XmlRootAttribute { ElementName = "ShiftData", IsNullable = true };
            var ser = new XmlSerializer(typeof(ShiftData),xRoot);
            return ser.Deserialize(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) as ShiftData;
        }
    }
}
