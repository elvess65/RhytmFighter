using UnityEngine;

namespace RhytmFighter.Animation
{
    public class AnimationEventsListener : MonoBehaviour
    {
        public event System.Action OnActionEvent; 
        public event System.Action OnDestroyEvent;
        public event System.Action OnOtherEvent;

        public void EventHandler()
        {
            OnActionEvent?.Invoke();
        }

        public void DestroyEventHandler()
        {
            OnDestroyEvent?.Invoke();
        }

        public void OtherEventHandler()
        {
            OnOtherEvent?.Invoke();
        }
    }
}
