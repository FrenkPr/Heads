using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heads
{
    static class PhysicsMngr
    {
        private static List<RigidBody> rigidBodies;
        public static CollisionInfo CollisionInfo;
        public static float Gravity { get; private set; }

        static PhysicsMngr()
        {
            rigidBodies = new List<RigidBody>();
            Gravity = 9;
        }

        public static void Add(RigidBody item)
        {
            rigidBodies.Add(item);
        }

        public static void Remove(RigidBody item)
        {
            rigidBodies.Remove(item);
        }

        public static void ClearAll()
        {
            rigidBodies.Clear();
        }

        public static void CheckCollisions()
        {
            for (int i = 0; i < rigidBodies.Count - 1; i++)
            {
                if (rigidBodies[i].IsActive && rigidBodies[i].IsCollisionAffected)
                {
                    for (int j = i + 1; j < rigidBodies.Count; j++)
                    {
                        if (rigidBodies[j].IsActive && rigidBodies[j].IsCollisionAffected)
                        {
                            bool firstCheck = rigidBodies[i].CollisionTypeMatches(rigidBodies[j].Type);
                            bool secondCheck = rigidBodies[j].CollisionTypeMatches(rigidBodies[i].Type);

                            if ((firstCheck || secondCheck) && rigidBodies[i].Collides(rigidBodies[j], ref CollisionInfo))
                            {
                                if (firstCheck)
                                {
                                    CollisionInfo.Collider = rigidBodies[j].GameObject;
                                    rigidBodies[i].GameObject.OnCollision(CollisionInfo);
                                }

                                else
                                {
                                    CollisionInfo.Collider = rigidBodies[i].GameObject;
                                    rigidBodies[j].GameObject.OnCollision(CollisionInfo);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void Update()
        {
            for (int i = 0; i < rigidBodies.Count; i++)
            {
                if (rigidBodies[i].IsActive)
                {
                    rigidBodies[i].Update();
                }
            }
        }
    }
}
