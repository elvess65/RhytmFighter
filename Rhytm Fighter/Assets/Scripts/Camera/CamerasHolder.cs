using Cinemachine;
using UnityEngine;

namespace RhytmFighter.CameraSystem
{
    public class CamerasHolder : MonoBehaviour
    {
        [Header("Camera")]
        public Camera WorldCamera;

        [Header("Cinemachine Virtual Cameras")]
        public CinemachineStateDrivenCamera VCStateDriven;
        public CinemachineVirtualCamera VCamMain;
        public CinemachineVirtualCamera VCamBattle;
        public CinemachineVirtualCamera VCamFront;

        [Header("Cinemachine Additional")]
        public CinemachineTargetGroup BattleTargetGroup;
    }
}
