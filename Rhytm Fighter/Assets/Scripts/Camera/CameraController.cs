using RhytmFighter.Core;
using RhytmFighter.UI.Tools;
using UnityEngine;

namespace RhytmFighter.Camera
{
    public class CameraController : iUpdatable
    {
        private FollowObject m_FollowObject;


        public void InitializeCamera(Transform root, Transform target, float followSpeed)
        {
            m_FollowObject = new FollowObject();
            m_FollowObject.SetRoot(root);
            m_FollowObject.SetTarget(target);
            m_FollowObject.SetSpeed(followSpeed);
        }

        public void SetTarget(Transform target) => m_FollowObject.SetTarget(target);

        public void SetFollowPoint(Vector3 followPoint) => m_FollowObject.SetFollowPoint(followPoint);

        public void ClearFollowPoint() => m_FollowObject.ClearFollowPoint();

        public void SetSpeed(float speed) => m_FollowObject.SetSpeed(speed);

        public void PerformUpdate(float deltaTime) => m_FollowObject?.PerformUpdate(deltaTime);
    }
}
