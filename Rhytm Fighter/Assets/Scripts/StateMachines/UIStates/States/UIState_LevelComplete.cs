using RhytmFighter.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.StateMachines.UIState
{
    public class UIState_LevelComplete : UIState_NoUI
    {
        private Text m_TextPressToContinue;

        public UIState_LevelComplete(Button buttonDefence, Text textBattleStatus, UIComponent_TickIndicator tickIndicator, Transform playerUIParent, Transform inventoryUIParent, Text textPressToContinue) : base(buttonDefence, textBattleStatus, tickIndicator, playerUIParent, inventoryUIParent)
        {
            m_TextPressToContinue = textPressToContinue;
        }

        public override void EnterState()
        {
            base.EnterState();

            //Text
            m_TextPressToContinue.gameObject.SetActive(true);

            //m_TextBattleStatus.gameObject.SetActive(true);
            //m_TextBattleStatus.text = "Victory";
            m_TextBattleStatus.color = Color.green;
        }
    }
}
