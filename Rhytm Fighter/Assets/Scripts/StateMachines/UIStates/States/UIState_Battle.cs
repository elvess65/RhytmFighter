using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.StateMachines.UIState
{
    public class UIState_Battle : UIState_Abstract
    {
        private WaitForSeconds m_WaitBeatIndicatorDelay;
        private Image m_BeatIndicatorImage;


        public UIState_Battle(Button buttonDefence, Text textBattleStatus, GameObject beatIndicator, Transform playerUIParent) :
            base(buttonDefence, textBattleStatus, beatIndicator, playerUIParent)
        {
            m_WaitBeatIndicatorDelay = new WaitForSeconds((float)Rhytm.RhytmController.GetInstance().TickDurationSeconds / 8);
            m_BeatIndicatorImage = m_BeatIndicator.GetComponent<Image>();
        }

        public override void EnterState()
        {
            base.EnterState();

            //Events
            Rhytm.RhytmController.GetInstance().OnTick += TickHandler;
            Rhytm.RhytmController.GetInstance().OnEventProcessingTick += TickHandler;

            //UI
            m_ButtonDefence.gameObject.SetActive(true);

            m_TextBattleStatus.text = "Prepare for battle";
            m_TextBattleStatus.color = Color.yellow;

            m_BeatIndicatorImage.color = Color.yellow;
            Core.GameManager.Instance.StartCoroutine(DisableBattleStatusTextCoroutine());
        }

        public override void ExitState()
        {
            base.ExitState();

            //Event
            Rhytm.RhytmController.GetInstance().OnTick -= TickHandler;
            Rhytm.RhytmController.GetInstance().OnEventProcessingTick -= TickHandler;

            //UI
            m_ButtonDefence.gameObject.SetActive(false);

            m_TextBattleStatus.text = Core.GameManager.Instance.PlayerModel.IsDestroyed ? "Game Over" : "Victory";
            m_TextBattleStatus.color = Core.GameManager.Instance.PlayerModel.IsDestroyed ? Color.red : Color.green;

            m_BeatIndicatorImage.color = Color.green;
            Core.GameManager.Instance.StartCoroutine(DisableBattleStatusTextCoroutine());
        }

        public void BattleStarted()
        {
            //UI
            m_BeatIndicatorImage.color = Color.red;

            m_TextBattleStatus.text = "Fight";
            m_TextBattleStatus.color = Color.red;

            Core.GameManager.Instance.StartCoroutine(DisableBattleStatusTextCoroutine());
        }


        private void TickHandler(int ticksSinceStart)
        {
            Core.GameManager.Instance.StartCoroutine(BeatIndicatorAnimationCoroutine());
        }

        private System.Collections.IEnumerator BeatIndicatorAnimationCoroutine()
        {
            m_BeatIndicator.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);

            yield return m_WaitBeatIndicatorDelay;

            m_BeatIndicator.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
        }
    }
}
