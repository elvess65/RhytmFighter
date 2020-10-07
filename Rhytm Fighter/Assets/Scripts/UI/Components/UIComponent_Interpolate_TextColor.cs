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
        [SerializeField] public Color FromColor;

        [SerializeField] private Text controlledText;

        public Color InitColor { get; private set; }


        public override void Initialize()
        {
            if (controlledText == null)
                controlledText = GetComponent<Text>();

            InitColor = controlledText.color;
            FinishInterpolation();
        }

        public override void PrepareForInterpolation()
        {
            controlledText.color = FromColor;
        }

        public override void FinishInterpolation()
        {
            
        }

        public override void ProcessInterpolation(float progress)
        {
            controlledText.color = Color.Lerp(FromColor, InitColor, progress);
        }
    }
}
