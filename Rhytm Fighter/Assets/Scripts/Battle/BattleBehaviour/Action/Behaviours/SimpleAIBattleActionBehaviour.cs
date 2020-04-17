using RhytmFighter.Battle.Command;
using RhytmFighter.Battle.Command.Model;

namespace RhytmFighter.Battle.Action.Behaviours
{
    public class SimpleAIBattleActionBehaviour : SimpleBattleActionBehaviour
    {
        private int m_CycleIterator = 0;
        private int m_ActionIterator = 0;
        private int m_IdlesBeforeAttack;
        private int m_IdlesAfterAttack;
        private PatternActionTypes[] m_ActionPattern;


        public SimpleAIBattleActionBehaviour(int applyDelay, int useDelay, int damage) : base(applyDelay, useDelay, damage)
        {
            m_IdlesBeforeAttack = 2;
            m_IdlesAfterAttack = 3;

            m_ActionPattern = new PatternActionTypes[]
            {
                PatternActionTypes.SimpleAttack,
                PatternActionTypes.Idle,
                PatternActionTypes.SimpleAttack,
                PatternActionTypes.Idle
            };
        }

        public override void ExecuteAction(int currentTick, CommandTypes type)
        {
            if (m_CycleIterator++ >= m_IdlesBeforeAttack)
            {
                if (m_ActionIterator < m_ActionPattern.Length)
                {
                    PatternActionTypes action = m_ActionPattern[m_ActionIterator++];
                    switch(action)
                    {
                        case PatternActionTypes.SimpleAttack:

                            ExecuteCommand(new AttackCommandModel(m_ControlledObject, Target, m_ApplyDelay, m_Damage));

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
