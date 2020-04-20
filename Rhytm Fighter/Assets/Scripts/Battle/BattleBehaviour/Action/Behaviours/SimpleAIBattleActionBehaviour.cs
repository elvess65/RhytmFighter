using RhytmFighter.Battle.Command;
using RhytmFighter.Battle.Command.Model;

namespace RhytmFighter.Battle.Action.Behaviours
{
    public class SimpleAIBattleActionBehaviour : SimpleBattleActionBehaviour
    {
        private int m_ActionIterator = 0;
        private PatternActionTypes[] m_ActionPattern;


        public SimpleAIBattleActionBehaviour(int applyDelay, int useDelay, int damage) : base(applyDelay, useDelay, damage)
        {
            m_ActionPattern = new PatternActionTypes[]
            {
                PatternActionTypes.Idle,
                PatternActionTypes.SimpleAttack,
                PatternActionTypes.Idle,
                PatternActionTypes.SimpleAttack,
                PatternActionTypes.Idle,
                PatternActionTypes.Idle,
                PatternActionTypes.SimpleAttack,
                PatternActionTypes.Idle,
                PatternActionTypes.Idle
            };
        }

        public override void ExecuteAction(int currentTick, CommandTypes type)
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

                if (m_ActionIterator >= m_ActionPattern.Length)
                    m_ActionIterator = 0;
            }
        }
    }
}
