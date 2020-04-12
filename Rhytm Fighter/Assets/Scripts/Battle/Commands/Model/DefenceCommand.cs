namespace RhytmFighter.Battle.Command
{
    public class DefenceCommand : AbstractPeriodicBattleCommand
    {
        public DefenceCommand(iBattleObject sender) : base(sender, sender, 0, 0)
        {
            Type = CommandTypes.Defence;
        }
    }
}
