using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using RhytmFighter.Assets;
using RhytmFighter.Battle.Action;
using RhytmFighter.Battle.Health;
using RhytmFighter.Persistant.Enums;
using RhytmFighter.Objects.View;
using System.Collections.Generic;

namespace RhytmFighter.Objects.Model
{
    public class PlayerModel : AbstractBattleNPCModel
    {
        public System.Action<int> OnActionPointUsed;
        public System.Action<int> OnActionPointRestored;

        public bool HasActionPoints => m_CurrentActionPoints > 0;
        public int ActionPoints { get; private set; }

        private PlayerView m_PlayerView;

        //ActionPoints
        private int m_CurrentActionPoints;
        private int m_TicksToRestoreActionPoint;
        private int m_ActionPointRestoreTick;
        private List<int> m_TicksActionPointIsResored;


        public PlayerModel(int id, GridCellData correspondingCell, float moveSpeed, iBattleActionBehaviour actionBehaviour, iHealthBehaviour healthBehaviour)
            : base(id, correspondingCell, moveSpeed, actionBehaviour, healthBehaviour, false)
        {
            ActionPoints = 3;
            m_TicksToRestoreActionPoint = 2;
            m_CurrentActionPoints = ActionPoints;
            m_TicksActionPointIsResored = new List<int>();
        }

        public override void ShowView(CellView cellView)
        {
            base.ShowView(cellView);

            m_PlayerView = View as PlayerView;
        }


        public void FinishFocusing()
        {
            m_PlayerView.FinishFocusing();
        }

        public void NotifyView_SwitchMoveStrategy(MovementStrategyTypes strategyType)
        {
            m_PlayerView.SwitchMoveStrategy(strategyType);
        }


        public void UseActionPoint()
        {
            m_CurrentActionPoints--;
            m_TicksActionPointIsResored.Add(Rhytm.RhytmController.GetInstance().CurrentTick + m_TicksToRestoreActionPoint);

            if (m_CurrentActionPoints < 0)
                m_CurrentActionPoints = 0;

            UnityEngine.Debug.Log("Use point on tick: " + Rhytm.RhytmController.GetInstance().CurrentTick + " Restores: " + (Rhytm.RhytmController.GetInstance().CurrentTick + m_TicksToRestoreActionPoint) + " Left: " + m_CurrentActionPoints);

            OnActionPointUsed?.Invoke(m_CurrentActionPoints);
        }

        public void ProcessActionPointRestore(int currentTick)
        {
            UnityEngine.Debug.Log("ProcessActionPointRestore");
            if (m_TicksActionPointIsResored.Count > 0 && m_TicksActionPointIsResored.Contains(currentTick))
            {
                m_TicksActionPointIsResored.Remove(currentTick);

                m_CurrentActionPoints++;

                if (m_CurrentActionPoints > ActionPoints)
                    m_CurrentActionPoints = ActionPoints;

                UnityEngine.Debug.Log("Restore AP " + currentTick + " Left: " + m_CurrentActionPoints);
                OnActionPointRestored?.Invoke(m_CurrentActionPoints);
            }
        }




        protected override AbstractGridObjectView CreateView(CellView cellView)
        {
            return AssetsManager.GetPrefabAssets().InstantiatePrefab(AssetsManager.GetPrefabAssets().PlayerViewPrefab, cellView.transform.position);
        }
    }
}
