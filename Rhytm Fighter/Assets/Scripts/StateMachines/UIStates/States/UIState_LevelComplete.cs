using RhytmFighter.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.StateMachines.UIState
{
    public class UIState_LevelComplete : UIState_NoUI
    {
        public UIState_LevelComplete(Button buttonDefence, Text textBattleStatus, UIComponent_TickIndicator tickIndicator, Transform playerUIParent, Transform inventoryUIParent) : base(buttonDefence, textBattleStatus, tickIndicator, playerUIParent, inventoryUIParent)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            //Text
            m_TextBattleStatus.gameObject.SetActive(true);
            m_TextBattleStatus.text = "Level Complete";
            m_TextBattleStatus.color = Color.green;
        }
    }
}
