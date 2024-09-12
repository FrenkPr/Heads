using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;

namespace Heads
{
    enum MouseValue
    {
        LeftClick,
        RightClick
    }

    enum KeyCodeType
    {
        Up,
        Down,
        Left,
        Right,
        Fire,
        Length
    }

    struct KeyboardConfig
    {
        public Dictionary<KeyCodeType, KeyCode> KeyCode;

        public KeyboardConfig(List<KeyCode> keys)
        {
            KeyCode = new Dictionary<KeyCodeType, KeyCode>();

            for (int i = 0; i < keys.Count; i++)
            {
                KeyCode.Add((KeyCodeType)i, keys[i]);
            }
        }
    }

    class KeyboardController : Controller
    {
        private KeyboardConfig keys;

        public KeyboardController(int index, List<KeyCode> keys) : base(index)
        {
            this.keys = new KeyboardConfig(keys);
        }

        public override float GetHorizontal()
        {
            float direction = 0;

            if (IsKeyHold(keys.KeyCode[KeyCodeType.Left]) && IsKeyHold(keys.KeyCode[KeyCodeType.Right]))
            {
                direction = 0;
            }

            else
            {
                if (IsKeyHold(keys.KeyCode[KeyCodeType.Left]))
                {
                    direction = -1;
                }

                else if (IsKeyHold(keys.KeyCode[KeyCodeType.Right]))
                {
                    direction = 1;
                }
            }

            return direction;
        }

        public override float GetVertical()
        {
            float direction = 0;

            if (IsKeyHold(keys.KeyCode[KeyCodeType.Up]) && IsKeyHold(keys.KeyCode[KeyCodeType.Down]))
            {
                direction = 0;
            }

            else
            {
                if (IsKeyHold(keys.KeyCode[KeyCodeType.Up]))
                {
                    direction = -1;
                }

                else if (IsKeyHold(keys.KeyCode[KeyCodeType.Down]))
                {
                    direction = 1;
                }
            }

            return direction;
        }

        public override bool IsFireHold()
        {
            return IsKeyHold(keys.KeyCode[KeyCodeType.Fire]);
        }

        public bool IsKeyHold(KeyCodeType value)
        {
            return IsKeyHold(keys.KeyCode[value]);
        }

        public bool IsKeyHold(KeyCode value)
        {
            return Game.Window.GetKey(value);
        }

        public bool IsMouseValueHold(MouseValue value)
        {
            bool res = false;

            switch (value)
            {
                case MouseValue.LeftClick:
                    res = Game.Window.MouseLeft;
                    break;

                case MouseValue.RightClick:
                    res = Game.Window.MouseRight;
                    break;
            }

            return res;
        }
    }
}
