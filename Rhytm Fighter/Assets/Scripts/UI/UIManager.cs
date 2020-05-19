using RhytmFighter.StateMachines.UIState;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.UI
{
    public class UIManager : MonoBehaviour
    {
        public System.Action OnButtonDefencePressed;

        public GameObject BeatIndicator;

        [Header("Battle")]
        public Button Button_Defence;
        public Text Text_BattleStatus;

        [Header("Inventory")]
        public Button Button_Potion;
        public Transform InventoryUIParent;
        public Text Text_PotionAmount;

        [Header("General")]
        public Transform PlayerUIParent;

        private UIStateMachine m_StateMachine;

        //States
        private UIState_NoUI m_UIStateNoUI;
        private UIState_Battle m_UIStateBattle;
        private UIState_Adventure m_UIStateAdventure;

       
        public void Initialize()
        {
            //UI States
            m_StateMachine = new UIStateMachine();

            m_UIStateNoUI = new UIState_NoUI(Button_Defence, Text_BattleStatus, BeatIndicator, PlayerUIParent, InventoryUIParent);
            m_UIStateBattle = new UIState_Battle(Button_Defence, Text_BattleStatus, BeatIndicator, PlayerUIParent);
            m_UIStateAdventure = new UIState_Adventure(Button_Defence, Text_BattleStatus, BeatIndicator, PlayerUIParent);

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
            Debug.Log("ButtonPotion_PressHandler");
        }
    }
}
