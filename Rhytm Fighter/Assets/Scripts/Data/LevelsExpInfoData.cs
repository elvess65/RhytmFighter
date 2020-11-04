namespace RhytmFighter.Data
{
    /// <summary>
    /// Информация об опыте и уровнях персонажей
    /// </summary>
    public class LevelsExpInfoData : AbstractData<LevelsExpInfoData>
    {
        public PlayerLevelExpProgressionConfig LevelExpProgressionConfig;

        public int GetLevelByExp(int expAmount)
        {
            return LevelExpProgressionConfig.EvaluateLevel(expAmount);
        }
    }
}
