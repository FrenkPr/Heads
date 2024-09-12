using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Heads
{
    class CompoundCollider : Collider
    {
        public Collider MainCollider;
        public List<Collider> SubColliders;

        public CompoundCollider(RigidBody owner, Collider mainCollider) : base(owner)
        {
            SubColliders = new List<Collider>();
            MainCollider = mainCollider;

            //if (MainCollider is CircleCollider)
            //{
            //    MainCollider.Offset = Vector2.Zero;
            //}

            //else if (MainCollider is BoxCollider box)
            //{
            //    MainCollider.Offset = new Vector2(box.Width, box.Height);
            //}
        }

        public void AddSubCollider(Collider collider)
        {
            SubColliders.Add(collider);
        }

        public override bool Collides(Collider other, ref CollisionInfo collisionInfo)
        {
            return other.Collides(MainCollider, ref collisionInfo) && InnerCollidersCollides(other);
        }

        public override bool Collides(CircleCollider other, ref CollisionInfo collisionInfo)
        {
            return other.Collides(MainCollider, ref collisionInfo) && InnerCollidersCollides(other);
        }

        public override bool Collides(BoxCollider other, ref CollisionInfo collisionInfo)
        {
            return other.Collides(MainCollider, ref collisionInfo) && InnerCollidersCollides(other);
        }

        private bool InnerCollidersCollides(Collider other)
        {
            for (int i = 0; i < SubColliders.Count; i++)
            {
                if (SubColliders[i].Collides(other, ref PhysicsMngr.CollisionInfo))
                {
                    return true;
                }
            }

            return false;
        }

        public override bool Collides(CompoundCollider other)
        {
            if (MainCollider.Collides(other.MainCollider, ref PhysicsMngr.CollisionInfo))
            {
                if (SubColliders.Count == 0 && other.SubColliders.Count == 0)
                {
                    return true;
                }

                else if (SubColliders.Count == 0 && other.SubColliders.Count != 0)
                {
                    for (int i = 0; i < other.SubColliders.Count; i++)
                    {
                        if (MainCollider.Collides(other.SubColliders[i], ref PhysicsMngr.CollisionInfo))
                        {
                            return true;
                        }
                    }
                }

                else if (SubColliders.Count != 0 && other.SubColliders.Count == 0)
                {
                    for (int i = 0; i < SubColliders.Count; i++)
                    {
                        if (SubColliders[i].Collides(other.MainCollider, ref PhysicsMngr.CollisionInfo))
                        {
                            return true;
                        }
                    }
                }

                else
                {
                    for (int i = 0; i < SubColliders.Count; i++)
                    {
                        for (int j = 0; j < other.SubColliders.Count; j++)
                        {
                            if (SubColliders[i].Collides(other.SubColliders[j], ref PhysicsMngr.CollisionInfo))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}
