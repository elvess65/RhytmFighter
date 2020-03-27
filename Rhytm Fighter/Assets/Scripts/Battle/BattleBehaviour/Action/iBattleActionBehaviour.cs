using RhytmFighter.Battle.Command;

namespace RhytmFighter.Battle.Action
{
    public interface iBattleActionBehaviour
    {
        event System.Action<BattleCommand> OnActionExecuted;

        iBattleObject Target { get; set; }

        void SetControlledObject(iBattleObject controlledObject);
        void ExecuteAction();
    }

    public enum PatternActionTypes
    {
        SimpleAttack,
        Idle
    }
}
