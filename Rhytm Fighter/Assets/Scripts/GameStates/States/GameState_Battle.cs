using RhytmFighter.Battle;
using RhytmFighter.Characters;
using RhytmFighter.Rhytm;
using UnityEngine;

namespace RhytmFighter.GameState
{
    public class GameState_Battle : GameState_Abstract
	{
        private iBattleObject m_Player;
        private RhytmInputProxy m_RhytmInputProxy;

        public GameState_Battle(PlayerCharacterController playerCharacterController, RhytmInputProxy rhytmInputProxy) : base(playerCharacterController)
        {
            m_RhytmInputProxy = rhytmInputProxy;
        }

        public void SetPlayer(iBattleObject player)
        {
            m_Player = player;
        }


		public override void EnterState()
		{

		}

		public override void ExitState()
		{

		}

		public override void HandleTouch(Vector3 mouseScreenPos)
		{
            bool inputIsValid = m_RhytmInputProxy.IsInputValid();
            if (inputIsValid)
                Debug.Log("Execute action");
		}
    }
}
