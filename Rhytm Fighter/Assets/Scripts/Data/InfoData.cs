namespace RhytmFighter.Data
{
    /// <summary>
    /// Info data
    /// </summary>
    public class InfoData 
    {
        public LevelsInfoData LevelsInfoData { get; private set; }
        public LevelsExpInfoData WeaponExpInfoData { get; private set; }

        public InfoData(string serializedLevelsData, string serializedWeaponLevelsExpData)
        {
            LevelsInfoData = LevelsInfoData.DeserializeData(serializedLevelsData);
            LevelsInfoData.ReorginizeData();

            WeaponExpInfoData = LevelsExpInfoData.DeserializeData(serializedWeaponLevelsExpData);
        }
    }
}
