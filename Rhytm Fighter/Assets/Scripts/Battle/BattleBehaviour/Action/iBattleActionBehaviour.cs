using RhytmFighter.Battle.Command.Model;

namespace RhytmFighter.Battle.Action
{
    public interface iBattleActionBehaviour
    {
        event System.Action<AbstractCommandModel> OnActionExecuted;

        iBattleObject Target { get; set; }

        void SetControlledObject(iBattleObject controlledObject);
        void ExecuteAction(int currentTick, CommandTypes type);
    }

    public enum PatternActionTypes
    {
        SimpleAttack,
        Idle
    }
}
