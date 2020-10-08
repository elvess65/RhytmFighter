﻿using RhytmFighter.UI.Components;
using RhytmFighter.UI.Widget;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.StateMachines.UIState
{
    public class UIState_GameOverState : UIState_NoUI
    {
        public UIState_GameOverState(Button buttonDefence, Text textBattleStatus,
                                     UIWidget_Tick tickIndicator,
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
            m_TextBattleStatus.text = "Game Over";
            m_TextBattleStatus.color = Color.red;
        }
    }
}
