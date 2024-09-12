using System;
using System.Collections.Generic;
using OpenTK;

namespace Heads
{
    abstract class Actor : GameObject
    {
        public float Energy { get; protected set; }
        public int MaxEnergy { get; protected set; }
        protected BulletType bulletType;
        public bool IsAlive { get { return Energy > 0; } }
        public ProgressBar EnergyBar;
        public static Dictionary<Actor, RigidBody> ActiveActors { get; private set; }

        public Actor(string textureName, Vector2 moveSpeed = default, int numFrames = 0, int width = 0, int height = 0) : base(textureName, numFrames, width, height)
        {
            MaxEnergy = 100;
            EnergyBar = new ProgressBar("frameProgressBar", "progressBar", new Vector2(Game.PixelsToUnits(4)));
            ResetEnergy();

            RigidBody = new RigidBody(this, moveSpeed);
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);

            //DebugMngr.AddItem(RigidBody.Collider);

            UpdateMngr.Add(this);
            DrawMngr.Add(this);

            if (ActiveActors == null)
            {
                ActiveActors = new Dictionary<Actor, RigidBody>();
            }
        }

        public void Shoot(Vector2 direction)
        {
            Bullet bullet = BulletMngr.GetBullet(bulletType);

            if (bullet != null)
            {
                bullet.Shoot(Position, direction);
                bullet.Forward = direction;
            }
        }

        public override void Update()
        {
            base.Update();

            if (IsActive)
            {
                if (RigidBody.Velocity != Vector2.Zero)
                {
                    Forward = RigidBody.Velocity;
                }

                if (!(this is Player))
                {
                    EnergyBar.Position = new Vector2(Position.X - EnergyBar.HalfWidth, Position.Y - HalfHeight - EnergyBar.Height - 0.1f);
                }

                ActiveActors[this] = RigidBody;
            }
        }

        public override void OnCollision(CollisionInfo collisionInfo)
        {
            OnActorToActorCollision(collisionInfo);
        }

        private void OnActorToActorCollision(CollisionInfo collisionInfo)
        {
            if (!(collisionInfo.Collider is Actor))
            {
                return;
            }

            // Horizontal Collision
            if (collisionInfo.Delta.X < collisionInfo.Delta.Y)
            {
                if (this is Player player1 && collisionInfo.Collider is Player player2)
                {
                    if ((Game.KeyboardCtrls[player1.Id].IsKeyHold(KeyCodeType.Left) && Game.KeyboardCtrls[player2.Id].IsKeyHold(KeyCodeType.Right)) ||
                        (Game.KeyboardCtrls[player1.Id].IsKeyHold(KeyCodeType.Right) && Game.KeyboardCtrls[player2.Id].IsKeyHold(KeyCodeType.Left)))
                    {
                        if (X < collisionInfo.Collider.X)
                        {
                            // Collision from Left (inverse horizontal delta)
                            collisionInfo.Delta.X = -collisionInfo.Delta.X;
                        }

                        X += collisionInfo.Delta.X;
                        collisionInfo.Collider.X -= collisionInfo.Delta.X;

                        RigidBody.Velocity.X = 0;
                        collisionInfo.Collider.RigidBody.Velocity.X = 0;
                    }

                    else if (Game.KeyboardCtrls[player1.Id].IsKeyHold(KeyCodeType.Left) ||
                             Game.KeyboardCtrls[player1.Id].IsKeyHold(KeyCodeType.Right))
                    {
                        if (player1.X < player2.X)
                        {
                            // Collision from Left (inverse horizontal delta)
                            collisionInfo.Delta.X = -collisionInfo.Delta.X;
                        }

                        player1.X += collisionInfo.Delta.X;
                        player1.RigidBody.Velocity.X = 0;
                    }

                    else
                    {
                        if (player2.X < player1.X)
                        {
                            // Collision from Left (inverse horizontal delta)
                            collisionInfo.Delta.X = -collisionInfo.Delta.X;
                        }

                        player2.X += collisionInfo.Delta.X;
                        player2.RigidBody.Velocity.X = 0;
                    }
                }

                else
                {
                    if (X < collisionInfo.Collider.X)
                    {
                        // Collision from Left (inverse horizontal delta)
                        collisionInfo.Delta.X = -collisionInfo.Delta.X;
                    }

                    X += collisionInfo.Delta.X;
                }
            }

            // Vertical Collision
            else
            {
                if (this is Player player1 && collisionInfo.Collider is Player player2)
                {
                    if ((Game.KeyboardCtrls[player1.Id].IsKeyHold(KeyCodeType.Up) && Game.KeyboardCtrls[player2.Id].IsKeyHold(KeyCodeType.Down)) ||
                        (Game.KeyboardCtrls[player1.Id].IsKeyHold(KeyCodeType.Down) && Game.KeyboardCtrls[player2.Id].IsKeyHold(KeyCodeType.Up)))
                    {
                        if (Y < collisionInfo.Collider.Y)
                        {
                            // Collision from Left (inverse horizontal delta)
                            collisionInfo.Delta.Y = -collisionInfo.Delta.Y;
                        }

                        Y += collisionInfo.Delta.Y;
                        collisionInfo.Collider.Y -= collisionInfo.Delta.Y;

                        RigidBody.Velocity.Y = 0;
                        collisionInfo.Collider.RigidBody.Velocity.Y = 0;
                    }

                    else if (Game.KeyboardCtrls[player1.Id].IsKeyHold(KeyCodeType.Up) ||
                             Game.KeyboardCtrls[player1.Id].IsKeyHold(KeyCodeType.Down))
                    {
                        if (player1.Y < player2.Y)
                        {
                            // Collision from Top (inverse vertical delta)
                            collisionInfo.Delta.Y = -collisionInfo.Delta.Y;
                        }

                        player1.Y += collisionInfo.Delta.Y;
                        player1.RigidBody.Velocity.Y = 0;
                    }

                    else
                    {
                        if (player2.Y < player1.Y)
                        {
                            // Collision from Top (inverse vertical delta)
                            collisionInfo.Delta.Y = -collisionInfo.Delta.Y;
                        }

                        player2.Y += collisionInfo.Delta.Y;
                        player2.RigidBody.Velocity.Y = 0;
                    }
                }

                else
                {
                    if (Y < collisionInfo.Collider.Y)
                    {
                        // Collision from Top (inverse vertical delta)
                        collisionInfo.Delta.Y = -collisionInfo.Delta.Y;
                    }

                    Y += collisionInfo.Delta.Y;
                }
            }
        }

        public virtual void OnDie()
        {

        }

        public void ResetEnergy()
        {
            Energy = MaxEnergy;
            EnergyBar.Scale((float)Energy / (float)MaxEnergy);
        }

        public void AddDamage(int dmg)
        {
            Energy -= dmg;
            EnergyBar.Scale((float)Energy / (float)MaxEnergy);
        }
    }
}
