using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.UI.Widget
{
    /// <summary>
    /// Виджет отображения текста нажатия для дейтсвия
    /// </summary>
    public class UIWidget_TapToAction : UIWidget
    {
        [Space(10)]
        public Text Text_TapToAction;

        public void Initialize()
        {
            InternalInitialize();

            Text_TapToAction.text = "Tap to continue";
        }
    }
}
