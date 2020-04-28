using Frameworks.Grid.Data;
using RhytmFighter.Battle;
using RhytmFighter.Battle.Action;
using RhytmFighter.Battle.AI;
using RhytmFighter.Battle.Command.Model;
using RhytmFighter.Battle.Health;
using RhytmFighter.Core.Enums;

namespace RhytmFighter.Core
{
    public interface iBattleObject 
    {
        event System.Action<iBattleObject> OnDestroyed;

        int ID { get; }
        bool IsEnemy { get; }
        float ActionExecutionTime { get; }
        GridCellData CorrespondingCell { get; }
        UnityEngine.Vector3 ViewPosition { get; }
        UnityEngine.Vector3 ViewForwardDir { get; }
        UnityEngine.Vector3 ProjectileSpawnPosition { get; }
        UnityEngine.Vector3 ProjectileHitPosition { get; }
        UnityEngine.Vector3 DefenceSpawnPosition { get; }

        iBattleObject Target { get; set; }
        iBattleActionBehaviour ActionBehaviour { get; }
        iHealthBehaviour HealthBehaviour { get; }
        CommandTypes LastExecutedCommand { get; }
        BattleCommandsModificatorProcessor ModificatorsProcessor { get; }
        AbstractAI AI { get; }

        void ApplyCommand(AbstractCommandModel command);
        void ReleaseCommand(AbstractCommandModel command);
        void NotifyViewAboutCommand(CommandTypes commandType);
        float GetActionEventExecuteTime(CommandTypes commandType);
    }
}
