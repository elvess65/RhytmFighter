using RhytmFighter.Battle.Core;
using UnityEngine;

namespace RhytmFighter.Enviroment
{
    public class DistanceToTargetTracker : MonoBehaviour
    {
        public System.Action OnTargetEntered;
        public System.Action OnTargetExited;

        public float SQRDistanceToTarget = 1;

        private bool m_EnterProcessed = false;
        private Transform m_Target;

        void Update()
        {
            if (m_Target == null && BattleManager.Instance.PlayerModelShortcut != null)
                m_Target = BattleManager.Instance.PlayerModelShortcut.ViewTransform;

            if (m_Target != null)
            {
                float sqrDistance = (m_Target.position - transform.position).sqrMagnitude;
                if (sqrDistance <= SQRDistanceToTarget)
                {
                    if (!m_EnterProcessed)
                    {
                        m_EnterProcessed = true;
                        OnTargetEntered?.Invoke();
                    }
                }
                else
                {
                    if (m_EnterProcessed)
                    {
                        m_EnterProcessed = false;
                        OnTargetExited?.Invoke();
                    }
                }
            }
        }
    }
}
