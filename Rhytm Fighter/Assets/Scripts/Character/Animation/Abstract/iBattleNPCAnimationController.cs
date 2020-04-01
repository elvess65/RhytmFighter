namespace RhytmFighter.Characters.Animation
{
    public interface iBattleNPCAnimationController : iMovableAnimationController
    {
        void PlayAttackAnimation();

        void PlayTakeDamageAnimation();

        void PlayDestroyAnimation();
    }
}
