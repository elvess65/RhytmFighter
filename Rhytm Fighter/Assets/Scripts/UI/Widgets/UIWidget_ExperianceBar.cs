using System.Collections;
using RhytmFighter.UI.Bar;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.UI.Widget
{
    /// <summary>
    /// Виджет опыта игрока
    /// </summary>
    public class UIWidget_ExperianceBar : UIWidget
    {
        [Space(10)]
        public AbstractBarStrategy Bar;
        public Text Text_Level;
        public Text Text_Experiance;
        public Text Text_GainedExperiance;
        
        private int m_CurExp = 0;
        private WaitForSeconds m_WaitForGainedExpShowTime;


        public void Initialize(int curExp)
        {
            Text_GainedExperiance.enabled = false;
            m_WaitForGainedExpShowTime = new WaitForSeconds(1);

            UpdateData(curExp);
        }

        public void UpdateBar(int expGained)
        {
            UpdateData(m_CurExp + expGained);
            ShowGainedExp(expGained);
        }


        private void UpdateData(int curExp)
        {
            m_CurExp = curExp;

            int curLevel = GetLevelByExp(m_CurExp);
            int expToNextLvl = GetExpToNextLevel(curLevel);

            Bar.SetProgress(m_CurExp, expToNextLvl);
            UpdateTexts(curLevel, m_CurExp, expToNextLvl);
        }

        private void UpdateTexts(int curLevel, int curExp, int expToNextLvl)
        {
            Text_Level.text = curLevel.ToString();
            Text_Experiance.text = $"{curExp}/{expToNextLvl}";
        }

        private void ShowGainedExp(int gainedExp)
        {
            Text_GainedExperiance.text = $"+{gainedExp}";
        }



        private int GetLevelByExp(int exp)
        {
            return 2;
        }

        private int GetExpToNextLevel(int curLevel)
        {
            return 12;
        }

        IEnumerator WaitGainedExpShowTime()
        {
            Text_GainedExperiance.enabled = true;

            yield return m_WaitForGainedExpShowTime;

            Text_GainedExperiance.enabled = false;
        }
    }
}
