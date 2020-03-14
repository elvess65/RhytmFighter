using RhytmFighter.Characters;
using RhytmFighter.Interfaces;
using UnityEngine;

namespace RhytmFighter.GameState
{
    public abstract class GameState_Abstract : iUpdatable
    {
        protected PlayerCharacterController m_PlayerCharacterController;

        public GameState_Abstract(PlayerCharacterController playerCharacterController)
        {
            m_PlayerCharacterController = playerCharacterController;
        }


        public abstract void EnterState();
        public abstract void ExitState();

        public virtual void HandleTouch(Vector3 mouseScreenPos)
        { }


        public void PerformUpdate(float deltaTime)
        {
            m_PlayerCharacterController.PerformUpdate(deltaTime);
        }
    }
}
