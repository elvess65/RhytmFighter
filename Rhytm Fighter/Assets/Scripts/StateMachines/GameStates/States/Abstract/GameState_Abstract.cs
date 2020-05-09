using RhytmFighter.Characters;
using RhytmFighter.Rhytm;
using UnityEngine;

namespace RhytmFighter.StateMachines.GameState
{
    public abstract class GameState_Abstract : AbstractState
    {
        protected PlayerCharacterController m_PlayerCharacterController;
        protected RhytmInputProxy m_RhytmInputProxy;

        public GameState_Abstract(PlayerCharacterController playerCharacterController, RhytmInputProxy rhytmInputProxy)
        {
            m_PlayerCharacterController = playerCharacterController;
            m_RhytmInputProxy = rhytmInputProxy;
        }

        public virtual void HandleTouch(Vector3 mouseScreenPos)
        {
            m_RhytmInputProxy.RegisterInput();
        }

        public override void PerformUpdate(float deltaTime)
        {
            m_PlayerCharacterController.PerformUpdate(deltaTime);
        }
    }
}
