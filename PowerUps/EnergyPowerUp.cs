using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heads
{
    class EnergyPowerUp : PowerUp
    {
        public EnergyPowerUp() : base("energyPowerUp", 80, 80)
        {
            Type = PowerUpType.Energy;
        }

        public override void OnCollision(CollisionInfo collisionInfo)
        {
            OnAttach((Actor)collisionInfo.Collider);

            actor.ResetEnergy();

            OnDetach();

            PowerUpsMngr.RestorePowerUp(this);
        }
    }
}
