namespace RhytmFighter.Characters.Animation
{
    [System.Serializable]
    public class SimpleAnimationController : AbstractAnimationController, iBattleNPCAnimationController
    {
        private const string m_MOVE_KEY = "move";
        private const string m_ATTACK_KEY = "attack";

        public void PlayMoveAnimation()
        {
            Controller.SetBool(m_MOVE_KEY, true);
        }

        public void PlayIdleAnimation()
        {
            Controller.SetBool(m_MOVE_KEY, false);
        }

        public void PlayActionAnimation(AnimationActionTypes type)
        {
            UnityEngine.Debug.Log("Play animation: " + type);
            Controller.SetTrigger(m_ATTACK_KEY);
        }

        public void PlayTakeDamageAnimation()
        {

        }

        public void PlayDestroyAnimation()
        {

        }
    }
}

