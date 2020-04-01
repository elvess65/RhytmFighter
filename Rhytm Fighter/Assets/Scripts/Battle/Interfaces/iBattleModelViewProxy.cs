namespace RhytmFighter.Battle
{
    public interface iBattleModelViewProxy
    {
        void NotifyView_ExecuteAction();
        void NotifyView_TakeDamage();
        void NotifyView_IncreaseHP();
        void NotifyView_Destroyed();
    }
}
