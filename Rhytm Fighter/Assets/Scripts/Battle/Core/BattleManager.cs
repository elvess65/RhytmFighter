using FrameworkPackage.Utils;
using Frameworks.Grid.View;
using RhytmFighter.Battle.Action.Behaviours;
using RhytmFighter.Battle.Health.Behaviours;
using RhytmFighter.Data;
using RhytmFighter.Objects.Model;
using System.Collections.Generic;
using UnityEngine;
using RhytmFighter.CameraSystem;
using RhytmFighter.StateMachines.GameState;
using RhytmFighter.Enviroment.Effects;
using RhytmFighter.Assets;
using RhytmFighter.Persistant;
using RhytmFighter.Persistant.Abstract;
using RhytmFighter.Persistant.Enums;
using RhytmFighter.Battle.Core.Abstract;
using RhytmFighter.Battle.Command.Model;
using RhytmFighter.StateMachines.UIState;
using RhytmFighter.Data.Models;
using RhytmFighter.Data.Models.DataTableModels;
using static RhytmFighter.Data.Models.AccountModel;
using RhytmFighter.Persistant.SceneLoading;

namespace RhytmFighter.Battle.Core
{
    public class BattleManager : Singleton<BattleManager>
    {
        public ManagersHolder ManagersHolder;
        public CamerasHolder CamerasHolder;
  
        [Header("Temp")]
        public AudioSource Music;
        public AudioSource Rhytm;
        public AudioSource AttackSound;
        public AudioSource DefenceExecuteSound;
        public AudioSource HitSound;
        public AudioSource DestroySound;
        public AudioSource DefenceSound;
        public AudioSource BeatSound;
        public AudioSource FinishBattleSound;
        public AudioSource SipSound;
        public AudioSource DashSound;
        public AudioSource ErrorSound;
        public Metronome Metronome;

        private GameStateMachine m_GameStateMachine;
        private ControllersHolder m_ControllersHolder;
        private List<iUpdatable> m_Updateables;

        //States
        private GameState_Idle m_GameStateIdle;
        private GameState_Battle m_GameStateBattle;
        private GameState_Adventure m_GameStateAdventure;
        private GameState_TapToAction m_GameStateTapToAction;


        public float NPCMoveSpeed => (float)m_ControllersHolder.RhytmController.TickDurationSeconds *
                                     ManagersHolder.SettingsManager.GeneralSettings.MoveSpeedTickDurationMultiplayer;
        //Shortcuts
        public AccountModel AccountModelShortcut => GameManager.Instance.DataHolder.AccountModel;
        public BattleSessionModel BattleSessionModelShortcut = GameManager.Instance.DataHolder.BattleSessionModel;
        public PlayerModel PlayerModelShortcut => m_ControllersHolder?.PlayerCharacterController?.PlayerModel;
        public CharacterData CurrentCharacterDataShortcut => DataHelper.GetCharacterData(BattleSessionModelShortcut.SelectedCharactedID);
        

        private void Update()
        {
            if (m_Updateables != null)
            {
                for (int i = 0; i < m_Updateables.Count; i++)
                    m_Updateables[i].PerformUpdate(Time.deltaTime);
            }
        }

        #region Initialization
        public void Initialize()
        {
            InitializeCore();
            InitializeStateMachine();
            InitializeDataDependends();
            InitializeUpdatables();
            InitializeEvents();
            ApplySettings();

            InitializationFinished();
        }

        private void InitializeCore()
        {
            m_GameStateMachine = new GameStateMachine();
            m_ControllersHolder = new ControllersHolder();
        }

        private void InitializeStateMachine()
        {
            //Create states
            m_GameStateIdle = new GameState_Idle(m_ControllersHolder.PlayerCharacterController, m_ControllersHolder.RhytmInputProxy);
            m_GameStateBattle = new GameState_Battle(m_ControllersHolder.PlayerCharacterController, m_ControllersHolder.RhytmInputProxy);
            m_GameStateAdventure = new GameState_Adventure(m_ControllersHolder.LevelController, m_ControllersHolder.PlayerCharacterController, m_ControllersHolder.RhytmInputProxy);
            m_GameStateTapToAction = new GameState_TapToAction();

            //Subscribe for events
            m_GameStateAdventure.OnPlayerInteractWithItem += PlayerInteractWithItemHandler;
            m_GameStateAdventure.OnPlayerInteractWithNPC += PlayerInteractWithNPCHandler;

            //Initialize state machine with default state
            m_GameStateMachine.Initialize(m_GameStateIdle);
        }

        private void InitializeDataDependends()
        {
            EnvironmentDataModel.LevelParams levelParams = GameManager.Instance.DataHolder.DataTableModel.EnvironmentDataModel.GetLevelParams(BattleSessionModelShortcut.CurrentLevelID);
            float completionProgress = GameManager.Instance.DataHolder.DataTableModel.EnvironmentDataModel.GetCompletionForProgression(BattleSessionModelShortcut.CompletedLevelsIDs.ToArray());

            //Set object params
            m_ControllersHolder.RhytmController.SetBPM(levelParams.BPM);
            m_ControllersHolder.RhytmInputProxy.SetInputPrecious(ManagersHolder.SettingsManager.GeneralSettings.InputPrecious);

            //Initialize managers (May require data)
            ManagersHolder.Initialize();

            //Build level
            BuildLevel(levelParams, completionProgress);

            //Create player
            CreatePlayer();

            //Metronome
            Metronome.bpm = levelParams.BPM * 4;
        }

        private void InitializeUpdatables()
        {
            //Initialize updatables
            m_Updateables = new List<iUpdatable>
            {
                m_ControllersHolder.InputController,
                m_ControllersHolder.RhytmController,
                m_ControllersHolder.CommandsController,
                m_ControllersHolder.BattleController,
                m_ControllersHolder.CameraController,
                m_GameStateMachine,
                ManagersHolder.UIManager
            };
        }

        private void InitializeEvents()
        {
            //Input
            m_ControllersHolder.InputController.OnTouch += m_GameStateMachine.HandleTouch;

            //Player controller 
            m_ControllersHolder.PlayerCharacterController.OnTeleportStarted += TeleportStartedHandler;
            m_ControllersHolder.PlayerCharacterController.OnTeleportFinished += TeleportFinishedHandler;

            //Battle
            m_ControllersHolder.BattleController.OnPrepareForBattle += PrepareForBattleHandler;
            m_ControllersHolder.BattleController.OnBattleStarted += BattleStartedHandler;
            m_ControllersHolder.BattleController.OnEnemyDestroyed += BattleEnemyDestroyedHandler;
            m_ControllersHolder.BattleController.OnExperianceGained += ExperianceGainedHandler;
            m_ControllersHolder.BattleController.OnBattleFinished += BattleFinishedHandler;
            m_ControllersHolder.BattleController.OnLevelFinished += LevelCompleteHandler;

            //Rhytm
            m_ControllersHolder.RhytmController.OnStarted += TickingStartedHandler;
            m_ControllersHolder.RhytmController.OnTick += TickHandler;

            //UI
            ManagersHolder.UIManager.OnTryDefence += ButtonDefence_PressHandler;
            ManagersHolder.UIManager.OnTryUsePotion += TryUsePotion;
        }

        private void DisposeEvents()
        {
            //Player controller 
            m_ControllersHolder.PlayerCharacterController.OnTeleportStarted -= TeleportStartedHandler;
            m_ControllersHolder.PlayerCharacterController.OnTeleportFinished -= TeleportFinishedHandler;

            //Battle
            m_ControllersHolder.BattleController.OnPrepareForBattle -= PrepareForBattleHandler;
            m_ControllersHolder.BattleController.OnBattleStarted -= BattleStartedHandler;
            m_ControllersHolder.BattleController.OnEnemyDestroyed -= BattleEnemyDestroyedHandler;
            m_ControllersHolder.BattleController.OnExperianceGained -= ExperianceGainedHandler;
            m_ControllersHolder.BattleController.OnBattleFinished -= BattleFinishedHandler;
            m_ControllersHolder.BattleController.OnLevelFinished -= LevelCompleteHandler;

            //Rhytm
            m_ControllersHolder.RhytmController.OnStarted = null;
            m_ControllersHolder.RhytmController.OnTick = null;

            //UI
            ManagersHolder.UIManager.OnTryDefence -= ButtonDefence_PressHandler;
            ManagersHolder.UIManager.OnTryUsePotion -= TryUsePotion;
        }

        private void ApplySettings()
        {
            if (ManagersHolder.SettingsManager.GeneralSettings.MuteAudio)
                AudioListener.volume = 0;
        }

        private void InitializationFinished()
        {
            //Start beat
            m_ControllersHolder.RhytmController.StartTicking();

            //Change state
            m_GameStateMachine.ChangeState(m_GameStateAdventure);
            ManagersHolder.UIManager.ChangeState<UIState_Adventure>();
        }


        private void BuildLevel(EnvironmentDataModel.LevelParams levelParams, float completionProgress)
        {
            m_ControllersHolder.LevelController.GenerateLevel(levelParams, true, true, completionProgress);
            m_ControllersHolder.LevelController.RoomViewBuilder.OnCellWithObjectDetected += CellWithObjectDetectedHandler;
        }
        #endregion

        #region Player
        private void CreatePlayer()
        {
            //Health
            SimpleHealthBehaviour healthBehaviour = new SimpleHealthBehaviour(DataHelper.GetCharacterHP(CurrentCharacterDataShortcut.ID) - 1,
                                                                              DataHelper.GetCharacterHP(CurrentCharacterDataShortcut.ID));

            //Battle
            SimpleBattleActionBehaviour battleBehaviour = new SimpleBattleActionBehaviour(DataHelper.GetCharacterDamage(CurrentCharacterDataShortcut.ID).Item2);
            int actionPoints = 0;//GameManager.Instance.DataHolder.PlayerDataModel.ActionPoints;
            int ticksToRestoreActionPoint = 0;//GameManager.Instance.DataHolder.PlayerDataModel.TickToRestoreActionPoint;

            //Start cell view
            CellView startCellView = m_ControllersHolder.LevelController.RoomViewBuilder.GetCellVisual(
                                                            m_ControllersHolder.LevelController.Model.GetCurrenRoomData().ID,
                                                            m_ControllersHolder.LevelController.Model.GetCurrenRoomData().GridData.WidthInCells / 2, 0);


            //Create player model
            PlayerModel playerModel = new PlayerModel(0, startCellView.CorrespondingCellData, NPCMoveSpeed, battleBehaviour, healthBehaviour, actionPoints, ticksToRestoreActionPoint);
            playerModel.OnDestroyed += PlayerDestroyedHandler;
            playerModel.OnActionPointUsed += ManagersHolder.UIManager.UseActionPoint;
            playerModel.OnActionPointRestored += ManagersHolder.UIManager.RestoreActionPoint;
            playerModel.OnAllActionPointsResotored += ManagersHolder.UIManager.RestoreAllActionPoints;

            battleBehaviour.OnActionExecuted += (AbstractCommandModel command) => playerModel.UseActionPoint();

            m_ControllersHolder.PlayerCharacterController.OnNoActionPoints += () => Debug.Log("No action points");

            //Create player view
            m_ControllersHolder.PlayerCharacterController.CreateCharacter(playerModel, startCellView, m_ControllersHolder.LevelController);

            //Attach player to battle controller
            m_ControllersHolder.BattleController.Player = m_ControllersHolder.PlayerCharacterController.PlayerModel;

            //Initialize camera
            StartCoroutine(WaitDelayBeforeInitializeCamera());
        }

        private void PlayerDestroyedHandler(iBattleObject sender)
        {
            GameOverHandler();

            //Play victory animation for enemy
            m_ControllersHolder?.PlayerCharacterController?.PlayerModel?.Target?.NotifyViewAboutVictory();

        }

        private void PlayerInteractWithObject(AbstractInteractableObjectModel interactableObject)
        {
            interactableObject.Interact();
        }

        private void TeleportStartedHandler()
        {
            m_ControllersHolder.InputController.OnTouch -= m_GameStateMachine.HandleTouch;
        }

        private void TeleportFinishedHandler()
        {
            m_ControllersHolder.InputController.OnTouch += m_GameStateMachine.HandleTouch;
        }

        System.Collections.IEnumerator WaitDelayBeforeInitializeCamera()
        {
            yield return null;

            //Initialize camera
            m_ControllersHolder.CameraController.InitializeCamera(m_ControllersHolder.PlayerCharacterController.PlayerModel.View.transform);

            yield return new WaitForSeconds(1);

            //Activate mmain camera
            m_ControllersHolder.CameraController.ActivateCamera(CameraTypes.Main);

        }

        System.Collections.IEnumerator TEMP_INTERATCION_COROUTINE(float animationDelay)
        {
            yield return new WaitForSeconds(animationDelay);

            //UI
            AccountModelShortcut.Inventory.GetPotionByType(PotionTypes.Heal).IncrementPieceAmount();
            ManagersHolder.UIManager.UIView_InventoryHUD.WidgetPotion_UpdateAmount();

            //State
            m_GameStateMachine.ChangeState(m_GameStateAdventure);
        }

        System.Collections.IEnumerator FINISH_LEVEL_COROUTINE(System.Action onDelayed)
        {
            yield return new WaitForSeconds(1);

            onDelayed?.Invoke();
        }
        #endregion

        #region Grid
        private void CellWithObjectDetectedHandler(AbstractGridObjectModel gridObject)
        {
            //Detect NPC
            if (gridObject.Type == GridObjectTypes.NPC)
            {
                //Detect battle NPC
                iBattleObject battleObject = gridObject as iBattleObject;
                if (battleObject != null)
                {
                    //Detect enemy
                    if (battleObject.IsEnemy)
                    {
                        gridObject.OnViewShowed += () => m_ControllersHolder.BattleController.AddEnemy(battleObject);
                        m_ControllersHolder.BattleController.PrepareForBattle();
                    }
                }
            }
        }

        private void PlayerInteractWithItemHandler(AbstractItemModel interactableItem)
        {
            //Interact
            PlayerInteractWithObject(interactableItem);

            //Lock input for animation time
            m_GameStateMachine.ChangeState(m_GameStateIdle);

            //Debug animation time
            StartCoroutine(TEMP_INTERATCION_COROUTINE(0.5f));
        }

        private void PlayerInteractWithNPCHandler(AbstractNPCModel interactableNPC)
        {
            //Interact
            PlayerInteractWithObject(interactableNPC);

            Debug.Log("INTERACT WITH NPC: " + interactableNPC.View.gameObject.name + " " + interactableNPC.ID + " " + interactableNPC.Type);
        }
        #endregion

        #region Battle
        private void PrepareForBattleHandler()
        {
            //No need to stop movement if players destination cell is the cell where enemy was detected
            if (m_ControllersHolder.PlayerCharacterController.PlayerModel.IsMoving)
                m_ControllersHolder.PlayerCharacterController.StopMove();

            m_ControllersHolder.RhytmController.OnEventProcessingTick += EventProcessingTickHandler;
            m_ControllersHolder.RhytmController.OnEventProcessingTick += m_ControllersHolder.PlayerCharacterController.PlayerModel.ProcessActionPointRestore;

            m_ControllersHolder.PlayerCharacterController.PrepareForBattle();   //Prepare character for battle
            m_GameStateMachine.ChangeState(m_GameStateIdle);                    //Change state
            ManagersHolder.UIManager.ChangeState<UIState_PrepareForBattle>();   //Prepare UI for battle      

            //Debug - Prepare sound for battle
            Rhytm.volume = 0.5f;
        }

        private void BattleStartedHandler()
        {
            //Subscribe for events
            m_ControllersHolder.RhytmController.OnTick += m_ControllersHolder.BattleController.ProcessEnemyActions;   
            m_ControllersHolder.RhytmController.OnEventProcessingTick += m_ControllersHolder.CommandsController.ProcessPendingCommands;

            m_GameStateMachine.ChangeState(m_GameStateBattle);              //Change state
            ManagersHolder.UIManager.ChangeState<UIState_BattleStart>();    //Show Battle UI
        }

        private void BattleEnemyDestroyedHandler(bool lastEnemyDestroyed)
        {
            //Unscribe from events
            m_ControllersHolder.RhytmController.OnTick -= m_ControllersHolder.BattleController.ProcessEnemyActions;
            m_ControllersHolder.RhytmController.OnEventProcessingTick -= m_ControllersHolder.CommandsController.ProcessPendingCommands;

            //UI
            if (!lastEnemyDestroyed)
                ManagersHolder.UIManager.ChangeState<UIState_WaitNextEnemy>();
            else
                ManagersHolder.UIManager.ChangeState<UIState_BattleFinished>();

            m_GameStateMachine.ChangeState(m_GameStateIdle);            //Change state
        }

        private void ExperianceGainedHandler(int gainedAmount)
        {
            GameManager.Instance.DataHolder.AccountModel.CurrencyAmount += gainedAmount;
            ManagersHolder.UIManager.UIView_PlayerHUD.UIWidget_Currency.AddCurrency(gainedAmount);
        }

        private void BattleFinishedHandler()
        {
            m_ControllersHolder.RhytmController.OnEventProcessingTick -= EventProcessingTickHandler;
            m_ControllersHolder.RhytmController.OnEventProcessingTick -= m_ControllersHolder.PlayerCharacterController.PlayerModel.ProcessActionPointRestore;

            m_ControllersHolder.PlayerCharacterController.FinishBattle();       //Finish battle for player
            m_GameStateMachine.ChangeState(m_GameStateAdventure);               //Change state
            ManagersHolder.UIManager.ChangeState<UIState_Adventure>();          //Finish battle for UI    

            Rhytm.volume = 0;   //Debug - Finish battle for sound
        }
        #endregion

        #region Reloading
        private void GameOverHandler()
        {
            //Finilize level
            Finilize(() => ManagersHolder.UIManager.ChangeState<UIState_GameOverState>());

            //Ability to reload level 
            StartCoroutine(FINISH_LEVEL_COROUTINE(() =>
            {
                m_GameStateTapToAction.OnTouch += () =>
                {
                    //Reload scene
                    GameManager.Instance.SceneLoader.OnSceneUnloadingComplete += BattleSceneUnloadedHandler;
                    GameManager.Instance.SceneLoader.UnloadLevel(SceneLoader.BATTLE_SCENE_NAME);
                };

                m_GameStateMachine.ChangeState(m_GameStateTapToAction);
                ManagersHolder.UIManager.ChangeState<UIState_TapToActionState>();
            }));
        }

        private void LevelCompleteHandler()
        {
            //Finilize level
            Finilize(() => ManagersHolder.UIManager.ChangeState<UIState_LevelComplete>());

            //Align and activate victory camera
            m_ControllersHolder.CameraController.AlignDefaultCameraToTarget();
            m_ControllersHolder.CameraController.ActivateCamera(CameraTypes.Default);

            //Play victory animation for player
            m_ControllersHolder.PlayerCharacterController.PlayerModel.NotifyViewAboutVictory();

            //Ability to reload level 
            StartCoroutine(FINISH_LEVEL_COROUTINE(() =>
            {
                m_GameStateTapToAction.OnTouch += () =>
                {
                    //GameManager.Instance.DataHolder.PlayerDataModel.CurrentLevelID++;

                    //Reload scene
                    GameManager.Instance.SceneLoader.OnSceneUnloadingComplete += BattleSceneUnloadedHandler;
                    GameManager.Instance.SceneLoader.UnloadLevel(SceneLoader.BATTLE_SCENE_NAME);
                };

                m_GameStateMachine.ChangeState(m_GameStateTapToAction);
                ManagersHolder.UIManager.ChangeState<UIState_TapToActionState>();
            }));
        }

        private void Finilize(System.Action toUIStateTransition)
        {
            //Unscribe from events
            DisposeEvents();

            Rhytm.Stop();   //Debug - Finish battle for sound
            Music.Stop();

            //Show state
            m_GameStateMachine.ChangeState(m_GameStateIdle);
            toUIStateTransition?.Invoke();
        }

        private void BattleSceneUnloadedHandler()
        {
            GameManager.Instance.SceneLoader.OnSceneUnloadingComplete -= BattleSceneUnloadedHandler;
            GameManager.Instance.SceneLoader.LoadLevel(SceneLoader.BATTLE_SCENE_NAME);
        }
        #endregion

        #region Rhytm
        private void TickingStartedHandler()
        {
            Rhytm.volume = 0;
            //Metronome.StartMetronome();
        }

        private void TickHandler(int ticksSinceStart)
        {
            if (ticksSinceStart % 8 == 0)
            {
                Music.Play();
                Rhytm.Play();
            }
        }

        private void EventProcessingTickHandler(int ticksSinceStart)
        {
            //BeatSound.Play();
        }
        #endregion

        #region UI

        private void ButtonDefence_PressHandler()
        {
            if (m_ControllersHolder.RhytmInputProxy.IsInputTickValid() && m_ControllersHolder.RhytmInputProxy.IsInputAllowed())
            {
                m_ControllersHolder.PlayerCharacterController.ExecuteAction(CommandTypes.Defence);
                m_ControllersHolder.RhytmInputProxy.RegisterInput();
            }
        }

        #region POTION

        private void TryUsePotion()
        {
            if (AccountModelShortcut.Inventory.GetPotionByType(PotionTypes.Heal).HasPotions &&
                PlayerModelShortcut.HealthBehaviour.HP < PlayerModelShortcut.HealthBehaviour.MaxHP)
            {
                //Decrement potion
                AccountModelShortcut.Inventory.GetPotionByType(PotionTypes.Heal).DecrementPotion();

                //Increase HP
                PlayerModelShortcut.HealthBehaviour.IncreaseHP(5);

                //Refresh UI
                ManagersHolder.UIManager.UIView_InventoryHUD.WidgetPotion_UsePotion(true);

                //Effects
                SipSound.Play();
                AssetsManager.GetPrefabAssets().InstantiatePrefab<AbstractVisualEffect>(AssetsManager.GetPrefabAssets().HealEffectPrefab,
                                                                                        PlayerModelShortcut.ViewPosition,
                                                                                        Quaternion.Euler(-90, 0, 0)).ScheduleHideView();
            }
            else
            {
                //Refresh UI
                ManagersHolder.UIManager.UIView_InventoryHUD.WidgetPotion_UsePotion(false);

                //Effects
                ErrorSound.Play();
            }
        }

        #endregion

        #endregion
    }
}
