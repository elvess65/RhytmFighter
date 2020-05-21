using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.UI.Components
{
    public class UIComponent_Interpolate_TextColor : InterpolatableComponent
    {
        [SerializeField] private Text m_ControlledText;
        [SerializeField] private Color m_FromColor;

        private Color m_InitColor;

        public override void Initialize()
        {
            m_InitColor = m_ControlledText.color;
            FinishInterpolation();
        }

        public override void PrepareForInterpolation()
        {
            m_ControlledText.color = m_FromColor;
        }

        public override void FinishInterpolation()
        {
            
        }

        public override void ProcessInterpolation(float progress)
        {
            m_ControlledText.color = Color.Lerp(m_FromColor, m_InitColor, progress);
        }
    }
}
