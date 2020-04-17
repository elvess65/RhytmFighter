using RhytmFighter.Battle.Command.Model.Modificator;

namespace RhytmFighter.Battle.Command.Model
{
    public class DefenceCommandModel : AbstractPeriodicCommandModel, iModificator
    {
        private iCommandModificator m_Modificator;

        public DefenceCommandModel(iBattleObject sender) : base(sender, sender, 0, 0)
        {
            Type = CommandTypes.Defence;
            m_Modificator = new DefenceCommandModificator();
        }

        public iCommandModificator GetModificator()
        {
            return m_Modificator;
        }
    }
}
