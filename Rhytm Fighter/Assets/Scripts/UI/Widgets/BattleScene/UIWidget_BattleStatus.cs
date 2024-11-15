﻿using RhytmFighter.Battle.Core;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.UI.Widget
{
    /// <summary>
    /// Виджет отображения статусов боя
    /// </summary>
    public class UIWidget_BattleStatus : UIWidget
    {
        [Space(10)]
        public Text Text_BattleStatus;

        private WaitForSeconds m_WaitDisableBattleStatusUIDelay;
        private Coroutine m_DisableBattleStatusTextCoroutine;


        public void Initialize()
        {
            m_WaitDisableBattleStatusUIDelay = new WaitForSeconds((float)Rhytm.RhytmController.GetInstance().TickDurationSeconds * 2);
        }

        public void ShowBattleStatusWithDelay(string statusText, Color statusColor)
        {
            ShowBattleStatus(statusText, statusColor);

            if (m_DisableBattleStatusTextCoroutine != null)
                BattleManager.Instance.StopCoroutine(m_DisableBattleStatusTextCoroutine);

            m_DisableBattleStatusTextCoroutine = BattleManager.Instance.StartCoroutine(DisableBattleStatusTextCoroutine());
        }

        public void ShowBattleStatus(string statusText, Color statusColor)
        {
            Text_BattleStatus.text = statusText;
            Text_BattleStatus.color = statusColor;

            Text_BattleStatus.gameObject.SetActive(true);
        }


        private System.Collections.IEnumerator DisableBattleStatusTextCoroutine()
        {
            yield return m_WaitDisableBattleStatusUIDelay;

            Text_BattleStatus.gameObject.SetActive(false);
            m_DisableBattleStatusTextCoroutine = null;
        }
    }
}
