using UnityEngine;

namespace RhytmFighter.Data
{
    /// <summary>
    /// Объект прогрессии значения
    /// </summary>
    [System.Serializable]
    public class ProgressionConfig
    {
        [Tooltip("Базовое значение, от которого будет считатся прогрессия")]
        public int BaseValue;

        [Tooltip("Кривая прогрессии")]
        public AnimationCurve Curve;

        public float Evaluate(float t) => Curve.Evaluate(t);
    }
}