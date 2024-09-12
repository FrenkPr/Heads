using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heads
{
    class PS4Controller : JoypadController
    {
        public PS4Controller(int index) : base(index)
        {

        }

        public override bool IsFireHold()
        {
            return IsValuePressed(JoypadValue.Square);
        }

        public override bool IsValuePressed(JoypadValue value)
        {
            bool valuePressed = false;

            switch (value)
            {
                case JoypadValue.PS4_X:
                    valuePressed = Game.Window.JoystickB(controllerIndex);
                    break;
                case JoypadValue.Circle:
                    valuePressed = Game.Window.JoystickX(controllerIndex);
                    break;
                case JoypadValue.Triangle:
                    valuePressed = Game.Window.JoystickY(controllerIndex);
                    break;
                case JoypadValue.Square:
                    valuePressed = Game.Window.JoystickA(controllerIndex);
                    break;
                case JoypadValue.Start:
                    valuePressed = Game.Window.JoystickRightStick(controllerIndex);
                    break;
            }

            return valuePressed;
        }
    }
}
