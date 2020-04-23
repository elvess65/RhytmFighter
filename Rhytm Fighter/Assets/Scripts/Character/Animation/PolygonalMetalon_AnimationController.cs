using RhytmFighter.Core.Enums;
using UnityEngine;

namespace RhytmFighter.Characters.Animation
{
    public class PolygonalMetalon_AnimationController : AbstractAnimationController
    {
        public override void PlayAnimation(AnimationTypes animationType)
        {
            Debug.Log("Play animation: " + animationType);

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
                case AnimationTypes.Idle:
                    Controller.SetBool(key, false);
                    break;
                case AnimationTypes.IncreaseHP:
                    SetTrigger(key);
                    break;
                case AnimationTypes.StartMove:
                    Controller.SetBool(key, true);
                    break;
                case AnimationTypes.TakeDamage:
                    SetTrigger(key);
                    break;
            }
        }
    }
}
