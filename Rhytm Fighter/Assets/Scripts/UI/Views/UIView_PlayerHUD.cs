using RhytmFighter.Persistant.Abstract;
using RhytmFighter.UI.Widget;
using UnityEngine;

namespace RhytmFighter.UI.View
{
    /// <summary>
    /// Отображение виджетов игрока в HUD
    /// </summary>
    public class UIView_PlayerHUD : UIView_Abstract
    {
        [Space(10)]
        public Transform PlayerHealthBarParent;

        [Header("Widgets")]
        public UIWidget_Tick UIWidget_Tick;


        public override void Initialize()
        {
            UIWidget_Tick.Initialize((float)Rhytm.RhytmController.GetInstance().TickDurationSeconds / 8);

            RegisterWidget(UIWidget_Tick);
            RegisterUpdatable(UIWidget_Tick);
        }
    }
}
