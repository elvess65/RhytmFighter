using RhytmFighter.Animation;
using RhytmFighter.Persistant;
using RhytmFighter.Persistant.Abstract;
using RhytmFighter.UI.View;
using UnityEngine;

namespace RhytmFighter.OtherScenes.MenuScene
{
    public class MenuSceneManager : Singleton<MenuSceneManager>
    {
        [SerializeField] private UIView_MenuScene UIView_MenuScene;
        [SerializeField] private CharactersController CharacterController;
        [SerializeField] private CameraContoller CameraContoller;


        public void Initialize()
        {
            UIView_MenuScene.OnPlayPressHandler += UIView_MenuScene_Play_PressHandler;
            UIView_MenuScene.Initialize();

            CharacterController.Initialize();

            CameraContoller.OnCameraPushKeyFrameReached += CameraPushingFinishedHandler;
            CameraContoller.Initialize();
        }


        private void Update()
        {
            UIView_MenuScene.PerformUpdate(Time.deltaTime);
            CameraContoller.PerformUpdate(Time.deltaTime);
        }

        private void UIView_MenuScene_Play_PressHandler()
        {
            GameManager.Instance.DataHolder.BattleSessionModel.CurrentLevelID = 1;
            GameManager.Instance.DataHolder.BattleSessionModel.SelectedCharactedID = 1;

            //Camera is pushed by animation event

            //Disable all UI widgets
            UIView_MenuScene.SetWidgetsActive(false, true);

            //Subscribe for animation event
            AnimationEventsListener animationEventsListener = CharacterController.SelectedCharacterAnimationController.Controller.GetComponent<AnimationEventsListener>();
            animationEventsListener.OnOtherEvent += CameraContoller.PushCamera;

            //Play animation
            CharacterController.SelectedCharacterAnimationController.PlayAnimation(Persistant.Enums.AnimationTypes.MenuAction);
        }

        private void CameraPushingFinishedHandler()
        {
            GameManager.Instance.OnSceneUnloadingComplete += MenuSceneUnloadedHandler;
            GameManager.Instance.UnloadLevel(GameManager.MENU_SCENE_NAME);
        }

        private void MenuSceneUnloadedHandler()
        {
            GameManager.Instance.OnSceneUnloadingComplete -= MenuSceneUnloadedHandler;
            GameManager.Instance.LoadLevel(GameManager.BATTLE_SCENE_NAME);
        }
    }
}
