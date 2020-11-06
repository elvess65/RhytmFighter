using RhytmFighter.Persistant;
using RhytmFighter.UI.Widget;
using UnityEngine;

namespace RhytmFighter.UI.View
{
    /// <summary>
    /// Отображение виджетов в главном меню
    /// </summary>
    public class UIView_MenuScene : UIView_Abstract
    {
        public System.Action OnPlayPressHandler;

        [Space(10)]

        public UIWidget_Play UIWidget_Play;
        public UIWidget_Currency UIWidget_Currency;

        public override void Initialize()
        {
            UIWidget_Play.OnWidgetPress += WidgetPressHandler;
            UIWidget_Play.Initialize();

            UIWidget_Currency.Initialize(GameManager.Instance.DataHolder.AccountModel.CurrencyAmount);

            RegisterWidget(UIWidget_Play);
            RegisterWidget(UIWidget_Currency);

            RegisterUpdatable(UIWidget_Currency);
        }

        void WidgetPressHandler()
        {
            UIWidget_Play.LockInput(true);

            OnPlayPressHandler?.Invoke();
        }
    }
}
