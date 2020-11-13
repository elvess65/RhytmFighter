using RhytmFighter.Animation;
using RhytmFighter.Persistant;
using RhytmFighter.Persistant.Abstract;
using RhytmFighter.Persistant.SceneLoading;
using RhytmFighter.UI.View;
using UnityEngine;

namespace RhytmFighter.OtherScenes.MenuScene
{
    public class ForgeSceneManager : Singleton<ForgeSceneManager>
    {
        public System.Action OnSceneClosed;

        [SerializeField] private UIView_ForgeScene UIView;

        public void Initialize()
        {
            UIView.OnClose += CloseHandler;
            UIView.Initialize();
        }


        private void Update()
        {
            UIView.PerformUpdate(Time.deltaTime);
        }


        private void CloseHandler()
        {
            GameManager.Instance.SceneLoader.OnSceneUnloadingComplete += SceneUnloadedHandler;
            GameManager.Instance.SceneLoader.UnloadLevel(SceneLoader.FORGE_SCENE_NAME, true);
        }

        private void SceneUnloadedHandler()
        {
            GameManager.Instance.SceneLoader.OnSceneUnloadingComplete -= SceneUnloadedHandler;

            OnSceneClosed?.Invoke();
            OnSceneClosed = null;
        }
    }
}
