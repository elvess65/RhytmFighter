using System.Collections;
using FrameworkPackage.Utils;
using RhytmFighter.Persistant.Abstract;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.UI.Widget
{
    /// <summary>
    /// Виджет валюты
    /// </summary>
    public class UIWidget_Currency : UIWidget, iUpdatable
    {
        [Space(10)]
        public Text Text_Amount;
        public Text Text_GainedAmount;

        private int m_CurrentAmount;
        private int m_GainedAmountFrom;
        private WaitForSeconds m_WaitAnimDelay;
        private InterpolationData<int> m_LerpData;


        public void Initialize(int currencyAmount)
        {
            m_CurrentAmount = currencyAmount;
            Text_GainedAmount.enabled = false;

            m_WaitAnimDelay = new WaitForSeconds(1);
            m_LerpData = new InterpolationData<int>(1);

            ShowAmount(m_CurrentAmount);
        }

        public void AddCurrency(int gainedAmount)
        {
            m_CurrentAmount += gainedAmount;

            m_LerpData.From = m_CurrentAmount - gainedAmount;
            m_LerpData.To = m_CurrentAmount;
            m_GainedAmountFrom = gainedAmount;

            Text_GainedAmount.enabled = true;

            ShowGainedAmount(gainedAmount);
            StartCoroutine(WaitDelayBeforeAnimation());
        }

        public void PerformUpdate(float deltaTime)
        {
            if (m_LerpData.IsStarted)
            {
                m_LerpData.Increment();

                int amount = (int)Mathf.Lerp(m_LerpData.From, m_LerpData.To, m_LerpData.Progress);
                int gainedAmount = (int)Mathf.Lerp(m_GainedAmountFrom, 1, m_LerpData.Progress);

                ShowAmount(amount);
                ShowGainedAmount(gainedAmount);

                if (m_LerpData.Overtime())
                {
                    ShowAmount(m_CurrentAmount);
                    Text_GainedAmount.enabled = false;
                }
            }
        }


        private void ShowAmount(int amount)
        {
            Text_Amount.text = amount.ToString();
        }

        private void ShowGainedAmount(int amount)
        {
            Text_GainedAmount.text = $"+{amount}";
        }

        private IEnumerator WaitDelayBeforeAnimation()
        {
            yield return m_WaitAnimDelay;
            m_LerpData.Start();
        }
    }
}
