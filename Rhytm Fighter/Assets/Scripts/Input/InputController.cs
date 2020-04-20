using UnityEngine;
using RhytmFighter.Interfaces;
using UnityEngine.EventSystems;

namespace RhytmFighter.Input
{
    /// <summary>
    /// Tracking low level input
    /// </summary>
    public class InputController : iUpdatable
    {
        public event System.Action<Vector3> OnTouch;

        public void PerformUpdate(float deltaTime)
        {
            if (InputDetected() && !IsPointerOverUI())
                OnTouch?.Invoke(UnityEngine.Input.mousePosition);
        }

        private bool InputDetected()
        {
#if !UNITY_EDITOR
            return UnityEngine.Input.touchCount > 0 && UnityEngine.Input.touches[0].phase == TouchPhase.Began;
#endif

            return UnityEngine.Input.GetMouseButtonDown(0);
        }

        private bool IsPointerOverUI()
        {
#if !UNITY_EDITOR
            return EventSystem.current.IsPointerOverGameObject(UnityEngine.Input.touches[0].fingerId); 
#endif
            return EventSystem.current.IsPointerOverGameObject();
        }
    }
}
