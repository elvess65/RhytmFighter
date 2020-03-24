using UnityEngine;

namespace RhytmFighter.Battle.Action.Behaviours
{
    public class ExampleBattleActionBehaviour : iBattleActionBehaviour
    {
        private int m_CycleIterator = 0;
        private int m_ActionIterator = 0;
        private int m_IdlesBeforeAttack;
        private int m_IdlesAfterAttack;
        private PatternActionTypes[] m_ActionPattern;

        public ExampleBattleActionBehaviour()
        {
            m_IdlesBeforeAttack = 2;
            m_IdlesAfterAttack = 3;

            m_ActionPattern = new PatternActionTypes[]
            {
                PatternActionTypes.Action,
                PatternActionTypes.Idle,
                PatternActionTypes.Action,
                PatternActionTypes.Idle
            };
        }

        public void ExecuteAction()
        {
            if (m_CycleIterator++ >= m_IdlesBeforeAttack)
            {
                if (m_ActionIterator < m_ActionPattern.Length)
                {
                    PatternActionTypes action = m_ActionPattern[m_ActionIterator++];
                    switch(action)
                    {
                        case PatternActionTypes.Action:
                            Debug.LogError("Execute battle action");
                            break;
                    }
                }
                else if (m_CycleIterator < m_ActionPattern.Length + m_IdlesBeforeAttack + m_IdlesAfterAttack)
                {
                }
                else
                {
                    m_CycleIterator = 0;
                    m_ActionIterator = 0;
                }
            } 
        }
    }
}
