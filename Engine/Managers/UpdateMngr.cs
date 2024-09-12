using System.Collections.Generic;

namespace Heads
{
    static class UpdateMngr
    {
        private static List<IUpdatable> obj;

        static UpdateMngr()
        {
            obj = new List<IUpdatable>();
        }

        public static void Add(IUpdatable item)
        {
            obj.Add(item);
        }

        public static void Remove(IUpdatable item)
        {
            obj.Remove(item);
        }

        public static void ClearAll()
        {
            obj.Clear();
        }

        public static void Update()
        {
            for (int i = 0; i < obj.Count; i++)
            {
                obj[i].Update();
            }
        }
    }
}
