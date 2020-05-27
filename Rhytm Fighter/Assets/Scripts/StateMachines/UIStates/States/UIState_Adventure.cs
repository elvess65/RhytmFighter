using RhytmFighter.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.StateMachines.UIState
{
    public class UIState_Adventure : UIState_Abstract
    {
        private Transform m_InventoryUIParent;

        public UIState_Adventure(Button buttonDefence, Text textBattleStatus, UIComponent_TickIndicator tickIndicator, Transform playerUIParent, Transform inventoryUIParent) :
            base(buttonDefence, textBattleStatus, tickIndicator, playerUIParent)
        {
            m_InventoryUIParent = inventoryUIParent;
        }

        public override void EnterState()
        {
            base.EnterState();

            //UI
            m_TickIndicator.gameObject.SetActive(true);
            m_PlayerUIParent.gameObject.SetActive(true);
            m_InventoryUIParent.gameObject.SetActive(true);

            m_TickIndicator.ToNormalState();

            //Events
            Rhytm.RhytmController.GetInstance().OnTick += TickHandler;
        }

        public override void ExitState()
        {
            base.ExitState();

            //Events
            Rhytm.RhytmController.GetInstance().OnTick -= TickHandler;
        }


        private void TickHandler(int ticksSinceStart)
        {
            m_TickIndicator.PlayTickAnimation();
        }
    }
}
