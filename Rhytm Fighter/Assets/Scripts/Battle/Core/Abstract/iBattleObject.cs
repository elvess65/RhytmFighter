using Frameworks.Grid.Data;
using RhytmFighter.Battle.Action;
using RhytmFighter.Battle.AI;
using RhytmFighter.Battle.AI.Abstract;
using RhytmFighter.Battle.Command;
using RhytmFighter.Battle.Command.Model;
using RhytmFighter.Battle.Health;
using RhytmFighter.Persistant.Enums;

namespace RhytmFighter.Battle.Core.Abstract
{
    public interface iBattleObject 
    {
        event System.Action<iBattleObject> OnDestroyed;

        int ID { get; }
        bool IsEnemy { get; }
        bool IsDestroyed { get; }
        GridCellData CorrespondingCell { get; }
        UnityEngine.Transform ViewTransform { get; }
        UnityEngine.Vector3 ViewPosition { get; }
        UnityEngine.Vector3 ViewForwardDir { get; }
        UnityEngine.Vector3 ProjectileSpawnPosition { get; }
        UnityEngine.Vector3 ProjectileImpactPosition { get; }
        UnityEngine.Vector3 DefenceSpawnPosition { get; }

        iBattleObject Target { get; set; }
        iBattleActionBehaviour ActionBehaviour { get; }
        iHealthBehaviour HealthBehaviour { get; }
        BattleCommandsModificatorProcessor ModificatorsProcessor { get; }
        AbstractAI AI { get; }
        int ExperianceForDestroy { get; }

        void ApplyCommand(AbstractCommandModel command);
        void ReleaseCommand(AbstractCommandModel command);
        void NotifyViewAboutCommand(CommandTypes commandType);
        void NotifyViewAboutVictory();
        float GetActionEventExecuteTime(CommandTypes commandType);
    }
}
