using RhytmFighter.Core;

namespace RhytmFighter.StateMachines
{
    public class AbstractStateMachine : iUpdatable
    {
        protected AbstractState m_CurrentState;

        public virtual void Initialize(AbstractState initialState)
        {
            SetState(initialState);
        }

        public virtual void ChangeState(AbstractState state)
        {
            m_CurrentState.ExitState();
            SetState(state);
        }

        public virtual void PerformUpdate(float deltaTime)
        {
            m_CurrentState.PerformUpdate(deltaTime);
        }


        protected virtual void SetState(AbstractState state)
        {
            m_CurrentState = state;
            state.EnterState();
        }
    }
}
