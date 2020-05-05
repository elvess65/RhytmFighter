using RhytmFighter.Animation;
using RhytmFighter.Core.Enums;
using System;

namespace Frameworks.Grid.View.Cell
{
    public class Animation_CellAppearanceStrategy : iCellAppearanceStrategy
    {
        public event Action<CellView> OnShowed;
        public event Action<CellView> OnHided;

        public bool IsShowed { get; private set; }

        private AnimationStates m_AnimationState;
        private AnimationEventsListener m_EventsListener;
        private AbstractAnimationController m_AnimationController;


        public Animation_CellAppearanceStrategy(AbstractAnimationController animationController)
        {
            m_AnimationState = AnimationStates.None;

            m_AnimationController = animationController;
            m_AnimationController.Initialize();

            m_EventsListener = m_AnimationController.GetComponent<AnimationEventsListener>();
            m_EventsListener.OnEvent += EventsListener_OnEvent;

            IsShowed = true;
        }

        private void EventsListener_OnEvent()
        {
        }

        public void Show()
        {
            if (!m_AnimationController.gameObject.activeSelf)
                m_AnimationController.gameObject.SetActive(true);

            m_AnimationController.PlayAnimation(AnimationTypes.Show);
            IsShowed = true;
        }

        public void Hide(bool hideImmediate)
        {
            if (!hideImmediate)
                m_AnimationController.PlayAnimation(AnimationTypes.Hide);
            else
                m_AnimationController.gameObject.SetActive(false);

            IsShowed = false;
        }
    }
}
