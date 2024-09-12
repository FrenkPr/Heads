using System;
using System.Collections.Generic;
using OpenTK;
using Aiv.Fast2D;

namespace Heads
{
    enum RigidBodyType { Player = 1, PlayerBullet = 2, Enemy = 4, EnemyBullet = 8, PowerUp = 16 }

    class RigidBody
    {
        public GameObject GameObject;
        public Collider Collider;
        public bool IsCollisionAffected;
        public RigidBodyType Type;
        public float Friction;
        protected uint collisionMask;
        public bool IsActive { get { return GameObject.IsActive; } }
        public Vector2 Position { get { return GameObject.Position; } set { GameObject.Position = value; } }
        public Vector2 Velocity;

        public RigidBody(GameObject owner, Vector2 moveSpeed)
        {
            GameObject = owner;
            Velocity = moveSpeed;
            IsCollisionAffected = true;

            PhysicsMngr.Add(this);
        }

        public bool Collides(RigidBody otherBody, ref CollisionInfo collisionInfo)
        {
            return Collider.Collides(otherBody.Collider, ref collisionInfo);
        }

        public void Update()
        {
            if (IsActive)
            {
                ApplyFriction();

                Position += Velocity * Game.DeltaTime;  //updates game object position
            }
        }

        public void ApplyFriction()
        {
            if (Friction > 0 && Velocity != Vector2.Zero)
            {
                float fAmount = Friction * Game.DeltaTime;
                float newVelocityLength = Velocity.Length - fAmount;

                if (newVelocityLength < 0)
                {
                    Velocity = Vector2.Zero;
                    return;
                }

                Velocity = Velocity.Normalized() * newVelocityLength;
            }
        }

        public void AddCollisionType(RigidBodyType type)
        {
            collisionMask |= (uint)type;
        }

        public bool CollisionTypeMatches(RigidBodyType type)
        {
            return ((uint)type & collisionMask) != 0;
        }

        public void Destroy()
        {
            GameObject = null;
            Collider = null;
            PhysicsMngr.Remove(this);
        }
    }
}
