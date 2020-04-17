namespace RhytmFighter.Battle.Command.Model
{
    public class AttackCommandModel : AbstractCommandModel
    {
        public int Damage { get; set; }

        public AttackCommandModel(iBattleObject sender, iBattleObject target, int applyDelay, int damage) : 
            base(sender, target, applyDelay)
        {
            Damage = damage;

            Type = CommandTypes.Attack;
        }
    }
}