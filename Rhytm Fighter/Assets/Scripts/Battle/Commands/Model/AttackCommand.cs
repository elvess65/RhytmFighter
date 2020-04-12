namespace RhytmFighter.Battle.Command
{
    public class AttackCommand : AbstractBattleCommand
    {
        public int Damage { get; private set; }

        public AttackCommand(iBattleObject sender, iBattleObject target, int applyDelay, int damage) : 
            base(sender, target, applyDelay)
        {
            Damage = damage;

            Type = CommandTypes.Attack;
        }
    }
}