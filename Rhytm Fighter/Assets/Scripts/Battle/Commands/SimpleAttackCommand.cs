namespace RhytmFighter.Battle.Command
{
    public class SimpleAttackCommand : BattleCommand
    {
        public int Damage { get; private set; }

        public SimpleAttackCommand(iBattleObject sender, iBattleObject target, int applyDelay, int useDelay, int damage) : 
            base(sender, target, applyDelay, useDelay)
        {
            Damage = damage;
            Type = CommandTypes.SimpleAttack;
        }
    }
}