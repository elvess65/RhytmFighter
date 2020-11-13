using RhytmFighter.Data;
using RhytmFighter.Persistant;
using RhytmFighter.UI.Widget;
using UnityEngine;

namespace RhytmFighter.UI.View
{
    /// <summary>
    /// Отображение виджетов в Forge
    /// </summary>
    public class UIView_ForgeScene : UIView_Abstract
    {
        public System.Action OnClose;

        [Space(10)]
        [SerializeField] private UIWidget_Button UIWidget_ButtonClose;
        [SerializeField] private UIWidget_Button UIWidget_ButtonAdd;
        [SerializeField] private UIWidget_Currency UIWidget_Currency;
        [SerializeField] private UIWidget_ExperianceBar UIWidget_ExperianceBar;


        public override void Initialize()
        {
            UIWidget_ButtonClose.OnWidgetPress += UIWidget_ButtonClose_PressHandler;
            UIWidget_ButtonAdd.OnWidgetPress += UIWidget_ButtonAdd_PressHandler;

            UIWidget_Currency.Initialize(GameManager.Instance.DataHolder.AccountModel.CurrencyAmount);

            int selectedCharacterID = GameManager.Instance.DataHolder.BattleSessionModel.SelectedCharactedID;
            UIWidget_ExperianceBar.Initialize(selectedCharacterID, DataHelper.GetCharacterData(selectedCharacterID).WeaponExperiance);

            RegisterWidget(UIWidget_ButtonClose);
            RegisterWidget(UIWidget_ButtonAdd);
            RegisterWidget(UIWidget_Currency);
            RegisterWidget(UIWidget_ExperianceBar);

            RegisterUpdatable(UIWidget_Currency);
            RegisterUpdatable(UIWidget_ExperianceBar);
        }

        private void UIWidget_ButtonClose_PressHandler()
        {
            OnClose?.Invoke();
        }

        private void UIWidget_ButtonAdd_PressHandler()
        {
            Debug.Log("Add");
        }
    }
}
