namespace RhytmFighter.Characters.Animation
{
    public class EnemyNPSAnimationController : AbstractAnimationController, iBattleNPCAnimationController
    {
        private const string m_MOVE_KEY = "Run Forward";
        private const string m_ATTACK_KEY = "Stab Attack";
        private const string m_TAKE_DAMAGE_KEY = "Take Damage";
        private const string m_DESTROY_KEY = "Die";

        public void PlayAttackAnimation()
        {
            Controller.SetTrigger(m_ATTACK_KEY);
        }

        public void PlayDestroyAnimation()
        {
            Controller.SetTrigger(m_DESTROY_KEY);
        }

        public void PlayIdleAnimation()
        {
            Controller.SetBool(m_MOVE_KEY, false);
        }

        public void PlayMoveAnimation()
        {
            Controller.SetBool(m_MOVE_KEY, true);
        }

        public void PlayTakeDamageAnimation()
        {
            Controller.SetTrigger(m_TAKE_DAMAGE_KEY);
        }
    }
}
