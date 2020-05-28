using System.Collections.Generic;

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
            public int LevelDepth = 4;
            public int LevelSeed = 10;
            public int MinWidth = 3;
            public int MaxWidth = 5;
            public int MinHeight = 4;
            public int Maxheight = 7;
            public float CellSize = 1;
            public int FillPercent = 90;
            public int EnviromentFillPercent = 90;
            public int BPM = 130;
        }
    }
}
