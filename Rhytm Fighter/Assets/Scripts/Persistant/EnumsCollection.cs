namespace RhytmFighter.Persistant.Enums
{
    //Commands
    public enum CommandTypes { None, Attack, Defence }
    public enum CommandExecutionLayers { PeriodicExecution, SingleExecution }

    //Tick input
    public enum InputTickResult { PreTick, PostTick }

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
        IncreaseHP,
        Show,
        Hide,
        BattleIdle,
        StrafeLeft,
        StrafeRight,
        Teleport
    }
    public enum AnimationStates { None, Showing, Hiding }

    //AI
    public enum AITypes { None, Simple, SimpleDefencible }
    public enum AIActionTypes
    {
        SimpleAttack,
        Defence,
        Idle
    }

    //Camera
    public enum CameraTypes { Default, Main, Battle }

    //Movement
    public enum MovementStrategyTypes { Bezier, Teleport }
}