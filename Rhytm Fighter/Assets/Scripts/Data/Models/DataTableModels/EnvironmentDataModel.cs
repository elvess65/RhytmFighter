using RhytmFighter.Persistant.Converters;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RhytmFighter.Data.Models.DataTableModels
{
    /// <summary>
    /// Информация о построении уровней
    /// </summary>
    public class EnvironmentDataModel : DeserializableDataModel<EnvironmentDataModel>
    {
        public LevelParams[] LevelParamsData;

        private Dictionary<int, LevelParams> m_LevelParams;

        
        public override void ReorganizeData()
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

        /// <summary>
        /// Получить прогресс прохождения уровней для рассчета прогрессий
        /// </summary>
        /// <param name="completedLevelIDs"></param>
        /// <returns></returns>
        public float GetCompletionForProgression(int[] completedLevelIDs)
        {
            int completedLevels = 0;
 
            foreach(LevelParams levelParam in m_LevelParams.Values)
            {
                if (IsLevelComplete(completedLevelIDs, levelParam.ID))
                    completedLevels++;
            }

            return (float)completedLevels / Mathf.Clamp(m_LevelParams.Count - 1, 1, m_LevelParams.Count); 
        }


        private bool IsLevelComplete(int[] completedLevelIDs, int levelID)
        {
            for (int i = 0; i < completedLevelIDs.Length; i++)
            {
                if (completedLevelIDs[i] == levelID)
                    return true;
            }

            return false;
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
            public int ObstacleFillPercent;
            public float CellSize;
            public LevelSizeProgressionConfig LevelProgressionConfig;
        }

        [Serializable]
        public class EnviromentData
        {
            public float CellOffset = 1;
            public float ElevationOffset = 0.2f;
            public int EnviromentDecortionPercent = 20;
            public int ObstaclesHolesPercent = 20;
            public int EnviromentElevatedPercent = 20;
            public int EnviromentLightPercent = 20;
            public int EnviromentDarkPercent = 20;
        }

        [Serializable]
        public class ContentData
        {
            public ObjectProgressionConfig ItemProgressionConfig;
            public ObjectProgressionConfig EnemyViewProgressionConfig;
            public NPCProgressionConfig EnemyDataProgressionConfig;
            public NPCProgressionConfig BossDataProgressionConfig;
        }
    }
}
