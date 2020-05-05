using RhytmFighter.Core.Enums;

namespace RhytmFighter.Animation.NPC
{
    public class StandartNPCAnimationController : AbstractAnimationController
    {
        public override void PlayAnimation(AnimationTypes animationType)
        {
            string key = GetAnimationName(animationType);

            switch (animationType)
            {
                case AnimationTypes.Attack:
                    SetTrigger(key);
                    break;
                case AnimationTypes.Defence:
                    SetTrigger(key);
                    break;
                case AnimationTypes.Destroy:
                    SetTrigger(key);
                    break;
                case AnimationTypes.StopMove:
                    key = GetAnimationName(AnimationTypes.StartMove);
                    Controller.SetBool(key, false);
                    break;
                case AnimationTypes.IncreaseHP:
                    SetTrigger(key);
                    break;
                case AnimationTypes.StartMove:
                    Controller.SetBool(key, true);
                    break;
                case AnimationTypes.TakeDamage:
                    //if (IsPlayingIdle())
                    //    SetTrigger(key);
                    break;
            }
        }
    }
}

