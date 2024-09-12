using OpenTK;

namespace Heads
{
    enum PowerUpType
    {
        Energy,
        Length
    }

    abstract class PowerUp : GameObject
    {
        protected Actor actor;
        public PowerUpType Type { get; protected set; }

        public PowerUp(string textureId, float width = 0, float height = 0) : base(textureId, 1, width, height)
        {
            RigidBody = new RigidBody(this, Vector2.Zero);
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);

            RigidBody.Type = RigidBodyType.PowerUp;
            RigidBody.AddCollisionType(RigidBodyType.Player | RigidBodyType.Enemy);

            UpdateMngr.Add(this);
            DrawMngr.Add(this);
        }

        public virtual void OnAttach(Actor a)
        {
            actor = a;
        }

        public void OnDetach()
        {
            actor = null;
        }
    }
}
