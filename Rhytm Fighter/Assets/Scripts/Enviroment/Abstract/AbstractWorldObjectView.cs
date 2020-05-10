using UnityEngine;

namespace RhytmFighter.Enviroment
{
    public abstract class AbstractWorldObjectView : MonoBehaviour
    {
        public System.Action OnDisposed;

        protected const float m_DISPOSE_DELAY = 2;


        public void ScheduleHideView()
        {
            DisposeView(m_DISPOSE_DELAY);
        }


        protected virtual void DisposeView(float delay)
        {
            OnDisposed?.Invoke();
            Destroy(gameObject, delay);  
        }

        protected virtual void DisposeView()
        {
            OnDisposed?.Invoke();
            Destroy(gameObject);
        }
    }
}
