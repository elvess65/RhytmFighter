using RhytmFighter.Core.Enums;
using UnityEngine;

namespace RhytmFighter.Characters.Animation
{
    public class PolygonalMetalon_AnimationController : AbstractAnimationController
    {
        private const string m_MOVE_KEY = "Run Forward";
        private const string m_ATTACK_KEY = "Stab Attack";
        private const string m_TAKE_DAMAGE_KEY = "Take Damage";
        private const string m_DESTROY_KEY = "Die";

        public override void PlayAnimation(AnimationTypes animationType)
        {
            Debug.Log("Play animation: " + animationType);
            Controller.SetTrigger(m_ATTACK_KEY);
        }


        void PlayDestroyAnimation()
        {
            Controller.SetTrigger(m_DESTROY_KEY);
        }

        void PlayIdleAnimation()
        {
            Controller.SetBool(m_MOVE_KEY, false);
        }

        void PlayMoveAnimation()
        {
            Controller.SetBool(m_MOVE_KEY, true);
        }

        void PlayTakeDamageAnimation()
        {
            Controller.SetTrigger(m_TAKE_DAMAGE_KEY);
        }
    }
}
