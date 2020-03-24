namespace RhytmFighter.Battle.Health
{
    public interface iHealthBehaviour 
    {
        void ReduceHP(int dmg);
        void RestoreHP(int amount);
    }
}
