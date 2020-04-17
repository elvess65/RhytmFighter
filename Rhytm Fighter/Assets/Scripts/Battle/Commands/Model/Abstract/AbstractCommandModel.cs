namespace RhytmFighter.Battle.Command.Model
{
    public abstract class AbstractCommandModel
    {
        public iBattleObject Sender { get; private set; }
        public iBattleObject Target { get; private set; }
        public CommandTypes Type { get; protected set; }
        public CommandExecutionLayers Layer { get; protected set; }
        /// <summary>
        /// Amount of ticks, command should be applied (ex. apply shield on the 3rd tick after create)
        /// </summary>
        public int ApplyDelay { get; private set; }
      
        public AbstractCommandModel(iBattleObject sender, iBattleObject target, int applyDelay)
        {
            Sender = sender;
            Target = target;
            ApplyDelay = applyDelay;
            Layer = CommandExecutionLayers.SingleExecution;
        }

        public override string ToString() => $"Type: {Type} Sender: {Sender.ID} Target: {Target.ID}";
    }

    public enum CommandTypes { Attack, Defence }
    public enum CommandExecutionLayers { PeriodicExecution, SingleExecution }
}
