namespace RhytmFighter.Battle.Command
{
    public class DefenceCommand : BattleCommand
    {
        public DefenceCommand(iBattleObject sender) : base(sender, sender, 0, 0)
        {
            Type = CommandTypes.Defence;
        }
    }
}
