using OpenTK;
using System;
using System.Collections.Generic;

namespace Heads
{
    class Enemy : Actor
    {
        private float visionRadius;
        private float shootDistance;

        private float halfConeAngle;
        private Vector2 pointToReach;

        private float followSpeed;
        private float walkSpeed;

        private StateMachine fsm;
        public Player Rival;

        public bool GoToEnergyPowerUp;

        public Enemy() : base("enemy", new Vector2(2))
        {
            halfConeAngle = MathHelper.DegreesToRadians(40);
            visionRadius = 5;
            shootDistance = 3;
            walkSpeed = 1.5f;
            followSpeed = walkSpeed * 2;

            bulletType = BulletType.EnemyBullet;
            RigidBody.Type = RigidBodyType.Enemy;

            RigidBody.AddCollisionType(RigidBodyType.Player | RigidBodyType.PlayerBullet);

            fsm = new StateMachine();

            fsm.AddState(StateType.Walk, new WalkState(this));
            fsm.AddState(StateType.Follow, new FollowState(this));
            fsm.AddState(StateType.Shoot, new ShootState(this));

            fsm.GoTo(StateType.Walk);
        }

        public void ComputeRandomPoint()
        {
            Vector2 randPosition = Vector2.Zero;
            List<EnergyPowerUp> visiblePowerUps = GetVisibleEnergyPowerUps();

            if (visiblePowerUps == null)
            {
                randPosition.X = RandomGenerator.GetRandomFloat() * (Game.Window.OrthoWidth - 2) + 1;
                randPosition.Y = RandomGenerator.GetRandomFloat() * (Game.Window.OrthoHeight - 2) + 1;
            }

            else
            {
                if (visiblePowerUps.Count == 0)
                {
                    randPosition.X = RandomGenerator.GetRandomFloat() * (Game.Window.OrthoWidth - 2) + 1;
                    randPosition.Y = RandomGenerator.GetRandomFloat() * (Game.Window.OrthoHeight - 2) + 1;
                }

                else
                {
                    randPosition = GetNearestEnergyPowerUpPositionToEnemy(visiblePowerUps);
                }
            }

            pointToReach = randPosition;
        }

        private Vector2 GetNearestEnergyPowerUpPositionToEnemy(List<EnergyPowerUp> powerUps)
        {
            Vector2 minDist = powerUps[0].Position - Position;
            Vector2 nearestTarget = powerUps[0].Position;

            for (int i = 1; i < powerUps.Count; i++)
            {
                Vector2 dist = powerUps[i].Position - Position;

                if (dist.LengthSquared < minDist.LengthSquared)
                {
                    minDist = dist;
                    nearestTarget = powerUps[i].Position;
                }
            }

            return nearestTarget;
        }

        public void HeadToPoint()
        {
            Vector2 dist = pointToReach - Position;

            if (dist.LengthSquared <= 0.5f)
            {
                ComputeRandomPoint();
            }

            //Console.WriteLine("Dist LengthSquared: " + dist.LengthSquared + "\ndist Vector: " + dist);

            RigidBody.Velocity = walkSpeed * dist.Normalized();
        }

        public void HeadToPointFast()
        {
            Vector2 dist = pointToReach - Position;

            if (dist.LengthSquared <= 0.5f)
            {
                GoToEnergyPowerUp = false;
                ComputeRandomPoint();
            }

            //Console.WriteLine("Dist LengthSquared: " + dist.LengthSquared + "\ndist Vector: " + dist);

            RigidBody.Velocity = followSpeed * dist.Normalized();
        }

        public bool CanAttackPlayer()
        {
            if (Rival == null || !Rival.IsAlive)
            {
                return false;
            }

            Vector2 distVect = Rival.Position - Position;

            return distVect.LengthSquared < shootDistance * shootDistance;
        }

        public void HeadToPlayer()
        {
            if (Rival != null)
            {
                Vector2 distVect = Rival.Position - Position;
                RigidBody.Velocity = distVect.Normalized() * followSpeed;
            }
        }

        public List<Player> GetVisiblePlayers()
        {
            List<Player> players = ((PlayScene)Game.CurrentScene).Players;
            List<Player> visiblePlayers = new List<Player>();

            for (int i = 0; i < players.Count; i++)
            {
                if (!players[i].IsAlive)
                {
                    continue;
                }

                Vector2 distVect = players[i].Position - Position;

                if (distVect.LengthSquared < visionRadius * visionRadius)
                {
                    // Player is inside vision radius
                    // Check for cone angle
                    float angleCos = MathHelper.Clamp(Vector2.Dot(Forward, distVect.Normalized()), -1, 1);
                    float playerAngle = (float)Math.Acos(angleCos);

                    if (playerAngle < halfConeAngle)
                    {
                        visiblePlayers.Add(players[i]);
                    }
                }
            }

            return visiblePlayers;
        }

        public List<EnergyPowerUp> GetVisibleEnergyPowerUps()
        {
            List<EnergyPowerUp> energyPowerUps = PowerUpsMngr.GetActiveEnergyPowerUps();
            List<EnergyPowerUp> visibleEnergyPowerUps = new List<EnergyPowerUp>();

            if (energyPowerUps == null)
            {
                return null;
            }

            for (int i = 0; i < energyPowerUps.Count; i++)
            {
                Vector2 distVect = energyPowerUps[i].Position - Position;

                // Check for cone angle
                double angleCos = MathHelper.Clamp(Vector2.Dot(Forward, distVect.Normalized()), -1, 1);
                float powerUpAngle = (float)Math.Acos(angleCos);

                if (powerUpAngle < halfConeAngle)
                {
                    visibleEnergyPowerUps.Add(energyPowerUps[i]);
                }

            }

            return visibleEnergyPowerUps;
        }

        //this method decides, during a fight with player,
        //if to head to an energy power up to get healed
        //or if to continue shooting to the player via a fuzzy logic
        public bool HeadToEnergyPowerUpChoice()
        {
            if (GetVisibleEnergyPowerUps().Count == 0)
            {
                return false;
            }

            Player bestPlayer = GetBestPlayerToFight();
            Vector2 nearestPowerUpPosition = GetNearestEnergyPowerUpPositionToEnemy(GetVisibleEnergyPowerUps());

            if (bestPlayer != null)
            {
                //the distance from player to nearest enemy energy powerUp
                Vector2 distanceFromPlayerToEnergyPowerUp = bestPlayer.Position - nearestPowerUpPosition;

                //the distance from enemy to his nearest energy powerUp
                Vector2 distanceFromEnemyToEnergyPowerUp = Position - nearestPowerUpPosition;

                //distance fuzzy
                float longestDistanceSquared = Math.Max(distanceFromPlayerToEnergyPowerUp.LengthSquared, distanceFromEnemyToEnergyPowerUp.LengthSquared);
                float distanceFuzzy = 1 - (Math.Abs(distanceFromPlayerToEnergyPowerUp.LengthSquared - distanceFromEnemyToEnergyPowerUp.LengthSquared) / longestDistanceSquared);

                // Energy fuzzy
                float playerEnergyFuzzy = 1 - bestPlayer.Energy / bestPlayer.MaxEnergy;
                float enemyEnergyFuzzy = 1 - Energy / MaxEnergy;

                //computes fuzzies to compare
                float shootFuzzy = distanceFuzzy + playerEnergyFuzzy;
                float headToEnergyPowerUpFuzzy = enemyEnergyFuzzy;

                // Check for best result
                if (headToEnergyPowerUpFuzzy > shootFuzzy)
                {
                    pointToReach = nearestPowerUpPosition;

                    return true;
                }
            }

            return false;
        }

        public Player GetBestPlayerToFight()
        {
            Player bestPlayer = null;
            List<Player> visiblePlayers = GetVisiblePlayers();

            if (visiblePlayers.Count > 0)
            {
                // We need to decide only if we currently have 2 Players
                if (visiblePlayers.Count > 1)
                {
                    // Init the FuzzySum variable to -1 (our min value)
                    float maxFuzzy = -1;

                    for (int i = 0; i < visiblePlayers.Count; i++)
                    {
                        // Distance
                        Vector2 distanceFromPlayer = Position - visiblePlayers[i].Position;
                        float fuzzyDistance = 1 - distanceFromPlayer.LengthSquared / (visionRadius * visionRadius);

                        // Energy
                        float fuzzyEnergy = 1 - visiblePlayers[i].Energy / visiblePlayers[i].MaxEnergy;

                        // Angle
                        float playerAngle = (float)Math.Acos(MathHelper.Clamp(Vector2.Dot(visiblePlayers[i].Forward, distanceFromPlayer.Normalized()), -1, 1));
                        float fuzzyAngle = 1 - (playerAngle / (float)Math.PI);

                        // Sum
                        float fuzzySum = fuzzyDistance + fuzzyEnergy + fuzzyAngle;

                        // Check for best result
                        if (fuzzySum > maxFuzzy)
                        {
                            maxFuzzy = fuzzySum;
                            bestPlayer = visiblePlayers[i];
                        }
                    }
                }

                else
                {
                    // We only have 1 Player
                    bestPlayer = visiblePlayers[0];
                }
            }

            return bestPlayer;
        }

        public void ShootPlayer()
        {
            Shoot(Forward);
        }

        public void LookAtPlayer()
        {
            if (Rival != null)
            {
                Vector2 direction = Rival.Position - Position;
                Forward = direction;
            }
        }

        public override void Update()
        {
            if (IsActive)
            {
                if (RigidBody.Velocity != Vector2.Zero)
                {
                    Forward = RigidBody.Velocity;
                    EnergyBar.Position = new Vector2(Position.X - EnergyBar.HalfWidth, Position.Y - HalfHeight - 0.2f);
                    ActiveActors[this] = RigidBody;
                }

                fsm.Update();
            }
        }

        public override void OnDie()
        {
            EnemyMngr.RestoreEnemy(this);
        }
    }
}
