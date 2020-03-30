using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using RhytmFighter.Assets;
using RhytmFighter.Battle.Action;
using RhytmFighter.Battle.Health;
using RhytmFighter.Objects.View;

namespace RhytmFighter.Objects.Model
{
    public class PlayerModel : AbstractBattleNPCModel
    {
        public PlayerModel(int id, GridCellData correspondingCell, float moveSpeed, iBattleActionBehaviour actionBehaviour, iHealthBehaviour healthBehaviour)
            : base(id, correspondingCell, moveSpeed, actionBehaviour, healthBehaviour, false)
        {
        }


        protected override AbstractGridObjectView CreateView(CellView cellView)
        {
            return AssetsManager.GetPrefabAssets().InstantiatePrefab(AssetsManager.GetPrefabAssets().PlayerViewPrefab, cellView.transform.position);
        }
    }
}
