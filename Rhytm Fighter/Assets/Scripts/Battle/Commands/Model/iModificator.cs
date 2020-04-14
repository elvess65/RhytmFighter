namespace RhytmFighter.Battle.Command
{
    public interface iModificator 
    {
        CommandTypes Type { get; }
        iCommandModificator GetModificator();
    }
}
