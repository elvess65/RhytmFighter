﻿namespace RhytmFighter.Core.Enums
{
    //Commands
    public enum CommandTypes { None, Attack, Defence }
    public enum CommandExecutionLayers { PeriodicExecution, SingleExecution }

    //Grid
    public enum GridObjectTypes { Item, NPC }
    public enum AddNoteResult { None, AddedToLeft, AddedToRight }
    public enum GateTypes { ToParentNode, ToNextNode }

    //Animation
    public enum AnimationTypes
    {
        Attack,
        Defence,
        TakeDamage,
        Destroy,
        StartMove,
        StopMove,
        Idle,
        IncreaseHP
    }

    //AI
    public enum AITypes { None, Simple }
    public enum AIActionTypes
    {
        SimpleAttack,
        Defence,
        Idle
    }
}