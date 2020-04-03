using RhytmFighter.Assets;
using RhytmFighter.UI.Bar;
using UnityEngine;

namespace RhytmFighter.Objects.View
{
    public class PlayerView : AbstractBattleNPCView
    {
        private DoubleBarBehaviour m_HealthBarBehaviour;


        public override void NotifyView_TakeDamage(int curHP, int maxHP, int dmg)
        {
            base.NotifyView_TakeDamage(curHP, maxHP, dmg);

            m_HealthBarBehaviour.SetProgress(curHP, maxHP);
        }


        protected override void CreateHealthBar()
        {
            m_HealthBarBehaviour = AssetsManager.GetPrefabAssets().InstantiatePrefab(AssetsManager.GetPrefabAssets().PlayerHealthBarPrefab);
            m_HealthBarBehaviour.RectTransform.SetParent(FindObjectOfType<Canvas>().transform);
            m_HealthBarBehaviour.RectTransform.anchoredPosition3D = Vector3.zero;
        }

        protected override void DestroyHealthBar()
        {
            m_HealthBarBehaviour.SetProgress(0, 1);
        }
    }
}
