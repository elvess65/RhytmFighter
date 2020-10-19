using RhytmFighter.UI.View;
using UnityEngine;

namespace RhytmFighter.StateMachines.UIState
{
    /// <summary>
    /// Состояние проиграша игрока
    /// </summary>
    public class UIState_GameOverState : UIState_NoUI
    {
        public UIState_GameOverState(UIView_InventoryHUD uiView_InventoryHUD, UIView_PlayerHUD uiView_PlayerHUD, UIView_BattleHUD uiView_BattleHUD) :
            base(uiView_InventoryHUD, uiView_PlayerHUD, uiView_BattleHUD)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            //Text
            UIView_BattleHUD.DisableView(false);
            UIView_BattleHUD.UIWidget_Defence.Root.gameObject.SetActive(false);
            UIView_BattleHUD.UIWidget_BattleStatus.ShowBattleStatus("Game Over", Color.red);
        }
    }
}
