using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using RhytmFighter.Assets;
using RhytmFighter.Battle.Action;
using RhytmFighter.Battle.Health;
using RhytmFighter.Objects.View;
using UnityEngine;

namespace RhytmFighter.Objects.Model
{
    public class StandardEnemyNPCModel : AbstractBattleNPCModel
    {
        public StandardEnemyNPCModel(int id, GridCellData correspondingCell, iBattleActionBehaviour actionBehaviour, iHealthBehaviour healthBehaviour) : 
            base(id, correspondingCell, actionBehaviour, healthBehaviour, true)
        {
        }

        protected override AbstractGridObjectView CreateView(CellView cellView)
        {
            return AssetsManager.GetPrefabAssets().InstantiatePrefab(AssetsManager.GetPrefabAssets().StandartEnemyNPCViewPrefab, cellView.transform.position);
        }
    }
}
