using RhytmFighter.Settings.Proxy;
using UnityEngine;

namespace RhytmFighter.Settings
{
    /// <summary>
    /// Settings Manager
    /// </summary>
    public class SettingsManager : MonoBehaviour
    {
        [Header("Simulation")]
        public ProxySettings ProxySettings;

        [Header("Other settings")]
        public CameraSettings CameraSettings;
    }
}
