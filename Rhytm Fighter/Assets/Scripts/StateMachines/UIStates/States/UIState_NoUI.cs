using RhytmFighter.UI.Components;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.StateMachines.UIState
{
    public class UIState_NoUI : UIState_Abstract
    {
        protected List<Transform> m_UIObjects;

        public UIState_NoUI(Button buttonDefence, Text textBattleStatus, UIComponent_TickIndicator tickIndicator, Transform playerUIParent, Transform inventoryUIParent) :
            base(buttonDefence, textBattleStatus, tickIndicator, playerUIParent)
        {
            m_UIObjects = new List<Transform>()
            {
                buttonDefence.transform,
                textBattleStatus.transform,
                tickIndicator.transform,
                playerUIParent,
                inventoryUIParent
            };
        }

        public override void EnterState()
        {
            base.EnterState();

            //UI
            foreach (Transform uiObject in m_UIObjects)
                uiObject.gameObject.SetActive(false);
        }
    }
}
