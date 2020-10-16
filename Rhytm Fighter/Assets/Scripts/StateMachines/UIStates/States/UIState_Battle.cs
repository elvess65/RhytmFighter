using RhytmFighter.UI.View;

namespace RhytmFighter.StateMachines.UIState
{
    /// <summary>
    /// Состояние боя
    /// </summary>
    public abstract class UIState_Battle : UIState_Abstract
    {
        public UIState_Battle(UIView_InventoryHUD uiView_InventoryHUD, UIView_PlayerHUD uiView_PlayerHUD, UIView_BattleHUD uiView_BattleHUD) :
                base(uiView_InventoryHUD, uiView_PlayerHUD, uiView_BattleHUD)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            //Events
            Rhytm.RhytmController.GetInstance().OnTick += TickHandler;
            Rhytm.RhytmController.GetInstance().OnEventProcessingTick += ProcessTickHandler;

            //UI
            UIView_BattleHUD.UIWidget_Defence.WidgetButton.gameObject.SetActive(true);
        }

        public override void ExitState()
        {
            base.ExitState();

            //Event
            Rhytm.RhytmController.GetInstance().OnTick -= TickHandler;
            Rhytm.RhytmController.GetInstance().OnEventProcessingTick -= ProcessTickHandler;

            //UI
            UIView_BattleHUD.UIWidget_Defence.WidgetButton.gameObject.SetActive(false);
        }


        protected virtual void TickHandler(int ticksSinceStart)
        {
            UIView_PlayerHUD.UIWidget_Tick.PlayTickAnimation();
        }

        protected virtual void ProcessTickHandler(int ticksSinceStart)
        {
            UIView_PlayerHUD.UIWidget_Tick.PlayTickAnimation();
        }
    }
}
