using RhytmFighter.Interfaces;
using UnityEngine;

namespace RhytmFighter.UI.Tools
{
    public class FollowObject : iUpdatable
    {
        private Transform m_Root;       //Root camera object
        private float m_Speed;          //Moving speed
        private Transform m_Target;     //Target should be followed


        public void SetRoot(Transform root) => m_Root = root;

        public void SetTarget(Transform target) => m_Target = target;

        public void SetSpeed(float speed) => m_Speed = speed;

        public void PerformUpdate(float deltaTime)
        {
            if (m_Target != null && m_Root != null)
                m_Root.transform.position = Vector3.Lerp(m_Root.transform.position, m_Target.position, deltaTime * m_Speed);
        }
    }
}
