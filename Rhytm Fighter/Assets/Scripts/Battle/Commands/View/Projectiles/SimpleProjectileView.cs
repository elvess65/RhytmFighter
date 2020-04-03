using FrameworkPackage.Utils;
using UnityEngine;

namespace RhytmFighter.Battle.Command.View
{
    public class SimpleProjectileView : AbstractProjectileView
    {
        private InterpolationData<Vector3> m_MoveData;

        public override void Initialize(Vector3 senderPos, Vector3 targetPos, float applyTime)
        {
            m_MoveData = new InterpolationData<Vector3>(applyTime);
            m_MoveData.From = senderPos;
            m_MoveData.To = targetPos;
            m_MoveData.Start();
        }

        public override void PerformUpdate(float deltaTime)
        {
            if (m_MoveData.IsStarted)
            {
                m_MoveData.Increment();
                transform.position = Vector3.Lerp(m_MoveData.From, m_MoveData.To, m_MoveData.Progress);

                if (m_MoveData.Overtime())
                    DestroyProjectile();
            }
        }
    }
}
