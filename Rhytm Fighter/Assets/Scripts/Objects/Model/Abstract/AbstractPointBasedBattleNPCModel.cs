using System.Collections.Generic;
using Frameworks.Grid.Data;
using RhytmFighter.Battle.Action;
using RhytmFighter.Battle.Health;

namespace RhytmFighter.Objects.Model
{
    public abstract class AbstractPointBasedBattleNPCModel : AbstractBattleNPCModel
    {
        public System.Action<int> OnActionPointUsed;
        public System.Action<int> OnActionPointRestored;
    
        private int m_ActionPoints;
        private int m_CurrentActionPoints;
        private int m_TicksToRestoreActionPoint;
        private int m_ActionPointRestoreTick;
        private List<int> m_TicksActionPointIsResored;


        public AbstractPointBasedBattleNPCModel(int id, GridCellData correspondingCell, float moveSpeed, 
                                                iBattleActionBehaviour actionBehaviour, iHealthBehaviour healthBehaviour, 
                                                bool isEnemy, int actionPoints, int tickToRestoreActionPoint) : base(id, correspondingCell, moveSpeed, actionBehaviour, healthBehaviour, isEnemy)
        {
            m_ActionPoints = actionPoints;
            m_CurrentActionPoints = m_ActionPoints;
            m_TicksActionPointIsResored = new List<int>();
            m_TicksToRestoreActionPoint = tickToRestoreActionPoint;
        }

        public void UseActionPoint()
        {
            m_CurrentActionPoints--;
            if (m_CurrentActionPoints < 0)
                m_CurrentActionPoints = 0;

            m_TicksActionPointIsResored.Add(Rhytm.RhytmController.GetInstance().CurrentTick + m_TicksToRestoreActionPoint);

            OnActionPointUsed?.Invoke(m_CurrentActionPoints);
        }

        public void ProcessActionPointRestore(int currentTick)
        {
            if (m_TicksActionPointIsResored.Count > 0 && m_TicksActionPointIsResored.Contains(currentTick))
            {
                m_TicksActionPointIsResored.Remove(currentTick);

                m_CurrentActionPoints++;
                if (m_CurrentActionPoints > m_ActionPoints)
                    m_CurrentActionPoints = m_ActionPoints;

                OnActionPointRestored?.Invoke(m_CurrentActionPoints);
            }
        }

        public bool HasActionPoints()
        {
            return m_CurrentActionPoints > 0;
        }
    }
}
