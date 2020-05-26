using RhytmFighter.Core;
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

       
        public void Initialize()
        {
            //Components
            UIComponent_TickIndicator.Initialize((float)Rhytm.RhytmController.GetInstance().TickDurationSeconds / 8);
            UIComponent_PotionCooldown.Initialize(5);

            //UI States
            m_StateMachine = new UIStateMachine();

            m_UIStateNoUI = new UIState_NoUI(Button_Defence, Text_BattleStatus, UIComponent_TickIndicator, PlayerUIParent, InventoryUIParent);
            m_UIStateBattle = new UIState_Battle(Button_Defence, Text_BattleStatus, UIComponent_TickIndicator, PlayerUIParent);
            m_UIStateAdventure = new UIState_Adventure(Button_Defence, Text_BattleStatus, UIComponent_TickIndicator, PlayerUIParent);

            m_StateMachine.Initialize(m_UIStateNoUI);

            //Buttons
            Button_Potion.onClick.AddListener(ButtonPotion_PressHandler);
        }


        public void ToNoUIState()
        {
            m_StateMachine.ChangeState(m_UIStateNoUI);
        }

        public void ToAdventureUIState()
        {
            m_StateMachine.ChangeState(m_UIStateAdventure);
        }

        public void ToBattleUIState()
        {
            m_StateMachine.ChangeState(m_UIStateBattle);
        }


        public void PrepareForBattle()
        {
            m_StateMachine.ChangeState(m_UIStateBattle);
        }

        public void BattleStart()
        {
            m_UIStateBattle.BattleStarted();
        }

        public void NextEnemy()
        {
            m_UIStateBattle.NextEnemy();
        }

        public void BattleFinish()
        {
            m_StateMachine.ChangeState(m_UIStateAdventure);
        }


        public void ButtonDefence_PressHandler()
        {
            OnButtonDefencePressed?.Invoke();
        }

        public void ButtonPotion_PressHandler()
        {
            if (!UIComponent_PotionCooldown.IsInCooldown && GameManager.Instance.m_Poitions > 0 && GameManager.Instance.PlayerModel.HealthBehaviour.HP < GameManager.Instance.PlayerModel.HealthBehaviour.MaxHP)
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
