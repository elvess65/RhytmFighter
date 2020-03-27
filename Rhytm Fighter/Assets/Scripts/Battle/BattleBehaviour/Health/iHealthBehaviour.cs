namespace RhytmFighter.Battle.Health
{
    public interface iHealthBehaviour 
    {
        event System.Action<int> OnHPReduced;
        event System.Action<int> OnHPIncreased;
        event System.Action OnDestroyed;

        int HP { get; }
        int MaxHP { get; }

        void ReduceHP(int dmg);
        void IncreaseHP(int amount);
    }
}
