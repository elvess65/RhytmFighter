using RhytmFighter.Battle.Core;
using RhytmFighter.Data;
using RhytmFighter.Data.Models;
using RhytmFighter.OtherScenes.MenuScene;
using RhytmFighter.Persistant.Abstract;
using RhytmFighter.Persistant.SceneLoading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RhytmFighter.Persistant
{
    public class GameManager : Singleton<GameManager>
    {
        public ModelsHolder DataHolder { get; private set; }

        public System.Action OnSceneLoadingComplete;
        public System.Action OnSceneUnloadingComplete;

        private string m_CurrentLoadingLevel = string.Empty;
        private string m_CurrentUnloadingLevel = string.Empty;
        

        public const string BATTLE_SCENE_NAME = "BattleScene";
        public const string MENU_SCENE_NAME = "MenuScene";

        private const string m_BOOT_SCENE_NAME = "BootScene";
        private const string m_TRANSITION_SCENE_NAME = "TransitionScene";


        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            
            InitializeCore();

            OnSceneLoadingComplete += SceneLoadCompleteHandler;

            LoadLevel(m_TRANSITION_SCENE_NAME);
        }

        private void InitializeCore()
        {
            DataHolder = new ModelsHolder();
        }

        private void InitializeConnection()
        {
            DataHolder.DBProxy.OnConnectionSuccess += ConnectionResultSuccess;
            DataHolder.DBProxy.OnConnectionError += ConnectionResultError;
            DataHolder.DBProxy.Initialize();
        }


        private void ConnectionResultSuccess(string serializedAccountData, string serializedEnviromentData, string serializedLevelingData)
        {
            //Set data
            DataHolder.AccountModel = AccountModel.DeserializeData(serializedAccountData);
            DataHolder.AccountModel.ReorganizeData();

            DataHolder.DataTableModel = new DataTableModel(serializedEnviromentData, serializedLevelingData);
            DataHolder.DataTableModel.ReorganizeData();

            LoadLevel(MENU_SCENE_NAME);
        }

        private void ConnectionResultError(int errorCode) => Debug.LogError($"Connection error {errorCode}");

        #region SceneLoading

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
                asyncOperation.completed += UnloadOperationComplete;
            else
                Debug.LogError($"Unable to unload level {levelName}");
        }

        private void UnloadOperationComplete(AsyncOperation asyncOperation)
        {
            OnSceneUnloadingComplete?.Invoke();
        }

        private void FadeInFinishedOnUnloadHandler()
        {
            SceneLoadingManager.Instance.OnFadeIn -= FadeInFinishedOnUnloadHandler;

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(m_BOOT_SCENE_NAME));
            Unload(m_CurrentUnloadingLevel);
        }

        #endregion
    }
}
