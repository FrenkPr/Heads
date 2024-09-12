using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Heads
{
    static class DebugMngr
    {
        private static List<Collider> items;
        private static List<Sprite> sprites;

        static DebugMngr()
        {
            items = new List<Collider>();
            sprites = new List<Sprite>();
        }

        public static void AddItem(Collider c)
        {
            if (!(c is CompoundCollider))
            {
                items.Add(c);
            }

            if (c is CircleCollider)
            {
                sprites.Add(new Sprite(((CircleCollider)c).Radius * 2, ((CircleCollider)c).Radius * 2));
            }

            else if (c is BoxCollider)
            {
                sprites.Add(new Sprite(((BoxCollider)c).Width, ((BoxCollider)c).Height));
            }

            else if (c is CompoundCollider collider)
            {
                items.Add(collider.MainCollider);

                if (collider.MainCollider is CircleCollider)
                {
                    sprites.Add(new Sprite(((CircleCollider)collider.MainCollider).Radius * 2, ((CircleCollider)collider.MainCollider).Radius * 2));
                }

                else if (collider.MainCollider is BoxCollider)
                {
                    sprites.Add(new Sprite(((BoxCollider)collider.MainCollider).Width, ((BoxCollider)collider.MainCollider).Height));
                }

                foreach (Collider subCollider in collider.SubColliders)
                {
                    items.Add(subCollider);

                    if (subCollider is CircleCollider)
                    {
                        sprites.Add(new Sprite(((CircleCollider)subCollider).Radius * 2, ((CircleCollider)subCollider).Radius * 2));
                    }

                    else if (subCollider is BoxCollider)
                    {
                        sprites.Add(new Sprite(((BoxCollider)subCollider).Width, ((BoxCollider)subCollider).Height));
                    }
                }
            }

            else
            {
                sprites.Add(new Sprite(c.RigidBody.GameObject.HalfWidth * 2, c.RigidBody.GameObject.HalfHeight * 2));
            }
        }

        public static void RemoveItem(Collider c)
        {
            int index = items.IndexOf(c);
            sprites.RemoveAt(index);
            items.Remove(c);
        }

        public static void ClearAll()
        {
            items.Clear();
            sprites.Clear();
        }

        public static void Draw()
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].RigidBody.IsActive)
                {
                    Vector4 col;

                    if (items[i] is CircleCollider)
                    {
                        col = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
                    }

                    else
                    {
                        col = new Vector4(0.0f, 0.0f, 1.0f, 1.0f);
                    }

                    sprites[i].position = items[i].Position - new Vector2(items[i].RigidBody.GameObject.HalfWidth, items[i].RigidBody.GameObject.HalfHeight) + items[i].Offset;
                    sprites[i].DrawWireframe(col);
                }
            }
        }
    }
}
