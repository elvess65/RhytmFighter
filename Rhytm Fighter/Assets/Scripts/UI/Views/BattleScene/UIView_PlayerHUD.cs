using RhytmFighter.Persistant;
using RhytmFighter.UI.Widget;
using UnityEngine;

namespace RhytmFighter.UI.View
{
    /// <summary>
    /// Отображение виджетов игрока в HUD
    /// </summary>
    public class UIView_PlayerHUD : UIView_Abstract
    {
        [Header("Widgets")]
        public UIWidget_Tick UIWidget_Tick;
        public UIWidget_HealthBar UIWidget_HealthBar;
        public UIWidget_ExperianceBar UIWidget_ExperianceBar;


        public override void Initialize()
        {
            UIWidget_Tick.Initialize((float)Rhytm.RhytmController.GetInstance().TickDurationSeconds / 8);
            UIWidget_ExperianceBar.Initialize(GameManager.Instance.DataHolder.PlayerDataModel.Character.CharacterExp);

            RegisterWidget(UIWidget_Tick);
            RegisterWidget(UIWidget_HealthBar);
            RegisterWidget(UIWidget_ExperianceBar);

            RegisterUpdatable(UIWidget_Tick);
        }
    }
}
