using System;
using OpenTK;

namespace Heads
{
    class CircleCollider : Collider
    {
        public float Radius;

        public CircleCollider(RigidBody owner, float radius) : base(owner)
        {
            Radius = radius;
        }

        public override bool Collides(Collider other, ref CollisionInfo collisionInfo)
        {
            return other.Collides(this, ref collisionInfo);
        }

        public override bool Collides(CircleCollider other, ref CollisionInfo collisionInfo)
        {
            Vector2 dist = other.Position - Position;
            float deltaX = Math.Abs(dist.X) - (other.Radius + this.Radius);
            float deltaY = Math.Abs(dist.Y) - (other.Radius + this.Radius);

            if (deltaX > 0 || deltaY > 0)
            {
                return false;
            }

            collisionInfo.Delta = new Vector2(-deltaX, -deltaY);
            collisionInfo.Type = CollisionType.CirclesIntersection;

            return true;
        }

        public override bool Collides(BoxCollider other, ref CollisionInfo collisionInfo)
        {
            return other.Collides(this, ref collisionInfo);
        }

        public override bool Collides(CompoundCollider other)
        {
            return other.Collides(this, ref PhysicsMngr.CollisionInfo);
        }
    }
}
