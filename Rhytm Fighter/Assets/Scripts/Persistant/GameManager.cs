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

        private string m_CurrentLoadingLevel = string.Empty;
        private string m_CurrentUnloadingLevel = string.Empty;
        private System.Action m_OnSceneLoadingComplete;
        private System.Action m_OnSceneUnloadingComplete;

        private const string m_BOOT_SCENE_NAME = "BootScene";
        private const string m_BATTLE_SCENE_NAME = "BattleScene";
        private const string m_TRANSITION_SCENE_NAME = "TransitionScene";


        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            
            InitializeCore();

            m_OnSceneLoadingComplete += SceneLoadCompleteHandler;
            m_OnSceneUnloadingComplete += SceneUnloadCompleteHandler;
            LoadLevel(m_TRANSITION_SCENE_NAME);
        }

        private void InitializeCore()
        {
            DataHolder = new DataHolder();
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
        public void ReloadBattleLevel()
        {
            UnloadLevel(m_BATTLE_SCENE_NAME);
        }


        public void LoadLevel(string levelName)
        {
            m_CurrentLoadingLevel = levelName;

            if (SceneLoadingManager.Instance != null)
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
            m_OnSceneLoadingComplete?.Invoke();
        }

        private void SceneLoadCompleteHandler()
        {
            switch (m_CurrentLoadingLevel)
            {
                case m_BATTLE_SCENE_NAME:
                    SceneManager.SetActiveScene(SceneManager.GetSceneByName(m_BATTLE_SCENE_NAME));
                    SceneLoadingManager.Instance.FadeOut();
                    BattleManager.Instance.Initialize();
                    break;
                case m_TRANSITION_SCENE_NAME:
                    InitializeConnection();
                    break;
            }
        }


        public void UnloadLevel(string levelName)
        {
            m_CurrentUnloadingLevel = levelName;

            SceneLoadingManager.Instance.OnFadeIn += FadeInFinishedOnUnloadHandler;
            SceneLoadingManager.Instance.FadeIn();
        }

        private void Unload(string levelName)
        {
            AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(levelName);
            if (asyncOperation != null)
            {
                asyncOperation.completed += UnloadOperationComplete;
            }
            else
                Debug.LogError($"Unable to unload level {levelName}");
        }

        private void UnloadOperationComplete(AsyncOperation asyncOperation)
        {
            m_OnSceneUnloadingComplete?.Invoke();
        }

        private void SceneUnloadCompleteHandler()
        {
            switch(m_CurrentUnloadingLevel)
            {
                case m_BATTLE_SCENE_NAME:
                    LoadLevel(m_BATTLE_SCENE_NAME);
                    break;
            }
        }

        private void FadeInFinishedOnUnloadHandler()
        {
            SceneLoadingManager.Instance.OnFadeIn -= FadeInFinishedOnUnloadHandler;

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(m_BOOT_SCENE_NAME));
            Unload(m_BATTLE_SCENE_NAME);
        }
        #endregion
    }
}
