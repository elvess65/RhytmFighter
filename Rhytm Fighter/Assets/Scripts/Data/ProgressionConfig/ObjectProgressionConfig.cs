﻿using UnityEngine;

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
        [Space]

        [Tooltip("Прогрессия количества (Минимальное и Максимальное количество)")]
        public MinMaxProgressionConfig AmountProgression;

        [Tooltip("Прогрессия ИД")]
        public ArrayProgressionConfig ViewIDProgression;


        public (int, int) EvaluateAmountInt(float t)
        {
            return AmountProgression.EvaluateInt(t);
        }

        public (float, float) EvaluateAmountFloat(float t)
        {
            return AmountProgression.EvaluateFloat(t);
        }

        public int[] EvaluateViewIDs(float t)
        {
            return ViewIDProgression.Evaluate(t);
        }

    }
}
