using RhytmFighter.Persistant.Abstract;

namespace RhytmFighter.Persistant.SceneLoading
{
    public class SceneLoadingManager : Singleton<SceneLoadingManager>
    {
        public UnityEngine.GameObject CameraObject;

        public System.Action OnFadeIn;
        public System.Action OnFadeOut;

        [UnityEngine.SerializeField] private SceneTransitionFadeController m_TransitionController;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            m_TransitionController.OnFadeIn += FadeInHandler;
            m_TransitionController.OnFadeOut += FadeOutHandler;
        }

        public void FadeIn()
        {
            m_TransitionController.FadeIn();
        }

        public void FadeOut()
        {
            m_TransitionController.FadeOut();
        }


        private void FadeInHandler()
        {
            CameraObject.SetActive(true);
            OnFadeIn?.Invoke();
        }

        private void FadeOutHandler()
        {
            CameraObject.SetActive(false);
            OnFadeOut?.Invoke();
        }
    }
}
