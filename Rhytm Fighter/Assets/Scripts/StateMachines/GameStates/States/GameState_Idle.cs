using RhytmFighter.Characters;
using RhytmFighter.Rhytm;
using RhytmFighter.StateMachines.GameState;
using UnityEngine;

namespace RhytmFighter.GameState
{
    public class GameState_Idle : GameState_Abstract        
	{
        public GameState_Idle(PlayerCharacterController playerCharacterController, RhytmInputProxy rhytmInputProxy) :
            base(playerCharacterController, rhytmInputProxy)
        {
        }

		public override void EnterState()
        {
        }

        public override void ExitState()
        {
        }

        public override void HandleTouch(Vector3 mouseScreenPos)
        {
        }
    }
}
