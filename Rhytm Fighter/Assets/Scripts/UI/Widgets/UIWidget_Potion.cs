using FrameworkPackage.Utils;
using RhytmFighter.Persistant.Abstract;
using RhytmFighter.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.UI.Widget
{
    public class UIWidget_Potion : MonoBehaviour, iUpdatable
    {
        public System.Action OnWidgetPress;

        [SerializeField] private Button WidgetButton;
        [SerializeField] private Image WidgetImage;
        [SerializeField] private Text Text_Amount;
        [SerializeField] private UIComponent_InterpolatableGroup UIComponent_CooldownGroup;
        [SerializeField] private UIComponent_Interpolate_FilledImage UIComponent_PiecesAmount;

        private int m_PiecesAmount;
        private int m_PiecesPerPotion;
        private InterpolationData<float> m_PiecesAmountLerpData;

        private int PotionsAmount => m_PiecesAmount / m_PiecesPerPotion;
        private UIComponent_Interpolate_TextColor m_UIComponent_Interpolate_TextColor_Amount;


        public void Initialize(int piecesAmount, int piecesPerPotion, float cooldownTime)
        {
            m_PiecesAmount = piecesAmount;
            m_PiecesPerPotion = piecesPerPotion;
            m_PiecesAmountLerpData = new InterpolationData<float>(1);

            UIComponent_CooldownGroup.Initialize(cooldownTime);
            UIComponent_PiecesAmount.Initialize();
      
            WidgetButton.onClick.AddListener(WidgetPressHandler);
            m_UIComponent_Interpolate_TextColor_Amount = Text_Amount.GetComponent<UIComponent_Interpolate_TextColor>();

            UpdateAll(true);
        }

        public void PerformUpdate(float deltaTime)
        {
            if (m_PiecesAmountLerpData.IsStarted)
            {
                m_PiecesAmountLerpData.Increment();
                UIComponent_PiecesAmount.ProcessInterpolation(m_PiecesAmountLerpData.Progress);

                if (m_PiecesAmountLerpData.Overtime())
                {
                    m_PiecesAmountLerpData.Stop();
                    UIComponent_PiecesAmount.FinishInterpolation();

                    if (PotionsAmount > 0)
                    {
                        int remainPieces = m_PiecesAmount % m_PiecesPerPotion;
                        float progress = remainPieces / (float)m_PiecesPerPotion;

                        UIComponent_PiecesAmount.From = progress;
                        UIComponent_PiecesAmount.PrepareForInterpolation();
                        UIComponent_PiecesAmount.FinishInterpolation();
                    }

                    UpdateAmountAndBackground();
                }
            }

            UIComponent_CooldownGroup.PerformUpdate(deltaTime);
        }

        public void RefreshAmount(int newPiecesAmount)
        {
            m_PiecesAmount = newPiecesAmount;
            UpdateAll(false);
        }

        public void UsePotion(int newPiecesAmount)
        {
            m_PiecesAmount = newPiecesAmount;

            UIComponent_CooldownGroup.Execute();
            UpdateAmountAndBackground();
        }


        private void UpdateAll(bool silent)
        {
            //Progress
            float progress = m_PiecesAmount / (float)m_PiecesPerPotion;

            if (progress > 1)
            {
                if (progress == PotionsAmount)
                    progress = progress / PotionsAmount;
                else
                    progress = progress % PotionsAmount;
            }

            if (silent)
            {
                UIComponent_PiecesAmount.From = progress;
                UIComponent_PiecesAmount.PrepareForInterpolation();
                UIComponent_PiecesAmount.FinishInterpolation();

                UpdateAmountAndBackground();

                return;
            }

            //Prepare animation
            UIComponent_PiecesAmount.From = UIComponent_PiecesAmount.CurrentValue;
            UIComponent_PiecesAmount.To = progress;
            UIComponent_PiecesAmount.PrepareForInterpolation();

            //Start animation
            m_PiecesAmountLerpData.Start();
        }

        private void UpdateAmountAndBackground()
        {
            int amount = PotionsAmount;

            Text_Amount.text = $"x{amount}";
            Text_Amount.color = amount == 0 ? m_UIComponent_Interpolate_TextColor_Amount.FromColor :
                                              m_UIComponent_Interpolate_TextColor_Amount.InitColor;

            Color color = WidgetImage.color;
            color.a = amount == 0 ? 0.5f : 1;
            WidgetImage.color = color;
        }


        private void WidgetPressHandler()
        {
            if (PotionsAmount > 0 && !AnyAnimationIsPlaying())
                OnWidgetPress?.Invoke();
        }


        private bool AnyAnimationIsPlaying() =>
            UIComponent_CooldownGroup.IsInProgress ||
            m_PiecesAmountLerpData.IsStarted;
    }
}
