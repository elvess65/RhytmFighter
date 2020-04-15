using UnityEngine;

namespace RhytmFighter.Battle.Command.View
{
    public class SimpleProjectileView : AbstractProjectileView
    {
        protected override void ProcessUpdate()
        {
            transform.position = Vector3.Lerp(m_LerpData.From, m_LerpData.To, m_LerpData.Progress);
        }
    }
}
