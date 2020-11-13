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
        public SceneLoader SceneLoader { get; private set; }


        public void InitializeConnection()
        {
            DataHolder.DBProxy.OnConnectionSuccess += ConnectionResultSuccess;
            DataHolder.DBProxy.OnConnectionError += ConnectionResultError;
            DataHolder.DBProxy.Initialize();
        }


        private void InitializeCore()
        {
            DataHolder = new ModelsHolder();
            SceneLoader = new SceneLoader();
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            
            InitializeCore();
        }
 

        private void ConnectionResultSuccess(string serializedAccountData, string serializedEnviromentData, string serializedLevelingData)
        {
            //Set data
            DataHolder.AccountModel = AccountModel.DeserializeData(serializedAccountData);
            DataHolder.AccountModel.ReorganizeData();

            DataHolder.DataTableModel = new DataTableModel(serializedEnviromentData, serializedLevelingData);
            DataHolder.DataTableModel.ReorganizeData();

            SceneLoader.LoadLevel(SceneLoader.MENU_SCENE_NAME);
        }

        private void ConnectionResultError(int errorCode) => Debug.LogError($"Connection error {errorCode}");

    }
}
