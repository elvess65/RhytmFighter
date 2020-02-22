using UnityEngine;

namespace RhytmFighter.Data.DataBase.Simulation
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Simulation_LevelsData", menuName = "DBSimulation/LevelsData", order = 101)]
    public class DBSimulation_LevelsData : ScriptableObject
    {
        public int LevelDepth;
    }
}
