using RhytmFighter.Persistant.Enums;
using System;

namespace RhytmFighter.Persistant.Converters
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


        public static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return origin.AddSeconds(timestamp);
        }

        public static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }
    }
}
