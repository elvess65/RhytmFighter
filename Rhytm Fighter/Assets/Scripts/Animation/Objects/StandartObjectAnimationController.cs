using RhytmFighter.Core.Enums;

namespace RhytmFighter.Animation.Objects
{
    public class StandartObjectAnimationController : AbstractAnimationController
    {
        public override void PlayAnimation(AnimationTypes animationType)
        {
            switch(animationType)
            {
                case AnimationTypes.Show:
                    SetTrigger(GetAnimationName(AnimationTypes.Show));
                    break;
                case AnimationTypes.Hide:
                    SetTrigger(GetAnimationName(AnimationTypes.Hide));
                    break;
            }
        }
    }
}
