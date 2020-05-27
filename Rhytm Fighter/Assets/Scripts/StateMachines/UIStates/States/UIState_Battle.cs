using RhytmFighter.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.StateMachines.UIState
{
    public class UIState_Battle : UIState_Abstract
    { 
        public UIState_Battle(Button buttonDefence, Text textBattleStatus, UIComponent_TickIndicator tickIndicator, Transform playerUIParent) :
            base(buttonDefence, textBattleStatus, tickIndicator, playerUIParent)
        {
        }


        public override void EnterState()
        {
            base.EnterState();

            //Events
            Rhytm.RhytmController.GetInstance().OnTick += TickHandler;
            Rhytm.RhytmController.GetInstance().OnEventProcessingTick += ProcessTickHandler;

            //UI
            m_ButtonDefence.gameObject.SetActive(true);
        }

        public override void ExitState()
        {
            base.ExitState();

            //Event
            Rhytm.RhytmController.GetInstance().OnTick -= TickHandler;
            Rhytm.RhytmController.GetInstance().OnEventProcessingTick -= ProcessTickHandler;

            //UI
            m_ButtonDefence.gameObject.SetActive(false);
        }


        protected virtual void TickHandler(int ticksSinceStart)
        {
            m_TickIndicator.PlayTickAnimation();
        }

        protected virtual void ProcessTickHandler(int ticksSinceStart)
        {
            m_TickIndicator.PlayTickAnimation();
        }
    }
}
