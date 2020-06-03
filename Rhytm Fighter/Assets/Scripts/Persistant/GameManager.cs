using System.Collections.Generic;
using RhytmFighter.Battle.Core;
using RhytmFighter.Data;
using RhytmFighter.Persistant.Abstract;
using RhytmFighter.Persistant.SceneLoading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RhytmFighter.Persistant
{
    public class GameManager : Singleton<GameManager>
    {
        public DataHolder DataHolder { get; private set; }

        private string m_CurrentLevelName = string.Empty;
        private System.Action m_OnSceneLoadingComplete;
        private List<AsyncOperation> m_SceneLoadingOperations;

        private const string m_BATTLE_SCENE_NAME = "BattleScene";
        private const string m_TRANSITION_SCENE_NAME = "TransitionScene";


        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            
            InitializeCore();

            m_OnSceneLoadingComplete += SceneLoadCompleteHandler;
            LoadLevel(m_TRANSITION_SCENE_NAME);
        }

        private void InitializeCore()
        {
            DataHolder = new DataHolder();
            m_SceneLoadingOperations = new List<AsyncOperation>();
        }

        private void InitializeConnection()
        {
            DataHolder.DBProxy.OnConnectionSuccess += ConnectionResultSuccess;
            DataHolder.DBProxy.OnConnectionError += ConnectionResultError;
            DataHolder.DBProxy.Initialize();
        }


        private void ConnectionResultSuccess(string serializedPlayerData, string serializedLevelsData)
        {
            //Set data
            DataHolder.PlayerDataModel = PlayerData.DeserializeData(serializedPlayerData);
            DataHolder.InfoData = new InfoData(serializedLevelsData);

            LoadLevel(m_BATTLE_SCENE_NAME);
        }

        private void ConnectionResultError(int errorCode) => Debug.LogError($"Connection error {errorCode}");

        #region SceneLoading
        public void LoadNextLevel()
        {
            UnloadLevel(m_BATTLE_SCENE_NAME);
            LoadLevel(m_BATTLE_SCENE_NAME);
        }

        public void LoadLevel(string levelName)
        {
            m_CurrentLevelName = levelName;

            if (SceneLoadingManager.Instance != null)
            {
                SceneLoadingManager.Instance.OnFadeIn += FadeInFinishedOnLoadHandler;
                SceneLoadingManager.Instance.FadeIn();
            }
            else 
                Load(levelName);
        }

        public void UnloadLevel(string levelName)
        {
            AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(levelName);
            if (asyncOperation != null)
            {
                asyncOperation.completed += UnloadOperationComplete;
            }
            else
                Debug.LogError($"Unable to unload level {levelName}");
        }


        private void Load(string levelName)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
            if (asyncOperation != null)
            {
                m_SceneLoadingOperations.Add(asyncOperation);
                asyncOperation.completed += LoadOperationComplete;
            }
            else
                Debug.LogError($"Unable to load level {levelName}");
        }

        private void FadeInFinishedOnLoadHandler()
        {
            SceneLoadingManager.Instance.OnFadeIn -= FadeInFinishedOnLoadHandler;
            Load(m_CurrentLevelName);
        }

        private void SceneLoadCompleteHandler()
        {
            switch (m_CurrentLevelName)
            {
                case m_BATTLE_SCENE_NAME:
                    SceneLoadingManager.Instance.FadeOut();
                    BattleManager.Instance.Initialize();
                    break;
                case m_TRANSITION_SCENE_NAME:
                    InitializeConnection();
                    break;
            }
        }

        
        private void LoadOperationComplete(AsyncOperation asyncOperation)
        {
            if (m_SceneLoadingOperations.Contains(asyncOperation))
                m_SceneLoadingOperations.Remove(asyncOperation);

            m_OnSceneLoadingComplete?.Invoke();
        }

        private void UnloadOperationComplete(AsyncOperation asyncOperation)
        {
        }
        #endregion

    }
}
