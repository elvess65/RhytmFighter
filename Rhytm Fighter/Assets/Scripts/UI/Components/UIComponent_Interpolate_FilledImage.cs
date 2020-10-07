using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.UI.Components
{
    /// <summary>
    /// Контролирует заполнение компонента Image
    /// </summary>
    public class UIComponent_Interpolate_FilledImage : InterpolatableComponent
    {
        public float From;
        public float To;

        [SerializeField]
        private Image ControlledImage;

        [Tooltip("Автоматическое включение/отключение Image при начале/окончании интерполирования")]
        [SerializeField] private bool AutoActivation = false;

        public float CurrentValue => ControlledImage.fillAmount;


        public override void Initialize()
        {
            if (ControlledImage == null)
                ControlledImage = GetComponent<Image>();

            if (AutoActivation)
                ControlledImage.enabled = false;
        }

        public override void PrepareForInterpolation()
        {
            ControlledImage.fillAmount = From;

            if (AutoActivation)
                ControlledImage.enabled = false;
        }

        public override void FinishInterpolation()
        {
            if (AutoActivation)
                ControlledImage.enabled = false;
        }

        public override void ProcessInterpolation(float progress)
        {
            ControlledImage.fillAmount = Mathf.Lerp(From, To, progress);
        }
    }
}
