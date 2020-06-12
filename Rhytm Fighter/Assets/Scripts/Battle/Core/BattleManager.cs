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

namespace RhytmFighter.Battle.Core
{
    public class BattleManager : Singleton<BattleManager>
    {
        public System.Action OnPotionUsed;

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
        public Metronome Metronome;

        private GameStateMachine m_GameStateMachine;
        private ControllersHolder m_ControllersHolder;
        private List<iUpdatable> m_Updateables;

        //States
        private GameState_Idle m_GameStateIdle;
        private GameState_Battle m_GameStateBattle;
        private GameState_Adventure m_GameStateAdventure;
        private GameState_TapToAction m_GameStateTapToAction;


        public PlayerData PlayerDataModel => GameManager.Instance.DataHolder.PlayerDataModel;
        public PlayerModel PlayerModel => m_ControllersHolder?.PlayerCharacterController?.PlayerModel;
        public float NPCMoveSpeed => (float)m_ControllersHolder.RhytmController.TickDurationSeconds * 
                                     ManagersHolder.SettingsManager.GeneralSettings.MoveSpeedTickDurationMultiplayer;


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
            InitializeDataDependents();
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

        private void InitializeDataDependents()
        {
            LevelsData.LevelParams levelParams = GameManager.Instance.DataHolder.InfoData.LevelsData.GetLevelParams(GameManager.Instance.DataHolder.PlayerDataModel.CurrentLevelID);

            //Set object params
            m_ControllersHolder.RhytmController.SetBPM(levelParams.BPM);
            m_ControllersHolder.RhytmInputProxy.SetInputPrecious(ManagersHolder.SettingsManager.GeneralSettings.InputPrecious);

            //Initialize managers (May require data)
            ManagersHolder.Initialize();

            //Build level
            BuildLevel(levelParams);

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
            m_ControllersHolder.BattleController.OnBattleFinished += BattleFinishedHandler;
            m_ControllersHolder.BattleController.OnLevelFinished += LevelCompleteHandler;

            //Rhytm
            m_ControllersHolder.RhytmController.OnStarted += TickingStartedHandler;
            m_ControllersHolder.RhytmController.OnTick += TickHandler;

            //UI
            ManagersHolder.UIManager.OnButtonDefencePressed += ButtonDefence_PressHandler;
            ManagersHolder.UIManager.OnButtonPotionPressed += ButtonPoition_PressHandler;
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
            m_ControllersHolder.BattleController.OnBattleFinished -= BattleFinishedHandler;
            m_ControllersHolder.BattleController.OnLevelFinished -= LevelCompleteHandler;

            //Rhytm
            m_ControllersHolder.RhytmController.OnStarted = null;
            m_ControllersHolder.RhytmController.OnTick = null;

            //UI
            ManagersHolder.UIManager.OnButtonDefencePressed -= ButtonDefence_PressHandler;
            ManagersHolder.UIManager.OnButtonPotionPressed -= ButtonPoition_PressHandler;
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
            ManagersHolder.UIManager.ToAdventureUIState();
        }


        private void BuildLevel(LevelsData.LevelParams levelParams)
        {
            m_ControllersHolder.LevelController.GenerateLevel(levelParams, true, true);
            m_ControllersHolder.LevelController.RoomViewBuilder.OnCellWithObjectDetected += CellWithObjectDetectedHandler;
        }
        #endregion

        #region Player
        private void CreatePlayer()
        {
            //Health
            SimpleHealthBehaviour healthBehaviour = GameManager.Instance.DataHolder.PlayerDataModel.IsFirstLevel ?
                                                        new SimpleHealthBehaviour(GameManager.Instance.DataHolder.PlayerDataModel.Character.FirstLevelCurrentHP, GameManager.Instance.DataHolder.PlayerDataModel.Character.HP) :
                                                        new SimpleHealthBehaviour(GameManager.Instance.DataHolder.PlayerDataModel.Character.HP);

            //Battle
            SimpleBattleActionBehaviour battleBehaviour = new SimpleBattleActionBehaviour(GameManager.Instance.DataHolder.PlayerDataModel.Character.Damage);
            int actionPoints = GameManager.Instance.DataHolder.PlayerDataModel.ActionPoints;
            int ticksToRestoreActionPoint = GameManager.Instance.DataHolder.PlayerDataModel.TickToRestoreActionPoint;

            //Start cell view
            CellView startCellView = m_ControllersHolder.LevelController.RoomViewBuilder.GetCellVisual(
                                                            m_ControllersHolder.LevelController.Model.GetCurrenRoomData().ID,
                                                            m_ControllersHolder.LevelController.Model.GetCurrenRoomData().GridData.WidthInCells / 2, 0);


            //Create player model
            PlayerModel playerModel = new PlayerModel(0, startCellView.CorrespondingCellData, NPCMoveSpeed, battleBehaviour, healthBehaviour, actionPoints, ticksToRestoreActionPoint);
            playerModel.OnDestroyed += PlayerDestroyedHandler;
            playerModel.OnActionPointUsed += ManagersHolder.UIManager.UseActionPoint;
            playerModel.OnActionPointRestored += ManagersHolder.UIManager.RestoreActionPoint;
            battleBehaviour.OnActionExecuted += (AbstractCommandModel command) => playerModel.UseActionPoint();

            m_ControllersHolder.PlayerCharacterController.OnNoActionPoints += () => Debug.Log("No action points");

            //Create player view
            m_ControllersHolder.PlayerCharacterController.CreateCharacter(playerModel, startCellView, m_ControllersHolder.LevelController);

            //Attach player to battle controller
            m_ControllersHolder.BattleController.Player = m_ControllersHolder.PlayerCharacterController.PlayerModel;

            //Initialize camera
            StartCoroutine(WaitEndOfFrameBeforeInitializeCamera());

            //TEMP
            UpdatePoitionAmount();
        }

        private void PlayerDestroyedHandler(iBattleObject sender)
        {
            GameOverHandler();
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

        System.Collections.IEnumerator WaitEndOfFrameBeforeInitializeCamera()
        {
            yield return null;

            //Initialize camera
            m_ControllersHolder.CameraController.InitializeCamera(m_ControllersHolder.PlayerCharacterController.PlayerModel.View.transform);

        }

        System.Collections.IEnumerator TEMP_INTERATCION_COROUTINE(float animationDelay)
        {
            yield return new WaitForSeconds(animationDelay);

            PlayerDataModel.Inventory.PotionsAmount++;
            UpdatePoitionAmount();
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
            Debug.Log("PRepare for battle");
            //No need to stop movement if players destination cell is the cell where enemy was detected
            if (m_ControllersHolder.PlayerCharacterController.PlayerModel.IsMoving)
                m_ControllersHolder.PlayerCharacterController.StopMove();

            m_ControllersHolder.RhytmController.OnEventProcessingTick += EventProcessingTickHandler;
            m_ControllersHolder.RhytmController.OnEventProcessingTick += m_ControllersHolder.PlayerCharacterController.PlayerModel.ProcessActionPointRestore;

            m_ControllersHolder.PlayerCharacterController.PrepareForBattle();   //Prepare character for battle
            m_GameStateMachine.ChangeState(m_GameStateIdle);                    //Change state
            ManagersHolder.UIManager.ToPrepareForBattleUIState();               //Prepare UI for battle      

            //Debug - Prepare sound for battle
            Rhytm.volume = 0.5f;
        }

        private void BattleStartedHandler()
        {
            //Subscribe for events
            //m_ControllersHolder.RhytmController.OnTick += m_ControllersHolder.BattleController.ProcessEnemyActions;   
            m_ControllersHolder.RhytmController.OnEventProcessingTick += m_ControllersHolder.CommandsController.ProcessPendingCommands;

            Debug.Log("Battle");
            m_GameStateMachine.ChangeState(m_GameStateBattle);      //Change state
            ManagersHolder.UIManager.ToBattleStartUIState();        //Show Battle UI
        }

        private void BattleEnemyDestroyedHandler(bool lastEnemyDestroyed)
        {
            //Unscribe from events
            m_ControllersHolder.RhytmController.OnTick -= m_ControllersHolder.BattleController.ProcessEnemyActions;
            m_ControllersHolder.RhytmController.OnEventProcessingTick -= m_ControllersHolder.CommandsController.ProcessPendingCommands;

            //UI
            if (!lastEnemyDestroyed)
                ManagersHolder.UIManager.ToWaitingForNextEnemyActivationUIState();
            else
                ManagersHolder.UIManager.ToBattleFinishedUIState();

            m_GameStateMachine.ChangeState(m_GameStateIdle);            //Change state
        }

        private void BattleFinishedHandler()
        {
            m_ControllersHolder.RhytmController.OnEventProcessingTick -= EventProcessingTickHandler;
            m_ControllersHolder.RhytmController.OnEventProcessingTick -= m_ControllersHolder.PlayerCharacterController.PlayerModel.ProcessActionPointRestore;

            m_ControllersHolder.PlayerCharacterController.FinishBattle();       //Finish battle for player
            m_GameStateMachine.ChangeState(m_GameStateAdventure);               //Change state
            ManagersHolder.UIManager.ToAdventureUIState();                      //Finish battle for UI    

            Rhytm.volume = 0;   //Debug - Finish battle for sound
        }
        #endregion

        #region Reloading
        private void GameOverHandler()
        {
            //Finilize level
            Finilize(ManagersHolder.UIManager.ToGameOverUIState);

            //Ability to reload level 
            StartCoroutine(FINISH_LEVEL_COROUTINE(() =>
            {
                m_GameStateTapToAction.OnTouch += () =>
                {
                    GameManager.Instance.ReloadBattleLevel();
                };

                m_GameStateMachine.ChangeState(m_GameStateTapToAction);
                ManagersHolder.UIManager.ToTapToActionUIState();
            }));
        }

        private void LevelCompleteHandler()
        {
            //Finilize level
            Finilize(ManagersHolder.UIManager.ToLevelComleteUIState);

            //Ability to reload level 
            StartCoroutine(FINISH_LEVEL_COROUTINE(() =>
            {
                m_GameStateTapToAction.OnTouch += () =>
                {
                    //GameManager.Instance.DataHolder.PlayerDataModel.CurrentLevelID++;
                    GameManager.Instance.ReloadBattleLevel();
                };

                m_GameStateMachine.ChangeState(m_GameStateTapToAction);
                ManagersHolder.UIManager.ToTapToActionUIState();
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

        private void ButtonPoition_PressHandler()
        {
            if (Instance.PlayerDataModel.Inventory.PotionsAmount > 0 && PlayerModel.HealthBehaviour.HP < PlayerModel.HealthBehaviour.MaxHP)
            {
                if (m_ControllersHolder.BattleController.IsInBattle)
                {
                    if (m_ControllersHolder.RhytmInputProxy.IsInputTickValid() && m_ControllersHolder.RhytmInputProxy.IsInputAllowed())
                    {
                        UsePotion();
                        m_ControllersHolder.RhytmInputProxy.RegisterInput();
                        m_ControllersHolder.PlayerCharacterController.PlayerModel.UseActionPoint();
                    }
                }
                else
                    UsePotion();
            }
        }

        private void UsePotion()
        {
            PlayerDataModel.Inventory.PotionsAmount--;
            UpdatePoitionAmount();
            SipSound.Play();
            AssetsManager.GetPrefabAssets().InstantiatePrefab<AbstractVisualEffect>(AssetsManager.GetPrefabAssets().HealEffectPrefab,
                                                                                    PlayerModel.ViewPosition,
                                                                                    Quaternion.Euler(-90, 0, 0)).ScheduleHideView();

            PlayerModel.HealthBehaviour.IncreaseHP(5);

            OnPotionUsed?.Invoke();
        }

        private void UpdatePoitionAmount()
        {
            ManagersHolder.UIManager.Text_PotionAmount.text = $"x{PlayerDataModel.Inventory.PotionsAmount}";

            ManagersHolder.UIManager.Text_PotionAmount.GetComponent<CanvasGroup>().alpha = PlayerDataModel.Inventory.PotionsAmount > 0 ? 1 : 0.5f;

            Color color = ManagersHolder.UIManager.Button_Potion.image.color;
            color.a = PlayerDataModel.Inventory.PotionsAmount > 0 ? 1 : 0.5f;
            ManagersHolder.UIManager.Button_Potion.image.color = color;
        }
        #endregion
    }
}
