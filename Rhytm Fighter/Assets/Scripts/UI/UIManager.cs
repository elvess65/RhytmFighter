using RhytmFighter.Battle.Core;
using RhytmFighter.Persistant;
using RhytmFighter.Persistant.Abstract;
using RhytmFighter.StateMachines.UIState;
using RhytmFighter.UI.Components;
using RhytmFighter.UI.Widgets;
using UnityEngine;
using UnityEngine.UI;
using static RhytmFighter.Data.PlayerData;

namespace RhytmFighter.UI
{
    public class UIManager : MonoBehaviour, iUpdatable
    {
        public System.Action OnButtonDefencePressed;
        public System.Action OnButtonPotionPressed;

        public UIComponent_TickIndicator UIComponent_TickIndicator;
        public UIComponent_ActionPointsIndicator UIComponent_ActionPointsIndicator;

        [Header("Battle")]
        public Button Button_Defence;
        public Text Text_BattleStatus;
        public Text Text_PressToContinue;

        [Header("Inventory")]
        public Button Button_Potion;
        public Transform InventoryUIParent;
        public Text Text_PotionAmount;
        public UIComponent_InterpolatableGroup UIComponent_PotionCooldown;
        public UIWidget_PotionIndicator UIWidget_PotionIndicator;

        [Header("General")]
        public Transform PlayerHealthbarParent;

        private UIStateMachine m_StateMachine;

        //States
        private UIState_NoUI m_UIStateNoUI;
        private UIState_Battle m_UIStateBattle;
        private UIState_Adventure m_UIStateAdventure;
        private UIState_PrepareForBattle m_UIState_PrepareForBattle;
        private UIState_BattleStart m_UIState_BattleStart;
        private UIState_WaitNextEnemy m_UIState_WaitNextEnemy;
        private UIState_BattleFinished m_UIState_BattleFinished;
        private UIState_GameOverState m_UIState_GameOver;
        private UIState_LevelComplete m_UIState_LevelComplete;
        private UIState_TapToActionState m_UIState_TapToActionState;

        public void Initialize()
        {
            //Components
            UIComponent_TickIndicator.Initialize((float)Rhytm.RhytmController.GetInstance().TickDurationSeconds / 8);
            UIComponent_PotionCooldown.Initialize(5);

            PotionData potionData = GameManager.Instance.DataHolder.PlayerDataModel.Inventory.GetPotionByType(Persistant.Enums.PotionTypes.Heal);
            UIWidget_PotionIndicator.Initialize(potionData.PiecesAmount, potionData.PiecesPerPotion, 5);

            /*UIComponent_ActionPointsIndicator.Initialize(GameManager.Instance.DataHolder.PlayerDataModel.ActionPoints, 
                                                         (float)(Rhytm.RhytmController.GetInstance().TickDurationSeconds * 
                                                         GameManager.Instance.DataHolder.PlayerDataModel.TickToRestoreActionPoint + 
                                                         Rhytm.RhytmController.GetInstance().ProcessTickDelta));*/

            Text_PressToContinue.gameObject.SetActive(false);

            //UI States
            m_StateMachine = new UIStateMachine();

            m_UIStateAdventure = new UIState_Adventure                  (UIComponent_TickIndicator, PlayerHealthbarParent, InventoryUIParent);
            m_UIState_TapToActionState = new UIState_TapToActionState   (Text_BattleStatus, Text_PressToContinue);

            m_UIState_PrepareForBattle = new UIState_PrepareForBattle   (Button_Defence, Text_BattleStatus, UIComponent_TickIndicator, UIComponent_ActionPointsIndicator);
            m_UIState_BattleStart = new UIState_BattleStart             (Button_Defence, Text_BattleStatus, UIComponent_TickIndicator, UIComponent_ActionPointsIndicator);
            m_UIState_WaitNextEnemy = new UIState_WaitNextEnemy         (Button_Defence, Text_BattleStatus, UIComponent_TickIndicator, UIComponent_ActionPointsIndicator);
            m_UIState_BattleFinished = new UIState_BattleFinished       (Text_BattleStatus, UIComponent_TickIndicator);

            m_UIStateNoUI = new UIState_NoUI                            (Button_Defence, Text_BattleStatus, UIComponent_TickIndicator, UIComponent_ActionPointsIndicator, PlayerHealthbarParent, InventoryUIParent);
            m_UIState_GameOver = new UIState_GameOverState              (Button_Defence, Text_BattleStatus, UIComponent_TickIndicator, UIComponent_ActionPointsIndicator, PlayerHealthbarParent, InventoryUIParent);
            m_UIState_LevelComplete = new UIState_LevelComplete         (Button_Defence, Text_BattleStatus, UIComponent_TickIndicator, UIComponent_ActionPointsIndicator, PlayerHealthbarParent, InventoryUIParent);
            

            m_StateMachine.Initialize(m_UIStateNoUI);

            //Buttons
            Button_Potion.onClick.AddListener(ButtonPotion_PressHandler);

            //Events
            BattleManager.Instance.OnPotionUsed += PotionUsedHandler;
        }


        public void ToAdventureUIState()
        {
            m_StateMachine.ChangeState(m_UIStateAdventure);
        }

        public void ToPrepareForBattleUIState()
        {
            m_StateMachine.ChangeState(m_UIState_PrepareForBattle);
        }

        public void ToBattleStartUIState()
        {
            m_StateMachine.ChangeState(m_UIState_BattleStart);
        }

        public void ToWaitingForNextEnemyActivationUIState()
        {
            m_StateMachine.ChangeState(m_UIState_WaitNextEnemy);
        }

        public void ToBattleFinishedUIState()
        {
            m_StateMachine.ChangeState(m_UIState_BattleFinished);
        }

        public void ToGameOverUIState()
        {
            m_StateMachine.ChangeState(m_UIState_GameOver);
        }

        public void ToLevelComleteUIState()
        {
            m_StateMachine.ChangeState(m_UIState_LevelComplete);
        }

        public void ToTapToActionUIState()
        {
            m_StateMachine.ChangeState(m_UIState_TapToActionState);
        }


        public void UseActionPoint(int curActionPoints)
        {
            UIComponent_ActionPointsIndicator.UseActionPoint(curActionPoints);
        }

        public void RestoreActionPoint(int curActionPoints)
        {
            UIComponent_ActionPointsIndicator.RestoreActionPoint(curActionPoints);
        }

        public void RestoreAllActionPoints()
        {
            UIComponent_ActionPointsIndicator.RestoreAllActionPoints();
        }


        public void UpdatePotionAmount()
        {
            UIWidget_PotionIndicator.RefreshAmount(GameManager.Instance.DataHolder.PlayerDataModel.
                Inventory.GetPotionByType(Persistant.Enums.PotionTypes.Heal).PiecesAmount);
        }


        public void ButtonDefence_PressHandler()
        {
            OnButtonDefencePressed?.Invoke();
        }

        public void ButtonPotion_PressHandler()
        {
            if (!UIComponent_PotionCooldown.IsInProgress)
                OnButtonPotionPressed?.Invoke();
        }

        public void PerformUpdate(float deltaTime)
        {
            UIComponent_PotionCooldown.PerformUpdate(deltaTime);
            UIComponent_TickIndicator.PerformUpdate(deltaTime);
            UIComponent_ActionPointsIndicator.PerformUpdate(deltaTime);
            UIWidget_PotionIndicator.PerformUpdate(deltaTime);
        }
        


        private void PotionUsedHandler()
        {
            UIComponent_PotionCooldown.Execute();
        }
    }
}
