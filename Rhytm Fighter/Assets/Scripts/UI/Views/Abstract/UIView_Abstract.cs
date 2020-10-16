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

        protected iUpdatable[] m_Updatables;

        public abstract void Initialize();
 
        public virtual void PerformUpdate(float deltaTime)
        {
            if (m_Updatables != null)
            {
                for (int i = 0; i < m_Updatables.Length; i++)
                    m_Updatables[i].PerformUpdate(deltaTime);
            }
        }

        public void DisableView(bool isDisabled)
        {
            Root.gameObject.SetActive(!isDisabled);
        }
    }
}
