using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.StateMachines.UIState
{
    public class UIState_Adventure : UIState_Abstract
    {
        private WaitForSeconds m_WaitBeatIndicatorDelay;


        public UIState_Adventure(Button buttonDefence, Text textBattleStatus, GameObject beatIndicator, Transform playerUIParent) :
            base(buttonDefence, textBattleStatus, beatIndicator, playerUIParent)
        {
            m_WaitBeatIndicatorDelay = new WaitForSeconds((float)Rhytm.RhytmController.GetInstance().TickDurationSeconds / 8);
        }

        public override void EnterState()
        {
            base.EnterState();

            //Events
            Rhytm.RhytmController.GetInstance().OnTick += TickHandler;
        }

        public override void ExitState()
        {
            base.ExitState();

            //Events
            Rhytm.RhytmController.GetInstance().OnTick -= TickHandler;
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
