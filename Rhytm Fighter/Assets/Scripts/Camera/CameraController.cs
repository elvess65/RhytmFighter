using RhytmFighter.Interfaces;
using UnityEngine;

namespace RhytmFighter.Camera
{
    public class CameraController : iUpdatable
    {
        //Root camera object
        private Transform m_Root;   

        //Target should be followed
        private Transform m_Target;

        //Moving speed
        private float m_Speed;


        public void InitializeCamera(Transform root, Transform target, float followSpeed)
        {
            m_Root = root;
            m_Speed = followSpeed;

            SetTarget(target);
        }

        public void SetTarget(Transform target) => m_Target = target;

        public void SetSpeed(float speed) => m_Speed = speed;

        public void PerformUpdate(float deltaTime)
        {
            if (m_Target != null && m_Root != null)
                m_Root.transform.position = Vector3.Lerp(m_Root.transform.position, m_Target.position, Time.deltaTime * m_Speed);
        }
    }
}
