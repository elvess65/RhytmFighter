using RhytmFighter.Data;
using UnityEngine;

namespace RhytmFighter.Main
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager m_Instance;
        public static GameManager Instance => m_Instance;

        [Header("Links")]
        public DataHolders DataHolders;


        private void Awake()
        {
            if (m_Instance != null)
                Destroy(gameObject);

            m_Instance = this;
        }

        private void Start()
        {
            DataHolders.DBProxy.OnConnectionSuccess += ConnectionResultSuccess;
            DataHolders.DBProxy.OnConnectionError += ConnectionResultError;
            DataHolders.DBProxy.Initialize();
        }

        private void ConnectionResultSuccess(string serializedInfoData, string serializedPlayerData)
        {
            DataHolders.InfoData = InfoData.DeserializeData(serializedInfoData);
            DataHolders.PlayerData = PlayerData.DeserializeData(serializedPlayerData);

            Debug.Log("Connection success");
        }

        private void ConnectionResultError(int errorCode) => Debug.LogError($"Connection error {errorCode}");
    }
}
