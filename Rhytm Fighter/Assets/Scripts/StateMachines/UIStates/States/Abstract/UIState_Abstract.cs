using RhytmFighter.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.StateMachines.UIState
{
    public abstract class UIState_Abstract : AbstractState
    {
        protected Button m_ButtonDefence;
        protected Text m_TextBattleStatus;
        protected Transform m_PlayerUIParent;
        protected UIComponent_TickIndicator m_TickIndicator;

        private WaitForSeconds m_WaitDisableBattleUIDelay;


        public UIState_Abstract(Button buttonDefence, Text textBattleStatus, UIComponent_TickIndicator tickIndicator, Transform playerUIParent)
        {
            //UI Objects
            m_ButtonDefence = buttonDefence;
            m_TextBattleStatus = textBattleStatus;
            m_TickIndicator = tickIndicator;
            m_PlayerUIParent = playerUIParent;

            //Coroutine
            m_WaitDisableBattleUIDelay = new WaitForSeconds((float)Rhytm.RhytmController.GetInstance().TickDurationSeconds);
        }

        public override void PerformUpdate(float deltaTime)
        {
        }


        protected System.Collections.IEnumerator DisableBattleStatusTextCoroutine()
        {
            m_TextBattleStatus.gameObject.SetActive(true);

            yield return m_WaitDisableBattleUIDelay;

            m_TextBattleStatus.gameObject.SetActive(false);
        }
    }
}
