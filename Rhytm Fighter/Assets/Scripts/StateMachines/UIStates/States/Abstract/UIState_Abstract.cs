using RhytmFighter.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.StateMachines.UIState
{
    public abstract class UIState_Abstract : AbstractState
    {
        //protected Button m_ButtonDefence;
        protected Text m_TextBattleStatus;
        //protected Transform m_PlayerHealthBarParent;
        //protected UIComponent_TickIndicator m_TickIndicator;
        //protected UIComponent_ActionPointsIndicator m_ActionPointIndicator;

        private WaitForSeconds m_WaitDisableBattleStatusUIDelay;


        public UIState_Abstract(Text textBattleStatus)
        {
            //UI Objects
            m_TextBattleStatus = textBattleStatus;

            //Coroutine
            m_WaitDisableBattleStatusUIDelay = new WaitForSeconds((float)Rhytm.RhytmController.GetInstance().TickDurationSeconds);
        }

        public override void PerformUpdate(float deltaTime)
        {
        }


        protected System.Collections.IEnumerator DisableBattleStatusTextCoroutine()
        {
            m_TextBattleStatus.gameObject.SetActive(true);

            yield return m_WaitDisableBattleStatusUIDelay;

            m_TextBattleStatus.gameObject.SetActive(false);
        }
    }
}
