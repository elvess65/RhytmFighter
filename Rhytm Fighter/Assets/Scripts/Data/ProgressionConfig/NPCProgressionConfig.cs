using UnityEngine;

namespace RhytmFighter.Data
{
    /// <summary>
    /// Рассчитывает прогрессию НПС в зависимости от степени прохождения
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(fileName = "NewNPC ProgressionConfig", menuName = "DBSimulation/Progressions/NPC Progression Config", order = 101)]
    public class NPCProgressionConfig : ScriptableObject
    {
        /// <summary>
        /// По кривой рассчитывается значение урона и ХП, а потом умножается на множитель,
        /// который рассчитывается по отдельной кривой.
        /// Изменяя базовые значения, кривые и множитель можно добится изменения
        /// прогресии в зависимости от степени прохождения
        
        /// Кривая в 0 - нет изменений относительно базового значения
        /// Кривая в 1 - базовое значение увеличилось в два раза
        /// Множитель рассчитывается по другому:
        /// Кривая в 1 - множитель остается в базовом значении
        /// Кривая в 2 - множитель увеличился в два раза
        /// </summary>

        [Header("Уровень и урон НПС")]

        [Tooltip("Прогрессия множителя")]
        public ProgressionConfig MultiplayerProgression;

        [Space(10)]

        [Tooltip("Прогрессия ХП")]
        public IgnorableProgressionConfig HPProgression;

        [Space(10)]

        [Tooltip("Прогрессия % разброса ХП")]
        public MinMaxProgressionConfig HPSpreadPercentProgression;

        [Space(10)]

        [Tooltip("Прогрессия урона")]
        public IgnorableProgressionConfig DamageProgression;


        [Space(10)]

        [Tooltip("Прогрессия опыта, который дается за уничтожение врага")]
        public IgnorableProgressionConfig ExperianceProgression;

        [Space(10)]

        [Tooltip("Прогрессия % разброса опыта за уничтожение врага")]
        public MinMaxProgressionConfig ExperianceSpreadProgression;


        public int EvaluateHP(float t)
        {
            float multiplayer = HPProgression.IgnoreMultiplayer ? 1 : EvaluateMultiplayer(t);
            return (int)(HPProgression.BaseValue * multiplayer + HPProgression.BaseValue * HPProgression.Evaluate(t));
        }

        public (int, int) EvaluateHPSpread(float t)
        {
            return HPSpreadPercentProgression.EvaluateInt(t);
        }


        public int EvaluateDamage(float t)
        {
            float multiplayer = DamageProgression.IgnoreMultiplayer ? 1 : EvaluateMultiplayer(t);
            return (int)(DamageProgression.BaseValue * multiplayer + DamageProgression.BaseValue * DamageProgression.Evaluate(t));
        }        


        public int EvaluateExp(float t)
        {
            float multiplayer = ExperianceProgression.IgnoreMultiplayer ? 1 : EvaluateMultiplayer(t);
            return (int)(ExperianceProgression.BaseValue * multiplayer + ExperianceProgression.BaseValue * ExperianceProgression.Evaluate(t));
        }

        public (int, int) EvaluateExpSpread(float t)
        {
            return ExperianceSpreadProgression.EvaluateInt(t);
        }


        private float EvaluateMultiplayer(float t)
        {
            return MultiplayerProgression.BaseValue * MultiplayerProgression.Evaluate(t);
        }


        /// <summary>
        /// Объект прогрессии значения с игнорируемым множителем
        /// </summary>
        [System.Serializable]
        public class IgnorableProgressionConfig : ProgressionConfig
        {
            [Tooltip("Игнорировать мультипликатор")]
            public bool IgnoreMultiplayer;
        }
    }
}
