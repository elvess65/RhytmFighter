namespace RhytmFighter.Data
{
    /// <summary>
    /// Объект прогрессии пары значений Min:Max
    /// </summary>
    [System.Serializable]
    public class MinMaxProgressionConfig
    {
        public ProgressionConfig MinProgression;
        public ProgressionConfig MaxProgression;
    }
}
