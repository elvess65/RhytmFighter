using UnityEngine;

namespace RhytmFighter.UI.Components
{
    /// <summary>
    /// Дает возможность интерполировать компоненты объекта
    /// </summary>
    public abstract class InterpolatableComponent : MonoBehaviour
    {
        public abstract void Initialize();
        public abstract void PrepareForInterpolation();
        public abstract void FinishInterpolation();
        public abstract void ProcessInterpolation(float progress);
    }
}
