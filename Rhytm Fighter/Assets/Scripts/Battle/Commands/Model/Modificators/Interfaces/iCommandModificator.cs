namespace RhytmFighter.Battle.Command.Model.Modificator
{
    public interface iCommandModificator 
    {
        void TryModifyCommand(AbstractCommandModel command);
    }
}
