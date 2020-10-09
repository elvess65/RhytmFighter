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
        [Header("Widgets")]
        public UIWidget_Tick UIWidget_Tick;

        [Header("Parents")]
        public Transform PlayerHealthBarParent;

        public override void Initialize()
        {
            UIWidget_Tick.Initialize((float)Rhytm.RhytmController.GetInstance().TickDurationSeconds / 8);

            //Create updatables list
            m_Updatables = new iUpdatable[]
            {
                UIWidget_Tick,
            };
        }
    }
}
