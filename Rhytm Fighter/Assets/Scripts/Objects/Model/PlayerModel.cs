using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using RhytmFighter.Battle.Action;
using RhytmFighter.Battle.Health;
using RhytmFighter.Objects.View;

namespace RhytmFighter.Objects.Model
{
    public class PlayerModel : AbstractBattleNPCModel
    {
        public PlayerModel(int id, GridCellData correspondingCell, iBattleActionBehaviour actionBehaviour, iHealthBehaviour healthBehaviour) 
            : base(id, correspondingCell, actionBehaviour, healthBehaviour, false)
        {
        }

        protected override AbstractGridObjectView CreateView(CellView cellView)
        {
            throw new System.NotImplementedException();
        }
    }
}
