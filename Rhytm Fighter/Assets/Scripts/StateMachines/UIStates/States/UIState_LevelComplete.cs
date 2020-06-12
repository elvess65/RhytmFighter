using RhytmFighter.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.StateMachines.UIState
{
    public class UIState_LevelComplete : UIState_NoUI
    {
        public UIState_LevelComplete(Button buttonDefence, Text textBattleStatus,
                                     UIComponent_TickIndicator tickIndicator,
                                     UIComponent_ActionPointsIndicator apIndicator,
                                     Transform playerHealthBarParent,
                                     Transform inventoryUIParent) :
            base(buttonDefence, textBattleStatus, tickIndicator, apIndicator, playerHealthBarParent, inventoryUIParent)
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
