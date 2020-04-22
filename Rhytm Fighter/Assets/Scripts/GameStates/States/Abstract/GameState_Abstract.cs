using RhytmFighter.Characters;
using RhytmFighter.Core;
using RhytmFighter.Rhytm;
using UnityEngine;

namespace RhytmFighter.GameState
{
    public abstract class GameState_Abstract : iUpdatable
    {
        protected PlayerCharacterController m_PlayerCharacterController;
        protected RhytmInputProxy m_RhytmInputProxy;

        public GameState_Abstract(PlayerCharacterController playerCharacterController, RhytmInputProxy rhytmInputProxy)
        {
            m_PlayerCharacterController = playerCharacterController;
            m_RhytmInputProxy = rhytmInputProxy;
        }


        public abstract void EnterState();
        public abstract void ExitState();

        public virtual void HandleTouch(Vector3 mouseScreenPos)
        {
            m_RhytmInputProxy.RegisterInput();
        }


        public void PerformUpdate(float deltaTime)
        {
            m_PlayerCharacterController.PerformUpdate(deltaTime);
        }
    }
}
