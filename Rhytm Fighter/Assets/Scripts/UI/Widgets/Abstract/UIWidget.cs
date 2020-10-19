using UnityEngine;

namespace RhytmFighter.UI.Widget
{
    /// <summary>
    /// Базовый класс виджета
    /// </summary>
    public abstract class UIWidget : MonoBehaviour
    {
        public Transform Root;

        public void DisableWidget(bool isDisabled, bool isAnimated)
        {
            if (!isAnimated)
                Root.gameObject.SetActive(!isDisabled);
            else
            {
                Debug.Log("Disable widget animated");
                Root.gameObject.SetActive(!isDisabled);
            }
        }

        protected virtual void InternalInitialize()
        {
        }
    }
}
