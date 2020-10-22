using RhytmFighter.Persistant;
using RhytmFighter.Persistant.Abstract;
using RhytmFighter.UI.View;
using UnityEngine;

namespace RhytmFighter.OtherScenes.MenuScene
{
    public class MenuSceneManager : Singleton<MenuSceneManager>
    {
        public UIView_MenuScene UIView_MenuScene;

        void Update()
        {
            UIView_MenuScene.PerformUpdate(Time.deltaTime);
        }

        public void Initialize()
        {
            UIView_MenuScene.OnPlayPressHandler += UIView_MenuScene_Play_PressHandler;
            UIView_MenuScene.Initialize();
        }

        void UIView_MenuScene_Play_PressHandler()
        {
            GameManager.Instance.OnSceneUnloadingComplete += () => GameManager.Instance.LoadLevel(GameManager.BATTLE_SCENE_NAME);
            GameManager.Instance.UnloadLevel(GameManager.MENU_SCENE_NAME);
        }
    }
}
