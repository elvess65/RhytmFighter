using UnityEngine;

namespace RhytmFighter.Battle.Command.View
{
    public abstract class AbstractDefenceView : AbstractCommandView
    {
        protected override void FinalizeView()
        {
            Animator animator = GetComponent<Animator>();
            if (animator != null)
                animator.SetTrigger("Hide");

            base.FinalizeView();
        }
    }
}
