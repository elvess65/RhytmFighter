using RhytmFighter.Settings.Proxy;

namespace RhytmFighter.Data.DataBase
{
    /// <summary>
    /// Data base connection proxy
    /// </summary>
    public class DBProxy
    {
        public System.Action<string, string> OnConnectionSuccess;
        public System.Action<int> OnConnectionError;

        private iDataProvider m_DataProvider;

        
        public void Initialize(ProxySettings settings)
        {
            if (settings.UseSimulation)
            {
                ProxySettings_Simulation simulationSettings = settings as ProxySettings_Simulation;
                m_DataProvider = new SimulationDataProvider(simulationSettings.SimulateSuccess);
            }
            else
            {
                ProxySettings_RemoteExample remoteSettings = settings as ProxySettings_RemoteExample;
                UnityEngine.Debug.Log("Connecting to remote IP: " + remoteSettings.IP);
                OnConnectionErrorHandler(1);
            }

            m_DataProvider.OnConnectionSuccess += ConnectionSuccessHandler;
            m_DataProvider.OnConnectionError += OnConnectionErrorHandler;
            m_DataProvider.Connect();
        }


        private void ConnectionSuccessHandler(string serializedPlayerData, string serializedLevelsData) => OnConnectionSuccess?.Invoke(serializedPlayerData, serializedLevelsData);

        private void OnConnectionErrorHandler(int errorCode) => OnConnectionError?.Invoke(errorCode);
    }
}
