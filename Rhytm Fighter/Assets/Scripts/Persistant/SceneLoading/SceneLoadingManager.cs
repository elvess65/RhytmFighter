using RhytmFighter.Persistant.Abstract;

namespace RhytmFighter.Persistant.SceneLoading
{
    public class SceneLoadingManager : Singleton<SceneLoadingManager>
    {
        public UnityEngine.GameObject CameraObject;

        public System.Action OnFadeIn;
        public System.Action OnFadeOut;

        private FadeStates m_CurFadeState = FadeStates.Default;

        private enum FadeStates { Default, FadingIn, FadingOut, FadedIn, FadedOut };

        [UnityEngine.SerializeField] private SceneTransitionFadeController m_TransitionController;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            m_TransitionController.OnFadeIn += FadeInHandler;
            m_TransitionController.OnFadeOut += FadeOutHandler;
            m_TransitionController.Initialize();
        }

        public void FadeIn()
        {
            switch(m_CurFadeState)
            {
                case FadeStates.FadedOut:
                case FadeStates.Default:
                    m_CurFadeState = FadeStates.FadingIn;
                    m_TransitionController.FadeIn();
                    break;
                case FadeStates.FadedIn:
                    FadeInHandler();
                    break;
            }
        }

        public void FadeOut()
        {
            switch (m_CurFadeState)
            {
                case FadeStates.FadedIn:
                case FadeStates.Default:
                    m_CurFadeState = FadeStates.FadingOut;
                    m_TransitionController.FadeOut();
                    break;
                case FadeStates.FadedOut:
                    FadeOutHandler();
                    break;
            }
        }


        private void FadeInHandler()
        {
            m_CurFadeState = FadeStates.FadedIn;
            CameraObject.SetActive(true);
            OnFadeIn?.Invoke();
        }

        private void FadeOutHandler()
        {
            m_CurFadeState = FadeStates.FadedOut;
            CameraObject.SetActive(false);
            OnFadeOut?.Invoke();
        }
    }
}
