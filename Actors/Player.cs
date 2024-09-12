using Aiv.Fast2D;
using OpenTK;

namespace Heads
{
    class Player : Actor
    {
        private bool isFirePressed;
        private Vector2 moveSpeed;
        public int Id { get; private set; }

        public Player(int id) : base("player")
        {
            Id = id;

            moveSpeed = new Vector2(5);

            IsActive = true;
            EnergyBar.IsActive = true;

            bulletType = BulletType.PlayerBullet;
            RigidBody.Type = RigidBodyType.Player;

            //added collision type with himself in case of more players
            RigidBody.AddCollisionType(RigidBodyType.Player | RigidBodyType.Enemy | RigidBodyType.EnemyBullet);

            RigidBody.Friction = 40;

            //DebugMngr.AddItem(RigidBody.Collider);
        }

        private bool IsJoypadFireHold()
        {
            if (Game.JoypadCtrls.Count != 0)
            {
                if (Game.JoypadCtrls[Id].IsFireHold())
                {
                    return true;
                }
            }

            return false;
        }

        public void Input()
        {
            if (!IsActive)
            {
                return;
            }

            Move();
            Shoot();
        }

        private void Move()
        {
            //MOVE INPUT
            Vector2 dir = GetKeyboardMoveDir() != Vector2.Zero ? GetKeyboardMoveDir() : GetJoypadMoveDir();

            if (dir.Length > 1)
            {
                dir.Normalize();
            }

            if (dir != Vector2.Zero)
            {
                RigidBody.Velocity = moveSpeed * dir;
            }
            //END MOVE INPUT
        }

        public void Shoot()
        {
            //SHOOT INPUT
            if (Game.KeyboardCtrls[Id].IsFireHold() || IsJoypadFireHold())
            {
                if (!isFirePressed)
                {
                    Shoot(Forward);
                    isFirePressed = true;
                }
            }

            else if (isFirePressed)
            {
                isFirePressed = false;
            }
            //END SHOOT INPUT
        }

        private Vector2 GetKeyboardMoveDir()
        {
            return new Vector2(Game.KeyboardCtrls[Id].GetHorizontal(), Game.KeyboardCtrls[Id].GetVertical());
        }

        private Vector2 GetJoypadMoveDir()
        {
            return Game.JoypadCtrls.Count != 0 ? new Vector2(Game.JoypadCtrls[Id].GetHorizontal(), Game.JoypadCtrls[Id].GetVertical()) : Vector2.Zero;
        }

        public override void OnDie()
        {
            IsActive = false;
            ActiveActors.Remove(this);

            ((PlayScene)Game.CurrentScene).OnPlayerDies();
        }

        public override void OnCollision(CollisionInfo collisionInfo)
        {
            base.OnCollision(collisionInfo);

            if (collisionInfo.Collider is EnemyBullet bullet)
            {
                AddDamage(bullet.Dmg);
                BulletMngr.RestoreBullet(bullet);
            }

            if (!IsAlive && IsActive)
            {
                OnDie();
            }
        }

        public override void Update()
        {
            base.Update();

            if (Energy < MaxEnergy)
            {
                ResetEnergy();
            }

            //System.Console.WriteLine($"PLAYER {Id + 1} FORWARD: {Forward}");
            //System.Console.WriteLine("Player position: " + Position);
        }
    }
}
