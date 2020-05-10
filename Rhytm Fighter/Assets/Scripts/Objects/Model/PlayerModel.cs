using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using RhytmFighter.Assets;
using RhytmFighter.Battle.Action;
using RhytmFighter.Battle.Health;
using RhytmFighter.Core.Enums;
using RhytmFighter.Objects.View;

namespace RhytmFighter.Objects.Model
{
    public class PlayerModel : AbstractBattleNPCModel
    {
        private PlayerView m_PlayerView;


        public PlayerModel(int id, GridCellData correspondingCell, float moveSpeed, iBattleActionBehaviour actionBehaviour, iHealthBehaviour healthBehaviour)
            : base(id, correspondingCell, moveSpeed, actionBehaviour, healthBehaviour, false)
        {
        }

        public override void ShowView(CellView cellView)
        {
            base.ShowView(cellView);

            m_PlayerView = View as PlayerView;
        }


        public void NotifyView_FinishFocusing()
        {
            //Notify view
            m_PlayerView.NotifyView_FinishRotate();
        }

        public void NotifyView_SwitchMoveStrategy(MovementStrategyTypes strategyType)
        {
            m_PlayerView.NotifyView_SwitchMoveStrategy(strategyType);
        }


        protected override AbstractGridObjectView CreateView(CellView cellView)
        {
            return AssetsManager.GetPrefabAssets().InstantiatePrefab(AssetsManager.GetPrefabAssets().PlayerViewPrefab, cellView.transform.position);
        }
    }
}
