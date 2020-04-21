using RhytmFighter.Interfaces;

namespace RhytmFighter.Battle.AI
{
    public abstract class AbstractAI : iUpdatable
    {
        protected iBattleObject m_ControlledObject;


        public AbstractAI(iBattleObject controlledObject)
        {
            m_ControlledObject = controlledObject;
        }

        public abstract void ExecuteAction(int currentTick);

        public abstract void PerformUpdate(float deltaTime);


        protected enum PatternActionTypes
        {
            SimpleAttack,
            Defence,
            Idle
        }
    }

    public enum AITypes
    {
        None,
        Simple
    }
}
