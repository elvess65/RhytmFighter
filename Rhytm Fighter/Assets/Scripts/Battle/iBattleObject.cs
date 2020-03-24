using RhytmFighter.Battle.Action;
using RhytmFighter.Battle.Health;

namespace RhytmFighter.Battle
{
    public interface iBattleObject 
    {
        int ID { get; }
        bool IsEnemy { get; }

        iBattleActionBehaviour ActionBehaviour { get; }
        iHealthBehaviour HealthBehaviour { get; } 
    }
}
