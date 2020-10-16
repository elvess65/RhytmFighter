using RhytmFighter.UI.View;

namespace RhytmFighter.StateMachines.UIState
{
    public abstract class UIState_Abstract : AbstractState
    {
        protected UIView_InventoryHUD UIView_InventoryHUD;
        protected UIView_PlayerHUD UIView_PlayerHUD;
        protected UIView_BattleHUD UIView_BattleHUD;

        public UIState_Abstract(UIView_InventoryHUD uiView_InventoryHUD, UIView_PlayerHUD uiView_PlayerHUD, UIView_BattleHUD uiView_BattleHUD)
        {
            UIView_InventoryHUD = uiView_InventoryHUD;
            UIView_PlayerHUD = uiView_PlayerHUD;
            UIView_BattleHUD = uiView_BattleHUD;
        }

        public override void PerformUpdate(float deltaTime)
        {
        }
    }
}
