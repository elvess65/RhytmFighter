using RhytmFighter.Main;
using UnityEngine;

namespace RhytmFighter.UI.Tools
{
    public class LookAtCamera : MonoBehaviour
    {
        private UnityEngine.Camera m_Camera;

        void Start()
        {
            m_Camera = GameManager.Instance.WorldCamera;
        }

        void Update()
        {
            transform.LookAt(m_Camera.transform);
        }
    }
}
