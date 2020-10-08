using RhytmFighter.UI.Components;
using RhytmFighter.UI.Widget;
using UnityEngine;

namespace RhytmFighter.StateMachines.UIState
{
    public class UIState_Adventure : UIState_Abstract
    {
        private UIWidget_Tick m_TickIndicator;
        private Transform m_PlayerHealthBarParent;
        private Transform m_InventoryUIParent;

        public UIState_Adventure(UIWidget_Tick tickIndicator, Transform playerUIParent, Transform inventoryUIParent) :
                base(null)
        {
            m_TickIndicator = tickIndicator;
            m_PlayerHealthBarParent = playerUIParent;
            m_InventoryUIParent = inventoryUIParent;
        }


        public override void EnterState()
        {
            base.EnterState();

            //Events
            Rhytm.RhytmController.GetInstance().OnTick += TickHandler;

            //UI
            m_TickIndicator.gameObject.SetActive(true);
            m_PlayerHealthBarParent.gameObject.SetActive(true);
            m_InventoryUIParent.gameObject.SetActive(true);

            //Tick indicator
            m_TickIndicator.ToNormalState();
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
