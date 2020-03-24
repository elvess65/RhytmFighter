namespace RhytmFighter.Battle.Action
{
    public interface iBattleActionBehaviour
    {
        event System.Action OnActionExecuted;

        void ExecuteAction();
    }

    public enum PatternActionTypes
    {
        Action,
        Idle
    }
}
