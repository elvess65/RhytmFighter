namespace RhytmFighter.UI.Components
{
    public class UIComponent_ActionPointIndicatorItemRestoreEffect : UIComponent_Interpolate_FilledImage
    {
        public override void PrepareForInterpolation()
        {
            base.PrepareForInterpolation();

            UnityEngine.Color color = m_ControlledImage.color;
            color.a = 0.5f;
            m_ControlledImage.color = color;
        }

        public override void FinishInterpolation()
        {
            base.FinishInterpolation();

            ProcessInterpolation(1);

            UnityEngine.Color color = m_ControlledImage.color;
            color.a = 1f;
            m_ControlledImage.color = color;
        }
    }
}
