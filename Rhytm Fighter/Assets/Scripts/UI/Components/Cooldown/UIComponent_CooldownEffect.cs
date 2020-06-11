namespace RhytmFighter.UI.Components
{
    public class UIComponent_CooldownEffect : UIComponent_Interpolate_FilledImage
    {
        public override void Initialize()
        {
            FinishInterpolation();
        }

        public override void PrepareForInterpolation()
        {
            base.PrepareForInterpolation();

            m_ControlledImage.enabled = true;
        }

        public override void FinishInterpolation()
        {
            base.FinishInterpolation();

            m_ControlledImage.enabled = false;
        }
    }
}
