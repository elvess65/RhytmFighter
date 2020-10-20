using RhytmFighter.UI.View;

namespace RhytmFighter.StateMachines.UIState
{
    /// <summary>
    /// Состояние нажать для дейтсвия
    /// </summary>
    public class UIState_TapToActionState : UIState_Abstract
    {
        protected UIView_FinishLevelHUD UIView_FinishLevelHUD;

        public UIState_TapToActionState(UIView_InventoryHUD uiView_InventoryHUD,
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

            UIView_FinishLevelHUD.UIWidget_TapToAction.SetWidgetActive(true, true);
        }
    }
}
