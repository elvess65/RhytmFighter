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
        }

        public override void ExitState()
        {
        }

        public override void HandleTouch(Vector3 mouseScreenPos)
        {
        }
    }
}
