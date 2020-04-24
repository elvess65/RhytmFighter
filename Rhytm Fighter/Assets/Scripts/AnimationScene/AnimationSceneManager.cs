using RhytmFighter.Characters.Animation;
using UnityEngine;

namespace RhytmFighter.AnimationScene
{
    public class AnimationSceneManager : MonoBehaviour
    {
        public Transform Parent;

        private AnimationSceneUIManager m_UIManager;

        void Start()
        {
            m_UIManager = GetComponent<AnimationSceneUIManager>();

            AbstractAnimationController animationController = Parent.GetComponentInChildren<AbstractAnimationController>();
            if (animationController != null)
            {
                animationController.Initialize();

                m_UIManager.SetController(animationController);

                for (int i = 0; i < animationController.ExposedAnimationKeys.Length; i++)
                    m_UIManager.AddButton(animationController.ExposedAnimationKeys[i].Type, animationController.ExposedAnimationKeys[i].Key);
            }
        }
    }
}
