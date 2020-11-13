using System.Collections;
using System.Collections.Generic;
using RhytmFighter.Battle.Core;
using RhytmFighter.OtherScenes.MenuScene;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RhytmFighter.Persistant.SceneLoading
{
    public class SceneLoader
    {
        public System.Action OnSceneLoadingComplete;
        public System.Action OnSceneUnloadingComplete;

        private string m_CurrentLoadingLevel = string.Empty;
        private string m_CurrentUnloadingLevel = string.Empty;

        public const string BATTLE_SCENE_NAME = "BattleScene";
        public const string MENU_SCENE_NAME = "MenuScene";
        public const string FORGE_SCENE_NAME = "ForgeScene";

        private const string m_TRANSITION_SCENE_NAME = "TransitionScene";
        private const string m_BOOT_SCENE_NAME = "BootScene";


        public SceneLoader()
        {
            OnSceneLoadingComplete += SceneLoadCompleteHandler;

            LoadLevel(m_TRANSITION_SCENE_NAME);
        }


        public void LoadLevel(string levelName, bool noFade = false)
        {
            m_CurrentLoadingLevel = levelName;

            if (!noFade && SceneLoadingManager.Instance != null)
            {
                SceneLoadingManager.Instance.OnFadeIn += FadeInFinishedOnLoadHandler;
                SceneLoadingManager.Instance.FadeIn();
            }
            else
                Load(levelName);
        }

        private void FadeInFinishedOnLoadHandler()
        {
            SceneLoadingManager.Instance.OnFadeIn -= FadeInFinishedOnLoadHandler;
            Load(m_CurrentLoadingLevel);
        }

        private void Load(string levelName)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
            if (asyncOperation != null)
                asyncOperation.completed += LoadOperationComplete;
            else
                Debug.LogError($"Unable to load level {levelName}");
        }

        private void LoadOperationComplete(AsyncOperation asyncOperation)
        {
            OnSceneLoadingComplete?.Invoke();
        }

        private void SceneLoadCompleteHandler()
        {
            switch (m_CurrentLoadingLevel)
            {
                case MENU_SCENE_NAME:
                    SceneManager.SetActiveScene(SceneManager.GetSceneByName(MENU_SCENE_NAME));
                    SceneLoadingManager.Instance.FadeOut();
                    MenuSceneManager.Instance.Initialize();
                    break;
                case BATTLE_SCENE_NAME:
                    SceneManager.SetActiveScene(SceneManager.GetSceneByName(BATTLE_SCENE_NAME));
                    SceneLoadingManager.Instance.FadeOut();
                    BattleManager.Instance.Initialize();
                    break;
                case FORGE_SCENE_NAME:
                    SceneManager.SetActiveScene(SceneManager.GetSceneByName(FORGE_SCENE_NAME));
                    SceneLoadingManager.Instance.FadeOut();
                    ForgeSceneManager.Instance.Initialize();
                    break;
                case m_TRANSITION_SCENE_NAME:
                    GameManager.Instance.InitializeConnection();
                    break;
            }
        }


        public void UnloadLevel(string levelName, bool noFade = false)
        {
            m_CurrentUnloadingLevel = levelName;

            if (!noFade)
            {
                SceneLoadingManager.Instance.OnFadeIn += FadeInFinishedOnUnloadHandler;
                SceneLoadingManager.Instance.FadeIn();
            }
            else
            {
                ExecuteSceneUnloadTransition();
            }
        }

        private void FadeInFinishedOnUnloadHandler()
        {
            SceneLoadingManager.Instance.OnFadeIn -= FadeInFinishedOnUnloadHandler;
            ExecuteSceneUnloadTransition();
        }

        private void ExecuteSceneUnloadTransition()
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(m_BOOT_SCENE_NAME));
            Unload(m_CurrentUnloadingLevel);
        }

        private void Unload(string levelName)
        {
            AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(levelName);
            if (asyncOperation != null)
                asyncOperation.completed += UnloadOperationComplete;
            else
                Debug.LogError($"Unable to unload level {levelName}");
        }

        private void UnloadOperationComplete(AsyncOperation asyncOperation)
        {
            OnSceneUnloadingComplete?.Invoke();
        }
    }
}
