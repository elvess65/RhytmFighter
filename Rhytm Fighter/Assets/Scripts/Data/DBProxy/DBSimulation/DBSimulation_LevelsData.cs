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

            [Header("Level")]
            public float CellSize = 1;
            [Range(0, 100)] public int ObstacleFillPercent = 50;

            public LevelProgressionConfig LevelProgressionConfig;
        }

        [System.Serializable]
        public class EnviromentData
        {
            public float CellOffset = 1;
            public float ElevationOffset = 0.2f;

            [Header("Obstacles")]
            [Range(0, 100)] public int ObstaclesHolesPercent = 20;

            [Header("Normal")]
            [Range(0, 100)] public int EnviromentDecortionPercent = 20;
            [Range(0, 100)] public int EnviromentElevatedPercent = 20;
            [Range(0, 100)] public int EnviromentLightPercent = 20;
            [Range(0, 100)] public int EnviromentDarkPercent = 20;

        }

        [System.Serializable]
        public class ContentData
        {
            [Header("Items")]
            public ObjectProgressionConfig ItemProgressionConfig;

            [Header("Enemy View")]
            public ObjectProgressionConfig EnemyViewProgressionConfig;

            [Header("Enemy Data")]
            public NPCProgressionConfig EnemyDataProgressionConfig;
            public NPCProgressionConfig BossDataProgressionConfig;
        }
    }
}
