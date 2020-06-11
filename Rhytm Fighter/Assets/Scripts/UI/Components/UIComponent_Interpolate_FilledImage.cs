using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.UI.Components
{
    public class UIComponent_Interpolate_FilledImage : InterpolatableComponent
    {
        [SerializeField] protected Image m_ControlledImage;
        [SerializeField] protected float m_From;
        [SerializeField] protected float m_To;


        public override void Initialize()
        {
        }

        public override void PrepareForInterpolation()
        {
            m_ControlledImage.fillAmount = m_From;
        }

        public override void FinishInterpolation()
        {
        }

        public override void ProcessInterpolation(float progress)
        {
            m_ControlledImage.fillAmount = Mathf.Lerp(m_From, m_To, progress);
        }
    }
}
