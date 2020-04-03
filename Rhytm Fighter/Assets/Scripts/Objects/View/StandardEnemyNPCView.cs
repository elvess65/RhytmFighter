using RhytmFighter.Assets;
using RhytmFighter.UI.Bar;
using RhytmFighter.UI.Tools;
using UnityEngine;

namespace RhytmFighter.Objects.View
{
    public class StandardEnemyNPCView : AbstractBattleNPCView
    {
        public Transform HealthBarParent;

        private SingleBarBehaviour m_HealthBarBehaviour;
        private FollowObject m_HealthBarFollow;

        private const float m_FOLLOW_SPEED = 3;


        public override void PerformUpdate(float deltaTime)
        {
            base.PerformUpdate(deltaTime);

            m_HealthBarFollow?.PerformUpdate(deltaTime);
        }

        public override void NotifyView_TakeDamage(int curHP, int maxHP, int dmg)
        {
            base.NotifyView_TakeDamage(curHP, maxHP, dmg);

            m_HealthBarBehaviour.SetProgress(curHP, maxHP);
        }


        protected override void CreateHealthBar()
        {
            //Create health bar
            m_HealthBarBehaviour = AssetsManager.GetPrefabAssets().InstantiatePrefab(AssetsManager.GetPrefabAssets().EnemyHealthBarPrefab);
            m_HealthBarBehaviour.transform.position = HealthBarParent.position;

            //Create health bar follow controller
            m_HealthBarFollow = new FollowObject();
            m_HealthBarFollow.SetRoot(m_HealthBarBehaviour.transform);
            m_HealthBarFollow.SetSpeed(m_FOLLOW_SPEED);
            m_HealthBarFollow.SetTarget(HealthBarParent);
        }

        protected override void DestroyHealthBar()
        {
            m_HealthBarBehaviour.SetProgress(0, 1);
            Destroy(m_HealthBarBehaviour.gameObject, 1);

            m_HealthBarFollow = null;
        }
    }
}
