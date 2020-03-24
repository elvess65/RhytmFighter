using RhytmFighter.Battle;
using UnityEngine;

namespace RhytmFighter.Objects.View
{
    public class AbstractBattleNPCView : AbstractNPCView, iBattleModelViewProxy
    {
        public virtual void ExecuteAction()
        {
        }

        public virtual void TakeDamage()
        {
        }
    }
}
