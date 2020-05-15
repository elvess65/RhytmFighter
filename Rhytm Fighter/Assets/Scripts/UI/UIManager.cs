using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.UI
{
    public class UIManager : MonoBehaviour
    {
        public System.Action OnButtonDefencePressed;

        public GameObject BeatIndicator;

        [Header("Battle")]
        public Button Button_Defence;
        public Text Text_BattleStatus;

        private WaitForSeconds m_WaitDisableBattleUIDelay;
        private WaitForSeconds m_WaitBeatIndicatorDelay;


        public void Initialize()
        {
            m_WaitDisableBattleUIDelay = new WaitForSeconds((float)Rhytm.RhytmController.GetInstance().TickDurationSeconds);
            m_WaitBeatIndicatorDelay = new WaitForSeconds((float)Rhytm.RhytmController.GetInstance().TickDurationSeconds / 8);
        }

        public void PrepareForBattle()
        {
            BeatIndicator.GetComponent<Image>().color = Color.yellow;

            Text_BattleStatus.text = "Prepare for battle";
            Text_BattleStatus.color = Color.yellow;

            StartCoroutine(DisableBattleUICoroutine());
        }

        public void BattleStart()
        {
            BeatIndicator.GetComponent<Image>().color = Color.red;

            Text_BattleStatus.text = "Fight";
            Text_BattleStatus.color = Color.red;

            StartCoroutine(DisableBattleUICoroutine());
        }

        public void BattleFinish()
        {
            BeatIndicator.GetComponent<Image>().color = Color.green;

            Text_BattleStatus.text = "Victory";
            Text_BattleStatus.color = Color.green;

            StartCoroutine(DisableBattleUICoroutine());
        }

        public void NotifyAboutTick()
        {
            StartCoroutine(BeatIndicatorAnimationCoroutine());
        }

        public void ButtonDefence_PressHandler()
        {
            OnButtonDefencePressed?.Invoke();
        }


        private System.Collections.IEnumerator DisableBattleUICoroutine()
        {
            Text_BattleStatus.gameObject.SetActive(true);

            yield return m_WaitDisableBattleUIDelay;

            Text_BattleStatus.gameObject.SetActive(false);
        }

        private System.Collections.IEnumerator BeatIndicatorAnimationCoroutine()
        {
            BeatIndicator.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);

            yield return m_WaitBeatIndicatorDelay;

            BeatIndicator.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
        }
    }
}
