using RhytmFighter.Battle.Command.Model;
using RhytmFighter.Core;
using RhytmFighter.Core.Enums;

namespace RhytmFighter.Battle.Action
{
    public interface iBattleActionBehaviour
    {
        event System.Action<AbstractCommandModel> OnActionExecuted;

        iBattleObject Target { get; set; }

        void SetControlledObject(iBattleObject controlledObject);
        void ExecuteAction(CommandTypes type);
    }
}
