using RhytmFighter.UI.View;

namespace RhytmFighter.StateMachines.UIState
{
    /// <summary>
    /// Состояние перемещения
    /// </summary>
    public class UIState_Adventure : UIState_Abstract
    {
        public UIState_Adventure(UIView_InventoryHUD uiView_InventoryHUD, UIView_PlayerHUD uiView_PlayerHUD, UIView_BattleHUD uiView_BattleHUD) :
                base(uiView_InventoryHUD, uiView_PlayerHUD, uiView_BattleHUD)
        {
        }


        public override void EnterState()
        {
            base.EnterState();

            //Events
            Rhytm.RhytmController.GetInstance().OnTick += TickHandler;

            //UI
            UIView_PlayerHUD.SetWidgetsActive(true, true);
            UIView_InventoryHUD.SetWidgetsActive(true, true);
            

            //Tick indicator
            UIView_PlayerHUD.UIWidget_Tick.ToNormalState();
        }

        public override void ExitState()
        {
            base.ExitState();

            //Events
            Rhytm.RhytmController.GetInstance().OnTick -= TickHandler;
        }


        private void TickHandler(int ticksSinceStart)
        {
            UIView_PlayerHUD.UIWidget_Tick.PlayTickAnimation();
        }
    }
}
