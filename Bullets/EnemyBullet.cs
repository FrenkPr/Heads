using OpenTK;

namespace Heads
{
    class EnemyBullet : Bullet
    {
        public EnemyBullet() : base("bullet")
        {
            Dmg = 25;

            Type = BulletType.EnemyBullet;
            RigidBody.Type = RigidBodyType.EnemyBullet;

            sprite.SetAdditiveTint(255, 0, 255, 0);
        }
    }
}
