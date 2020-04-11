namespace RhytmFighter.Battle.Command
{
    public abstract class BattleCommand
    {
        public iBattleObject Sender { get; private set; }
        public iBattleObject Target { get; private set; }
        public CommandTypes Type { get; protected set; }
        public int ApplyDelay { get; private set; }
        public int UseDelay { get; private set; }

        public BattleCommand(iBattleObject sender, iBattleObject target, int applyDelay, int useDelay)
        {
            Sender = sender;
            Target = target;
            ApplyDelay = applyDelay;
            UseDelay = useDelay;
        }

        public override string ToString() => $"Type: {Type} Sender: {Sender.ID} Target: {Target.ID}";
    }

    public enum CommandTypes
    {
        SimpleAttack,
        Defence
    }
}
