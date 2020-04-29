using RhytmFighter.Core;
using RhytmFighter.Core.Enums;

namespace RhytmFighter.Battle.Command.Model
{
    public abstract class AbstractCommandModel
    {
        private static int m_ID_COUNTER = 0;

        public int ID { get; private set; }
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
            ID = m_ID_COUNTER++;
            Sender = sender;
            Target = target;
            ApplyDelay = applyDelay;
            Layer = CommandExecutionLayers.SingleExecution;
        }

        public override string ToString() => $"Type: {Type} Sender: {Sender.ID} Target: {Target.ID}";
    }
}
