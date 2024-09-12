using OpenTK;

namespace Heads
{
    class PlayerBullet : Bullet
    {
        public PlayerBullet() : base("bullet")
        {
            Dmg = 25;

            Type = BulletType.PlayerBullet;
            RigidBody.Type = RigidBodyType.PlayerBullet;
            RigidBody.AddCollisionType(RigidBodyType.Enemy | RigidBodyType.EnemyBullet);
        }
    }
}
