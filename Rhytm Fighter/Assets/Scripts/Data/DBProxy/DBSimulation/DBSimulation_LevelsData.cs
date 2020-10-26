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
            public int MinAmountOfItems = 1;
            public int MaxAmountOfItems = 4;
            public int[] AvailableItemsIDs = new int[] { 1, 2 };

            [Header("Enemies")]
            public ObjectProgressionConfig EnemyViewProgressionConfig;
            public int MinAmountOfEnemies = 1;
            public int MaxAmountOfEnemies = 2;
            public int[] AvailableEnemyViewIDs = new int[] { 1, 2 };

            [Header(" - Enemy progression")]
            public NPCProgressionConfig EnemyProgressionConfig;
            public NPCProgressionConfig BossProgressionConfig;
        }
    }
}
