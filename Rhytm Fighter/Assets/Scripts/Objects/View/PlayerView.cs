using RhytmFighter.Assets;
using RhytmFighter.UI.Bar;
using UnityEngine;

namespace RhytmFighter.Objects.View
{
    public class PlayerView : AbstractBattleNPCView
    {
        private SingleBarBehaviour m_HealthBarBehaviour;

        protected override void CreateHealthBar()
        {
            m_HealthBarBehaviour = AssetsManager.GetPrefabAssets().InstantiatePrefab(AssetsManager.GetPrefabAssets().PlayerHealthBarPrefab);
            m_HealthBarBehaviour.RectTransform.SetParent(FindObjectOfType<Canvas>().transform);
            m_HealthBarBehaviour.RectTransform.anchoredPosition3D = Vector3.zero;
        }

        protected override void DestroyHealthBar()
        {
            Destroy(m_HealthBarBehaviour);
        }
    }
}
