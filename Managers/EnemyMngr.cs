using System.Collections.Generic;
using OpenTK;

namespace Heads
{
    static class EnemyMngr
    {
        private static Queue<Enemy> enemies;
        private static int numEnemies;

        public static void Init()
        {
            numEnemies = 16;
            enemies = new Queue<Enemy>();

            for (int j = 0; j < numEnemies; j++)
            {
                enemies.Enqueue(new Enemy());
            }
        }

        public static void RestoreEnemy(Enemy enemy)
        {
            enemies.Enqueue(enemy);
            enemy.IsActive = false;
            enemy.EnergyBar.IsActive = false;
            Actor.ActiveActors.Remove(enemy);

            enemy.ResetEnergy();
        }

        public static Enemy SpawnEnemy()
        {
            if (enemies.Count > 0)
            {
                Dictionary<Actor, RigidBody> actors = Actor.ActiveActors;
                List<PowerUp> powerUps = PowerUpsMngr.GetActivePowerUps();
                Enemy enemy = enemies.Dequeue();

                enemy.Position = new Vector2(RandomGenerator.GetRandomInt((int)enemy.HalfWidth, (int)(Game.OrthoWidth - enemy.HalfWidth - 1)) + RandomGenerator.GetRandomFloat(), RandomGenerator.GetRandomInt((int)enemy.HalfHeight, (int)(Game.OrthoHeight - enemy.HalfHeight - 1)) + RandomGenerator.GetRandomFloat());

                if (actors.Count > 0)
                {
                    bool collided = true;

                    while (collided)
                    {
                        collided = false;

                        foreach (var actorRigidBody in actors.Values)
                        {
                            if (enemy.RigidBody.Collides(actorRigidBody, ref PhysicsMngr.CollisionInfo))
                            {
                                enemy.Position = new Vector2(RandomGenerator.GetRandomInt((int)enemy.HalfWidth, (int)(Game.OrthoWidth - enemy.HalfWidth - 1)) + RandomGenerator.GetRandomFloat(), RandomGenerator.GetRandomInt((int)enemy.HalfHeight, (int)(Game.OrthoHeight - enemy.HalfHeight - 1)) + RandomGenerator.GetRandomFloat());
                                collided = true;
                            }
                        }
                    }
                }

                if (powerUps.Count > 0)
                {
                    bool collided = true;

                    while (collided)
                    {
                        collided = false;

                        foreach (var powerUp in powerUps)
                        {
                            if (enemy.RigidBody.Collides(powerUp.RigidBody, ref PhysicsMngr.CollisionInfo))
                            {
                                enemy.Position = new Vector2(RandomGenerator.GetRandomInt((int)enemy.HalfWidth, (int)(Game.OrthoWidth - enemy.HalfWidth - 1)) + RandomGenerator.GetRandomFloat(), RandomGenerator.GetRandomInt((int)enemy.HalfHeight, (int)(Game.OrthoHeight - enemy.HalfHeight - 1)) + RandomGenerator.GetRandomFloat());
                                collided = true;
                            }
                        }
                    }
                }

                enemy.EnergyBar.Position = new Vector2(enemy.Position.X - enemy.EnergyBar.HalfWidth, enemy.Position.Y - enemy.HalfHeight - enemy.EnergyBar.Height);

                Actor.ActiveActors.Add(enemy, enemy.RigidBody);
                enemy.IsActive = true;
                enemy.EnergyBar.IsActive = true;

                return enemy;
            }

            return null;
        }

        public static void ClearAll()
        {
            enemies.Clear();
        }
    }
}
