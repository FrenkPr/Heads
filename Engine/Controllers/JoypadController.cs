using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heads
{
    enum JoypadValue
    {
        PS4_X,
        Circle,
        Triangle,
        Square,
        A,
        B,
        Xbox_X,
        Y,
        Start,
        R1,
        R2,
        L1,
        L2,
        L3,
        R3
    }

    abstract class JoypadController : Controller
    {
        public JoypadController(int index) : base(index)
        {

        }

        public override float GetHorizontal()
        {
            return Game.Window.JoystickAxisLeft(controllerIndex).X;
        }

        public override float GetVertical()
        {
            return Game.Window.JoystickAxisLeft(controllerIndex).Y;
        }

        public abstract bool IsValuePressed(JoypadValue value);
    }
}
