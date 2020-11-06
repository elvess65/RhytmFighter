using UnityEngine;

namespace RhytmFighter.Data
{
    /// <summary>
    /// Рассчитывает прогрессию размеров уровня и комнат в зависимости от степени прохождения
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(fileName = "NewLevelSize ProgressionConfig", menuName = "DBSimulation/Progressions/Level Size Progression Config", order = 101)]
    public class LevelSizeProgressionConfig : ScriptableObject
    {
        /// Кривая в 0 - нет изменений относительно базового значения
        /// Кривая в 1 - базовое значение увеличилось в два раза 

        [Header("Глубина графа и размеры комнат")]

        [Tooltip("Прогрессия глубины уровня (Минимальное и Максимальное количество)")]
        public MinMaxProgressionConfig DepthProgression;

        [Space(10)]

        [Tooltip("Прогрессия ширины уровня (Минимальное и Максимальное количество)")]
        public MinMaxProgressionConfig WidthProgression;

        [Space(10)]

        [Tooltip("Прогрессия длины уровня (Минимальное и Максимальное количество)")]
        public MinMaxProgressionConfig HeightProgression;


        public (int, int) EvaluateDepth(float t)
        {
            return DepthProgression.EvaluateInt(t);
        }

        public (int, int) EvaluateWidth(float t)
        {
            return WidthProgression.EvaluateInt(t);
        }

        public (int, int) EvaluateHeight(float t)
        {
            return HeightProgression.EvaluateInt(t);
        }
    }
}
