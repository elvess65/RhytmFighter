using RhytmFighter.Core;
using UnityEngine;

namespace RhytmFighter.UI.Tools
{
    public class FollowObject : iUpdatable
    {
        private Transform m_Root;                           //Root camera object
        private Transform m_Target;                         //Target should be followed
        private Vector3 m_FollowPoint;                      //Point should be followed
        private float m_Speed;                              //Moving speed
        private FollowMode m_FollowMode = FollowMode.None;  //Current follow mode

        private enum FollowMode { None, Object, Point }


        public void SetRoot(Transform root)
        {
            m_Root = root;
        }

        public void SetTarget(Transform target)
        {
            m_Target = target;

            if (m_FollowMode != FollowMode.Object)
                m_FollowMode = FollowMode.Object;
        }

        public void SetFollowPoint(Vector3 followPoint)
        {
            m_FollowPoint = followPoint;

            if (m_FollowMode != FollowMode.Point)
                m_FollowMode = FollowMode.Point;
        }

        public void ClearFollowPoint()
        {
            m_FollowMode = FollowMode.Object;
        }

        public void SetSpeed(float speed)
        {
            m_Speed = speed;
        }

        public void PerformUpdate(float deltaTime)
        {
            switch (m_FollowMode)
            {
                case FollowMode.Object:

                    if (m_Target != null && m_Root != null)
                        FollowPosition(m_Target.position, deltaTime);

                    break;

                case FollowMode.Point:

                    FollowPosition(m_FollowPoint, deltaTime);

                    break;
            }
        }


        private void FollowPosition(Vector3 pos, float deltaTime)
        {
            m_Root.transform.position = Vector3.Lerp(m_Root.transform.position, pos, deltaTime * m_Speed);
        }
    }
}
