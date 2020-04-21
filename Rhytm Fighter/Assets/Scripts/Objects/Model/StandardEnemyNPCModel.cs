using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using RhytmFighter.Assets;
using RhytmFighter.Battle.Action;
using RhytmFighter.Battle.AI;
using RhytmFighter.Battle.Health;
using RhytmFighter.Objects.View;

namespace RhytmFighter.Objects.Model
{
    public class StandardEnemyNPCModel : AbstractBattleNPCModel
    {
        public StandardEnemyNPCModel(int id, GridCellData correspondingCell, float moveSpeed, iBattleActionBehaviour actionBehaviour, iHealthBehaviour healthBehaviour, AITypes aiType) : 
            base(id, correspondingCell, moveSpeed, actionBehaviour, healthBehaviour, true)
        {
            AI = GetAI(aiType);
        }

        protected override AbstractGridObjectView CreateView(CellView cellView) 
        {
            return AssetsManager.GetPrefabAssets().InstantiatePrefab(AssetsManager.GetPrefabAssets().StandartEnemyNPCViewPrefab, cellView.transform.position);
        }

        #region AI
        AbstractAI GetAI(AITypes type)
        {
            AbstractAI result = null;

            switch (type)
            {
                case AITypes.Simple:
                    result = new SimpleAI(this);
                    break;
            }

            return result;
        }
        #endregion
    }
}
