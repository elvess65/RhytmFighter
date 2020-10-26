using UnityEngine;

namespace RhytmFighter.Data
{
    /// <summary>
    /// Рассчитывает прогрессию НПС в зависимости от его уровня
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(fileName = "NewNPC ProgressionConfig", menuName = "DBSimulation/Progressions/NPC Progression Config", order = 101)]
    public class NPCProgressionConfig : ScriptableObject
    {
        [Tooltip("Прогрессия множителя")]
        public ProgressionConfig MultiplayerProgression;

        [Tooltip("Прогрессия ХП")]
        public IgnorableProgressionConfig HPProgression;

        [Tooltip("Прогрессия урона")]
        public IgnorableProgressionConfig DamageProgression;


        public float EvaluateMultiplayer(float t)
        {
            return MultiplayerProgression.BaseValue * MultiplayerProgression.Evaluate(t);
        }


        public float EvaluateHP(float t)
        {
            float multiplayer = HPProgression.IgnoreMultiplayer ? 1 : EvaluateMultiplayer(t);
            return HPProgression.BaseValue * multiplayer + HPProgression.BaseValue * HPProgression.Evaluate(t);
        }

        public float EvaluateHPSpreadMin(float t) => 0;

        public float EvaluateHPSpreadMax(float t) => 0;


        public float EvaluateDamage(float t)
        {
            float multiplayer = DamageProgression.IgnoreMultiplayer ? 1 : EvaluateMultiplayer(t);
            return DamageProgression.BaseValue * multiplayer + DamageProgression.BaseValue * DamageProgression.Evaluate(t);
        }        

        public float EvaluateDamageSpreadMin(float t) => 0;

        public float EvaluateDamageSpreadMax(float t) => 0;



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
