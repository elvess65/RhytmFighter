﻿using RhytmFighter.Characters;
using RhytmFighter.Rhytm;
using UnityEngine;

namespace RhytmFighter.GameState
{
    public class GameState_Battle : GameState_Abstract
	{
        public GameState_Battle(PlayerCharacterController playerCharacterController, RhytmInputProxy rhytmInputProxy) :
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
            if (m_RhytmInputProxy.IsInputAllowed())
            {
                bool inputIsValid = m_RhytmInputProxy.IsInputTickValid();
                if (inputIsValid)
                    m_PlayerCharacterController.ExecuteAction();
            }

            base.HandleTouch(mouseScreenPos);
        }
    }
}
