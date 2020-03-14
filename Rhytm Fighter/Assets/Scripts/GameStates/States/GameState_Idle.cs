using RhytmFighter.Characters;
using UnityEngine;

namespace RhytmFighter.GameState
{
    public class GameState_Idle : GameState_Abstract        
	{
        public GameState_Idle(PlayerCharacterController playerCharacterController) : base(playerCharacterController)
        {

        }

		public override void EnterState()
        {
            Debug.Log("ENTER IDLE STATE");
        }

        public override void ExitState()
        {
            Debug.Log("EXIT IDLE STATE");
        }

        public override void HandleTouch(Vector3 mouseScreenPos)
        {
            Debug.Log("GameState_Idle: Handle Input");
        }
    }
}
