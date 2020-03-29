using UnityEngine;

namespace RhytmFighter.Assets
{
    public class AssetsManager : MonoBehaviour
    {
        private static AssetsManager m_Instance;


        public PrefabAssets PrefabAssets;


        public static PrefabAssets GetPrefabAssets() => m_Instance.PrefabAssets;

        public void Initialize() => m_Instance = this;
    }
}
