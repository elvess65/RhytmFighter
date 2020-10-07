using FrameworkPackage.Utils;
using RhytmFighter.Persistant.Abstract;
using RhytmFighter.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.UI.Widgets
{
    public class UIWidget_PotionIndicator : MonoBehaviour, iUpdatable
    {
        [SerializeField] private Button WidgetButton;
        [SerializeField] private Text Text_Amount;
        [SerializeField] private UIComponent_InterpolatableGroup UIComponent_CooldownGroup;
        [SerializeField] private UIComponent_Interpolate_FilledImage UIComponent_PiecesAmount;

        private int piecesAmount;
        private int piecesPerPotion;
        private InterpolationData<float> piecesAmountLerpData;

        private int potionsAmount => piecesAmount / piecesPerPotion;


        public void Initialize(int piecesAmount, int piecesPerPotion, float cooldownTime)
        {
            this.piecesAmount = piecesAmount;
            this.piecesPerPotion = piecesPerPotion;

            UIComponent_CooldownGroup.Initialize(cooldownTime);
            UIComponent_PiecesAmount.Initialize();

            piecesAmountLerpData = new InterpolationData<float>(1);
            WidgetButton.onClick.AddListener(WidgetPressHandler);

            UpdateImagePieces(true);
            UpdatePotionAmount();
        }

        public void PerformUpdate(float deltaTime)
        {
            if (piecesAmountLerpData.IsStarted)
            {
                piecesAmountLerpData.Increment();
                UIComponent_PiecesAmount.ProcessInterpolation(piecesAmountLerpData.Progress);

                if (piecesAmountLerpData.Overtime())
                {
                    piecesAmountLerpData.Stop();
                    UIComponent_PiecesAmount.FinishInterpolation();
                    UpdatePotionAmount();
                }
            }
        }

        public void RefreshAmount(int newPiecesAmount)
        {
            piecesAmount = newPiecesAmount;
            UpdateImagePieces(false);
        }


        private void UpdateImagePieces(bool silent)
        {
            //Progress
            float progress = 1 - (piecesAmount / (float)piecesPerPotion);

            //If more than one potion exists
            if (piecesAmount > piecesPerPotion)
            {
                UpdatePotionAmount();
                return;
            }

            //Prepare animation
            UIComponent_PiecesAmount.From = silent ? progress : UIComponent_PiecesAmount.CurrentValue;
            UIComponent_PiecesAmount.To = progress;
            UIComponent_PiecesAmount.PrepareForInterpolation();

            //Start animation
            piecesAmountLerpData.Start();
        }

        private void UpdatePotionAmount()
        {
            int amount = potionsAmount;
            Text_Amount.text = $"x{amount}";

            Color color = Text_Amount.color;
            color.a = amount == 0 ? 0.5f : 1;
            Text_Amount.color = color;
        }

        private void WidgetPressHandler()
        {
            if (potionsAmount > 0 && !AnyAnimationIsPlaying())
                Debug.Log("Press the widget");
        }

        private bool AnyAnimationIsPlaying() => UIComponent_CooldownGroup.IsInProgress ||
            piecesAmountLerpData.IsStarted;
    }
}
