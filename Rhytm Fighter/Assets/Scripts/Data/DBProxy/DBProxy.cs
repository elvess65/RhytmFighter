using RhytmFighter.Main;

namespace RhytmFighter.Data.DataBase
{
    /// <summary>
    /// Data base connection proxy
    /// </summary>
    public class DBProxy
    {
        public System.Action<string, string, string> OnConnectionSuccess;
        public System.Action<int> OnConnectionError;

        private iDataProvider m_DataProvider;


        public void Initialize()
        {
            if (GameManager.Instance.ManagerHolder.SettingsManager.SimulationSettings.UseSimulation)
                m_DataProvider = new SimulationDataProvider(GameManager.Instance.ManagerHolder.SettingsManager.SimulationSettings.SimulateSuccess);

            m_DataProvider.OnConnectionSuccess += ConnectionSuccessHandler;
            m_DataProvider.OnConnectionError += OnConnectionErrorHandler;
            m_DataProvider.Connect();
        }


        private void ConnectionSuccessHandler() => OnConnectionSuccess?.Invoke(m_DataProvider.GetInfoData(), 
                                                                               m_DataProvider.GetPlayerData(),
                                                                               m_DataProvider.GetLevelsData());

        private void OnConnectionErrorHandler(int errorCode) => OnConnectionError?.Invoke(errorCode);
    }
}
