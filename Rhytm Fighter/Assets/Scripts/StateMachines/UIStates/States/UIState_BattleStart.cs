using RhytmFighter.Battle.Core;
using RhytmFighter.UI.Components;
using RhytmFighter.UI.View;
using RhytmFighter.UI.Widget;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.StateMachines.UIState
{
    /// <summary>
    /// Состояние начала боя
    /// </summary>
    public class UIState_BattleStart : UIState_Battle
    {
        public UIState_BattleStart(UIView_InventoryHUD uiView_InventoryHUD, UIView_PlayerHUD uiView_PlayerHUD, UIView_BattleHUD uiView_BattleHUD) :
                base(uiView_InventoryHUD, uiView_PlayerHUD, uiView_BattleHUD)
        {
        }


        public override void EnterState()
        {
            base.EnterState();

            //Text
            UIView_BattleHUD.UIWidget_BattleStatus.ShowBattleStatusWithDelay("Fight", Color.red);

            //Tick indicator
            UIView_PlayerHUD.UIWidget_Tick.ToBattleState();
            UIView_PlayerHUD.UIWidget_Tick.PlayArrowsAnimation();
        }


        protected override void ProcessTickHandler(int ticksSinceStart)
        {
            base.ProcessTickHandler(ticksSinceStart);

            UIView_PlayerHUD.UIWidget_Tick.PlayArrowsAnimation();
        }
    }
}
