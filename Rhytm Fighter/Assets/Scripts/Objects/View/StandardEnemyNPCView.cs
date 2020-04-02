using RhytmFighter.Assets;
using RhytmFighter.UI.Bar;
using UnityEngine;

namespace RhytmFighter.Objects.View
{
    public class StandardEnemyNPCView : AbstractBattleNPCView
    {
        public Transform HealthBarParent;

        private SingleBarBehaviour m_HealthBarBehaviour;


        protected override void CreateHealthBar()
        {
            m_HealthBarBehaviour = AssetsManager.GetPrefabAssets().InstantiatePrefab(AssetsManager.GetPrefabAssets().EnemyHealthBarPrefab);
            m_HealthBarBehaviour.RectTransform.SetParent(HealthBarParent);
            m_HealthBarBehaviour.RectTransform.anchoredPosition3D = Vector3.zero;
        }

        protected override void DestroyHealthBar()
        {
            Destroy(m_HealthBarBehaviour);
        }
    }
}
