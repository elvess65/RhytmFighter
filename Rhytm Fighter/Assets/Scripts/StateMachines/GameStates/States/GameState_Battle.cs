using RhytmFighter.Characters;
using RhytmFighter.Persistant.Enums;
using RhytmFighter.Rhytm;
using UnityEngine;

namespace RhytmFighter.StateMachines.GameState
{
    public class GameState_Battle : GameState_Abstract
	{
        public GameState_Battle(PlayerCharacterController playerCharacterController, RhytmInputProxy rhytmInputProxy) :
            base(playerCharacterController, rhytmInputProxy)
        {
        }

		public override void HandleTouch(Vector3 mouseScreenPos)
		{
            if (m_RhytmInputProxy.IsInputAllowed())
            {
                bool inputIsValid = m_RhytmInputProxy.IsInputTickValid();
                if (inputIsValid)
                    m_PlayerCharacterController.ExecuteAction(CommandTypes.Attack);
            }

            base.HandleTouch(mouseScreenPos);
        }
    }
}
