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


        [System.Serializable]
        public class LevelParams
        {
            public int ID = 1;
            public int BPM = 130;

            public BuildData BuildParams;
            public EnviromentData EnviromentParams;
            public ContentData ContentParams;
        }

        [System.Serializable]
        public class BuildData
        {
            public bool OverrideSeed = true;
            public int Seed => OverrideSeed ? LevelSeed : (int)ConvertToUnixTimestamp(DateTime.Now);

            public int LevelSeed = 10;

            public int LevelDepth = 4;
            public int MinWidth = 3;
            public int MaxWidth = 5;
            public int MinHeight = 4;
            public int MaxHeight = 7;
            public float CellSize = 1;
            public int ObstacleFillPercent = 90;
        }

        [System.Serializable]
        public class EnviromentData
        {
            public int EnviromentFillPercent = 20;
        }

        [System.Serializable]
        public class ContentData
        {
            public AnimationCurve ProgressionCurve;
        }

        public static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return origin.AddSeconds(timestamp);
        }

        public static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }
    }
}
