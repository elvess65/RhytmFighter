namespace RhytmFighter.Battle.Command
{
    public abstract class AbstractPeriodicBattleCommand : AbstractBattleCommand
    {
        /// <summary>
        /// Amount of ticks command should be released (ex. cancel shield on the 2nd tick after apply)
        /// </summary>
        public int ReleaseDelay { get; private set; }

        public AbstractPeriodicBattleCommand(iBattleObject sender, iBattleObject target, int applyDelay, int releaseDelay) 
            : base(sender, target, applyDelay)
        {
            ReleaseDelay = releaseDelay;
            Layer = CommandExecutionLayers.PeriodicExecution;
        }
    }
}
