using System.Collections.Generic;
using Aiv.Fast2D;

namespace Heads
{
    static class TextureMngr
    {
        private static Dictionary<string, Texture> textures;

        static TextureMngr()
        {
            textures = new Dictionary<string, Texture>();
        }

        public static Texture AddTexture(string id, string path, bool repeatX = false, bool repeatY = false)
        {
            if (id == "" || id == null || path == "" || path == null)
            {
                return null;
            }

            if (textures.ContainsKey(id))
            {
                return null;
            }

            Texture t = new Texture(path, repeatX: repeatX, repeatY: repeatY);

            textures.Add(id, t);

            return t;
        }

        public static Texture GetTexture(string id)
        {
            return textures[id];
        }

        public static void ClearAll()
        {
            textures.Clear();
        }
    }
}
