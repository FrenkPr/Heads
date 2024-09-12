using OpenTK;

namespace Heads
{
    enum BulletType
    {
        PlayerBullet,
        EnemyBullet,
        Length
    }

    abstract class Bullet : GameObject
    {
        public int Dmg;
        protected Vector2 shootSpeed;
        public BulletType Type { get; protected set; }

        public Bullet(string textureName, int width = 0, int height = 0) : base(textureName, 1, width, height, DrawLayer.MiddleGround)
        {
            UpdateMngr.Add(this);
            DrawMngr.Add(this);

            shootSpeed = new Vector2(10);

            RigidBody = new RigidBody(this, Vector2.Zero);
            RigidBody.Collider = ColliderFactory.CreateCircleFor(this);
        }

        protected override void CheckOutOfScreen()
        {
            Vector2 distToOutOfScreen = Position - Game.ScreenCenter;

            if (IsActive && distToOutOfScreen.LengthSquared > Game.HalfDiagonalSquared)
            {
                BulletMngr.RestoreBullet(this);
            }
        }

        public void Shoot(Vector2 shootPos, Vector2 shootDir)
        {
            Position = shootPos;
            RigidBody.Velocity = shootSpeed * shootDir;
            Forward = shootDir;
        }

        public override void OnCollision(CollisionInfo collisionInfo)
        {
            //on any collision type this bullet will be restored
            BulletMngr.RestoreBullet(this);

            if (collisionInfo.Collider is Bullet)
            {
                BulletMngr.RestoreBullet((Bullet)collisionInfo.Collider);
            }

            else if (this is PlayerBullet && collisionInfo.Collider is Enemy enemy)
            {
                enemy.AddDamage(Dmg);

                if (!enemy.IsAlive)
                {
                    enemy.OnDie();
                    EnemyMngr.SpawnEnemy();
                }
            }
        }

        public virtual void Reset()
        {
            IsActive = false;
        }
    }
}
