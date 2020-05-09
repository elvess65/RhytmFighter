using Cinemachine;
using UnityEngine;

namespace RhytmFighter.CameraSystem
{
    public class CamerasHolder : MonoBehaviour
    {
        [Header("Camera")]
        public Camera WorldCamera;

        [Header("Cinemachine Virtual Cameras")]
        public CinemachineVirtualCamera VCDefault;
        public CinemachineVirtualCamera VCamMain;
        public CinemachineVirtualCamera VCamBattle;

        [Header("Cinemachine Additional")]
        public CinemachineBrain CMBrain;
        public CinemachineTargetGroup BattleTargetGroup;
    }
}
