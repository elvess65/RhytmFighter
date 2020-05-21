using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.UI.Components
{
    public class UIComponent_Interpolate_FilledImage : InterpolatableComponent
    {
        [SerializeField] private Image m_ControlledImage;
        [SerializeField] private float m_From;
        [SerializeField] private float m_To;


        public override void Initialize()
        {
            FinishInterpolation();
        }

        public override void PrepareForInterpolation()
        {
            m_ControlledImage.fillAmount = m_From;
            m_ControlledImage.enabled = true;
        }

        public override void FinishInterpolation()
        {
            m_ControlledImage.enabled = false;
        }

        public override void ProcessInterpolation(float progress)
        {
            m_ControlledImage.fillAmount = Mathf.Lerp(m_From, m_To, progress);
        }
    }
}
