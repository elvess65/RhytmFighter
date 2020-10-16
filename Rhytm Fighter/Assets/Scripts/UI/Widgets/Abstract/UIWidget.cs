using UnityEngine;

namespace RhytmFighter.UI.Widget
{
    /// <summary>
    /// Базовый класс виджета
    /// </summary>
    public abstract class UIWidget : MonoBehaviour
    {
        public Transform Root;

        protected virtual void InternalInitialize()
        {
        }
    }
}
