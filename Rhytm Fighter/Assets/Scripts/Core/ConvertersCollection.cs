using RhytmFighter.Core.Enums;

namespace RhytmFighter.Core.Converters
{
    public static class ConvertersCollection
    {
        public static AnimationTypes Command2Animation(CommandTypes commandType)
        {
            AnimationTypes animationType = AnimationTypes.Idle;

            switch(commandType)
            {
                case CommandTypes.Attack:
                    animationType = AnimationTypes.Attack;
                    break;
                case CommandTypes.Defence:
                    animationType = AnimationTypes.Defence;
                    break;
            }

            return animationType;
        }

        public static CommandTypes AIAction2Command(AIActionTypes aiActionType)
        {
            CommandTypes commandType = CommandTypes.None;

            switch(aiActionType)
            {
                case AIActionTypes.SimpleAttack:
                    commandType = CommandTypes.Attack;
                    break;
                case AIActionTypes.Defence:
                    commandType = CommandTypes.Defence;
                    break;
            }

            return commandType;
        }
    }
}
