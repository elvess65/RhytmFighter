using UnityEngine;

namespace RhytmFighter.Data.DataBase.Simulation
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Simulation_LevelsExpData", menuName = "DBSimulation/LevelsExpData", order = 101)]
    public class DBSimulation_LevelsExpData : ScriptableObject
    {
        public LevelExpProgressionConfig WeaponLevelExpProgressionConfig;
    }
}
