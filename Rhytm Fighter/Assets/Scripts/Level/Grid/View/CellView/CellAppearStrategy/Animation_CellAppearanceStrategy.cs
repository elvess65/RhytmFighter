using RhytmFighter.Animation;
using RhytmFighter.Core.Enums;
using System;
using UnityEngine;

namespace Frameworks.Grid.View.Cell
{
    public class Animation_CellAppearanceStrategy : iCellAppearanceStrategy
    {
        public event Action OnShowed;
        public event Action OnHided;

        public bool IsShowed { get; private set; }

        private AnimationStates m_AnimationState;
        private Collider m_ViewCollider;
        private Abstract_CellContentView m_ContentView;
        private AnimationEventsListener m_EventsListener;
        private AbstractAnimationController m_AnimationController;


        public Animation_CellAppearanceStrategy(AbstractAnimationController animationController, Collider viewCollider, Abstract_CellContentView contentView)
        {
            m_AnimationState = AnimationStates.None;
            m_ViewCollider = viewCollider;
            m_ContentView = contentView;

            m_AnimationController = animationController;
            m_AnimationController.Initialize();

            m_EventsListener = m_AnimationController.GetComponent<AnimationEventsListener>();
            m_EventsListener.OnActionEvent += EventsListener_OnEvent;

            OnShowed += ShowedHandler;

            IsShowed = true;
        }

        public void Show()
        {
            if (!m_ContentView.Graphics.activeSelf)
                m_AnimationController.StartCoroutine(WaitEndOfFrameBeforeActivateContent());

            m_AnimationState = AnimationStates.Showing;
            m_AnimationController.PlayAnimation(AnimationTypes.Show);

            IsShowed = true;
        }

        public void Hide(bool hideImmediate)
        {
            if (!hideImmediate)
            {
                m_AnimationState = AnimationStates.Hiding;
                m_AnimationController.PlayAnimation(AnimationTypes.Hide);
            }
            else
            {
                m_ContentView.Graphics.SetActive(false);
                m_AnimationState = AnimationStates.None;
            }

            m_ViewCollider.enabled = false;
            IsShowed = false;
        }


        private void ShowedHandler()
        {
            m_ViewCollider.enabled = true;
        }

        private void EventsListener_OnEvent()
        {
            switch(m_AnimationState)
            {
                case AnimationStates.Showing:
                    OnShowed?.Invoke();
                    break;
                case AnimationStates.Hiding:
                    OnHided?.Invoke();
                    break;
            }

            m_AnimationState = AnimationStates.None;
        }

        private System.Collections.IEnumerator WaitEndOfFrameBeforeActivateContent()
        {
            yield return null;
            m_ContentView.Graphics.SetActive(true);
        }
    }
}
