namespace RhytmFighter.Battle.Command.Model.Modificator
{
    public interface iCommandModificator 
    {
        bool TryModifyCommand(AbstractCommandModel command);
    }
}
