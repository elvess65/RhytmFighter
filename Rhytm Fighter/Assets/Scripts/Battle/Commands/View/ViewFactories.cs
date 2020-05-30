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
            result.Initialize(command.Target.ProjectileImpactPosition, command.Sender.ProjectileSpawnPosition, GetExistsTime(command), command.ID);

            return result;
        }

        protected override float GetExistsTime(AbstractCommandModel command)
        {
            double existTime = 0;

            if (command.Sender.IsEnemy || Rhytm.RhytmController.GetInstance().InputTickResult == Persistant.Enums.InputTickResult.PostTick)
                existTime = command.ApplyDelay * Rhytm.RhytmController.GetInstance().TimeToNextTick +
                            Rhytm.RhytmController.GetInstance().ProcessTickDelta;
            else
            {
                existTime = command.ApplyDelay * Rhytm.RhytmController.GetInstance().TickDurationSeconds +
                            Rhytm.RhytmController.GetInstance().DeltaInput +
                            Rhytm.RhytmController.GetInstance().ProcessTickDelta;
            }

            return (float)existTime;
        }
    }

    public class DefenceCommandViewFactory : AbstractCommandViewFactory
    {
        public override AbstractCommandView CreateView(AbstractCommandModel command)
        {
            AbstractDefenceView result = AssetsManager.GetPrefabAssets().InstantiatePrefab(AssetsManager.GetPrefabAssets().DefencePrefab);
            result.Initialize(command.Sender.DefenceSpawnPosition, GetExistsTime(command), command.ID);

            return result;
        }

        protected override float GetExistsTime(AbstractCommandModel command)
        {
            return (float)Rhytm.RhytmController.GetInstance().TickDurationSeconds * 0.25f * 2;
        }
    }
}
