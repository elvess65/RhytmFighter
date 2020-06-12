using RhytmFighter.Battle.AI.Abstract;
using RhytmFighter.Battle.Core.Abstract;
using RhytmFighter.Persistant.Enums;

namespace RhytmFighter.Battle.AI
{
    public class SimpleDefencibleAI : AbstractAI
    {
        private double m_TimeToNextAction;
        private float m_NextCommandEventExecutionTime;
        private CommandTypes m_NextCommandType;

        private int m_ActionIterator = 0;
        private AIActionTypes[] m_ActionPattern;


        public SimpleDefencibleAI(iBattleObject controlledObject) : base(controlledObject)
        {
            m_ActionPattern = new AIActionTypes[]
            {
                AIActionTypes.Idle,
                AIActionTypes.SimpleAttack,
                AIActionTypes.Idle,
                AIActionTypes.SimpleAttack,
                AIActionTypes.Idle,
                AIActionTypes.Idle,
                AIActionTypes.Defence,
                AIActionTypes.Idle,
                AIActionTypes.Defence,
            };
        }

        public override void ExecuteAction(int currentTick)
        {
            if (m_ActionIterator < m_ActionPattern.Length)
            {
                switch (m_ActionPattern[m_ActionIterator])
                {
                    case AIActionTypes.SimpleAttack:
                        CommandTypes type = CommandTypes.Attack;

                        m_ControlledObject.ActionBehaviour.ExecuteAction(type);
                        break;
                    case AIActionTypes.Defence:
                        type = CommandTypes.Defence;

                        m_ControlledObject.ActionBehaviour.ExecuteAction(type);
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

                if (m_TimeToNextAction - m_NextCommandEventExecutionTime <= 0)
                {
                    m_ControlledObject.NotifyViewAboutCommand(m_NextCommandType);
                    m_TimeToNextAction = 0;
                }
            }
        }


        private void IncrementIterator()
        {
            //Increment iterator
            m_ActionIterator++;

            if (m_ActionIterator >= m_ActionPattern.Length)
                m_ActionIterator = 0;

            //Get next action
            int iterations = 0;
            int curIndex = m_ActionIterator;
            AIActionTypes nextAction = AIActionTypes.Idle;

            while (iterations++ < m_ActionPattern.Length)
            {
                nextAction = m_ActionPattern[curIndex];

                if (nextAction != AIActionTypes.Idle)
                {
                    m_NextCommandType = Persistant.Converters.ConvertersCollection.AIAction2Command(nextAction);
                    m_NextCommandEventExecutionTime = m_ControlledObject.GetActionEventExecuteTime(m_NextCommandType);
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
