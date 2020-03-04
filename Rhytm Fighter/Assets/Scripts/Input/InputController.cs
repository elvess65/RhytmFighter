using UnityEngine;
using RhytmFighter.Interfaces;
using UnityEngine.EventSystems;

namespace RhytmFighter.Input
{
    /// <summary>
    /// Tracking low level input
    /// </summary>
    public class InputController : iUpdateable
    {
        public event System.Action<Vector3> OnTouch;

        public void Update(float deltaTime)
        {
            if (UnityEngine.Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                OnTouch?.Invoke(UnityEngine.Input.mousePosition);
        }
    }
}
