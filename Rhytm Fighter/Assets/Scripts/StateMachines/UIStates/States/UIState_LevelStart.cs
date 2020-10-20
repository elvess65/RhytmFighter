using RhytmFighter.UI.View;

namespace RhytmFighter.StateMachines.UIState
{
    /// <summary>
    /// Состояние начала уровня
    /// </summary>
    public class UIState_LevelStart : UIState_NoUI
    {
        public UIState_LevelStart(UIView_InventoryHUD uiView_InventoryHUD,
                            UIView_PlayerHUD uiView_PlayerHUD,
                            UIView_BattleHUD uiView_BattleHUD,
                            UIView_FinishLevelHUD uIView_FinishLevelHUD) :
            base(uiView_InventoryHUD, uiView_PlayerHUD, uiView_BattleHUD, uIView_FinishLevelHUD)
        {
        }

        protected override bool IsUIHidedWithAnimation() => false;
    }
}
