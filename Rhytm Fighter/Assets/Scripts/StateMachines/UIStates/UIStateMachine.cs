namespace RhytmFighter.StateMachines.UIState
{
    public class UIStateMachine : AbstractStateMachine
    {
        private UIState_Abstract m_CurrentUIState;


        protected override void SetState(AbstractState state)
        {
            base.SetState(state);

            m_CurrentUIState = m_CurrentState as UIState_Abstract;
        }
    }
}
