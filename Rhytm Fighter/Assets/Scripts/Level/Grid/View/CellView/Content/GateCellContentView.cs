using RhytmFighter.Enviroment;
using RhytmFighter.Enviroment.Effects;

namespace Frameworks.Grid.View.Cell
{
    public class GateCellContentView : SimpleCellContentView
    {
        public override void Initialize()
        {
            base.Initialize();

            ParticlesVisualEffect effect = GetComponent<ParticlesVisualEffect>();
            DistanceToTargetTracker distanceTracker = GetComponent<DistanceToTargetTracker>();

            distanceTracker.OnTargetEntered += effect.HideEffect;
            distanceTracker.OnTargetExited += effect.ShowEffect;
        }
    }
}
