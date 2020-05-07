using Cinemachine;
using RhytmFighter.Core;
using RhytmFighter.UI.Tools;
using UnityEngine;

namespace RhytmFighter.CameraSystem
{
    public class CameraController : iUpdatable
    {
        private FollowObject m_FollowObject;
        private CinemachineVirtualCameraBase m_VCam;

        public void InitializeCamera(CinemachineVirtualCameraBase vcam, Transform target)
        {
            //m_FollowObject = new FollowObject();
            //m_FollowObject.SetRoot(root);
            //m_FollowObject.SetTarget(target);
            //m_FollowObject.SetSpeed(followSpeed);

            m_VCam = vcam;
            m_VCam.Follow = target;
            m_VCam.LookAt = target;

            CinemachineStateDrivenCamera stateDrivenCamera = (CinemachineStateDrivenCamera)vcam;
            stateDrivenCamera.m_AnimatedTarget = target.GetComponentInChildren<Animator>();
        }

        public void SetTarget(Transform target) => m_FollowObject?.SetTarget(target);

        public void SetFollowPoint(Vector3 followPoint) => m_FollowObject?.SetFollowPoint(followPoint);

        public void ClearFollowPoint() => m_FollowObject?.ClearFollowPoint();

        public void SetSpeed(float speed) => m_FollowObject?.SetSpeed(speed);

        public void PerformUpdate(float deltaTime) => m_FollowObject?.PerformUpdate(deltaTime);
    }
}
