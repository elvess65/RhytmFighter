﻿using FrameworkPackage.Utils;
using Frameworks.Grid.View;
using RhytmFighter.Battle.Action.Behaviours;
using RhytmFighter.Battle.Health.Behaviours;
using RhytmFighter.Data;
using RhytmFighter.GameState;
using RhytmFighter.Objects.Model;
using System.Collections.Generic;
using UnityEngine;
using RhytmFighter.Core.Enums;
using UnityEngine.UI;

namespace RhytmFighter.Core
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager m_Instance;
        public static GameManager Instance => m_Instance;

        [Header("Links")]
        public ManagersHolder ManagersHolder;
        public Transform CameraRoot;
        public UnityEngine.Camera WorldCamera;

        [Header("Temp")]
        public GameObject BeatIndicatorTemp;
        public AudioSource Music;
        public AudioSource Rhytm;
        public AudioSource AttackSound;
        public AudioSource DefenceExecuteSound;
        public AudioSource HitSound;
        public AudioSource DestroySound;
        public AudioSource DefenceSound;
        public AudioSource BeatSound;
        public AudioSource FinishBattleSound;
        public Metronome Metronome;
        public Text BattleText;
        public UnityEngine.UI.Button Btn;

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

        private void Initialize()
        {
            int bpm = 130 / 2;
            double inputPrecious = 0.25;
            Metronome.bpm = bpm * 2;
            Music.clip.LoadAudioData();
            Rhytm.clip.LoadAudioData();

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

            //Initialize updatables
            m_Updateables = new List<iUpdatable>
            {
                m_ControllersHolder.InputController,
                m_ControllersHolder.RhytmController,
                m_ControllersHolder.CameraController,
                m_ControllersHolder.CommandsController,
                m_ControllersHolder.BattleController,
                m_GameStateMachine
            };

            //Subscribe for events
            m_ControllersHolder.InputController.OnTouch += m_GameStateMachine.HandleTouch;

            //m_ControllersHolder.BattleController.OnPrepareForBattle += PrepareForBattleHandler;
            //m_ControllersHolder.BattleController.OnBattleStarted += BattleStartedHandler;
            //m_ControllersHolder.BattleController.OnEnemyDestroyed += BattleEnemyDestroyedHandler;
            //m_ControllersHolder.BattleController.OnBattleFinished += BattleFinishedHandler;

            m_ControllersHolder.RhytmController.OnTick += TickHandler;
            m_ControllersHolder.RhytmController.OnStarted += TickingStartedHandler;

            //Temp
            Btn.onClick.AddListener(BtnPressHandler);
            PLAYER_MOVE_SPEED = (float)m_ControllersHolder.RhytmController.TickDurationSeconds * 4f;
            ENEMY_MOVE_SPEED = PLAYER_MOVE_SPEED;

            //Initialize connection
            m_DataHolder.DBProxy.OnConnectionSuccess += ConnectionResultSuccess;
            m_DataHolder.DBProxy.OnConnectionError += ConnectionResultError;
            m_DataHolder.DBProxy.Initialize();
        }

        private void InitializationFinished()
        {
            //Start beat
            m_ControllersHolder.RhytmController.StartTicking();
            //StartCoroutine(debug_tick());

            //Change state
            m_GameStateMachine.ChangeState(m_GameStateAdventure);

            //TEMP
            m_ControllersHolder.PlayerCharacterController.ForceFistBattle();
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

            SimpleHealthBehaviour healthBehaviour = new SimpleHealthBehaviour(20);
            SimpleBattleActionBehaviour battleBehaviour = new SimpleBattleActionBehaviour(1, 1, 2);
            CellView startCellView = m_ControllersHolder.LevelController.RoomViewBuilder.GetCellVisual(m_ControllersHolder.LevelController.Model.GetCurrenRoomData().ID,
                m_ControllersHolder.LevelController.Model.GetCurrenRoomData().GridData.WidthInCells / 2, 0);

            //Create player model
            PlayerModel playerModel = new PlayerModel(playerID, startCellView.CorrespondingCellData, PLAYER_MOVE_SPEED, battleBehaviour, healthBehaviour);
            m_ControllersHolder.PlayerCharacterController.CreateCharacter(playerModel, startCellView, m_ControllersHolder.LevelController);
            playerModel.OnDestroyed += PlayerDestroyedHandler;

            //Set player to battle controller
            m_ControllersHolder.BattleController.Player = m_ControllersHolder.PlayerCharacterController.PlayerModel;

            //Focus camera on character
            m_ControllersHolder.CameraController.InitializeCamera(CameraRoot, m_ControllersHolder.PlayerCharacterController.PlayerModel.View.transform, ManagersHolder.SettingsManager.CameraSettings.NormalMoveSpeed);

            //Finish initialization
            InitializationFinished();
        }


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


        private void PlayerDestroyedHandler(iBattleObject sender)
        {
            Debug.LogError("DESTROY PLAYER");

            BattleFinishedHandler();
            m_GameStateMachine.ChangeState(m_GameStateIdle);
        }


        private void PrepareForBattleHandler()
        {
            Debug.LogError("Battle - Prepare");

            //Debug - Show battle UI
            BeatIndicatorTemp.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            BeatIndicatorTemp.GetComponent<Image>().color = Color.yellow;
            BattleText.text = "Prepare for battle";
            BattleText.color = Color.yellow;
            StartCoroutine(disable_battle_ui());

            //No need to stop movement if players destination cell is the cell where enemy was detected
            if (m_ControllersHolder.PlayerCharacterController.PlayerModel.IsMoving)
                m_ControllersHolder.PlayerCharacterController.StopMove();

            m_GameStateMachine.ChangeState(m_GameStateIdle);
        }

        private void BattleStartedHandler()
        {
            Debug.LogError("Battle - Start");

            //Debug - Sound
            Rhytm.volume = 0.5f;
            playBeat = true;

            //Debug - Show Battle ui
            BeatIndicatorTemp.GetComponent<Image>().color = Color.red;
            BeatIndicatorTemp.transform.localScale = new Vector3(1f, 1f, 1f);
            BattleText.text = "Fight";
            BattleText.color = Color.red;
            scaleMltp = 2;
            StartCoroutine(disable_battle_ui());

            m_ControllersHolder.RhytmController.OnEventProcessingTick += EventProcessingTickHandler;
            m_ControllersHolder.RhytmController.OnTick += m_ControllersHolder.BattleController.ProcessEnemyActions;
            m_ControllersHolder.RhytmController.OnEventProcessingTick += m_ControllersHolder.CommandsController.ProcessPendingCommands;

            m_GameStateMachine.ChangeState(m_GameStateBattle);
        }


        private void BattleEnemyDestroyedHandler()
        {
            Debug.LogError("Battle - Enemy destroyed");

            m_ControllersHolder.RhytmController.OnTick -= m_ControllersHolder.BattleController.ProcessEnemyActions;
            m_ControllersHolder.RhytmController.OnEventProcessingTick -= m_ControllersHolder.CommandsController.ProcessPendingCommands;

            m_GameStateMachine.ChangeState(m_GameStateIdle);
        }

        private void BattleFinishedHandler()
        {
            Debug.LogError("Battle - Finished");

            //Hide UI
            BeatIndicatorTemp.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            BeatIndicatorTemp.GetComponent<Image>().color = Color.green;
            BattleText.text = "Victory";
            BattleText.color = Color.green;
            StartCoroutine(disable_battle_ui());

            //FinishBattleSound.Play();

            Rhytm.volume = 0;
            scaleMltp = 1;
            playBeat = false;

            m_ControllersHolder.RhytmController.OnEventProcessingTick -= EventProcessingTickHandler;

            m_GameStateMachine.ChangeState(m_GameStateAdventure);
        }

      
        private void TickingStartedHandler()
        {
            Music.Play();
            //Rhytm.Play();
            //Rhytm.volume = 0;
            Metronome.StartMetronome();
        }

        private void TickHandler(int ticksSinceStart)
        {
            if (playBeat)
                BeatSound.Play();

            StartCoroutine(BeatIndicatorTempCoroutine());
        }

        private void EventProcessingTickHandler(int ticksSinceStart)
        {
            if (playBeat)
                BeatSound.Play();

            StartCoroutine(BeatIndicatorTempCoroutine());
        }

        System.Collections.IEnumerator BeatIndicatorTempCoroutine()
        {
            BeatIndicatorTemp.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f) * scaleMltp;
            yield return new WaitForSeconds((float)m_ControllersHolder.RhytmController.TickDurationSeconds / 2);
            BeatIndicatorTemp.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f) * scaleMltp;
        }


        private void PlayerInteractWithObject(AbstractInteractableObjectModel interactableObject)
        {
            interactableObject.Interact();
        }

        System.Collections.IEnumerator TEMP_INTERATCION_COROUTINE(float animationDelay)
        {
            yield return new WaitForSeconds(animationDelay);

            m_GameStateMachine.ChangeState(m_GameStateAdventure);
        }

        //Temp
        void BtnPressHandler()
        {
            bool inputIsValid = m_ControllersHolder.RhytmInputProxy.IsInputTickValid();
            if (inputIsValid)
                m_ControllersHolder.PlayerCharacterController.ExecuteAction(CommandTypes.Defence);
        }

        private bool playBeat = false;
        private float scaleMltp = 1;

        System.Collections.IEnumerator debug_tick()
        {
            Music.PlayScheduled(AudioSettings.dspTime + 1);
            yield return new WaitForSeconds(1);
            m_ControllersHolder.RhytmController.StartTicking();
        }

        System.Collections.IEnumerator disable_battle_ui()
        {
            BattleText.gameObject.SetActive(true);
            yield return new WaitForSeconds((float)RhytmFighter.Rhytm.RhytmController.GetInstance().TickDurationSeconds);
            BattleText.gameObject.SetActive(false);
        }
    }
}
