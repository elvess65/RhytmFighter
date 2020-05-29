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
            [Header("General")]
            public int ID = 1;
            public int BPM = 130;

            [Header("BuildParams")]
            public BuildData BuildParams;

            [Header("EnviromentParams")]
            public EnviromentData EnviromentParams;

            [Header("ContentParams")]
            public ContentData ContentParams;
        }

        [System.Serializable]
        public class BuildData
        {
            [Header("Seed")]
            public bool OverrideSeed = true;
            public int LevelSeed = 10;

            [Space(10)]
            public int LevelDepth = 4;
            public int MinWidth = 3;
            public int MaxWidth = 5;
            public int MinHeight = 4;
            public int MaxHeight = 7;
            public float CellSize = 1;
            [Range(0, 100)] public int ObstacleFillPercent = 50;
        }

        [System.Serializable]
        public class EnviromentData
        {
            [Range(0, 100)] public int EnviromentFillPercent = 20;
        }

        [System.Serializable]
        public class ContentData
        {
            public AnimationCurve ProgressionCurve;
        }
    }
}
