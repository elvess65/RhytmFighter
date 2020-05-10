﻿using UnityEngine;

namespace RhytmFighter.Animation
{
    public class AnimationEventsListener : MonoBehaviour
    {
        public event System.Action OnActionEvent; 
        public event System.Action OnDestroyEvent;

        public void EventHandler()
        {
            OnActionEvent?.Invoke();
        }

        public void DestroyEventHandler()
        {
            OnActionEvent?.Invoke();
        }
    }
}
