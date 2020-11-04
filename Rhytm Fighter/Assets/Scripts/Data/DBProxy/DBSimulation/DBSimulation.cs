using UnityEngine;

namespace RhytmFighter.Data.DataBase.Simulation
{
    /// <summary>
    /// Local data base 
    /// </summary>
    [System.Serializable]
    public class DBSimulation : MonoBehaviour
    {
        public DBSimulation_PlayerData PlayerData;
        public DBSimulation_LevelsData LevelsData;
        public DBSimulation_LevelsExpData LevelsExpData;
    }
}
