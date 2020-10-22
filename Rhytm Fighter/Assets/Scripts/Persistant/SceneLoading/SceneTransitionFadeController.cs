using UnityEngine;

namespace RhytmFighter.Persistant.SceneLoading
{
    public class SceneTransitionFadeController : MonoBehaviour
    {
        public System.Action OnFadeIn;
        public System.Action OnFadeOut;

        [SerializeField] private Animator m_AnimationController;

        private const string m_FADE_IN_KEY = "FadeIn";
        private const string m_FADE_OUT_KEY = "FadeOut";


        public void FadeIn()
        {
            Debug.Log("Fade in");
            m_AnimationController.SetTrigger(m_FADE_IN_KEY);
        }

        public void FadeOut()
        {
            m_AnimationController.SetTrigger(m_FADE_OUT_KEY);
        }

        /// Is called from animation
        public void FadeInComplete()
        {
            Debug.Log("On faded in");
            //m_AnimationController.ResetTrigger(m_FADE_IN_KEY);
            OnFadeIn?.Invoke();
        }

        /// Is called from animation
        public void FadeOutComplete()
        {
            //m_AnimationController.ResetTrigger(m_FADE_OUT_KEY);
            OnFadeOut?.Invoke();
        }

    }
}
