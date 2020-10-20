using System.Diagnostics;
using RhytmFighter.UI.View;
using UnityEngine;

namespace RhytmFighter.StateMachines.UIState
{
    /// <summary>
    /// Состояние окончания уровня
    /// </summary>
    public class UIState_LevelComplete : UIState_NoUI
    {
        public UIState_LevelComplete(UIView_InventoryHUD uiView_InventoryHUD,
                                     UIView_PlayerHUD uiView_PlayerHUD,
                                     UIView_BattleHUD uiView_BattleHUD,
                                     UIView_FinishLevelHUD uIView_FinishLevelHUD) :
            base(uiView_InventoryHUD, uiView_PlayerHUD, uiView_BattleHUD, uIView_FinishLevelHUD)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            UIView_FinishLevelHUD.UIWidget_LevelComplete.SetWidgetActive(true, true);
            UIView_FinishLevelHUD.UIWidget_LevelComplete.ShowResult("Level Complete", Color.green);
        }

        protected override bool IsUIHidedWithAnimation() => true;
    }
}
