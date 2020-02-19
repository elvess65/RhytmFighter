using RhytmFighter.Data;
using RhytmFighter.Level;
using UnityEngine;

namespace RhytmFighter.Main
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager m_Instance;
        public static GameManager Instance => m_Instance;

        [Header("Links")]
        public DataHolder DataHolders;

        private LevelController m_LevelController;


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
            m_LevelController = new LevelController();

            //Initialize connection
            DataHolders.DBProxy.OnConnectionSuccess += ConnectionResultSuccess;
            DataHolders.DBProxy.OnConnectionError += ConnectionResultError;
            DataHolders.DBProxy.Initialize();
        }

        private void ConnectionResultSuccess(string serializedInfoData, string serializedPlayerData, string levelsData)
        {
            //Set data
            DataHolders.InfoData = InfoData.DeserializeData(serializedInfoData);
            DataHolders.PlayerData = PlayerData.DeserializeData(serializedPlayerData);
            DataHolders.LevelsData = LevelsData.DeserializeData(levelsData);

            Debug.Log("Connection success");

            m_LevelController.GenerateLevel(DataHolders.LevelsData.LevelDepth);
        }

        private void ConnectionResultError(int errorCode) => Debug.LogError($"Connection error {errorCode}");
    }
}
