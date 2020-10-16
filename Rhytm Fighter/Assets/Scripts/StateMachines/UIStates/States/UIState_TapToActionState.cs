using RhytmFighter.UI.View;

namespace RhytmFighter.StateMachines.UIState
{
    /// <summary>
    /// Состояние нажать для дейтсвия
    /// </summary>
    public class UIState_TapToActionState : UIState_Abstract
    {
        public UIState_TapToActionState(UIView_InventoryHUD uiView_InventoryHUD, UIView_PlayerHUD uiView_PlayerHUD, UIView_BattleHUD uiView_BattleHUD) :
                base(uiView_InventoryHUD, uiView_PlayerHUD, uiView_BattleHUD)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            UIView_BattleHUD.UIWidget_BattleStatus.Text_PressToContinue.gameObject.SetActive(true);
            UIView_BattleHUD.UIWidget_BattleStatus.Text_BattleStatus.gameObject.SetActive(true);
        }
    }
}
