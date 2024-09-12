using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heads
{
    enum StateType
    {
        Walk, Follow, Shoot
    }

    class StateMachine
    {
        private Dictionary<StateType, State> states;
        private State current;

        public StateMachine()
        {
            states = new Dictionary<StateType, State>();
            current = null;
        }

        public void AddState(StateType key, State state)
        {
            states[key] = state;
            state.SetStateMachine(this);
        }

        public void GoTo(StateType key)
        {
            if(current != null)
            {
                current.OnExit();
            }

            current = states[key];
            current.OnEnter();
        }

        public void Update()
        {
            current.Update();
        }
    }
}
