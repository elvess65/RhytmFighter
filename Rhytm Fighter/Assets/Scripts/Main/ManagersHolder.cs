using RhytmFighter.Assets;
using RhytmFighter.Settings;
using UnityEngine;

namespace RhytmFighter.Main
{
    /// <summary>
    /// Holder for managers
    /// </summary>
    public class ManagersHolder : MonoBehaviour
    {
        public AssetsManager AssetsManager;
        public SettingsManager SettingsManager;

        public void Initialize()
        {
            AssetsManager.Initialize();
        }
    }
}
