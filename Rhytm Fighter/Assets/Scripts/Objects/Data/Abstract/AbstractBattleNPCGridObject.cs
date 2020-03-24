using Frameworks.Grid.Data;
using RhytmFighter.Battle;
using RhytmFighter.Battle.Action;
using RhytmFighter.Battle.Health;

namespace RhytmFighter.Objects.Data
{
    public abstract class AbstractBattleNPCGridObject : AbstractNPCGridObject, iBattleObject
    {
        public bool IsEnemy { get; protected set; }
        public iBattleActionBehaviour ActionBehaviour { get; private set; }
        public iHealthBehaviour HealthBehaviour { get; private set; }

        public AbstractBattleNPCGridObject(int id, 
                                           GridCellData correspondingCell, 
                                           iBattleActionBehaviour actionBehaviour, 
                                           iHealthBehaviour healthBehaviour, 
                                           bool isEnemy) : base(id, correspondingCell)
        {
            IsEnemy = isEnemy;

            ActionBehaviour = actionBehaviour;
            HealthBehaviour = healthBehaviour;
        }
    }
}
