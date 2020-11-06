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
            bool useSimulation = true;
            if (useSimulation)
            {
                bool simulateError = false;
                m_DataProvider = new SimulationDataProvider(!simulateError);
            }
            else
            {
                UnityEngine.Debug.Log("Connecting to remote IP");
                OnConnectionErrorHandler(1);
            }

            m_DataProvider.OnConnectionSuccess += ConnectionSuccessHandler;
            m_DataProvider.OnConnectionError += OnConnectionErrorHandler;
            m_DataProvider.Connect();
        }


        private void ConnectionSuccessHandler(string serializedAccountData, string serializedEnviromentData, string serializedLevelsExpData) =>
            OnConnectionSuccess?.Invoke(serializedAccountData, serializedEnviromentData, serializedLevelsExpData);

        private void OnConnectionErrorHandler(int errorCode) => OnConnectionError?.Invoke(errorCode);
    }
}
