using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.UI.Components
{
    /// <summary>
    /// Контролирует изменение цвета компонента Text.
    /// Автоматически определяет изначальный цвет.
    /// </summary>
    public class UIComponent_Interpolate_TextColor : InterpolatableComponent
    {
        [SerializeField] private Text controlledText;
        [SerializeField] private Color fromColor;

        private Color m_InitColor;

        public override void Initialize()
        {
            if (controlledText == null)
                controlledText = GetComponent<Text>();

            m_InitColor = controlledText.color;
            FinishInterpolation();
        }

        public override void PrepareForInterpolation()
        {
            controlledText.color = fromColor;
        }

        public override void FinishInterpolation()
        {
            
        }

        public override void ProcessInterpolation(float progress)
        {
            controlledText.color = Color.Lerp(fromColor, m_InitColor, progress);
        }
    }
}
