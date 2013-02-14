//========================================//
// iRduino - Created by Mark Silverwood  //
//======================================//
namespace iRduino.ArduinoTemplates
{
    using iRduino.Classes;

    partial class ArduinoSketchT4
    {
        private readonly ConfigurationOptions configurationOptions;
        private readonly ArduinoPins pins;
        public ArduinoSketchT4(ConfigurationOptions configurationOptions, ArduinoPins pins)
        {
            this.configurationOptions = configurationOptions;
            this.pins = pins;
        }
    }
}