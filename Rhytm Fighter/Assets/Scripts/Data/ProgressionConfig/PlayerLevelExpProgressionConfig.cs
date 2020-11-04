using UnityEngine;

namespace RhytmFighter.Data
{
    /// <summary>
    /// Рассчитывает прогрессию необходимого опыта для получения уровня
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(fileName = "NewPlayerLevelExp ProgressionConfig", menuName = "DBSimulation/Progressions/Player Level Exp Progression Config", order = 101)]
    public class PlayerLevelExpProgressionConfig : ScriptableObject
    {
        /// Кривая в 0 - нет изменений относительно базового значения
        /// Кривая в 1 - базовое значение увеличилось в два раза 

        [Header("Прогрессия необходимого опыта для получения уровня персонажем")]

        [Tooltip("Прогрессия глубины уровня (Минимальное и Максимальное количество)")]
        public ProgressionConfig Config;

        public int EvaluateLevel(int expAmount)
        {
            if (expAmount < 5)
                return 1;

            return 2;
        }
    }
}
