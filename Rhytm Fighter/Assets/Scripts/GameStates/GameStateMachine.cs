using RhytmFighter.Core;
using UnityEngine;

namespace RhytmFighter.GameState
{
    public class GameStateMachine : iUpdatable
    {
        private GameState_Abstract m_CurrentState;


        public void Initialize(GameState_Abstract initialState)
        {
            SetState(initialState);
        }

        public void ChangeState(GameState_Abstract state)
        {
            m_CurrentState.ExitState();
            SetState(state);
        }

        public void PerformUpdate(float deltaTime)
        {
            m_CurrentState.PerformUpdate(deltaTime);
        }

        public void HandleTouch(Vector3 mouseScreenPos)
        {
            m_CurrentState.HandleTouch(mouseScreenPos);
        }


        void SetState(GameState_Abstract state)
        {
            m_CurrentState = state;
            state.EnterState();
        }
    }
}
