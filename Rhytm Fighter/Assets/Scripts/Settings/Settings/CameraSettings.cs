using UnityEngine;

namespace RhytmFighter.Settings.Proxy
{
    [CreateAssetMenu(fileName = "New CameraSettings", menuName = "Settings/Camera", order = 101)]
    public class CameraSettings : ScriptableObject
    {
        public float NormalMoveSpeed = 1;
    }
}
