using UnityEngine;

namespace RhytmFighter.UI.Components
{
    public abstract class InterpolatableComponent : MonoBehaviour
    {
        public abstract void Initialize();
        public abstract void PrepareForInterpolation();
        public abstract void FinishInterpolation();
        public abstract void ProcessInterpolation(float progress);
    }
}
