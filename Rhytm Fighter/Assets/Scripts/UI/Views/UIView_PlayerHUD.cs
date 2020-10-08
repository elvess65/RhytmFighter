using RhytmFighter.Persistant.Abstract;
using RhytmFighter.UI.Components;
using RhytmFighter.UI.Widget;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhytmFighter.UI.View
{
    /// <summary>
    /// Отображение виджетов игрока в HUD
    /// </summary>
    public class UIView_PlayerHUD : UIView_Abstract
    {
        public UIWidget_Tick UIWidget_Ticks;

        public override void Initialize()
        {
            //Create updatables list
            m_Updatables = new iUpdatable[]
            {
                UIWidget_Ticks,
            };
        }

        public override void PerformUpdate(float deltaTime)
        {
            
        }
    }
}
