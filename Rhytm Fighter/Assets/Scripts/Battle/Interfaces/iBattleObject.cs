using RhytmFighter.Battle.Action;
using RhytmFighter.Battle.Command;
using RhytmFighter.Battle.Health;

namespace RhytmFighter.Battle
{
    public interface iBattleObject 
    {
        event System.Action<iBattleObject> OnDestroyed;

        int ID { get; }
        bool IsEnemy { get; }
        UnityEngine.Vector3 ViewPosition { get; }

        iBattleObject Target { get; set; }
        iBattleActionBehaviour ActionBehaviour { get; }
        iHealthBehaviour HealthBehaviour { get; }
        
        void ApplyCommand(BattleCommand command);
    }
}
