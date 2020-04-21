namespace RhytmFighter.Battle.AI
{
    public class SimpleAI : AbstractAI
    {
        private double m_TimeToNextAction;
        private PatternActionTypes m_NextAction;

        private int m_ActionIterator = 0;
        private PatternActionTypes[] m_ActionPattern;

        public SimpleAI(iBattleObject controlledObject) : base(controlledObject)
        {
            m_ActionPattern = new PatternActionTypes[]
            {
                PatternActionTypes.Idle,
                PatternActionTypes.SimpleAttack,
                PatternActionTypes.Idle,
                PatternActionTypes.SimpleAttack,
                PatternActionTypes.Idle,
                PatternActionTypes.Idle,
                PatternActionTypes.Defence,
                PatternActionTypes.Idle,
                PatternActionTypes.Idle
            };
        }

        public override void ExecuteAction(int currentTick)
        {
            if (m_ActionIterator < m_ActionPattern.Length)
            {
                switch (m_ActionPattern[m_ActionIterator])
                {
                    case PatternActionTypes.SimpleAttack:
                        m_ControlledObject.ActionBehaviour.ExecuteAction(Command.Model.CommandTypes.Attack);
                        break;
                    case PatternActionTypes.Defence:
                        m_ControlledObject.ActionBehaviour.ExecuteAction(Command.Model.CommandTypes.Defence);
                        break;
                }

                IncrementIterator();
            }
        }

        public override void PerformUpdate(float deltaTime)
        {
            if (m_TimeToNextAction > 0)
            {
                m_TimeToNextAction -= deltaTime;

                //TODO: Substract animation offset
                //Play animation first
            }
        }

        private void IncrementIterator()
        {
            m_ActionIterator++;

            if (m_ActionIterator >= m_ActionPattern.Length)
                m_ActionIterator = 0;

            int curIndex = m_ActionIterator;
            int iterations = 0;
            while(iterations++ < m_ActionPattern.Length)
            {
                m_NextAction = m_ActionPattern[curIndex];

                if (m_NextAction != PatternActionTypes.Idle)
                {
                    m_TimeToNextAction = Rhytm.RhytmController.GetInstance().TimeToNextTick + (Rhytm.RhytmController.GetInstance().TickDurationSeconds * (iterations - 1));
                    break;
                }

                curIndex++;
                if (curIndex >= m_ActionPattern.Length)
                    curIndex = 0;
            }
        }
    }
}
