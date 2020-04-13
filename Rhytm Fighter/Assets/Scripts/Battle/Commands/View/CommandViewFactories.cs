using RhytmFighter.Assets;
using UnityEngine;

namespace RhytmFighter.Battle.Command.View
{
    public abstract class CommandViewFactory
    {
        public abstract AbstractCommandView CreateView(AbstractBattleCommand command);
    }

    public class AttackCommandViewFactory : CommandViewFactory
    {
        public override AbstractCommandView CreateView(AbstractBattleCommand command)
        {
            AbstractCommandView result = null;

            double viewLifeTime = command.ApplyDelay * Rhytm.RhytmController.GetInstance().TickDurationSeconds +
                                  (!command.Sender.IsEnemy ? Rhytm.RhytmController.GetInstance().DeltaInput : 0);

            //Initialize view
            result = AssetsManager.GetPrefabAssets().InstantiatePrefab(AssetsManager.GetPrefabAssets().ProjectilePrefab);

            result.Initialize(command.Sender.ProjectileSpawnPosition,
                              command.Target.ProjectileHitPosition,
                              (float)viewLifeTime);

            return result;
        }
    }

    public class DefenceCommandViewFactory : CommandViewFactory
    {
        public override AbstractCommandView CreateView(AbstractBattleCommand command)
        {
            Debug.LogWarning("Create view for defence command");

            return null;
        }
    }
}
