using RhytmFighter.UI.Components;
using RhytmFighter.UI.Widget;
using UnityEngine.UI;

namespace RhytmFighter.StateMachines.UIState
{
    public class UIState_Battle : UIState_Abstract
    {
        protected Button m_ButtonDefence;
        protected UIWidget_Tick m_TickIndicator;
        protected UIComponent_ActionPointsIndicator m_ActionPointIndicator;

        public UIState_Battle(Button buttonDefence, Text textBattleStatus, UIWidget_Tick tickIndicator, UIComponent_ActionPointsIndicator apIndicator) :
            base(textBattleStatus)
        {
            m_ButtonDefence = buttonDefence;
            m_TickIndicator = tickIndicator;
            m_ActionPointIndicator = apIndicator;
        }


        public override void EnterState()
        {
            base.EnterState();

            //Events
            Rhytm.RhytmController.GetInstance().OnTick += TickHandler;
            Rhytm.RhytmController.GetInstance().OnEventProcessingTick += ProcessTickHandler;

            //UI
            m_ButtonDefence.gameObject.SetActive(true);
            m_ActionPointIndicator.gameObject.SetActive(true);
        }

        public override void ExitState()
        {
            base.ExitState();

            //Event
            Rhytm.RhytmController.GetInstance().OnTick -= TickHandler;
            Rhytm.RhytmController.GetInstance().OnEventProcessingTick -= ProcessTickHandler;

            //UI
            m_ButtonDefence.gameObject.SetActive(false);
            m_ActionPointIndicator.gameObject.SetActive(false);
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
