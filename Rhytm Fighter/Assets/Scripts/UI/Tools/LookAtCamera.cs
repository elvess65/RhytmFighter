using RhytmFighter.Core;
using UnityEngine;

namespace RhytmFighter.UI.Tools
{
    public class LookAtCamera : MonoBehaviour
    {
        private Camera m_Camera;

        void Start()
        {
            m_Camera = GameManager.Instance.CamerasHolder.WorldCamera;
        }

        void Update()
        {
            transform.LookAt(m_Camera.transform);
        }
    }
}
