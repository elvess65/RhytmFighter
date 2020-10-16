using System;
using System.Collections.Generic;
using RhytmFighter.Persistant.Abstract;
using RhytmFighter.StateMachines.UIState;
using RhytmFighter.UI.View;
using UnityEngine;

namespace RhytmFighter.UI
{
    public class UIManager : MonoBehaviour, iUpdatable
    {
        public Action OnTryDefence;
        public Action OnTryUsePotion;

        [Header("Views")]
        public UIView_InventoryHUD UIView_InventoryHUD;
        public UIView_PlayerHUD UIView_PlayerHUD;
        public UIView_BattleHUD UIView_BattleHUD;

        private UIStateMachine m_StateMachine;

        private iUpdatable[] m_Updatables;
        private Dictionary<Type, UIState_Abstract> m_InitializedStates;


        public void Initialize()
        {
            InitializeViews();
            InitializeStateMachine();
            InitializeUpdatables();
        }

        public void PerformUpdate(float deltaTime)
        {
            for (int i = 0; i < m_Updatables.Length; i++)
                m_Updatables[i].PerformUpdate(deltaTime);
        }

        public void ChangeState<T>() where T : UIState_Abstract
        {
            m_StateMachine.ChangeState(m_InitializedStates[typeof(T)]);
        }


        private void InitializeViews()
        {
            UIView_InventoryHUD.Initialize();
            UIView_InventoryHUD.OnWidgetPotionPress += UIView_Inventory_Potion_WidgetPress;

            UIView_BattleHUD.Initialize();
            UIView_BattleHUD.OnWidgetDefencePointerDown += UIView_Battle_Defence_PressHandler;

            UIView_PlayerHUD.Initialize();

            /*UIComponent_ActionPointsIndicator.Initialize(GameManager.Instance.DataHolder.PlayerDataModel.ActionPoints, 
                                                         (float)(Rhytm.RhytmController.GetInstance().TickDurationSeconds * 
                                                         GameManager.Instance.DataHolder.PlayerDataModel.TickToRestoreActionPoint + 
                                                         Rhytm.RhytmController.GetInstance().ProcessTickDelta));*/

        }

        private void InitializeStateMachine()
        {
            m_StateMachine = new UIStateMachine();
            m_InitializedStates = new Dictionary<Type, UIState_Abstract>();

            m_InitializedStates.Add(typeof(UIState_Adventure), new UIState_Adventure(UIView_InventoryHUD, UIView_PlayerHUD, UIView_BattleHUD));
            m_InitializedStates.Add(typeof(UIState_TapToActionState), new UIState_TapToActionState(UIView_InventoryHUD, UIView_PlayerHUD, UIView_BattleHUD));

            m_InitializedStates.Add(typeof(UIState_PrepareForBattle), new UIState_PrepareForBattle(UIView_InventoryHUD, UIView_PlayerHUD, UIView_BattleHUD));
            m_InitializedStates.Add(typeof(UIState_BattleStart), new UIState_BattleStart(UIView_InventoryHUD, UIView_PlayerHUD, UIView_BattleHUD));
            m_InitializedStates.Add(typeof(UIState_WaitNextEnemy), new UIState_WaitNextEnemy(UIView_InventoryHUD, UIView_PlayerHUD, UIView_BattleHUD));
            m_InitializedStates.Add(typeof(UIState_BattleFinished), new UIState_BattleFinished(UIView_InventoryHUD, UIView_PlayerHUD, UIView_BattleHUD));

            m_InitializedStates.Add(typeof(UIState_NoUI), new UIState_NoUI(UIView_InventoryHUD, UIView_PlayerHUD, UIView_BattleHUD));
            m_InitializedStates.Add(typeof(UIState_GameOverState), new UIState_GameOverState(UIView_InventoryHUD, UIView_PlayerHUD, UIView_BattleHUD));
            m_InitializedStates.Add(typeof(UIState_LevelComplete), new UIState_LevelComplete(UIView_InventoryHUD, UIView_PlayerHUD, UIView_BattleHUD));

            m_StateMachine.Initialize(m_InitializedStates[typeof(UIState_NoUI)]);
        }

        private void InitializeUpdatables()
        {
            m_Updatables = new iUpdatable[]
            {
                UIView_InventoryHUD,
                UIView_PlayerHUD,
                UIView_BattleHUD
            };
        }


        #region VIEWS

        #region - ACTION POINTS

        public void UseActionPoint(int curActionPoints)
        {
            //UIComponent_ActionPointsIndicator.UseActionPoint(curActionPoints);
        }

        public void RestoreActionPoint(int curActionPoints)
        {
            //UIComponent_ActionPointsIndicator.RestoreActionPoint(curActionPoints);
        }

        public void RestoreAllActionPoints()
        {
            //UIComponent_ActionPointsIndicator.RestoreAllActionPoints();
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

        public void UIView_Battle_Defence_PressHandler()
        {
            OnTryDefence?.Invoke();
        }

        #endregion

        #endregion
    }
}
