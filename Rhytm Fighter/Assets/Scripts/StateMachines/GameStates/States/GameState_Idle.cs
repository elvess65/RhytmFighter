using RhytmFighter.Characters;
using RhytmFighter.Rhytm;
using UnityEngine;

namespace RhytmFighter.StateMachines.GameState
{
    public class GameState_Idle : GameState_Abstract        
	{
        public GameState_Idle(PlayerCharacterController playerCharacterController, RhytmInputProxy rhytmInputProxy) :
            base(playerCharacterController, rhytmInputProxy)
        {
        }

        public override void HandleTouch(Vector3 mouseScreenPos)
        {
        }
    }
}
