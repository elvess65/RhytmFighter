using UnityEngine;

namespace RhytmFighter.Data
{
    /// <summary>
    /// Рассчитывает прогрессию НПС в зависимости от его уровня
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(fileName = "NewObject ProgressionConfig", menuName = "DBSimulation/Progressions/Object Progression Config", order = 101)]
    public class ObjectProgressionConfig : ScriptableObject
    {
        [Tooltip("Прогрессия количества (Минимальное и Максимальное количество)")]
        public MinMaxProgressionConfig AmountProgression;

        [Tooltip("Прогрессия ИД отображений")]
        public ArrayProgressionConfig ViewIDProgression;

        public int EvaluateViewID(float t)
        {
            return ViewIDProgression.Evaluate(t);
        }

    }
}
