using UnityEngine;

namespace RhytmFighter.Data.DataBase.Simulation
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Simulation_LevelsData", menuName = "DBSimulation/LevelsData", order = 101)]
    public class DBSimulation_LevelsData : ScriptableObject
    {
        public LevelParams[] LevelParamsData;

        [System.Serializable]
        public class LevelParams
        {
            public int ID = 1;
            public int LevelDepth = 4;
            public int LevelSeed = 10;
            public int MinWidth = 3;
            public int MaxWidth = 5;
            public int MinHeight = 4;
            public int Maxheight = 7;
            public float CellSize = 1;
            [Range(0, 100)]
            public int FillPercent = 90;
            [Range(0, 100)]
            public int EnviromentFillPercent = 20;
            public int BPM = 130;
        }
    }
}
