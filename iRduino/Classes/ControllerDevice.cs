//========================================//
// iRduino - Created by Mark Silverwood  //
//======================================//

namespace iRduino.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;
    using SlimDX;
    using SlimDX.DirectInput;    
    
    public class ControllerDevice
    {
        public int ButtonCount;
        public Guid Guid;
        public string Name;
        public Joystick Pad;
        public JoystickState State;

        public void Acquire(Form parent)
        {
            var dinput = new DirectInput();

            Pad = new Joystick(dinput, Guid);
            foreach (DeviceObjectInstance doi in Pad.GetObjects(ObjectDeviceType.Axis))
            {
                Pad.GetObjectPropertiesById((int) doi.ObjectType).SetRange(-5000, 5000);
            }

            Pad.Properties.AxisMode = DeviceAxisMode.Absolute;
            Pad.SetCooperativeLevel(parent, (CooperativeLevel.Nonexclusive | CooperativeLevel.Background));
            ButtonCount = Pad.Capabilities.ButtonCount;
            Pad.Acquire();
        }

        public List<int> GetButtons()
        {
            if (Pad.Acquire().IsFailure)
                return null;

            if (Pad.Poll().IsFailure)
                return null;

            State = Pad.GetCurrentState();
            if (Result.Last.IsFailure)
                return null;

            var answer = new List<int>();

            bool[] buttons = State.GetButtons();
            for (int b = 0; b < buttons.Length; b++)
            {
                if (buttons[b])
                    answer.Add(b);
            }
            return answer;
        }

        public static IList<ControllerDevice> Available()
        {
            var dinput = new DirectInput();
            return
                dinput.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly)
                      .Select(di => new ControllerDevice {Guid = di.InstanceGuid, Name = di.InstanceName})
                      .ToList();
        }
    }
}