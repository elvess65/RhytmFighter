using System.Collections;
using System.Collections.Generic;
using RhytmFighter.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.StateMachines.UIState
{
    public class UIState_GameOverState : UIState_Abstract
    {
        public UIState_GameOverState(Button buttonDefence, Text textBattleStatus, UIComponent_TickIndicator tickIndicator, Transform playerUIParent) : base(buttonDefence, textBattleStatus, tickIndicator, playerUIParent)
        {
        }
    }
}
