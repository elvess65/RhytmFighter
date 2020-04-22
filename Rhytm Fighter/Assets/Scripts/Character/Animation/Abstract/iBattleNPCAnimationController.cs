namespace RhytmFighter.Characters.Animation
{
    public interface iBattleNPCAnimationController : iMovableAnimationController
    {
        void PlayActionAnimation(AnimationActionTypes type);

        void PlayTakeDamageAnimation();

        void PlayDestroyAnimation();
    }

    public enum AnimationActionTypes
    {
        Attack,
        Defence,
        Damage,
        Destroy,
        Move
    }
}
