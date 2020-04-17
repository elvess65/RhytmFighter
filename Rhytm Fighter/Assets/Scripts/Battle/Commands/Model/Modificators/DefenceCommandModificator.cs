namespace RhytmFighter.Battle.Command.Model.Modificator
{
    class DefenceCommandModificator : iCommandModificator
    {
        public void TryModifyCommand(AbstractCommandModel command)
        {
            switch (command)
            {
                case AttackCommandModel attackCommand:
                    attackCommand.Damage = 0;
                    break;
            }
        }
    }
}