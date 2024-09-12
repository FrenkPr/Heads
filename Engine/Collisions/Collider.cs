using System;
using OpenTK;

namespace Heads
{
    enum CollisionType
    {
        RectsIntersection,
        CirclesIntersection,
        CircleRectIntersection
    }

    struct CollisionInfo
    {
        public GameObject Collider;
        public Vector2 Delta;
        public CollisionType Type;
    }

    abstract class Collider
    {
        public Vector2 Offset;
        public RigidBody RigidBody;
        public Vector2 Position { get { return RigidBody.Position + Offset; } }

        public Collider(RigidBody owner)
        {
            RigidBody = owner;
            Offset = Vector2.Zero;
        }

        public abstract bool Collides(Collider other, ref CollisionInfo collisionInfo);
        public abstract bool Collides(CircleCollider other, ref CollisionInfo collisionInfo);
        public abstract bool Collides(BoxCollider other, ref CollisionInfo collisionInfo);
        public abstract bool Collides(CompoundCollider other);
    }
}
