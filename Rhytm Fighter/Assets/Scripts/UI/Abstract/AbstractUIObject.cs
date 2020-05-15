using UnityEngine;

namespace RhytmFighter.UI.Abstract
{
    public abstract class AbstractUIObject : MonoBehaviour
    {
        private RectTransform m_RectTransform;

        public RectTransform RectTransform
        {
            get
            {
                if (m_RectTransform == null)
                    m_RectTransform = GetComponent<RectTransform>();

                return m_RectTransform;
            }
        }
    }
}
