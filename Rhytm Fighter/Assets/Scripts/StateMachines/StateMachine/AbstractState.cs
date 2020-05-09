using RhytmFighter.Core;

namespace RhytmFighter.StateMachines
{
    public abstract class AbstractState : iUpdatable
    {
        public abstract void EnterState();
        public abstract void ExitState();
        public abstract void PerformUpdate(float deltaTime);
    }
}
