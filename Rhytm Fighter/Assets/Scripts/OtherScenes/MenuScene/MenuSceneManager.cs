using RhytmFighter.Animation;
using RhytmFighter.Persistant;
using RhytmFighter.Persistant.Abstract;
using RhytmFighter.Persistant.SceneLoading;
using RhytmFighter.UI.View;
using UnityEngine;

namespace RhytmFighter.OtherScenes.MenuScene
{
    public class MenuSceneManager : Singleton<MenuSceneManager>
    {
        [SerializeField] private UIView_MenuScene UIView;
        [SerializeField] private CameraContoller CameraContoller;
        [SerializeField] private CharactersController CharacterController;

        private AnimationEventsListener m_SelectedCharacterAnimationEventsListener;


        public void Initialize()
        {
            UIView.OnPlayButtonPressHandler += UIView_PlayButton_PressHandler;
            UIView.OnForgeButtonPressHandler += UIView_ForgeButton_PressHandler;
            UIView.Initialize();

            CharacterController.Initialize();

            CameraContoller.OnCameraPushKeyFrameReached += CameraPushingFinishedHandler;
            CameraContoller.Initialize();

            m_SelectedCharacterAnimationEventsListener = CharacterController.SelectedCharacterAnimationController.Controller.GetComponent<AnimationEventsListener>();

            GameManager.Instance.DataHolder.BattleSessionModel.CurrentLevelID = 1;
            GameManager.Instance.DataHolder.BattleSessionModel.SelectedCharactedID = 1;
        }


        private void Update()
        {
            UIView.PerformUpdate(Time.deltaTime);
            CameraContoller.PerformUpdate(Time.deltaTime);
        }


        #region Button Play

        private void UIView_PlayButton_PressHandler()
        {
            //Camera is pushed by animation event

            //Disable all UI widgets
            UIView.SetWidgetsActive(false, true);

            //Subscribe for animation event
            m_SelectedCharacterAnimationEventsListener.OnOtherEvent += CameraContoller.PushCamera;

            //Play animation
            CharacterController.SelectedCharacterAnimationController.PlayAnimation(Persistant.Enums.AnimationTypes.MenuAction);
        }

        private void CameraPushingFinishedHandler()
        {
            GameManager.Instance.SceneLoader.OnSceneUnloadingComplete += ButtonPlay_MenuSceneUnloadedHandler;
            GameManager.Instance.SceneLoader.UnloadLevel(SceneLoader.MENU_SCENE_NAME);
        }

        private void ButtonPlay_MenuSceneUnloadedHandler()
        {
            GameManager.Instance.SceneLoader.OnSceneUnloadingComplete -= ButtonPlay_MenuSceneUnloadedHandler;
            GameManager.Instance.SceneLoader.LoadLevel(SceneLoader.BATTLE_SCENE_NAME);
        }

        #endregion

        #region Button Forge

        private void UIView_ForgeButton_PressHandler()
        {
            //Disable all UI widgets
            UIView.SetWidgetsActive(false, true);

            //Subscribe for animation event
            m_SelectedCharacterAnimationEventsListener.OnOtherEvent += ShowForge;

            //Play animation
            CharacterController.SelectedCharacterAnimationController.PlayAnimation(Persistant.Enums.AnimationTypes.MenuAction);
        }

        private void ShowForge()
        {
            //Unscribe from animation event
            m_SelectedCharacterAnimationEventsListener.OnOtherEvent -= ShowForge;

            //Load scene
            GameManager.Instance.SceneLoader.OnSceneLoadingComplete += ButtonForge_ForgeLoadedHandler;
            GameManager.Instance.SceneLoader.LoadLevel(SceneLoader.FORGE_SCENE_NAME, true);  
        }

        private void ButtonForge_ForgeLoadedHandler()
        {
            GameManager.Instance.SceneLoader.OnSceneLoadingComplete -= ButtonForge_ForgeLoadedHandler;

            ForgeSceneManager.Instance.OnSceneClosed += ForgeClosedHandler;
        }

        private void ForgeClosedHandler()
        {
            //Enable all UI widgets
            UIView.SetWidgetsActive(true, true);

            //Enable all widgets input
            UIView.LockInput(false);  
        }

        #endregion
    }
}
