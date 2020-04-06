namespace RhytmFighter.Battle.Command.View
{
    public abstract class AbstractProjectileView : AbstractCommandView
    {
        protected virtual void DestroyProjectile()
        {
            gameObject.SetActive(false);
        }
    }
}
