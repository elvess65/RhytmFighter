using RhytmFighter.Data;
using UnityEngine;

namespace RhytmFighter.Main
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager m_Instance;
        public static GameManager Instance => m_Instance;

        [Header("Links")]
        public ManagersHolder ManagerHolder;

        private DataHolder m_DataHolder;
        private ControllersHolder m_ControllersHolder;


        private void Awake()
        {
            if (m_Instance != null)
                Destroy(gameObject);

            m_Instance = this;
        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            //Initialize objects
            m_DataHolder = new DataHolder();
            m_ControllersHolder = new ControllersHolder();

            //Initialize connection
            m_DataHolder.DBProxy.OnConnectionSuccess += ConnectionResultSuccess;
            m_DataHolder.DBProxy.OnConnectionError += ConnectionResultError;
            m_DataHolder.DBProxy.Initialize();
        }

        private void ConnectionResultSuccess(string serializedInfoData, string serializedPlayerData, string levelsData)
        {
            //Set data
            m_DataHolder.InfoData = InfoData.DeserializeData(serializedInfoData);
            m_DataHolder.PlayerData = PlayerData.DeserializeData(serializedPlayerData);
            m_DataHolder.LevelsData = LevelsData.DeserializeData(levelsData);

            Debug.Log("Connection success");

            //Build level
            m_ControllersHolder.LevelController.GenerateLevel(m_DataHolder.LevelsData.LevelDepth);
        }

        private void ConnectionResultError(int errorCode) => Debug.LogError($"Connection error {errorCode}");
    }
}
