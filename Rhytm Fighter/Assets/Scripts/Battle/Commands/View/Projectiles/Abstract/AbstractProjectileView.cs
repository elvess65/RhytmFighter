using UnityEngine;

namespace RhytmFighter.Battle.Command.View
{
    public abstract class AbstractProjectileView : AbstractCommandView
    {
        public GameObject CollisionEffectPrefab;

        public virtual void Initialize(Vector3 targetPos, Vector3 senderPos, float existTime)
        {
            m_LerpData.To = targetPos;

            base.Initialize(senderPos, existTime);
        }

        protected override void FinalizeView()
        {
            base.FinalizeView();

            GameObject collisionEffect = Instantiate(CollisionEffectPrefab, transform.position, Quaternion.identity);
            Destroy(collisionEffect.gameObject, 2);
        }
    }
}
