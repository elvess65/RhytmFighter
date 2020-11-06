using UnityEngine;

namespace RhytmFighter.Data
{
    /// <summary>
    /// Рассчитывает прогрессию улучшения оружия
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(fileName = "NewWeapon ProgressionUpgradeConfig", menuName = "DBSimulation/Progressions/Weapon Upgrade Progression Config", order = 101)]
    public class WeaponUpgradeProgressionConfig : ScriptableObject
    {
        [Header("Зависимость урона от уровня")]
        public MinMaxProgressionConfig DamageProgression;

        [Header("Зависимость цены единицы опыта от уровня")]
        public ProgressionConfig ExperiancePointCostProgression;


        public (int, int) EvaluateDamage(float t)
        {
            return DamageProgression.EvaluateInt(t);
        }

        public float EvaluateExperincePointPrice(float t)
        {
            return ExperiancePointCostProgression.BaseValue + ExperiancePointCostProgression.BaseValue * ExperiancePointCostProgression.Evaluate(t);
        }
    }
}
