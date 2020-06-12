namespace RhytmFighter.UI.Components
{
    public class UIComponent_ActionPointIndicatorItemRestoreEffect : UIComponent_Interpolate_FilledImage
    {
        private UnityEngine.Color m_CurrentColor;
        private UnityEngine.Vector3 m_InitScale;


        public override void Initialize()
        {
            base.Initialize();

            m_InitScale = m_ControlledImage.transform.localScale;
        }

        public override void PrepareForInterpolation()
        {
            base.PrepareForInterpolation();

            m_CurrentColor = m_ControlledImage.color;
            m_CurrentColor.a = 0.5f;
            m_ControlledImage.color = m_CurrentColor;

            m_ControlledImage.transform.localScale = m_InitScale / 2;
        }

        public override void FinishInterpolation()
        {
            base.FinishInterpolation();

            ProcessInterpolation(1);

            m_CurrentColor = m_ControlledImage.color;
            m_CurrentColor.a = 1f;
            m_ControlledImage.color = m_CurrentColor;

            m_ControlledImage.transform.localScale = m_InitScale;
        }
    }
}
