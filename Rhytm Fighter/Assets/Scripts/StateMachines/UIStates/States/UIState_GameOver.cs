using RhytmFighter.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.StateMachines.UIState
{
    public class UIState_GameOverState : UIState_NoUI
    {
        public UIState_GameOverState(Button buttonDefence, Text textBattleStatus, UIComponent_TickIndicator tickIndicator, Transform playerUIParent, Transform inventoryUIParent) : base(buttonDefence, textBattleStatus, tickIndicator, playerUIParent, inventoryUIParent)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            //Text
            m_TextBattleStatus.gameObject.SetActive(true);
            m_TextBattleStatus.text = "Game Over";
            m_TextBattleStatus.color = Color.red;
        }
    }
}
