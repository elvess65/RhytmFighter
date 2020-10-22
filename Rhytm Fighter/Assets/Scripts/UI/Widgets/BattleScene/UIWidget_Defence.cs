namespace RhytmFighter.UI.Widget
{
    /// <summary>
    /// Виджет отображения кнопки защиты 
    /// </summary>
    public class UIWidget_Defence : UIWidget_Clickable
    {
        /// <summary>
        /// Вызывается через UEvent
        /// </summary>
        public void WidgetPointerDownHandler()
        {
            WidgetPressHandler();
        }
    }
}
