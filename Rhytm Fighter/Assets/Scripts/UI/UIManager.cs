using RhytmFighter.Battle.Core;
using RhytmFighter.Persistant.Abstract;
using RhytmFighter.StateMachines.UIState;
using RhytmFighter.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.UI
{
    public class UIManager : MonoBehaviour, iUpdatable
    {
        public System.Action OnButtonDefencePressed;
        public System.Action OnButtonPotionPressed;

        public UIComponent_TickIndicator UIComponent_TickIndicator;

        [Header("Battle")]
        public Button Button_Defence;
        public Text Text_BattleStatus;

        [Header("Inventory")]
        public Button Button_Potion;
        public Transform InventoryUIParent;
        public Text Text_PotionAmount;
        public UIComponent_Cooldown UIComponent_PotionCooldown;

        [Header("General")]
        public Transform PlayerUIParent;

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

        public void Initialize()
        {
            //Components
            UIComponent_TickIndicator.Initialize((float)Rhytm.RhytmController.GetInstance().TickDurationSeconds / 8);
            UIComponent_PotionCooldown.Initialize(5);

            //UI States
            m_StateMachine = new UIStateMachine();

            m_UIStateNoUI = new UIState_NoUI(Button_Defence, Text_BattleStatus, UIComponent_TickIndicator, PlayerUIParent, InventoryUIParent);
            m_UIStateAdventure = new UIState_Adventure(Button_Defence, Text_BattleStatus, UIComponent_TickIndicator, PlayerUIParent, InventoryUIParent);
            m_UIState_PrepareForBattle = new UIState_PrepareForBattle(Button_Defence, Text_BattleStatus, UIComponent_TickIndicator, PlayerUIParent);
            m_UIState_BattleStart = new UIState_BattleStart(Button_Defence, Text_BattleStatus, UIComponent_TickIndicator, PlayerUIParent);
            m_UIState_WaitNextEnemy = new UIState_WaitNextEnemy(Button_Defence, Text_BattleStatus, UIComponent_TickIndicator, PlayerUIParent);
            m_UIState_BattleFinished = new UIState_BattleFinished(Button_Defence, Text_BattleStatus, UIComponent_TickIndicator, PlayerUIParent);
            m_UIState_GameOver = new UIState_GameOverState(Button_Defence, Text_BattleStatus, UIComponent_TickIndicator, PlayerUIParent, InventoryUIParent);
            m_UIState_LevelComplete = new UIState_LevelComplete(Button_Defence, Text_BattleStatus, UIComponent_TickIndicator, PlayerUIParent, InventoryUIParent);

            m_StateMachine.Initialize(m_UIStateNoUI);

            //Buttons
            Button_Potion.onClick.AddListener(ButtonPotion_PressHandler);
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


        public void ButtonDefence_PressHandler()
        {
            OnButtonDefencePressed?.Invoke();
        }

        public void ButtonPotion_PressHandler()
        {
            if (!UIComponent_PotionCooldown.IsInCooldown && BattleManager.Instance.PlayerDataModel.Inventory.PotionsAmount > 0 &&
                BattleManager.Instance.PlayerModel.HealthBehaviour.HP < BattleManager.Instance.PlayerModel.HealthBehaviour.MaxHP)
            {
                OnButtonPotionPressed?.Invoke();
                UIComponent_PotionCooldown.Cooldown();
            }
        }

        public void PerformUpdate(float deltaTime)
        {
            UIComponent_PotionCooldown.PerformUpdate(deltaTime);
            UIComponent_TickIndicator.PerformUpdate(deltaTime);
        }
    }
}
