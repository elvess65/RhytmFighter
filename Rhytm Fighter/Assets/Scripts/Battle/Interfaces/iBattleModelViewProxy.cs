namespace RhytmFighter.Battle
{
    public interface iBattleModelViewProxy
    {
        void ExecuteAction();
        void TakeDamage();
        void IncreaseHP();
        void Destroy();
    }
}
