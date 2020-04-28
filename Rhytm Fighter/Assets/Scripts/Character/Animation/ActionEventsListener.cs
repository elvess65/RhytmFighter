using UnityEngine;

namespace RhytmFighter.Characters.Animation
{
    public class ActionEventsListener : MonoBehaviour
    {
        public event System.Action OnEvent;

        public void EventHandler()
        {
            OnEvent?.Invoke();
        }
    }
}
