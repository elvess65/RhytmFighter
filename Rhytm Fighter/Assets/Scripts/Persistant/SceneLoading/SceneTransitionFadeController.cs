using FrameworkPackage.Utils;
using UnityEngine;

namespace RhytmFighter.Persistant.SceneLoading
{
    [RequireComponent(typeof(CanvasGroup))]
    public class SceneTransitionFadeController : MonoBehaviour
    {
        public System.Action OnFadeIn;
        public System.Action OnFadeOut;
       
        private CanvasGroup m_CanvasGroup;
        private InterpolationData<float> m_LerpData;
        private System.Action m_OnTransitionFinished;


        public void Initialize()
        {
            m_CanvasGroup = GetComponent<CanvasGroup>();
            m_LerpData = new InterpolationData<float>(1);
        }

        public void FadeIn()
        {
            m_LerpData.From = 0;
            m_LerpData.To = 1;
            m_OnTransitionFinished = () => OnFadeIn?.Invoke();

            m_LerpData.Start();
        }

        public void FadeOut()
        {
            m_LerpData.From = 1;
            m_LerpData.To = 0;
            m_OnTransitionFinished = () => OnFadeOut?.Invoke();

            m_LerpData.Start();
        }


        private void Update()
        {
            if (m_LerpData.IsStarted)
            {
                m_LerpData.Increment();
                m_CanvasGroup.alpha = Mathf.Lerp(m_LerpData.From, m_LerpData.To, m_LerpData.Progress);

                if (m_LerpData.Overtime())
                {
                    m_LerpData.Stop();
                    m_OnTransitionFinished?.Invoke();
                }
            }
        }
    }
}
