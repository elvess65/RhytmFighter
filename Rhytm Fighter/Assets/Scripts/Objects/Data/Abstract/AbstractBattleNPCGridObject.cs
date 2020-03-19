using Frameworks.Grid.Data;
using RhytmFighter.Battle;

namespace RhytmFighter.Objects.Data
{
    public abstract class AbstractBattleNPCGridObject : AbstractNPCGridObject, iBattleObject
    {
        public bool IsEnemy { get; protected set; }
        public iBattleBehaviour BattleBehaviour { get; private set; }

        public AbstractBattleNPCGridObject(int id, GridCellData correspondingCell, iBattleBehaviour battleBehaviour, bool isEnemy) : base(id, correspondingCell)
        {
            IsEnemy = isEnemy;
        }
    }
}
