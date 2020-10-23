using RhytmFighter.UI.Widget;

namespace RhytmFighter.UI.View
{
    /// <summary>
    /// Отображение виджетов в главном меню
    /// </summary>
    public class UIView_MenuScene : UIView_Abstract
    {
        public System.Action OnPlayPressHandler;

        public UIWidget_Play UIWidget_Play;

        public override void Initialize()
        {
            UIWidget_Play.OnWidgetPress += WidgetPressHandler;
            UIWidget_Play.Initialize();

            RegisterWidget(UIWidget_Play);
        }

        void WidgetPressHandler()
        {
            UIWidget_Play.LockInput(true);

            OnPlayPressHandler?.Invoke();
        }
    }
}
