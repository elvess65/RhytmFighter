using RhytmFighter.Battle.Core;
using UnityEngine;

namespace RhytmFighter.UI.Tools
{
    public class LookAtCamera : MonoBehaviour
    {
        private Camera m_Camera;

        void Start()
        {
            m_Camera = BattleManager.Instance.CamerasHolder.WorldCamera;
        }

        void Update()
        {
            transform.LookAt(m_Camera.transform);
        }
    }
}
