using Cinemachine;
using RhytmFighter.Animation;
using RhytmFighter.Core;
using RhytmFighter.UI.Tools;
using UnityEngine;

namespace RhytmFighter.CameraSystem
{
    public class CameraController : iUpdatable
    {
        private FollowObject m_FollowObject;
        private CinemachineStateDrivenCamera m_MainVCam;

        public void InitializeCamera(Transform target)
        {
            //m_FollowObject = new FollowObject();
            //m_FollowObject.SetRoot(root);
            //m_FollowObject.SetTarget(target);
            //m_FollowObject.SetSpeed(followSpeed);

            //Disable default vcam
            GameManager.Instance.CamerasHolder.DefaultCamMain.Priority = 0;

            //Initialize state driven vcam
            GameManager.Instance.CamerasHolder.VCStateDriven.Follow = target;
            GameManager.Instance.CamerasHolder.VCStateDriven.LookAt = target;
            GameManager.Instance.CamerasHolder.VCStateDriven.m_AnimatedTarget = target.GetComponent<AbstractAnimationController>().Controller;

            //Itinialize group vcam
            PushMemberToTargetGroup(target);
        }

        public void PushMemberToTargetGroup(Transform target)
        {
            GameManager.Instance.CamerasHolder.BattleTargetGroup.AddMember(target, 1, 1);
        }

        public void PeekMemberFromTargetGroup(Transform target)
        {
            GameManager.Instance.CamerasHolder.BattleTargetGroup.RemoveMember(target);
        }


        public void SetTarget(Transform target) => m_FollowObject?.SetTarget(target);

        public void SetFollowPoint(Vector3 followPoint) => m_FollowObject?.SetFollowPoint(followPoint);

        public void ClearFollowPoint() => m_FollowObject?.ClearFollowPoint();

        public void SetSpeed(float speed) => m_FollowObject?.SetSpeed(speed);

        public void PerformUpdate(float deltaTime) => m_FollowObject?.PerformUpdate(deltaTime);
    }
}
