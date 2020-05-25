using FrameworkPackage.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.UI.Components
{
    public class UIComponent_TickIndicator : MonoBehaviour
    {
        [SerializeField] Transform m_SourceObject;
        [SerializeField] Image m_SourceImage;
        [SerializeField] RectTransform[] m_SingleTickFlyingIndicator;

        private WaitForSeconds m_WaitBeatIndicatorDelay;
        private InterpolationData<float> m_LerpData;


        public void Initialize(float tickDuration)
        {
            m_LerpData = new InterpolationData<float>(tickDuration * 8);
            m_LerpData.From = 300;
            m_LerpData.To = 0;

            m_WaitBeatIndicatorDelay = new WaitForSeconds(tickDuration);
        }

        public void ToNormalState()
        {
            m_SourceImage.color = Color.green;

            for (int i = 0; i < m_SingleTickFlyingIndicator.Length; i++)
                m_SingleTickFlyingIndicator[i].gameObject.SetActive(false);
        }

        public void ToPrepareState()
        {
            m_SourceImage.color = Color.yellow;
        }

        public void ToBattleState()
        {
            m_SourceImage.color = Color.red;

            for (int i = 0; i < m_SingleTickFlyingIndicator.Length; i++)
                m_SingleTickFlyingIndicator[i].gameObject.SetActive(true);
        }

        public void HandleTick()
        {
            StartCoroutine(TickAnimationCoroutine());
            m_LerpData.TotalTime = (float)Rhytm.RhytmController.GetInstance().TimeToNextTick;

            m_LerpData.Start();
        }

        public void HandleProcessTick()
        {
            StartCoroutine(TickAnimationCoroutine());
        }


        private void Update()
        {
            if (m_LerpData.IsStarted)
            {
                m_LerpData.Increment();

                bool isInUseRange = 1 - Rhytm.RhytmController.GetInstance().ProgressToNextTickAnalog < Rhytm.RhytmInputProxy.InputPrecious;

                for (int i = 0; i < m_SingleTickFlyingIndicator.Length; i++)
                {
                    Vector3 anchoredPos = m_SingleTickFlyingIndicator[i].anchoredPosition;
                    anchoredPos.x = Mathf.Lerp(m_LerpData.From * (i % 2 == 1 ? -1 : 1), m_LerpData.To, m_LerpData.Progress);
                    m_SingleTickFlyingIndicator[i].anchoredPosition = anchoredPos;

                    m_SingleTickFlyingIndicator[i].GetComponent<Image>().color = isInUseRange ? Color.green : Color.white;
                }

                if (m_LerpData.Overtime())
                {
                    m_LerpData.Stop();
                }
            }
        }

        private System.Collections.IEnumerator TickAnimationCoroutine()
        {
            m_SourceImage.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);

            yield return m_WaitBeatIndicatorDelay;

            m_SourceImage.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
        }
    }
}
