using RhytmFighter.Core.Enums;

namespace RhytmFighter.Battle.Command.Model.Modificator
{
    public interface iCommandModificator 
    {
        int CommandID { get; }
        CommandTypes CommandType { get; }

        bool TryModifyCommand(AbstractCommandModel command);
    }
}
