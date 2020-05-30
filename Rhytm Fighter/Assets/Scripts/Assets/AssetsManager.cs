using UnityEngine;

namespace RhytmFighter.Assets
{
    public class AssetsManager : MonoBehaviour
    {
        private static AssetsManager m_Instance;


        public BattlePrefabAssets PrefabAssets;


        public static BattlePrefabAssets GetPrefabAssets() => m_Instance.PrefabAssets;

        public void Initialize()
        {
            m_Instance = this;

            PrefabAssets.Initialize();
        }
    }
}
