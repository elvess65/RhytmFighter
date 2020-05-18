using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.StateMachines.UIState
{
    public class UIState_NoUI : UIState_Abstract
    {
        protected List<Transform> m_UIObjects;

        public UIState_NoUI(Button buttonDefence, Text textBattleStatus, GameObject beatIndicator, Transform playerUIParent) :
            base(buttonDefence, textBattleStatus, beatIndicator, playerUIParent)
        {
            m_UIObjects = new List<Transform>()
            {
                buttonDefence.transform,
                textBattleStatus.transform,
                beatIndicator.transform,
                playerUIParent
            };
        }

        public override void EnterState()
        {
            base.EnterState();

            foreach (Transform uiObject in m_UIObjects)
                uiObject.gameObject.SetActive(false);
        }
    }
}
