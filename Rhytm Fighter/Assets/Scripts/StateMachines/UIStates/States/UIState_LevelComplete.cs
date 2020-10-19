using RhytmFighter.UI.View;
using UnityEngine;

namespace RhytmFighter.StateMachines.UIState
{
    /// <summary>
    /// Состояние окончания уровня
    /// </summary>
    public class UIState_LevelComplete : UIState_NoUI
    {
        public UIState_LevelComplete(UIView_InventoryHUD uiView_InventoryHUD, UIView_PlayerHUD uiView_PlayerHUD, UIView_BattleHUD uiView_BattleHUD) :
            base(uiView_InventoryHUD, uiView_PlayerHUD, uiView_BattleHUD)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            //Text
            UIView_BattleHUD.DisableView(false);
            UIView_BattleHUD.UIWidget_Defence.Root.gameObject.SetActive(false);
            UIView_BattleHUD.UIWidget_BattleStatus.ShowBattleStatus("Level Complete", Color.green);
        }
    }
}
