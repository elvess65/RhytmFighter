namespace RhytmFighter.Data
{
    /// <summary>
    /// Info data
    /// </summary>
    public class InfoData 
    {
        public LevelsInfoData LevelsInfoData { get; private set; }
        public LevelsExpInfoData LevelsExpInfoData { get; private set; }

        public InfoData(string serializedLevelsData, string serializedLevelsExpData)
        {
            LevelsInfoData = LevelsInfoData.DeserializeData(serializedLevelsData);
            LevelsInfoData.ReorginizeData();

            LevelsExpInfoData = LevelsExpInfoData.DeserializeData(serializedLevelsExpData);
        }
    }
}
