using RhytmFighter.Core.Enums;

namespace RhytmFighter.Characters.Animation
{
    [System.Serializable]
    public class Simple_AnimationController : AbstractAnimationController
    {
        private const string m_MOVE_KEY = "move";
        private const string m_ATTACK_KEY = "attack";

        public override void PlayAnimation(AnimationTypes animationKey)
        {
            UnityEngine.Debug.Log("Play animation: " + GetAnimationName(animationKey));
            Controller.SetTrigger(m_ATTACK_KEY);
        }

        public void PlayMoveAnimation()
        {
            Controller.SetBool(m_MOVE_KEY, true);
        }

        public void PlayIdleAnimation()
        {
            Controller.SetBool(m_MOVE_KEY, false);
        }

        public void PlayTakeDamageAnimation()
        {

        }

        public void PlayDestroyAnimation()
        {

        }
    }
}

