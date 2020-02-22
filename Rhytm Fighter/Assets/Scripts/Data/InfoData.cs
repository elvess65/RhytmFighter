namespace RhytmFighter.Data
{
    /// <summary>
    /// Info data
    /// </summary>
    public class InfoData 
    {
        public LevelsData LevelsData { get; private set; }

        public InfoData(string serializedLevelsData)
        {
            LevelsData = LevelsData.DeserializeData(serializedLevelsData);
        }
    }
}
