namespace RhytmFighter.Data
{
    /// <summary>
    /// Объект прогрессии пары значений Min:Max
    /// </summary>
    [System.Serializable]
    public class MinMaxProgressionConfig
    {
        /// Кривая в 0 - нет изменений относительно базового значения
        /// Кривая в 1 - базовое значение увеличилось в два раза 

        public ProgressionConfig MinProgression;
        public ProgressionConfig MaxProgression;


        public (int, int) EvaluateInt(float t)
        {
            (float min, float max) floatResult = EvaluateFloat(t);

            int intMin = UnityEngine.Mathf.RoundToInt(floatResult.min);
            int intMax = UnityEngine.Mathf.RoundToInt(floatResult.max);

            return (intMin, intMax);
        }

        public (float, float) EvaluateFloat(float t)
        {
            (float min, float max) result = (EvaluateMin(t), EvaluateMax(t));

            if (result.min > result.max)
                result.min = result.max;
            else if (result.max < result.min)
                result.max = result.min;

            return (EvaluateMin(t), EvaluateMax(t));
        }


        private float EvaluateMin(float t)
        {
            return MinProgression.BaseValue + MinProgression.BaseValue * MinProgression.Evaluate(t);
        }

        private float EvaluateMax(float t)
        {
            return MaxProgression.BaseValue + MaxProgression.BaseValue * MaxProgression.Evaluate(t);
        }
    }
}
