using UnityEngine;

namespace RhytmFighter.Battle.Command.View
{
    public abstract class AbstractProjectileView : AbstractCommandView
    {
        public virtual void Initialize(Vector3 targetPos, Vector3 senderPos, float existTime)
        {
            m_LerpData.To = targetPos;

            base.Initialize(senderPos, existTime);
        }
    }
}
