using Frameworks.Grid.Data;
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
        GridCellData CorrespondingCell { get; }
        UnityEngine.Vector3 ViewPosition { get; }
        UnityEngine.Vector3 ProjectileSpawnPosition { get; }
        UnityEngine.Vector3 ProjectileHitPosition { get; }

        iBattleObject Target { get; set; }
        iBattleActionBehaviour ActionBehaviour { get; }
        iHealthBehaviour HealthBehaviour { get; }
        
        void ApplyCommand(AbstractBattleCommand command);
        void ReleaseCommand(AbstractBattleCommand command);
    }
}
