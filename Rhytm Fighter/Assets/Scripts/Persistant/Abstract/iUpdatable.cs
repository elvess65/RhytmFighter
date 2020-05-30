namespace RhytmFighter.Persistant.Abstract
{
    /// <summary>
    /// Interface for objects, that can be updated
    /// </summary>
    public interface iUpdatable
    {
        void PerformUpdate(float deltaTime);
    }
}
