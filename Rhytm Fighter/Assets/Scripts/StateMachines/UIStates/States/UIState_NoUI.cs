using RhytmFighter.UI.Components;
using RhytmFighter.UI.Widget;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.StateMachines.UIState
{
    public class UIState_NoUI : UIState_Abstract
    {
        protected List<Transform> m_UIObjects;

        public UIState_NoUI(Button buttonDefence, Text textBattleStatus, 
                            UIWidget_Tick tickIndicator, 
                            UIComponent_ActionPointsIndicator apIndicator, 
                            Transform playerHealthBarParent, 
                            Transform inventoryUIParent) :
            base(textBattleStatus)
        {
            m_UIObjects = new List<Transform>()
            {
                buttonDefence.transform,
                textBattleStatus.transform,
                tickIndicator.transform,
                apIndicator.transform,
                playerHealthBarParent,
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
