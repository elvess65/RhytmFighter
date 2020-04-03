namespace RhytmFighter.Battle
{
    public interface iBattleModelViewProxy
    {
        void NotifyView_ExecuteAction();
        void NotifyView_TakeDamage(int curHP, int maxHP, int dmg);
        void NotifyView_IncreaseHP();
        void NotifyView_Destroyed();
    }
}
