using RhytmFighter.Persistant.Converters;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RhytmFighter.Data
{
    /// <summary>
    /// Level data
    /// </summary>
    public class LevelsData : AbstractData<LevelsData>
    {
        public LevelParams[] LevelParamsData;

        private Dictionary<int, LevelParams> m_LevelParams;

        /// <summary>
        /// Convert data array from data base to dictionary
        /// </summary>
        public void ReorginizeData()
        {
            m_LevelParams = new Dictionary<int, LevelParams>();
            for (int i = 0; i < LevelParamsData.Length; i++)
            {
                if (!m_LevelParams.ContainsKey(LevelParamsData[i].ID))
                    m_LevelParams.Add(LevelParamsData[i].ID, LevelParamsData[i]);
            }

            LevelParamsData = null;
        }

        /// <summary>
        /// Get level data by ID
        /// </summary>
        public LevelParams GetLevelParams(int levelID)
        {
            if (m_LevelParams.ContainsKey(levelID))
                return m_LevelParams[levelID];

            return null;
        }


        [Serializable]
        public class LevelParams
        {
            public int ID = 1;
            public int BPM = 130;

            public BuildData BuildParams;
            public EnviromentData EnviromentParams;
            public ContentData ContentParams;
        }

        [Serializable]
        public class BuildData
        {
            public int Seed => OverrideSeed? LevelSeed : (int)ConvertersCollection.ConvertToUnixTimestamp(DateTime.Now);

            public bool OverrideSeed;
            public int LevelSeed;
            public int LevelDepth;
            public int MinWidth;
            public int MaxWidth;
            public int MinHeight;
            public int MaxHeight;
            public float CellSize;
            public int ObstacleFillPercent;
        }

        [Serializable]
        public class EnviromentData
        {
            public int EnviromentFillPercent = 20;
        }

        [Serializable]
        public class ContentData
        {
            public AnimationCurve ProgressionCurve;
            public int MinAmountOfItems;
            public int MaxAmountOfItems;
            public int[] AvailableItemsIDs;

            public int MinAmountOfEnemies;
            public int MaxAmountOfEnemies;
            public int[] AvailableEnemyViewIDs;

            public int MinEnemyHP;
            public int MaxEnemyHP;
            public int MinEnemyDmg;
            public int MaxEnemyDmg;

            public int MinBossHP;
            public int MaxBossHP;
            public int MinBossDmg;
            public int MaxBossDmg;
        }
    }
}
