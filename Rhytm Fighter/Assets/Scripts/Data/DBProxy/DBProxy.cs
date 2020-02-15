namespace RhytmFighter.Data.DataBase
{
    /// <summary>
    /// Data base connection proxy
    /// </summary>
    [System.Serializable]
    public class DBProxy
    {
        public System.Action<string, string, string> OnConnectionSuccess;
        public System.Action<int> OnConnectionError;

        public bool UseSimulation = true;
        public bool SimulateSuccess = true;

        private iDataProvider m_DataProvider;


        public void Initialize()
        {
            if (UseSimulation)
                m_DataProvider = new SimulationDataProvider(SimulateSuccess);

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
