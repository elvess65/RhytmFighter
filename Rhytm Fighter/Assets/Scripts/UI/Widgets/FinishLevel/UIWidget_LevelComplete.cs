using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.UI.Widget
{
    /// <summary>
    /// Виджет отображения окончания уровня
    /// </summary>
    public class UIWidget_LevelComplete : UIWidget
    {
        [Space(10)]
        public Text Text_BattleResult;

        public void ShowResult(string statusText, Color statusColor)
        {
            Text_BattleResult.text = statusText;
            Text_BattleResult.color = statusColor;
        }
    }
}
