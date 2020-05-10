using RhytmFighter.Enviroment.Effects;
using UnityEngine;

namespace RhytmFighter.Battle.Command.View
{
    public abstract class AbstractProjectileView : AbstractCommandView
    {
        public virtual void Initialize(Vector3 targetPos, Vector3 senderPos, float existTime, int commandID)
        {
            m_LerpData.To = targetPos;
            transform.rotation = Quaternion.LookRotation(targetPos - senderPos);

            base.Initialize(senderPos, existTime, commandID);
        }

        protected override void DisposeView()
        {
            Assets.AssetsManager.GetPrefabAssets().InstantiatePrefab<AbstractVisualEffect>(Assets.AssetsManager.GetPrefabAssets().ProjectileImpactEffectPrefab,
                                                                                        transform.position,
                                                                                        Quaternion.identity).ScheduleHideView();

            base.DisposeView();
        }
    }
}
