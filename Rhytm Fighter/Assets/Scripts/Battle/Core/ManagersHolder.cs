using RhytmFighter.Assets;
using RhytmFighter.Settings;
using RhytmFighter.UI;
using UnityEngine;

namespace RhytmFighter.Battle.Core
{
    /// <summary>
    /// Holder for managers
    /// </summary>
    public class ManagersHolder : MonoBehaviour
    {
        public AssetsManager AssetsManager;
        public SettingsManager SettingsManager;
        public UIManager UIManager;

        public void Initialize()
        {
            AssetsManager.Initialize();
            UIManager.Initialize();
        }
    }
}
