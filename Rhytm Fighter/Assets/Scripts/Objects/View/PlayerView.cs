using RhytmFighter.Assets;
using RhytmFighter.UI.Bar;
using UnityEngine;

namespace RhytmFighter.Objects.View
{
    public class PlayerView : AbstractBattleNPCView
    {
        private DoubleBarBehaviour m_HealthBarBehaviour;


        public override void NotifyView_TakeDamage(int dmg)
        {
            base.NotifyView_TakeDamage(dmg);

            UpdateHealthBar();
        }


        protected override void CreateHealthBar()
        {
            m_HealthBarBehaviour = AssetsManager.GetPrefabAssets().InstantiatePrefab(AssetsManager.GetPrefabAssets().PlayerHealthBarPrefab);
            m_HealthBarBehaviour.RectTransform.SetParent(FindObjectOfType<Canvas>().transform);
            m_HealthBarBehaviour.RectTransform.anchoredPosition3D = Vector3.zero;
        }

        protected override void UpdateHealthBar()
        {
            m_HealthBarBehaviour.SetProgress(m_ModelAsBattleModel.HealthBehaviour.HP, m_ModelAsBattleModel.HealthBehaviour.MaxHP);
        }

        protected override void DestroyHealthBar()
        {
            UpdateHealthBar();
        }
    }
}
