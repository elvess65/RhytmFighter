using UnityEngine;

namespace RhytmFighter.Data
{
    /// <summary>
    /// Рассчитывает прогрессию необходимого опыта для получения уровня
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(fileName = "NewLevelExp ProgressionConfig", menuName = "DBSimulation/Progressions/Level Exp Progression Config", order = 101)]
    public class LevelExpProgressionConfig : ScriptableObject
    {
        [Header("Прогрессия необходимого опыта для получения уровня")]

        [Tooltip("Кривая прогресии опыта")]
        public AnimationCurve Config;

        [Tooltip("Обратная кривая прогресии опыта")]
        public AnimationCurve InvertedConfig;

        [Tooltip("Общее количество уровней")]
        public int TotalLevels = 10;

        [Tooltip("Общее количество опыта")]
        public int TotalExp = 100;


        public int EvaluateLevel(int expAmount)
        {
            //Прогресс опыта относительно общего количества (expAmount = 15, Exp Range: [0:100], progress = 0.15)
            float expProgress = expAmount / (float)TotalExp;

            //Текущее положение на кривой
            float curveValue = Config.Evaluate(expProgress);

            //Шаг уровня на кривой (TotalLevels = 20, levelCurveStep = 0.05)
            float levelCurveStep = 1f / TotalLevels;

            //Текущий уровень (curveValue = 0.15, levelCurveStep = 0.05, floatLevel = 3)
            float floatLevel = curveValue / levelCurveStep;
            int level = (int)floatLevel;

            return level;
        }

        public int EvaluateExpForLevel(int level)
        {
            float levelCurveStep = 1f / TotalLevels;            //Шаг уровня на кривой
            float curveValue = levelCurveStep * level;          //Положение на кривой

            float requExpRaw = InvertedConfig.Evaluate(curveValue) * TotalExp;
            int reqExpInt = Mathf.CeilToInt(requExpRaw);

            return reqExpInt;
        }


        public void CreateInvertedCurve()
        {
            InvertedConfig = new AnimationCurve();

            float totalTime = Config.keys[Config.length - 1].time;
            float sampleX = 0; //The "sample-point"
            float deltaX = 0.01f; //The "sample-delta"
            float lastY = Config.Evaluate(sampleX);
            while (sampleX < totalTime)
            {
                float y = Config.Evaluate(sampleX); //The "value"
                float deltaY = y - lastY; //The "value-delta"
                float tangent = deltaX / deltaY;
                Keyframe invertedKey = new Keyframe(y, sampleX, tangent, tangent);
                InvertedConfig.AddKey(invertedKey);

                sampleX += deltaX;
                lastY = y;
            }
            for (int i = 0; i < InvertedConfig.length; i++)
            {
                InvertedConfig.SmoothTangents(i, 0.1f);
            }
        }
    }
}
