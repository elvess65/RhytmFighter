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
                if (command is AttackCommand attackCommand)
                    UnityEngine.Debug.LogWarning("TRY TO MODIFY ATTACK COMMAND FROM DEFENCE MODIFICATOR " + attackCommand.Type + " " + attackCommand.Damage);
            }
        }
    }
}
