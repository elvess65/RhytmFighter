using RhytmFighter.Persistant.Enums;

namespace RhytmFighter.Battle.Command.Model.Modificator
{
    class DefenceCommandModificator : iCommandModificator
    {
        public int CommandID { get; private set; }
        public CommandTypes CommandType { get; private set; }

        public DefenceCommandModificator(int commandID, CommandTypes commandType)
        {
            CommandID = commandID;
            CommandType = commandType;
        }

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