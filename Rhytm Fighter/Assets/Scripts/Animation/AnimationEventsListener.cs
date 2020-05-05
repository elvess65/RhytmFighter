using UnityEngine;

namespace RhytmFighter.Animation
{
    public class AnimationEventsListener : MonoBehaviour
    {
        public event System.Action OnEvent;

        public void EventHandler()
        {
            OnEvent?.Invoke();
        }
    }
}
