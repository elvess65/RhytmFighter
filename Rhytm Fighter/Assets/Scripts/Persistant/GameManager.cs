using System.Collections.Generic;
using RhytmFighter.Battle.Core;
using RhytmFighter.Data;
using RhytmFighter.Persistant.Abstract;
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


        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            InitializeCore();
            InitializeConnection();
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

            m_OnSceneLoadingComplete += SceneLoadingComplete;
            LoadLevel(m_BATTLE_SCENE_NAME);
        }

        private void ConnectionResultError(int errorCode) => Debug.LogError($"Connection error {errorCode}");


        private void SceneLoadingComplete()
        {
            m_OnSceneLoadingComplete -= SceneLoadingComplete;

            switch (m_CurrentLevelName)
            {
                case m_BATTLE_SCENE_NAME:
                    BattleManager.Instance.Initialize();
                    break;
            }
        }


        #region SceneLoading
        public void LoadLevel(string levelName)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
            if (asyncOperation != null)
            {
                m_SceneLoadingOperations.Add(asyncOperation);
                asyncOperation.completed += LoadOperationComplete;

                m_CurrentLevelName = levelName;
            }
            else
                Debug.LogError($"Unable to load level {levelName}");
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

        private void LoadOperationComplete(AsyncOperation asyncOperation)
        {
            if (m_SceneLoadingOperations.Contains(asyncOperation))
                m_SceneLoadingOperations.Remove(asyncOperation);

            Debug.Log("Load complete");
            m_OnSceneLoadingComplete?.Invoke();
        }

        private void UnloadOperationComplete(AsyncOperation asyncOperation)
        {
            Debug.Log("Unload complete");
        }
        #endregion

    }
}
