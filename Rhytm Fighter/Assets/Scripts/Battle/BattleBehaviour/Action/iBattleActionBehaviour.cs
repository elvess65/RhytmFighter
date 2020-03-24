namespace RhytmFighter.Battle.Action
{
    public interface iBattleActionBehaviour
    {
        void ExecuteAction();
    }

    public enum PatternActionTypes
    {
        Action,
        Idle
    }
}
