using RhytmFighter.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.StateMachines.UIState
{
    public class UIState_TapToActionState : UIState_Abstract
    {
        private Text m_TextPressToContinue;

        public UIState_TapToActionState(Button buttonDefence, Text textBattleStatus, UIComponent_TickIndicator tickIndicator, Transform playerUIParent, Text textPressToContinue) : base(buttonDefence, textBattleStatus, tickIndicator, playerUIParent)
        {
            m_TextPressToContinue = textPressToContinue;
        }

        public override void EnterState()
        {
            base.EnterState();

            //Text
            m_TextPressToContinue.gameObject.SetActive(true);

            m_TextBattleStatus.gameObject.SetActive(true);
            //m_TextBattleStatus.text = "Victory";
            //m_TextBattleStatus.color = Color.green;
        }
    }
}
