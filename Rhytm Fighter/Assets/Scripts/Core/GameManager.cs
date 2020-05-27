using FrameworkPackage.Utils;
using Frameworks.Grid.View;
using RhytmFighter.Battle.Action.Behaviours;
using RhytmFighter.Battle.Health.Behaviours;
using RhytmFighter.Data;
using RhytmFighter.Objects.Model;
using System.Collections.Generic;
using UnityEngine;
using RhytmFighter.Core.Enums;
using RhytmFighter.CameraSystem;
using RhytmFighter.StateMachines.GameState;
using RhytmFighter.Enviroment.Effects;
using RhytmFighter.Assets;

namespace RhytmFighter.Core
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager m_Instance;
        public static GameManager Instance => m_Instance;

        [Header("Links")]
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

        private DataHolder m_DataHolder;
        private GameStateMachine m_GameStateMachine;
        private ControllersHolder m_ControllersHolder;
        private List<iUpdatable> m_Updateables;

        //States
        private GameState_Idle m_GameStateIdle;
        private GameState_Battle m_GameStateBattle;
        private GameState_Adventure m_GameStateAdventure;

        public static float ENEMY_MOVE_SPEED;
        public static float PLAYER_MOVE_SPEED;

        public PlayerModel PlayerModel => m_ControllersHolder.PlayerCharacterController.PlayerModel;


        public int m_Poitions = 0;

        private void Awake()
        {
            if (m_Instance != null)
                Destroy(gameObject);

            m_Instance = this;
        }

        private void Start()
        {
            Initialize();
        }

        private void Update()
        {
            for (int i = 0; i < m_Updateables.Count; i++)
                m_Updateables[i].PerformUpdate(Time.deltaTime);
        }


        #region Initialization
        private void Initialize()
        {
            //Temp
            int bpm = 130 / 2;
            double inputPrecious = 0.25;
            Metronome.bpm = bpm * 4;

            if (ManagersHolder.SettingsManager.GeneralSettings.MuteAudio)
                AudioListener.volume = 0;

            //Initialize objects
            m_DataHolder = new DataHolder();
            m_GameStateMachine = new GameStateMachine();
            m_ControllersHolder = new ControllersHolder();

            //Set object params
            m_ControllersHolder.RhytmController.SetBPM(bpm);
            m_ControllersHolder.RhytmInputProxy.SetInputPrecious(inputPrecious);

            //Initialize managers
            ManagersHolder.Initialize();

            //Initialize state machine
            m_GameStateIdle = new GameState_Idle(m_ControllersHolder.PlayerCharacterController,
                                                 m_ControllersHolder.RhytmInputProxy);

            m_GameStateBattle = new GameState_Battle(m_ControllersHolder.PlayerCharacterController,
                                                     m_ControllersHolder.RhytmInputProxy);

            m_GameStateAdventure = new GameState_Adventure(m_ControllersHolder.LevelController,
                                                           m_ControllersHolder.PlayerCharacterController,
                                                           m_ControllersHolder.RhytmInputProxy);

            m_GameStateAdventure.OnPlayerInteractWithItem += PlayerInteractWithItemHandler;
            m_GameStateAdventure.OnPlayerInteractWithNPC += PlayerInteractWithNPCHandler;
            m_GameStateMachine.Initialize(m_GameStateIdle);

            InitializeUpdatables();

            //Subscribe for events
            // - Input
            m_ControllersHolder.InputController.OnTouch += m_GameStateMachine.HandleTouch;

            // - Player controller 
            m_ControllersHolder.PlayerCharacterController.OnTeleportStarted += TeleportStartedHandler;
            m_ControllersHolder.PlayerCharacterController.OnTeleportFinished += TeleportFinishedHandler;

            // - Battle
            m_ControllersHolder.BattleController.OnPrepareForBattle += PrepareForBattleHandler;
            m_ControllersHolder.BattleController.OnBattleStarted += BattleStartedHandler;
            m_ControllersHolder.BattleController.OnEnemyDestroyed += BattleEnemyDestroyedHandler;
            m_ControllersHolder.BattleController.OnBattleFinished += BattleFinishedHandler;

            // - Rhytm
            m_ControllersHolder.RhytmController.OnTick += TickHandler;
            m_ControllersHolder.RhytmController.OnStarted += TickingStartedHandler;

            // - UI
            ManagersHolder.UIManager.OnButtonDefencePressed += ButtonDefence_PressHandler;
            ManagersHolder.UIManager.OnButtonPotionPressed += ButtonPoition_PressHandler;

            //Temp
            //Btn.onClick.AddListener(BtnPressHandler);
            PLAYER_MOVE_SPEED = (float)m_ControllersHolder.RhytmController.TickDurationSeconds * 4f;
            ENEMY_MOVE_SPEED = PLAYER_MOVE_SPEED;
            UpdatePoitionAmount();

            //Initialize connection
            m_DataHolder.DBProxy.OnConnectionSuccess += ConnectionResultSuccess;
            m_DataHolder.DBProxy.OnConnectionError += ConnectionResultError;
            m_DataHolder.DBProxy.Initialize();
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

        private void InitializationFinished()
        {
            //Start beat
            m_ControllersHolder.RhytmController.StartTicking();

            //Change state
            m_GameStateMachine.ChangeState(m_GameStateAdventure);
            ManagersHolder.UIManager.ToAdventureUIState();
        }

        private void ConnectionResultSuccess(string serializedPlayerData, string serializedLevelsData)
        {
            //Set data
            m_DataHolder.PlayerDataModel = PlayerData.DeserializeData(serializedPlayerData);
            m_DataHolder.InfoData = new InfoData(serializedLevelsData);

            //Build level
            BuildLevel();

            //Create player
            CreatePlayer();
        }

        private void ConnectionResultError(int errorCode) => Debug.LogError($"Connection error {errorCode}");


        private void BuildLevel()
        {
            m_ControllersHolder.LevelController.GenerateLevel(m_DataHolder.InfoData.LevelsData.GetLevelParams(m_DataHolder.PlayerDataModel.CurrentLevelID), false, true);
            m_ControllersHolder.LevelController.RoomViewBuilder.OnCellWithObjectDetected += CellWithObjectDetectedHandler;
        }

        private void CreatePlayer()
        {
            //Temp 
            //TODO Get from data
            int playerID = 0;

            //Temp
            SimpleHealthBehaviour healthBehaviour = new SimpleHealthBehaviour(10, 20);
            SimpleBattleActionBehaviour battleBehaviour = new SimpleBattleActionBehaviour(1, 1, 2);
            CellView startCellView = m_ControllersHolder.LevelController.RoomViewBuilder.GetCellVisual(m_ControllersHolder.LevelController.Model.GetCurrenRoomData().ID,
                m_ControllersHolder.LevelController.Model.GetCurrenRoomData().GridData.WidthInCells / 2, 0);

            //Create player model
            PlayerModel playerModel = new PlayerModel(playerID, startCellView.CorrespondingCellData, PLAYER_MOVE_SPEED, battleBehaviour, healthBehaviour);
            m_ControllersHolder.PlayerCharacterController.CreateCharacter(playerModel, startCellView, m_ControllersHolder.LevelController);
            playerModel.OnDestroyed += PlayerDestroyedHandler;

            //Set player to battle controller
            m_ControllersHolder.BattleController.Player = m_ControllersHolder.PlayerCharacterController.PlayerModel;

            //Initialize camera
            m_ControllersHolder.CameraController.InitializeCamera(m_ControllersHolder.PlayerCharacterController.PlayerModel.View.transform);

            //Finish initialization
            InitializationFinished(); 
        }
        #endregion

        #region Player controller
        private void TeleportStartedHandler()
        {
            m_ControllersHolder.InputController.OnTouch -= m_GameStateMachine.HandleTouch;
        }

        private void TeleportFinishedHandler()
        {
            m_ControllersHolder.InputController.OnTouch += m_GameStateMachine.HandleTouch;
        }
        #endregion

        #region Player
        private void PlayerDestroyedHandler(iBattleObject sender)
        {
            Debug.LogError("DESTROY PLAYER");

            BattleFinishedHandler();
            m_GameStateMachine.ChangeState(m_GameStateIdle);
        }

        private void PlayerInteractWithObject(AbstractInteractableObjectModel interactableObject)
        {
            interactableObject.Interact();
        }

        System.Collections.IEnumerator TEMP_INTERATCION_COROUTINE(float animationDelay)
        {
            yield return new WaitForSeconds(animationDelay);

            m_Poitions++;
            UpdatePoitionAmount();
            m_GameStateMachine.ChangeState(m_GameStateAdventure);
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
                        m_ControllersHolder.BattleController.AddEnemy(battleObject);
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
            Debug.LogError("Battle - Prepare");

            //No need to stop movement if players destination cell is the cell where enemy was detected
            if (m_ControllersHolder.PlayerCharacterController.PlayerModel.IsMoving)
                m_ControllersHolder.PlayerCharacterController.StopMove();

            m_ControllersHolder.PlayerCharacterController.PrepareForBattle();   //Prepare character for battle
            m_GameStateMachine.ChangeState(m_GameStateIdle);                    //Change state
            ManagersHolder.UIManager.ToPrepareForBattleUIState();               //Prepare UI for battle      

            //Debug - Prepare sound for battle
            Rhytm.volume = 0.5f;
        }

        private void BattleStartedHandler()
        {
            Debug.LogError("Battle - Start");

            //Subscribe for events
            m_ControllersHolder.RhytmController.OnTick += m_ControllersHolder.BattleController.ProcessEnemyActions;
            m_ControllersHolder.RhytmController.OnEventProcessingTick += EventProcessingTickHandler;
            m_ControllersHolder.RhytmController.OnEventProcessingTick += m_ControllersHolder.CommandsController.ProcessPendingCommands;

            m_GameStateMachine.ChangeState(m_GameStateBattle);      //Change state
            ManagersHolder.UIManager.ToBattleStartUIState();        //Show Battle UI
        }

        private void BattleEnemyDestroyedHandler(bool lastEnemyDestroyed)
        {
            Debug.LogError("Battle - Enemy destroyed " + lastEnemyDestroyed);

            //Unscribe from events
            m_ControllersHolder.RhytmController.OnTick -= m_ControllersHolder.BattleController.ProcessEnemyActions;
            m_ControllersHolder.RhytmController.OnEventProcessingTick -= EventProcessingTickHandler;
            m_ControllersHolder.RhytmController.OnEventProcessingTick -= m_ControllersHolder.CommandsController.ProcessPendingCommands;

            //UI
            if (!lastEnemyDestroyed)
                ManagersHolder.UIManager.ToWaitingForNextEnemyActivationUIState();

            m_GameStateMachine.ChangeState(m_GameStateIdle);            //Change state
        }

        private void BattleFinishedHandler()
        {
            Debug.LogError("Battle - Finished");

            m_ControllersHolder.PlayerCharacterController.FinishBattle();       //Finish battle for player
            m_GameStateMachine.ChangeState(m_GameStateAdventure);               //Change state
            ManagersHolder.UIManager.ToAdventureUIState();                      //Finish battle for UI    

            Rhytm.volume = 0;   //Debug - Finish battle for sound
        }
        #endregion

        #region Rhytm
        private void TickingStartedHandler()
        {
            Music.Play();
            Rhytm.Play();
            Rhytm.volume = 0;

            //Metronome.StartMetronome();
        }

        private void TickHandler(int ticksSinceStart)
        {
            BeatSound.Play();
        }

        private void EventProcessingTickHandler(int ticksSinceStart)
        {
            BeatSound.Play();
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
            m_Poitions--;
            UpdatePoitionAmount();
            SipSound.Play();
            AssetsManager.GetPrefabAssets().InstantiatePrefab<AbstractVisualEffect>(AssetsManager.GetPrefabAssets().HealEffectPrefab,
                                                                                PlayerModel.ViewPosition,
                                                                                Quaternion.Euler(-90, 0, 0)).ScheduleHideView();

            PlayerModel.HealthBehaviour.IncreaseHP(5);
        }

        private void UpdatePoitionAmount()
        {
            ManagersHolder.UIManager.Text_PotionAmount.text = $"x{m_Poitions}";

            ManagersHolder.UIManager.Text_PotionAmount.GetComponent<CanvasGroup>().alpha = m_Poitions > 0 ? 1 : 0.5f;

            Color color = ManagersHolder.UIManager.Button_Potion.image.color;
            color.a = m_Poitions > 0 ? 1 : 0.5f;
            ManagersHolder.UIManager.Button_Potion.image.color = color;
        }
        #endregion

        #region Temp
        private void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 150, 100), m_ControllersHolder.RhytmController.DeltaInput.ToString());
        }

        System.Collections.IEnumerator debug_start_tick_coroutine()
        {
            Music.PlayDelayed(1);
            yield return new WaitForSeconds(1);
            m_ControllersHolder.RhytmController.StartTicking();
        }
        #endregion
    }
}
