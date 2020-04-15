namespace RhytmFighter.Battle.Command
{
    public interface iCommandModificator 
    {
        void TryModifyCommand(AbstractBattleCommand command);
    }
}
