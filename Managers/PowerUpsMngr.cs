using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Heads
{
    static class PowerUpsMngr
    {
        private static Queue<PowerUp>[] powerUps;
        private static List<PowerUp> activePowerUps;
        private static int numPowerUpsPerCategory;
        private static float[] powerUpTimers;
        private static float[] timeToNextPowerUp;

        public static void Init()
        {
            numPowerUpsPerCategory = 2;
            powerUps = new Queue<PowerUp>[(int)PowerUpType.Length];
            activePowerUps = new List<PowerUp>();

            Type[] powerUpTypes = new Type[powerUps.Length];

            powerUpTypes[0] = typeof(EnergyPowerUp);


            powerUpTimers = new float[powerUps.Length];
            timeToNextPowerUp = new float[powerUps.Length];

            powerUpTimers[0] = 10;

            for (int i = 0; i < powerUps.Length; i++)
            {
                powerUps[i] = new Queue<PowerUp>(numPowerUpsPerCategory);
                timeToNextPowerUp[i] = powerUpTimers[i];

                for (int j = 0; j < numPowerUpsPerCategory; j++)
                {
                    powerUps[i].Enqueue((PowerUp)Activator.CreateInstance(powerUpTypes[i]));
                }
            }
        }

        public static void RestorePowerUp(PowerUp powerUp)
        {
            powerUps[(int)powerUp.Type].Enqueue(powerUp);
            activePowerUps.Remove((EnergyPowerUp)powerUp);

            powerUp.IsActive = false;
        }

        public static List<PowerUp> GetActivePowerUps()
        {
            return activePowerUps;
        }

        public static List<EnergyPowerUp> GetActiveEnergyPowerUps()
        {
            List<EnergyPowerUp> activeEnergyPowerUps = new List<EnergyPowerUp>();

            if (activePowerUps == null)
            {
                return null;
            }

            for (int i = 0; i < activePowerUps.Count; i++)
            {
                if (activePowerUps[i] is EnergyPowerUp)
                {
                    activeEnergyPowerUps.Add((EnergyPowerUp)activePowerUps[i]);
                }
            }

            return activeEnergyPowerUps;
        }

        public static void ClearAll()
        {
            powerUps = null;
            activePowerUps = null;
            powerUpTimers = null;
            timeToNextPowerUp = null;
        }

        public static void Spawn()
        {
            for (int i = 0; i < powerUps.Length; i++)
            {
                timeToNextPowerUp[i] -= Game.DeltaTime;

                if (powerUps[i].Count > 0 && timeToNextPowerUp[i] <= 0)
                {
                    Dictionary<Actor, RigidBody> actors = Actor.ActiveActors;

                    PowerUp powerUp = powerUps[i].Dequeue();

                    powerUp.Position = new Vector2(RandomGenerator.GetRandomInt((int)powerUp.HalfWidth, (int)(Game.OrthoWidth - powerUp.HalfWidth - 1)) + RandomGenerator.GetRandomFloat(), RandomGenerator.GetRandomInt((int)powerUp.HalfHeight, (int)(Game.OrthoHeight - powerUp.HalfHeight - 1)) + RandomGenerator.GetRandomFloat());

                    if (actors.Count > 0)
                    {
                        bool collided = true;

                        while (collided)
                        {
                            collided = false;

                            foreach (RigidBody actorRigidBody in actors.Values)
                            {
                                if (powerUp.RigidBody.Collides(actorRigidBody, ref PhysicsMngr.CollisionInfo))
                                {
                                    powerUp.Position = new Vector2(RandomGenerator.GetRandomInt((int)powerUp.HalfWidth, (int)(Game.OrthoWidth - powerUp.HalfWidth - 1)) + RandomGenerator.GetRandomFloat(), RandomGenerator.GetRandomInt((int)powerUp.HalfHeight, (int)(Game.OrthoHeight - powerUp.HalfHeight - 1)) + RandomGenerator.GetRandomFloat());
                                    collided = true;

                                    break;
                                }
                            }
                        }
                    }

                    if (activePowerUps.Count > 0)
                    {
                        bool collided = true;

                        while (collided)
                        {
                            collided = false;

                            foreach (PowerUp activePowerUp in activePowerUps)
                            {
                                if (powerUp.RigidBody.Collides(activePowerUp.RigidBody, ref PhysicsMngr.CollisionInfo))
                                {
                                    powerUp.Position = new Vector2(RandomGenerator.GetRandomInt((int)powerUp.HalfWidth, (int)(Game.OrthoWidth - powerUp.HalfWidth - 1)) + RandomGenerator.GetRandomFloat(), RandomGenerator.GetRandomInt((int)powerUp.HalfHeight, (int)(Game.OrthoHeight - powerUp.HalfHeight - 1)) + RandomGenerator.GetRandomFloat());
                                    collided = true;

                                    break;
                                }
                            }
                        }
                    }

                    activePowerUps.Add((EnergyPowerUp)powerUp);
                    powerUp.IsActive = true;

                    timeToNextPowerUp[i] = powerUpTimers[i];
                }

                else if (timeToNextPowerUp[i] <= 0)
                {
                    timeToNextPowerUp[i] = powerUpTimers[i];
                }
            }
        }
    }
}