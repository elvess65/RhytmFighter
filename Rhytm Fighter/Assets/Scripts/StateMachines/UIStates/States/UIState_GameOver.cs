using RhytmFighter.UI.View;
using UnityEngine;

namespace RhytmFighter.StateMachines.UIState
{
    /// <summary>
    /// Состояние проиграша игрока
    /// </summary>
    public class UIState_GameOverState : UIState_NoUI
    {
        public UIState_GameOverState(UIView_InventoryHUD uiView_InventoryHUD,
                                     UIView_PlayerHUD uiView_PlayerHUD,
                                     UIView_BattleHUD uiView_BattleHUD,
                                     UIView_FinishLevelHUD uIView_FinishLevelHUD) :
            base(uiView_InventoryHUD, uiView_PlayerHUD, uiView_BattleHUD, uIView_FinishLevelHUD)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            UIView_FinishLevelHUD.UIWidget_GameOver.SetWidgetActive(true, true);
            UIView_FinishLevelHUD.UIWidget_GameOver.ShowResult("Game Over", Color.red);
        }

        protected override bool IsUIHidedWithAnimation() => true;
    }
}
