namespace RhytmFighter.Data
{
    /// <summary>
    /// Информация об опыте и уровнях 
    /// </summary>
    public class LevelsExpInfoData : AbstractData<LevelsExpInfoData>
    {
        public LevelExpProgressionConfig WeaponLevelExpProgressionConfig;

        public int GetWeaponLevelByExp(int expAmount)
        {
            return WeaponLevelExpProgressionConfig.EvaluateLevel(expAmount);
        }

        public int GetWeaponExpForLevel(int level)
        {
            return WeaponLevelExpProgressionConfig.EvaluateExpForLevel(level); 
        }
    }
}
