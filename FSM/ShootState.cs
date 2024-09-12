using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Heads
{
    class ShootState : State
    {
        private Enemy enemy;

        private float shootTimeLimit;
        private float shootCoolDown;

        private float timeToNextCheckForNewPlayer;

        public ShootState(Enemy enemy)
        {
            shootTimeLimit = 2;
            shootCoolDown = 0;

            this.enemy = enemy;
        }

        public override void OnEnter()
        {
            enemy.RigidBody.Velocity = Vector2.Zero;
        }

        public override void Update()
        {
            timeToNextCheckForNewPlayer -= Game.DeltaTime;

            if (timeToNextCheckForNewPlayer <= 0)
            {
                enemy.Rival = enemy.GetBestPlayerToFight();
                timeToNextCheckForNewPlayer = RandomGenerator.GetRandomFloat() + 0.2f;
            }

            shootCoolDown -= Game.Window.DeltaTime;

            if (enemy.HeadToEnergyPowerUpChoice())
            {
                enemy.GoToEnergyPowerUp = true;

                stateMachine.GoTo(StateType.Follow);
            }

            else if (enemy.Rival == null || !enemy.CanAttackPlayer())
            {
                stateMachine.GoTo(StateType.Walk);
            }

            else
            {
                enemy.LookAtPlayer();

                if (shootCoolDown <= 0.0f)
                {
                    shootCoolDown = shootTimeLimit;
                    enemy.ShootPlayer();
                }
            }
        }
    }
}
