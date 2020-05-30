using RhytmFighter.Persistant.Enums;

namespace RhytmFighter.Battle.Command.Model.Modificator
{
    public interface iModificator 
    {
        CommandTypes Type { get; }
        iCommandModificator GetModificator();
    }
}
