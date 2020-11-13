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
        public System.Action OnPlayButtonPressHandler;
        public System.Action OnForgeButtonPressHandler;

        [Space(10)]

        public UIWidget_Button UIWidget_ButtonPlay;
        public UIWidget_Button UIWidget_ButtonForge;
        public UIWidget_Currency UIWidget_Currency;

        public override void Initialize()
        {
            UIWidget_ButtonPlay.OnWidgetPress += ButtonPlay_Widget_PressHandler;
            UIWidget_ButtonPlay.Initialize();

            UIWidget_ButtonForge.OnWidgetPress += ButtonForge_Widget_PressHandler;
            UIWidget_ButtonForge.Initialize();

            UIWidget_Currency.Initialize(GameManager.Instance.DataHolder.AccountModel.CurrencyAmount);

            RegisterWidget(UIWidget_ButtonPlay);
            RegisterWidget(UIWidget_ButtonForge);
            RegisterWidget(UIWidget_Currency);

            RegisterUpdatable(UIWidget_Currency);
        }

        public override void LockInput(bool isLocked)
        {
            UIWidget_ButtonPlay.LockInput(isLocked);
            UIWidget_ButtonForge.LockInput(isLocked);
        }


        void ButtonPlay_Widget_PressHandler()
        {
             LockInput(true);

             OnPlayButtonPressHandler?.Invoke();
        }

        void ButtonForge_Widget_PressHandler()
        {
            LockInput(true);

            OnForgeButtonPressHandler?.Invoke();
        }
    }
}
