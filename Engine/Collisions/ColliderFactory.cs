using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heads
{
    static class ColliderFactory
    {
        public static CircleCollider CreateCircleFor(GameObject obj, bool innerCircle = true)
        {
            float radius;

            if (innerCircle)
            {
                if (obj.HalfWidth < obj.HalfHeight)
                {
                    radius = obj.HalfWidth;
                }

                else
                {
                    radius = obj.HalfHeight;
                }
            }

            else
            {
                radius = (float)Math.Sqrt(Math.Pow(obj.HalfWidth, 2) + Math.Pow(obj.HalfHeight, 2));
            }

            return new CircleCollider(obj.RigidBody, radius);
        }

        public static BoxCollider CreateBoxFor(GameObject obj)
        {
            return new BoxCollider(obj.RigidBody, obj.Width, obj.Height);
        }

        public static CompoundCollider CreateCompoundFor(GameObject obj, Collider mainCollider)
        {
            return new CompoundCollider(obj.RigidBody, mainCollider);
        }
    }
}
