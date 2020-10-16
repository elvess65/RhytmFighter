using RhytmFighter.Battle.Core;
using RhytmFighter.UI.View;
using UnityEngine;

namespace RhytmFighter.StateMachines.UIState
{
    /// <summary>
    /// Состояние ожидания следующего противника
    /// </summary>
    public class UIState_WaitNextEnemy : UIState_Battle
    {
        public UIState_WaitNextEnemy(UIView_InventoryHUD uiView_InventoryHUD, UIView_PlayerHUD uiView_PlayerHUD, UIView_BattleHUD uiView_BattleHUD) :
            base(uiView_InventoryHUD, uiView_PlayerHUD, uiView_BattleHUD)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            //Text
            UIView_BattleHUD.UIWidget_BattleStatus.ShowBattleStatusWithDelay("Its not finished yet", Color.yellow);

            //Tick indicator
            UIView_PlayerHUD.UIWidget_Tick.ToPrepareState();
        }
    }
}
