using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heads
{
    class WalkState : State
    {
        private Enemy enemy;
        private float timeToNextCheckForVisiblePlayer;

        private float checkForVisiblePowerUpTimer;
        private float timeToNextcheckForVisiblePowerUp;

        public WalkState(Enemy enemy)
        {
            this.enemy = enemy;
            checkForVisiblePowerUpTimer = 1;
        }

        public override void OnEnter()
        {
            enemy.ComputeRandomPoint();
            timeToNextCheckForVisiblePlayer = 0;
            timeToNextcheckForVisiblePowerUp = 0;
        }

        public override void Update()
        {
            timeToNextCheckForVisiblePlayer -= Game.DeltaTime;
            timeToNextcheckForVisiblePowerUp -= Game.DeltaTime;

            if (timeToNextCheckForVisiblePlayer <= 0)
            {
                Player p = enemy.GetBestPlayerToFight();
                timeToNextCheckForVisiblePlayer = RandomGenerator.GetRandomFloat() + 0.2f;

                if (p != null)
                {
                    enemy.Rival = p;
                    stateMachine.GoTo(StateType.Follow);
                    return;
                }
            }

            if (enemy.GetVisibleEnergyPowerUps().Count != 0 && timeToNextcheckForVisiblePowerUp <= 0)
            {
                enemy.ComputeRandomPoint();
                timeToNextcheckForVisiblePowerUp = checkForVisiblePowerUpTimer;
            }

            if (enemy.Energy < enemy.MaxEnergy * 0.5f && enemy.GetVisibleEnergyPowerUps().Count != 0)
            {
                enemy.HeadToPointFast();
            }

            else
            {
                enemy.HeadToPoint();
            }
        }
    }
}
