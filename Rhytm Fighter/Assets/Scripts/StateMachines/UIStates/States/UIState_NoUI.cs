using RhytmFighter.UI.View;

namespace RhytmFighter.StateMachines.UIState
{
    /// <summary>
    /// Состояние отключенного UI
    /// </summary>
    public class UIState_NoUI : UIState_Abstract
    {
        public UIState_NoUI(UIView_InventoryHUD uiView_InventoryHUD, UIView_PlayerHUD uiView_PlayerHUD, UIView_BattleHUD uiView_BattleHUD) :
            base(uiView_InventoryHUD, uiView_PlayerHUD, uiView_BattleHUD)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            //UI
            UIView_BattleHUD.DisableView(true);
            UIView_PlayerHUD.DisableView(true);
            UIView_InventoryHUD.DisableView(true);
        }
    }
}
