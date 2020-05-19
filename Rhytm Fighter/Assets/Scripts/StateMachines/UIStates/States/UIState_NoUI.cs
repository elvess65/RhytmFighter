using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.StateMachines.UIState
{
    public class UIState_NoUI : UIState_Abstract
    {
        private Transform m_InventoryUIParent;

        protected List<Transform> m_UIObjects;

        public UIState_NoUI(Button buttonDefence, Text textBattleStatus, GameObject beatIndicator, Transform playerUIParent, Transform inventoryUIParent) :
            base(buttonDefence, textBattleStatus, beatIndicator, playerUIParent)
        {
            m_InventoryUIParent = inventoryUIParent;

            m_UIObjects = new List<Transform>()
            {
                buttonDefence.transform,
                textBattleStatus.transform,
                beatIndicator.transform,
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

        public override void ExitState()
        {
            base.ExitState();

            //UI
            m_BeatIndicator.gameObject.SetActive(true);
            m_PlayerUIParent.gameObject.SetActive(true);
            m_InventoryUIParent.gameObject.SetActive(true);
        }
    }
}
