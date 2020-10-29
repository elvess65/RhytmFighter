using System.Collections.Generic;
using UnityEngine;

namespace RhytmFighter.Data
{
    /// <summary>
    /// Объект прогрессии массива
    /// </summary>
    [System.Serializable]
    public class ArrayProgressionConfig
    {
        /// Кривая в 0 - доступен только 0 элемент
        /// Кривая в 1 - доступны все элементы

        public int[] DataArray;
        public AnimationCurve Curve;

        public int[] Evaluate(float t)
        {
            float index = Mathf.Lerp(0, DataArray.Length - 1, t);

            List<int> result = new List<int>();
            for (int i = 0; i <= index; i++)
                result.Add(DataArray[i]);

            return result.ToArray();
        }
    }
}
