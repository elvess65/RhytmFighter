using UnityEngine;

namespace RhytmFighter.Data
{
    /// <summary>
    /// Объект прогрессии массива
    /// </summary>
    [System.Serializable]
    public class ArrayProgressionConfig
    {
        public int[] DataArray;
        public AnimationCurve Curve;

        public int Evaluate(float t)
        {
            return DataArray[2];
        }
    }
}
