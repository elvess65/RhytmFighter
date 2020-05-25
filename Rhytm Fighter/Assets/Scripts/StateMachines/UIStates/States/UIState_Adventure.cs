using RhytmFighter.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.StateMachines.UIState
{
    public class UIState_Adventure : UIState_Abstract
    {
        public UIState_Adventure(Button buttonDefence, Text textBattleStatus, UIComponent_TickIndicator tickIndicator, Transform playerUIParent) :
            base(buttonDefence, textBattleStatus, tickIndicator, playerUIParent)
        {
            m_TickIndicator.Initialize((float)Rhytm.RhytmController.GetInstance().TickDurationSeconds / 8);
        }

        public override void EnterState()
        {
            base.EnterState();

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
            m_TickIndicator.HandleTick();
        }
    }
}
