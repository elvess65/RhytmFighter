﻿using RhytmFighter.Battle.Command;

namespace RhytmFighter.Battle.Action
{
    public interface iBattleActionBehaviour
    {
        event System.Action<AbstractBattleCommand> OnActionExecuted;

        iBattleObject Target { get; set; }

        void SetControlledObject(iBattleObject controlledObject);
        void ExecuteAction(int currentTick);
    }

    public enum PatternActionTypes
    {
        SimpleAttack,
        Idle
    }
}
