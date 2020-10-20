using RhytmFighter.UI.View;

namespace RhytmFighter.StateMachines.UIState
{
    /// <summary>
    /// Состояние отключенного UI
    /// </summary>
    public abstract class UIState_NoUI : UIState_Abstract
    {
        protected UIView_FinishLevelHUD UIView_FinishLevelHUD;

        public UIState_NoUI(UIView_InventoryHUD uiView_InventoryHUD,
                            UIView_PlayerHUD uiView_PlayerHUD,
                            UIView_BattleHUD uiView_BattleHUD,
                            UIView_FinishLevelHUD uIView_FinishLevelHUD) :
            base(uiView_InventoryHUD, uiView_PlayerHUD, uiView_BattleHUD)
        {
            UIView_FinishLevelHUD = uIView_FinishLevelHUD;
        }

        public override void EnterState()
        {
            base.EnterState();

            UIView_BattleHUD.SetWidgetsActive(false, IsUIHidedWithAnimation());
            UIView_PlayerHUD.SetWidgetsActive(false, IsUIHidedWithAnimation());
            UIView_InventoryHUD.SetWidgetsActive(false, IsUIHidedWithAnimation());
            UIView_FinishLevelHUD.SetWidgetsActive(false, IsUIHidedWithAnimation());
        }

        protected abstract bool IsUIHidedWithAnimation();
    }
}
