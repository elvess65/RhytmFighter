using RhytmFighter.Data.DataBase.Simulation;
using System;
using System.Collections;
using UnityEngine;

namespace RhytmFighter.Data.DataBase
{
    /// <summary>
    /// Data base provider (simulation, GPServices, server)
    /// </summary>
    interface iDataProvider
    {
        event Action OnConnectionSuccess;
        event Action<int> OnConnectionError;

        void Connect();
        string GetPlayerData();
        string GetInfoData();
    }

    /// <summary>
    /// Local data base simulation provider
    /// </summary>
    class SimulationDataProvider : iDataProvider
    {
        public event Action OnConnectionSuccess;
        public event Action<int> OnConnectionError;

        private bool m_SimulateSuccessConnection;
        private DBSimulation m_DataObject;
        private WaitForSeconds m_WaitConnectionDelay;

        private const float m_CONNECTION_TIME = 1;


        public SimulationDataProvider(bool simulateSuccessConnection)
        {
            m_DataObject = GameObject.FindObjectOfType<DBSimulation>();
            m_WaitConnectionDelay = new WaitForSeconds(m_CONNECTION_TIME);
            m_SimulateSuccessConnection = simulateSuccessConnection;
        }

        public void Connect()
        {
            Debug.Log("Start connection");

            if (m_SimulateSuccessConnection)
                Main.GameManager.Instance.StartCoroutine(SimulateSuccessConnectionDelay());
            else 
                Main.GameManager.Instance.StartCoroutine(SimulateErrorConnectionDelay(100));
        }

        public string GetPlayerData() => JsonUtility.ToJson(m_DataObject.PlayerData);

        public string GetInfoData() => JsonUtility.ToJson(m_DataObject.InfoData);


        IEnumerator SimulateSuccessConnectionDelay()
        {
            yield return m_WaitConnectionDelay;

            OnConnectionSuccess?.Invoke();
        }

        IEnumerator SimulateErrorConnectionDelay(int errorCode)
        {
            yield return m_WaitConnectionDelay;

            OnConnectionError?.Invoke(errorCode);
        }
    }
}

