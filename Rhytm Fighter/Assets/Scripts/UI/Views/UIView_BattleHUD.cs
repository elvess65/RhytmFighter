using RhytmFighter.UI.Widget;
using UnityEngine;

namespace RhytmFighter.UI.View
{
    /// <summary>
    /// Отображение виджетов боя в HUD
    /// </summary>
    public class UIView_BattleHUD : UIView_Abstract
    {
        public System.Action OnWidgetDefencePointerDown;

        [Header("Widgets")]
        public UIWidget_Defence UIWidget_Defence;
        public UIWidget_BattleStatus UIWidget_BattleStatus;


        public override void Initialize()
        {
            UIWidget_Defence.OnWidgetPress += WidgetDefence_PointerDownHandler;

            UIWidget_BattleStatus.Initialize();
        }

        #region WIDGET HANDLERS

        private void WidgetDefence_PointerDownHandler()
        {
            OnWidgetDefencePointerDown?.Invoke();
        }

        #endregion
    }
}
