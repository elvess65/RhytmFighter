using UnityEngine;

namespace RhytmFighter.Data.DataBase.Simulation
{
    /// <summary>
    /// Local data base 
    /// </summary>
    [System.Serializable]
    public class DBSimulation : MonoBehaviour
    {
        public DBSimulation_AccountData AccountData;
        public DBSimulation_EnvironmentData EnvironmentData;
        public DBSimulation_LevelingData LevelingData;
    }
}
