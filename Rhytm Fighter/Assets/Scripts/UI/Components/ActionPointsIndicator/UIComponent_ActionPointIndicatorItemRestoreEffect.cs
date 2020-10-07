namespace RhytmFighter.UI.Components
{
    public class UIComponent_ActionPointIndicatorItemRestoreEffect : UIComponent_Interpolate_FilledImage
    {
        private UnityEngine.Color m_CurrentColor;
        private UnityEngine.Vector3 m_InitScale;


        public override void Initialize()
        {
            base.Initialize();

            //m_InitScale = ControlledImage.transform.localScale;
        }

        public override void PrepareForInterpolation()
        {
            base.PrepareForInterpolation();

            //m_CurrentColor = ControlledImage.color;
            m_CurrentColor.a = 0.5f;
            //ControlledImage.color = m_CurrentColor;

            //ControlledImage.transform.localScale = m_InitScale / 2;
        }

        public override void FinishInterpolation()
        {
            base.FinishInterpolation();

            ProcessInterpolation(1);

            //m_CurrentColor = ControlledImage.color;
            m_CurrentColor.a = 1f;
            //ControlledImage.color = m_CurrentColor;

            //ControlledImage.transform.localScale = m_InitScale;
        }
    }
}
