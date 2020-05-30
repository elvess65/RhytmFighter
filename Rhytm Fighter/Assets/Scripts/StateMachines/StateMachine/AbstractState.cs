using RhytmFighter.Persistant.Abstract;

namespace RhytmFighter.StateMachines
{
    public abstract class AbstractState : iUpdatable
    {
        public bool StateIsActive { get; private set; }


        public virtual void EnterState()
        {
            StateIsActive = true;
        }

        public virtual void ExitState()
        {
            StateIsActive = false;
        }

        public abstract void PerformUpdate(float deltaTime);
    }
}
