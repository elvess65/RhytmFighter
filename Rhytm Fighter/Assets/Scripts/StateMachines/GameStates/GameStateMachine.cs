using UnityEngine;

namespace RhytmFighter.StateMachines.GameState
{
    public class GameStateMachine : AbstractStateMachine
    {
        private GameState_Abstract m_CurrentGameState;


        public void HandleTouch(Vector3 mouseScreenPos)
        {
            m_CurrentGameState.HandleTouch(mouseScreenPos);
        }

        protected override void SetState(AbstractState state)
        {
            base.SetState(state);

            m_CurrentGameState = m_CurrentState as GameState_Abstract;
        }
    }
}
