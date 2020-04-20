using RhytmFighter.Settings.Proxy;
using UnityEngine;

namespace RhytmFighter.Settings
{
    /// <summary>
    /// Settings Manager
    /// </summary>
    public class SettingsManager : MonoBehaviour
    {
        [Header("General")]
        public GeneralSettings GeneralSettings;

        [Header("Other settings")]
        public CameraSettings CameraSettings;
    }
}
