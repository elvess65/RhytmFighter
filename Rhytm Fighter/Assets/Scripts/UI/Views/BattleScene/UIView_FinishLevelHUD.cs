using RhytmFighter.UI.Widget;
using UnityEngine;

namespace RhytmFighter.UI.View
{
    /// <summary>
    /// Отображение виджетов окончания уровня
    /// </summary>
    public class UIView_FinishLevelHUD : UIView_Abstract
    {
        [Header("Widgets")]
        public UIWidget_GameOver UIWidget_GameOver;
        public UIWidget_LevelComplete UIWidget_LevelComplete;
        public UIWidget_TapToAction UIWidget_TapToAction;

        public override void Initialize()
        {
            UIWidget_TapToAction.Initialize();

            RegisterWidget(UIWidget_GameOver);
            RegisterWidget(UIWidget_LevelComplete);
            RegisterWidget(UIWidget_TapToAction);
        }
    }
}
