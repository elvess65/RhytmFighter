using RhytmFighter.UI.View;
using UnityEngine;

namespace RhytmFighter.StateMachines.UIState
{
    /// <summary>
    /// Состояние подготовки к бою
    /// </summary>
    public class UIState_PrepareForBattle : UIState_Battle
    {
        public UIState_PrepareForBattle(UIView_InventoryHUD uiView_InventoryHUD, UIView_PlayerHUD uiView_PlayerHUD, UIView_BattleHUD uiView_BattleHUD) :
                base(uiView_InventoryHUD, uiView_PlayerHUD, uiView_BattleHUD)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            //Text
            UIView_BattleHUD.UIWidget_BattleStatus.ShowBattleStatusWithDelay("Prepare for battle", Color.yellow);

            //Tick indicator
            UIView_PlayerHUD.UIWidget_Tick.ToPrepareState();
        }
    }
}
