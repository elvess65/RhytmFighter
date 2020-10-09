using RhytmFighter.Battle.Core;
using RhytmFighter.Persistant;
using RhytmFighter.Persistant.Abstract;
using RhytmFighter.Persistant.Enums;
using RhytmFighter.StateMachines.UIState;
using RhytmFighter.UI.Components;
using RhytmFighter.UI.View;
using RhytmFighter.UI.Widget;
using UnityEngine;
using UnityEngine.UI;
using static RhytmFighter.Data.PlayerData;

namespace RhytmFighter.UI
{
    public class UIManager : MonoBehaviour, iUpdatable
    {
        public System.Action OnButtonDefencePressed;
        public System.Action OnTryUsePotion;

        public UIComponent_ActionPointsIndicator UIComponent_ActionPointsIndicator;

        [Header("Battle")]
        public Button Button_Defence;
        public Text Text_BattleStatus;
        public Text Text_PressToContinue;

        [Header("Views")]
        public UIView_InventoryHUD UIView_InventoryHUD;
        public UIView_PlayerHUD UIView_PlayerHUD;
        public UIView_BattleHUD UIView_BattleHUD;

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

        private iUpdatable[] m_Updatables;

        public void Initialize()
        {
            //Components
            UIView_InventoryHUD.Initialize();
            UIView_InventoryHUD.OnWidgetPotionPress += UIView_Inventory_Potion_WidgetPress;

            UIView_PlayerHUD.Initialize();
            UIView_BattleHUD.Initialize();


            /*UIComponent_ActionPointsIndicator.Initialize(GameManager.Instance.DataHolder.PlayerDataModel.ActionPoints, 
                                                         (float)(Rhytm.RhytmController.GetInstance().TickDurationSeconds * 
                                                         GameManager.Instance.DataHolder.PlayerDataModel.TickToRestoreActionPoint + 
                                                         Rhytm.RhytmController.GetInstance().ProcessTickDelta));*/

            Text_PressToContinue.gameObject.SetActive(false);

            //UI States
            m_StateMachine = new UIStateMachine();

            m_UIStateAdventure = new UIState_Adventure                  (UIView_PlayerHUD.UIWidget_Tick, UIView_PlayerHUD.PlayerHealthBarParent, UIView_InventoryHUD.Root);
            m_UIState_TapToActionState = new UIState_TapToActionState   (Text_BattleStatus, Text_PressToContinue);

            m_UIState_PrepareForBattle = new UIState_PrepareForBattle   (Button_Defence, Text_BattleStatus, UIView_PlayerHUD.UIWidget_Tick, UIComponent_ActionPointsIndicator);
            m_UIState_BattleStart = new UIState_BattleStart             (Button_Defence, Text_BattleStatus, UIView_PlayerHUD.UIWidget_Tick, UIComponent_ActionPointsIndicator);
            m_UIState_WaitNextEnemy = new UIState_WaitNextEnemy         (Button_Defence, Text_BattleStatus, UIView_PlayerHUD.UIWidget_Tick, UIComponent_ActionPointsIndicator);
            m_UIState_BattleFinished = new UIState_BattleFinished       (Text_BattleStatus, UIView_PlayerHUD.UIWidget_Tick);

            m_UIStateNoUI = new UIState_NoUI                            (Button_Defence, Text_BattleStatus, UIView_PlayerHUD.UIWidget_Tick, UIComponent_ActionPointsIndicator, UIView_PlayerHUD.PlayerHealthBarParent, UIView_InventoryHUD.Root);
            m_UIState_GameOver = new UIState_GameOverState              (Button_Defence, Text_BattleStatus, UIView_PlayerHUD.UIWidget_Tick, UIComponent_ActionPointsIndicator, UIView_PlayerHUD.PlayerHealthBarParent, UIView_InventoryHUD.Root);
            m_UIState_LevelComplete = new UIState_LevelComplete         (Button_Defence, Text_BattleStatus, UIView_PlayerHUD.UIWidget_Tick, UIComponent_ActionPointsIndicator, UIView_PlayerHUD.PlayerHealthBarParent, UIView_InventoryHUD.Root);
            

            m_StateMachine.Initialize(m_UIStateNoUI);

            //Updatables
            m_Updatables = new iUpdatable[]
            {
                UIView_InventoryHUD,
                UIView_PlayerHUD,
                UIView_BattleHUD
            };
        }

        public void PerformUpdate(float deltaTime)
        {
            UIComponent_ActionPointsIndicator.PerformUpdate(deltaTime);

            for (int i = 0; i < m_Updatables.Length; i++)
                m_Updatables[i].PerformUpdate(deltaTime);
        }

        private void InitializeViews()
        {

        }

        private void InitializeStateMachine()
        {

        }


        #region STATES

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

        #endregion

        #region VIEWS

        #region - ACTION POINTS

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

        #endregion

        #region - INVENTORY

        #region - POTIONS

        private void UIView_Inventory_Potion_WidgetPress()
        {
            OnTryUsePotion?.Invoke();
        }

        #endregion

        #endregion

        #region - BATTLE

        public void ButtonDefence_PressHandler()
        {
            OnButtonDefencePressed?.Invoke();
        }

        #endregion

        #endregion
    }
}
