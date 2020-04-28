namespace RhytmFighter.Battle.Command.Model.Modificator
{
    class DefenceCommandModificator : iCommandModificator
    {
        public bool TryModifyCommand(AbstractCommandModel command)
        {
            bool result = false;
            switch (command)
            {
                case AttackCommandModel attackCommand:
                    attackCommand.Damage = 0;
                    result = true;
                    break;
            }

            return result;
        }
    }
}