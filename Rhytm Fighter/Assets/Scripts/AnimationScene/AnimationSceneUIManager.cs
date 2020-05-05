using RhytmFighter.Animation;
using RhytmFighter.Core.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.AnimationScene
{
    public class AnimationSceneUIManager : MonoBehaviour
    {
        public RectTransform Parent;
        public Button AnimationButtonPrefab;

        private AbstractAnimationController m_AnimationController;

        public void AddButton(AnimationTypes type, string key)
        {
            Button btn = Instantiate(AnimationButtonPrefab);
            btn.GetComponent<RectTransform>().SetParent(Parent);
            btn.transform.localScale = Vector3.one;
            btn.onClick.AddListener(() => { m_AnimationController.PlayAnimation(type); });

            Text text = btn.GetComponentInChildren<Text>();
            text.text = $"<b>{type}</b>\n{key}"; 
        }

        public void SetController(AbstractAnimationController animationController)
        {
            m_AnimationController = animationController;
        }
    }
}
