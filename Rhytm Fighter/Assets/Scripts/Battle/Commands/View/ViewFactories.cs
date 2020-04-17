using RhytmFighter.Assets;
using RhytmFighter.Battle.Command.Model;

namespace RhytmFighter.Battle.Command.View
{
    public abstract class AbstractCommandViewFactory
    {
        public abstract AbstractCommandView CreateView(AbstractCommandModel command);

        protected abstract float GetExistsTime(AbstractCommandModel command);
    }


    public class AttackCommandViewFactory : AbstractCommandViewFactory
    {
        public override AbstractCommandView CreateView(AbstractCommandModel command)
        {
            AbstractProjectileView result = AssetsManager.GetPrefabAssets().InstantiatePrefab(AssetsManager.GetPrefabAssets().ProjectilePrefab);
            result.Initialize(command.Target.ProjectileHitPosition, command.Sender.ProjectileSpawnPosition, GetExistsTime(command));

            return result;
        }

        protected override float GetExistsTime(AbstractCommandModel command)
        {
            double playerInputDelta = (!command.Sender.IsEnemy ? Rhytm.RhytmController.GetInstance().DeltaInput : 0);
            double existTime = command.ApplyDelay * Rhytm.RhytmController.GetInstance().TickDurationSeconds + playerInputDelta;

            return (float)existTime;
        }
    }


    public class DefenceCommandViewFactory : AbstractCommandViewFactory
    {
        public override AbstractCommandView CreateView(AbstractCommandModel command)
        {
            AbstractDefenceView result = AssetsManager.GetPrefabAssets().InstantiatePrefab(AssetsManager.GetPrefabAssets().DefencePrefab);
            result.Initialize(command.Sender.DefenceSpawnPosition, GetExistsTime(command));

            return result;
        }

        protected override float GetExistsTime(AbstractCommandModel command)
        {
            return 0.5f;
        }
    }
}
