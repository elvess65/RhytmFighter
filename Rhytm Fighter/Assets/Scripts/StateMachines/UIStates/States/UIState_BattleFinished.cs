using RhytmFighter.UI.View;
using UnityEngine;

namespace RhytmFighter.StateMachines.UIState
{
    /// <summary>
    /// Состояние окончания боя
    /// </summary>
    public class UIState_BattleFinished : UIState_Abstract
    {
        public UIState_BattleFinished(UIView_InventoryHUD uiView_InventoryHUD, UIView_PlayerHUD uiView_PlayerHUD, UIView_BattleHUD uiView_BattleHUD) :
            base(uiView_InventoryHUD, uiView_PlayerHUD, uiView_BattleHUD)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            //Text
            UIView_BattleHUD.UIWidget_BattleStatus.ShowBattleStatusWithDelay("Victory", Color.green);

            //Tick indicator
            UIView_PlayerHUD.UIWidget_Tick.ToNormalState();
        }
    }
}
