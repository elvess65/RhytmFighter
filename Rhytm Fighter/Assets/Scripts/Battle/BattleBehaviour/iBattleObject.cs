namespace RhytmFighter.Battle
{
    public interface iBattleObject 
    {
        int ID { get; }
        bool IsEnemy { get; }
        iBattleBehaviour BattleBehaviour { get; }
    }
}
