using RhytmFighter.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.StateMachines.UIState
{
    public class UIState_Battle : UIState_Abstract
    { 
        public UIState_Battle(Button buttonDefence, Text textBattleStatus, UIComponent_TickIndicator tickIndicator, Transform playerUIParent) :
            base(buttonDefence, textBattleStatus, tickIndicator, playerUIParent)
        {
        }


        public override void EnterState()
        {
            base.EnterState();

            //Events
            Rhytm.RhytmController.GetInstance().OnTick += TickHandler;
            Rhytm.RhytmController.GetInstance().OnEventProcessingTick += ProcessTickHandler;

            //UI
            m_ButtonDefence.gameObject.SetActive(true);

            // - Text
            m_TextBattleStatus.text = "Prepare for battle";
            m_TextBattleStatus.color = Color.yellow;
            Core.GameManager.Instance.StartCoroutine(DisableBattleStatusTextCoroutine());

            // - Tick indicator
            m_TickIndicator.ToPrepareState();
        }

        public override void ExitState()
        {
            base.ExitState();

            //Event
            Rhytm.RhytmController.GetInstance().OnTick -= TickHandler;
            Rhytm.RhytmController.GetInstance().OnEventProcessingTick -= ProcessTickHandler;

            //UI
            m_ButtonDefence.gameObject.SetActive(false);

            // - Text
            m_TextBattleStatus.text = Core.GameManager.Instance.PlayerModel.IsDestroyed ? "Game Over" : "Victory";
            m_TextBattleStatus.color = Core.GameManager.Instance.PlayerModel.IsDestroyed ? Color.red : Color.green;
            Core.GameManager.Instance.StartCoroutine(DisableBattleStatusTextCoroutine());

            // - Tick indicator
            m_TickIndicator.ToNormalState();
        }


        public void BattleStarted()
        {
            //UI
            m_TickIndicator.ToBattleState();

            m_TextBattleStatus.text = "Fight";
            m_TextBattleStatus.color = Color.red;

            Core.GameManager.Instance.StartCoroutine(DisableBattleStatusTextCoroutine());
        }

        public void NextEnemy()
        {
            //UI
            m_TextBattleStatus.text = "Its not finished yet";
            m_TextBattleStatus.color = Color.yellow;

            Core.GameManager.Instance.StartCoroutine(DisableBattleStatusTextCoroutine());
        }


        private void TickHandler(int ticksSinceStart)
        {
            m_TickIndicator.PlayTickAnimation();

        }

        private void ProcessTickHandler(int ticksSinceStart)
        {
            m_TickIndicator.PlayTickAnimation();
            m_TickIndicator.StartBeforeTickAnimation();
        }
    }
}
