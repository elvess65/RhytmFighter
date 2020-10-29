using UnityEngine;

namespace RhytmFighter.Data
{
    /// <summary>
    /// Рассчитывает прогрессию объекта в зависимости от степени прохождения
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(fileName = "NewObject ProgressionConfig", menuName = "DBSimulation/Progressions/Object Progression Config", order = 101)]
    public class ObjectProgressionConfig : ScriptableObject
    {
        [Header("Количество и доступные ID")]

        [Tooltip("Прогрессия количества (Минимальное и Максимальное количество)")]
        public MinMaxProgressionConfig AmountProgression;

        [Space(10)]

        [Tooltip("Прогрессия ИД")]
        public ArrayProgressionConfig ViewIDProgression;


        public (int, int) EvaluateAmount(float t)
        {
            return AmountProgression.EvaluateInt(t);
        }

        public int[] EvaluateViewIDs(float t)
        {
            return ViewIDProgression.Evaluate(t);
        }
    }
}
