using RhytmFighter.Persistant.Abstract;
using UnityEngine;

namespace RhytmFighter.UI.View
{
    /// <summary>
    /// Базовый класс для UI View
    /// </summary>
    public abstract class UIView_Abstract : MonoBehaviour, iUpdatable
    {
        public Transform Root;

        public abstract void Initialize();
        public abstract void PerformUpdate(float deltaTime);
    }
}
