using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.UI.Widget
{
    /// <summary>
    /// Базовый класс виджета, на который можно нажимать
    /// </summary>
    public abstract class UIWidget_Clickable : UIWidget
    {
        public System.Action OnWidgetPress;

        [SerializeField] public Button WidgetButton;

        protected override void InternalInitialize()
        {
            WidgetButton.onClick.AddListener(WidgetPressHandler);
        }

        protected virtual void WidgetPressHandler()
        {
            OnWidgetPress?.Invoke();
        }
    }
}
