using Frameworks.Grid.View;
using RhytmFighter.Battle;
using RhytmFighter.Battle.Action.Behaviours;
using RhytmFighter.Battle.Health.Behaviours;
using RhytmFighter.Data;
using RhytmFighter.GameState;
using RhytmFighter.Interfaces;
using RhytmFighter.Objects.Model;
using System.Collections.Generic;
using UnityEngine;

namespace RhytmFighter.Main
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager m_Instance;
        public static GameManager Instance => m_Instance;

        [Header("Links")]
        public ManagersHolder ManagersHolder;
        public Transform CameraRoot;

        [Header("Temp")]
        public GameObject BeatIndicatorTemp;

        private DataHolder m_DataHolder;
        private GameStateMachine m_GameStateMachine;
        private ControllersHolder m_ControllersHolder;
        private List<iUpdatable> m_Updateables;

        //States
        private GameState_Idle m_GameStateIdle;
        private GameState_Battle m_GameStateBattle;
        private GameState_Adventure m_GameStateAdventure;


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
            //Initialize objects
            m_DataHolder = new DataHolder();
            m_GameStateMachine = new GameStateMachine();
            m_ControllersHolder = new ControllersHolder();

            //Initialize managers
            ManagersHolder.Initialize();

            //Initialize state machine
            m_GameStateIdle = new GameState_Idle(m_ControllersHolder.PlayerCharacterController);
            m_GameStateBattle = new GameState_Battle(m_ControllersHolder.PlayerCharacterController, m_ControllersHolder.RhytmInputProxy);
            m_GameStateAdventure = new GameState_Adventure(m_ControllersHolder.LevelController, m_ControllersHolder.PlayerCharacterController);
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
                m_GameStateMachine
            };

            //Initialize connection
            m_DataHolder.DBProxy.OnConnectionSuccess += ConnectionResultSuccess;
            m_DataHolder.DBProxy.OnConnectionError += ConnectionResultError;
            m_DataHolder.DBProxy.Initialize(ManagersHolder.SettingsManager.ProxySettings);

            //Subscribe for events
            m_ControllersHolder.InputController.OnTouch += m_GameStateMachine.HandleTouch;
            m_ControllersHolder.BattleController.OnBattleStarted += BattleStartedHandler;
            m_ControllersHolder.BattleController.OnBattleFinished += BattleFinishedHandler;
            m_ControllersHolder.RhytmController.OnBeat += BeatHandler;
        }

        private void InitializationFinished()
        {
            //Start beat
            m_ControllersHolder.RhytmController.StartBeat();

            //Chacge state
            m_GameStateMachine.ChangeState(m_GameStateAdventure);
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
            float playerMoveSpeed = 2;
            CellView startCellView = m_ControllersHolder.LevelController.RoomViewBuilder.GetCellVisual(m_ControllersHolder.LevelController.Model.GetCurrenRoomData().ID, 0, 0);

            //Initialize character controller
            SimpleBattleActionBehaviour battleBehaviour = new SimpleBattleActionBehaviour(1, 1, 2);
            SimpleHealthBehaviour healthBehaviour = new SimpleHealthBehaviour(5);

            PlayerModel playerModel = new PlayerModel(0, startCellView, playerMoveSpeed, battleBehaviour, healthBehaviour);
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
                    {
                        m_ControllersHolder.BattleController.AddEnemyToActiveBattle(battleObject);
                        m_ControllersHolder.PlayerCharacterController.PlayerModel.Target = m_ControllersHolder.BattleController.GetClosestEnemy(m_ControllersHolder.PlayerCharacterController.PlayerModel);
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
            StartCoroutine(TEMP_INTERATCION_COROUTINE(2));
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


        private void BattleStartedHandler()
        {
            Debug.LogError("BEGIN BATTLE");

            m_ControllersHolder.RhytmController.OnBeat += m_ControllersHolder.BattleController.ProcessEnemyActions;
            m_ControllersHolder.RhytmController.OnBeat += m_ControllersHolder.CommandsController.ProcessPendingCommands;

            m_ControllersHolder.PlayerCharacterController.StopMove();

            m_GameStateMachine.ChangeState(m_GameStateBattle);
        }

        private void BattleFinishedHandler()
        {
            Debug.LogError("BATTLE FINISHED");

            m_ControllersHolder.RhytmController.OnBeat -= m_ControllersHolder.BattleController.ProcessEnemyActions;
            m_ControllersHolder.RhytmController.OnBeat -= m_ControllersHolder.CommandsController.ProcessPendingCommands;

            m_ControllersHolder.PlayerCharacterController.PlayerModel.Target = null;

            m_GameStateMachine.ChangeState(m_GameStateAdventure);
        }


        private void BeatHandler()
        {
            StartCoroutine(BeatIndicatorTempCoroutine());
        }

        System.Collections.IEnumerator BeatIndicatorTempCoroutine()
        {
            BeatIndicatorTemp.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            for (int i = 0; i < 5; i++)
                yield return null;
            BeatIndicatorTemp.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
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
    }
}
