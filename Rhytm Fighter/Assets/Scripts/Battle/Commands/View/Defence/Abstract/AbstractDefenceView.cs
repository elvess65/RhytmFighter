using RhytmFighter.Animation;
using UnityEngine;

namespace RhytmFighter.Battle.Command.View
{
    public abstract class AbstractDefenceView : AbstractCommandView
    {
        public AbstractAnimationController AnimationController;

        public override void Initialize(Vector3 senderPos, float existTime, int commandID)
        {
            base.Initialize(senderPos, existTime, commandID);

            AnimationController.Initialize();
        }

        protected override void DisposeView()
        {
            AnimationController.PlayAnimation(Persistant.Enums.AnimationTypes.Hide);

            base.DisposeView(m_DISPOSE_DELAY);
        }
    }
}
