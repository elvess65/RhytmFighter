using System.Collections;
using RhytmFighter.Persistant;
using RhytmFighter.Persistant.Abstract;
using RhytmFighter.UI.Bar;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.UI.Widget
{
    /// <summary>
    /// Виджет опыта
    /// </summary>
    public class UIWidget_ExperianceBar : UIWidget, iUpdatable
    {
        [Space(10)]
        public AbstractBarStrategy Bar;
        public Text Text_Level;
        public Text Text_Experiance;
        public Text Text_GainedExperiance;
        
        private int m_ExpAmount = 0;
        private int m_CharacterID;
        private WaitForSeconds m_WaitForGainedExpShowTime;


        public void Initialize(int characterID, int expAmount)
        {
            m_CharacterID = characterID;

            Text_GainedExperiance.enabled = false;
            m_WaitForGainedExpShowTime = new WaitForSeconds(1);

            UpdateData(expAmount);
        }

        public void UpdateBar(int expGained)
        {
            UpdateData(m_ExpAmount + expGained);
            ShowGainedExp(expGained);
        }

        public void PerformUpdate(float deltaTime)
        {
        }


        private void UpdateData(int expAmount)
        {
            m_ExpAmount = expAmount;

            int curLevel = GetLevelByExp(m_ExpAmount);
            int expToNextLvl = GetExpToNextLevel(curLevel);

            Bar.SetProgress(m_ExpAmount, expToNextLvl);
            UpdateTexts(curLevel, m_ExpAmount, expToNextLvl);
        }

        private void UpdateTexts(int curLevel, int curExp, int expToNextLvl)
        {
            Text_Level.text = curLevel.ToString();
            Text_Experiance.text = $"{curExp}/{expToNextLvl}";
        }

        private void ShowGainedExp(int gainedExp)
        {
            Text_GainedExperiance.text = $"+{gainedExp}";

            StartCoroutine(WaitGainedExpShowTime());
        }



        private int GetLevelByExp(int expAmount)
        {
            return GameManager.Instance.DataHolder.DataTableModel.LevelingDataModel.GetWeaponLevelByExp(m_CharacterID, expAmount);
        }

        private int GetExpToNextLevel(int curLevel)
        {
            return GameManager.Instance.DataHolder.DataTableModel.LevelingDataModel.GetWeaponExpForLevel(m_CharacterID, curLevel + 1);
        }

        IEnumerator WaitGainedExpShowTime()
        {
            Text_GainedExperiance.enabled = true;

            yield return m_WaitForGainedExpShowTime;

            Text_GainedExperiance.enabled = false;
        }
    }
}
