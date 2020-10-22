using RhytmFighter.Persistant;
using RhytmFighter.Persistant.Abstract;
using RhytmFighter.Persistant.Enums;
using RhytmFighter.UI.Widget;
using UnityEngine;
using static RhytmFighter.Data.PlayerData;

namespace RhytmFighter.UI.View
{
    /// <summary>
    /// Отображение виджетов инвентаря в HUD 
    /// </summary>
    public class UIView_InventoryHUD : UIView_Abstract
    {
        public System.Action OnWidgetPotionPress;

        [Header("Widgets")]
        public UIWidget_Potion UIWidget_Potion;

  
        public override void Initialize()
        {
            //Widget - Potion
            PotionData potionData = GameManager.Instance.DataHolder.PlayerDataModel.Inventory.GetPotionByType(PotionTypes.Heal);
            UIWidget_Potion.Initialize(potionData.PiecesAmount, potionData.PiecesPerPotion, 5);
            UIWidget_Potion.OnWidgetPress += WidgetPotion_PressHandler;

            RegisterWidget(UIWidget_Potion);
            RegisterUpdatable(UIWidget_Potion);
        }

        #region WIDGET HANDLERS

        public void WidgetPotion_UpdateAmount()
        {
            UIWidget_Potion.RefreshAmount(GameManager.Instance.DataHolder.PlayerDataModel.
                Inventory.GetPotionByType(PotionTypes.Heal).PiecesAmount);
        }

        public void WidgetPotion_UsePotion(bool isSuccess)
        {
            if (isSuccess)
            {
                UIWidget_Potion.UsePotion(GameManager.Instance.DataHolder.PlayerDataModel.
                    Inventory.GetPotionByType(PotionTypes.Heal).PiecesAmount);
            }
            else
                Debug.LogError("Can't use potion");
        }

        private void WidgetPotion_PressHandler()
        {
            OnWidgetPotionPress?.Invoke();  
        }

        #endregion
    }
}
