using Cinemachine;
using FrameworkPackage.Utils;
using RhytmFighter.Battle.Core;
using RhytmFighter.Persistant.Abstract;
using RhytmFighter.Persistant.Enums;
using System.Collections.Generic;
using UnityEngine;

namespace RhytmFighter.CameraSystem
{
    public class CameraController : iUpdatable
    {
        private InterpolationData<Quaternion> m_LerpData;
        private Transform m_SmoothingCameraTransform;
        private WaitForSeconds m_WaitBlendingFinishedEvent;
        private CinemachineVirtualCameraBase m_CurrentCamera;
        private Dictionary<CameraTypes, CinemachineVirtualCameraBase> m_Cameras;

        private const int m_ACTIVE_CAM_PRIORITY = 10;
        private const float m_BATTLE_CAM_NOISE_MIN = -25;
        private const float m_BATTLE_CAM_NOISE_MAX = 25;
        private readonly Vector3 m_DEFAULT_CAMERA_TARGET_OFFSET = new Vector3(0, 2.5f, 4);


        public void InitializeCamera(Transform target)
        {
            m_Cameras = new Dictionary<CameraTypes, CinemachineVirtualCameraBase>();
            m_LerpData = new InterpolationData<Quaternion>(BattleManager.Instance.CamerasHolder.CMBrain.m_DefaultBlend.m_Time);
            m_WaitBlendingFinishedEvent = new WaitForSeconds(BattleManager.Instance.CamerasHolder.CMBrain.m_DefaultBlend.m_Time);

            //Initialize vcam list
            m_Cameras[CameraTypes.Default] = BattleManager.Instance.CamerasHolder.VCDefault;
            m_Cameras[CameraTypes.Main] = BattleManager.Instance.CamerasHolder.VCamMain;
            m_Cameras[CameraTypes.Battle] = BattleManager.Instance.CamerasHolder.VCamBattle;

            //Initialize vcams
            m_CurrentCamera = m_Cameras[CameraTypes.Default];
            m_Cameras[CameraTypes.Main].Follow = target;
            m_Cameras[CameraTypes.Default].LookAt = target;
            m_Cameras[CameraTypes.Default].transform.position = target.position + m_DEFAULT_CAMERA_TARGET_OFFSET;

            //Itinialize group vcam
            PushMemberToTargetGroup(target, 1, 2);
        }

        public void PerformUpdate(float deltaTime)
        {
            if (m_LerpData.IsStarted)
            {
                m_LerpData.Increment();
                m_SmoothingCameraTransform.rotation = Quaternion.Slerp(m_LerpData.From, m_LerpData.To, m_LerpData.Progress);

                if (m_LerpData.Overtime())
                    m_LerpData.Stop();
            }
        }


        public void ActivateCamera(CameraTypes cameraType)
        {
            m_CurrentCamera.Priority = 0;
            m_CurrentCamera = m_Cameras[cameraType];
            m_CurrentCamera.Priority = m_ACTIVE_CAM_PRIORITY;
        }

        public void PushMemberToTargetGroup(Transform target, float weight = 1, float radius = 1)
        {
            BattleManager.Instance.CamerasHolder.BattleTargetGroup.AddMember(target, weight, radius);
        }

        public void PeekMemberFromTargetGroup()
        {
            if (BattleManager.Instance.CamerasHolder.BattleTargetGroup.m_Targets.Length == 1)
                return;

            CinemachineTargetGroup.Target target = BattleManager.Instance.CamerasHolder.BattleTargetGroup.m_Targets[BattleManager.Instance.CamerasHolder.BattleTargetGroup.m_Targets.Length - 1];
            BattleManager.Instance.CamerasHolder.BattleTargetGroup.RemoveMember(target.target);
        }


        public float GetNoiseForBattleCamera()
        {
            return Random.Range(m_BATTLE_CAM_NOISE_MIN, m_BATTLE_CAM_NOISE_MAX);
        }

        public void SubscribeForBlendingFinishedEvent(System.Action onBlendingFinished)
        {
            BattleManager.Instance.StartCoroutine(BlendingFinishedEventCoroutine(onBlendingFinished));
        }

        public void StartSmoothRotation(CameraTypes cameraType, Quaternion targetRotation)
        {
            m_SmoothingCameraTransform = m_Cameras[cameraType].transform;

            m_LerpData.From = m_SmoothingCameraTransform.transform.rotation;
            m_LerpData.To = targetRotation;
            m_LerpData.Start();
        }


        private System.Collections.IEnumerator BlendingFinishedEventCoroutine(System.Action onBlendingFinished)
        {
            yield return m_WaitBlendingFinishedEvent;

            onBlendingFinished?.Invoke();
        }  
    }
}
