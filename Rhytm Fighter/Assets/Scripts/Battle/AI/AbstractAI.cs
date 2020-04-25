using RhytmFighter.Core;

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
    }
}
