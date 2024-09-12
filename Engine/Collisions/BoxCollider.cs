using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heads
{
    class BoxCollider : Collider
    {
        private float halfWidth;
        private float halfHeight;
        public float Width { get => halfWidth * 2; }
        public float Height { get => halfHeight * 2; }

        public BoxCollider(RigidBody owner, float width, float height) : base(owner)
        {
            halfWidth = width * 0.5f;
            halfHeight = height * 0.5f;
        }

        public override bool Collides(Collider other, ref CollisionInfo collisionInfo)
        {
            return other.Collides(this, ref collisionInfo);
        }

        public override bool Collides(CircleCollider other, ref CollisionInfo collisionInfo)
        {
            float distX = other.Position.X - Math.Max(Position.X - halfWidth, Math.Min(other.Position.X, Position.X + halfWidth));
            float distY = other.Position.Y - Math.Max(Position.Y - halfHeight, Math.Min(other.Position.Y, Position.Y + halfHeight));
            float delta = Math.Abs(distX) + Math.Abs(distY) - other.Radius;

            if (delta > 0)
            {
                return false;
            }

            collisionInfo.Delta = new Vector2(-delta);
            collisionInfo.Type = CollisionType.CircleRectIntersection;

            return true;
        }

        public override bool Collides(BoxCollider other, ref CollisionInfo collisionInfo)
        {
            Vector2 dist = other.Position - Position;
            float deltaX = Math.Abs(dist.X) - (other.halfWidth + this.halfWidth);
            float deltaY = Math.Abs(dist.Y) - (other.halfHeight + this.halfHeight);

            if (deltaX > 0 || deltaY > 0)
            {
                return false;
            }

            collisionInfo.Type = CollisionType.RectsIntersection;   // we know it's rectTorect
            collisionInfo.Delta = new Vector2(-deltaX, -deltaY);    // to have positive values

            return true;
        }

        public override bool Collides(CompoundCollider other)
        {
            return other.Collides(this, ref PhysicsMngr.CollisionInfo);
        }
    }
}
