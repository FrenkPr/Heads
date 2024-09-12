using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heads
{
    abstract class State
    {
        protected StateMachine stateMachine;

        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public abstract void Update();

        public void SetStateMachine(StateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }
    }
}
