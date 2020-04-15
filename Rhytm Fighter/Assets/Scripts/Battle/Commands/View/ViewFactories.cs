using RhytmFighter.Assets;

namespace RhytmFighter.Battle.Command.View
{
    public abstract class AbstractCommandViewFactory
    {
        public abstract AbstractCommandView CreateView(AbstractBattleCommand command);

        protected abstract float GetExistsTime(AbstractBattleCommand command);
    }


    public class AttackCommandViewFactory : AbstractCommandViewFactory
    {
        public override AbstractCommandView CreateView(AbstractBattleCommand command)
        {
            AbstractProjectileView result = AssetsManager.GetPrefabAssets().InstantiatePrefab(AssetsManager.GetPrefabAssets().ProjectilePrefab);
            result.Initialize(command.Target.ProjectileHitPosition, command.Sender.ProjectileSpawnPosition, GetExistsTime(command));

            return result;
        }

        protected override float GetExistsTime(AbstractBattleCommand command)
        {
            double playerInputDelta = (!command.Sender.IsEnemy ? Rhytm.RhytmController.GetInstance().DeltaInput : 0);
            double existTime = command.ApplyDelay * Rhytm.RhytmController.GetInstance().TickDurationSeconds + playerInputDelta;

            return (float)existTime;
        }
    }


    public class DefenceCommandViewFactory : AbstractCommandViewFactory
    {
        public override AbstractCommandView CreateView(AbstractBattleCommand command)
        {
            AbstractDefenceView result = AssetsManager.GetPrefabAssets().InstantiatePrefab(AssetsManager.GetPrefabAssets().DefencePrefab);
            result.Initialize(command.Sender.DefenceSpawnPosition, GetExistsTime(command));

            return result;
        }

        protected override float GetExistsTime(AbstractBattleCommand command)
        {
            return 0.5f;
        }
    }
}
