using Cinemachine;
using RhytmFighter.Core;
using RhytmFighter.Core.Enums;
using System.Collections.Generic;
using UnityEngine;

namespace RhytmFighter.CameraSystem
{
    public class CameraController
    {
        private WaitForSeconds m_WaitBlendingFinishedEvent;
        private CinemachineVirtualCameraBase m_CurrentCamera;
        private Dictionary<CameraTypes, CinemachineVirtualCameraBase> m_Cameras;

        private const int m_ACTIVE_CAM_PRIORITY = 10;
        private const float m_BATTLE_CAM_NOISE_MIN = -25;
        private const float m_BATTLE_CAM_NOISE_MAX = 25;


        public void InitializeCamera(Transform target)
        {
            m_Cameras = new Dictionary<CameraTypes, CinemachineVirtualCameraBase>();
            m_WaitBlendingFinishedEvent = new WaitForSeconds(GameManager.Instance.CamerasHolder.CMBrain.m_DefaultBlend.m_Time);

            //Initialize vcam list
            m_Cameras[CameraTypes.Default] = GameManager.Instance.CamerasHolder.VCDefault;
            m_Cameras[CameraTypes.Main] = GameManager.Instance.CamerasHolder.VCamMain;
            m_Cameras[CameraTypes.Battle] = GameManager.Instance.CamerasHolder.VCamBattle;

            //Initialize vcams
            m_CurrentCamera = m_Cameras[CameraTypes.Default];
            m_Cameras[CameraTypes.Main].Follow = target;

            //Itinialize group vcam
            PushMemberToTargetGroup(target, 1, 2);

            //Activate mmain camera
            ActivateCamera(CameraTypes.Main);
        }


        public void ActivateCamera(CameraTypes cameraType)
        {
            m_CurrentCamera.Priority = 0;
            m_CurrentCamera = m_Cameras[cameraType];
            m_CurrentCamera.Priority = m_ACTIVE_CAM_PRIORITY;
        }

        public void PushMemberToTargetGroup(Transform target, float weight = 1, float radius = 1)
        {
            GameManager.Instance.CamerasHolder.BattleTargetGroup.AddMember(target, weight, radius);
        }

        public void PeekMemberFromTargetGroup()
        {
            if (GameManager.Instance.CamerasHolder.BattleTargetGroup.m_Targets.Length == 1)
                return;

            CinemachineTargetGroup.Target target = GameManager.Instance.CamerasHolder.BattleTargetGroup.m_Targets[GameManager.Instance.CamerasHolder.BattleTargetGroup.m_Targets.Length - 1];
            GameManager.Instance.CamerasHolder.BattleTargetGroup.RemoveMember(target.target);
        }


        public float GetNoiseForBattleCamera()
        {
            return Random.Range(m_BATTLE_CAM_NOISE_MIN, m_BATTLE_CAM_NOISE_MAX);
        }

        public void SubscribeForBlendingFinishedEvent(System.Action onBlendingFinished)
        {
            GameManager.Instance.StartCoroutine(BlendingFinishedEventCoroutine(onBlendingFinished));
        }


        private System.Collections.IEnumerator BlendingFinishedEventCoroutine(System.Action onBlendingFinished)
        {
            yield return m_WaitBlendingFinishedEvent;

            onBlendingFinished?.Invoke();
        }
    }
}
