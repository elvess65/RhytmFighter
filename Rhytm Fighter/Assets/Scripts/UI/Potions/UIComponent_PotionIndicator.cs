using RhytmFighter.Persistant.Abstract;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.UI.Components
{
    public class UIComponent_PotionIndicator : MonoBehaviour, iUpdatable
    {
        [SerializeField] private Text Text_PotionAmount;
        [SerializeField] private Image Image_Pieces;
        [SerializeField] private UIComponent_Cooldown UIComponent_Cooldown;

        private int piecesAmount;
        private int piecesPerPotion;

        public void Initialize(int piecesAmount, int piecesPerPotion)
        {
            this.piecesAmount = piecesAmount;
            this.piecesPerPotion = piecesPerPotion;

            UpdateImagePieces();
            UpdatePotionAmount();
        }

        public void IncrementPiece()
        {
            piecesAmount++;
            UpdateImagePieces();
            UpdatePotionAmount();
        }

        private void UpdateImagePieces()
        {
            float progress = piecesAmount / (float)piecesPerPotion;
            Image_Pieces.fillAmount = progress;
        }

        private void UpdatePotionAmount()
        {
            int amount = piecesPerPotion % piecesPerPotion;
            Text_PotionAmount.text = $"x{amount}";
        }

        public void PerformUpdate(float deltaTime)
        {
            
        }
    }
}
