using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heads
{
    class FollowState : State
    {
        private Enemy enemy;
        private float timeToNextCheckForNewPlayer;

        public FollowState(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public override void Update()
        {
            timeToNextCheckForNewPlayer -= Game.DeltaTime;

            if (timeToNextCheckForNewPlayer <= 0)
            {
                enemy.Rival = enemy.GetBestPlayerToFight();

                timeToNextCheckForNewPlayer = RandomGenerator.GetRandomFloat() + 0.2f;
            }

            if (enemy.GoToEnergyPowerUp)
            {
                enemy.HeadToPointFast();
            }

            else if (enemy.Rival == null || !enemy.Rival.IsAlive)
            {
                stateMachine.GoTo(StateType.Walk);
            }
            else if (enemy.CanAttackPlayer())
            {
                stateMachine.GoTo(StateType.Shoot);
            }
            else
            {
                enemy.HeadToPlayer();
            }
        }
    }
}
