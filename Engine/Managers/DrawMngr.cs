using System.Collections.Generic;

namespace Heads
{
    enum DrawLayer
    {
        Background,
        MiddleGround,
        Playground,
        Foreground,
        GUI,
        Length
    }

    static class DrawMngr
    {
        private static List<IDrawable>[] drawings;

        static DrawMngr()
        {
            drawings = new List<IDrawable>[(int)DrawLayer.Length];

            for (int i = 0; i < drawings.Length; i++)
            {
                drawings[i] = new List<IDrawable>();
            }
        }

        public static void Add(IDrawable item)
        {
            drawings[(int)item.DrawLayer].Add(item);
        }

        public static void Remove(IDrawable item)
        {
            drawings[(int)item.DrawLayer].Remove(item);
        }

        public static void ClearAll()
        {
            for (int i = 0; i < drawings.Length; i++)
            {
                drawings[i].Clear();
            }
        }

        public static void Draw()
        {
            for (int i = 0; i < drawings.Length; i++)
            {
                for (int j = 0; j < drawings[i].Count; j++)
                {
                    drawings[i][j].Draw();
                }
            }
        }
    }
}
