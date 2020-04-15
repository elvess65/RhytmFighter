namespace RhytmFighter.Battle.Command
{
    public class DefenceCommand : AbstractPeriodicBattleCommand, iModificator
    {
        private iCommandModificator m_Modificator;

        public DefenceCommand(iBattleObject sender) : base(sender, sender, 0, 0)
        {
            Type = CommandTypes.Defence;
            m_Modificator = new DefenceCommandModificator();
        }

        public iCommandModificator GetModificator()
        {
            return m_Modificator;
        }


        public class DefenceCommandModificator : iCommandModificator
        {
            public void TryModifyCommand(AbstractBattleCommand command)
            {
                switch (command)
                {
                    case AttackCommand attackCommand:
                        attackCommand.Damage = 0;
                        break;
                }
            }
        }
    }
}
