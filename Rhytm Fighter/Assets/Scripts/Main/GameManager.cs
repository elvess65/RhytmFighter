using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using RhytmFighter.Battle;
using RhytmFighter.Characters;
using RhytmFighter.Data;
using RhytmFighter.GameState;
using RhytmFighter.Interfaces;
using RhytmFighter.Objects;
using RhytmFighter.Objects.Data;
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
        public CharacterWrapper Player;//temp
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
            float playerMoveSpeed = 5;

            //Create player character
            m_ControllersHolder.PlayerCharacterController.CreateCharacter(Player, playerMoveSpeed, m_ControllersHolder.LevelController.RoomViewBuilder.GetCellVisual(m_ControllersHolder.LevelController.Model.GetCurrenRoomData().ID, 0, 0), m_ControllersHolder.LevelController);

            //Focus camera on character
            m_ControllersHolder.CameraController.InitializeCamera(CameraRoot, m_ControllersHolder.PlayerCharacterController.PlayerCharacter.transform, ManagersHolder.SettingsManager.CameraSettings.NormalMoveSpeed);

            //Finish initialization
            InitializationFinished();
        }


        private void CellWithObjectDetectedHandler(AbstractGridObject gridObject)
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
                        m_ControllersHolder.BattleController.AddEnemyToActiveBattle(battleObject);
                }
            }
        }

        private void PlayerInteractWithItemHandler(AbstractItemGridObject interactableItem)
        {
            //Interact
            PlayerInteractWithObject(interactableItem);

            //Lock input for animation time
            m_GameStateMachine.ChangeState(m_GameStateIdle);

            //Debug animation time
            StartCoroutine(TEMP_INTERATCION_COROUTINE(2));
        }

        private void PlayerInteractWithNPCHandler(AbstractNPCGridObject interactableNPC)
        {
            //Interact
            PlayerInteractWithObject(interactableNPC);

            Debug.Log("INTERACT WITH NPC: " + interactableNPC.View.gameObject.name + " " + interactableNPC.ID + " " + interactableNPC.Type);
        }


        private void BattleStartedHandler()
        {
            Debug.LogError("BEGIN BATTLE");

            m_ControllersHolder.RhytmController.OnBeat += m_ControllersHolder.BattleController.RhytmBeatHandler;

            m_ControllersHolder.PlayerCharacterController.StopMove();
            m_GameStateMachine.ChangeState(m_GameStateBattle);
        }

        private void BattleFinishedHandler()
        {
            Debug.LogError("BATTLE FINISHED");

            m_ControllersHolder.RhytmController.OnBeat -= m_ControllersHolder.BattleController.RhytmBeatHandler;

            m_GameStateMachine.ChangeState(m_GameStateAdventure);
        }


        private void BeatHandler()
        {
            StartCoroutine(BeatIndicatorTempCoroutine());
        }

        System.Collections.IEnumerator BeatIndicatorTempCoroutine()
        {
            BeatIndicatorTemp.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            yield return null;
            yield return null;
            yield return null;
            BeatIndicatorTemp.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
        }


        private void PlayerInteractWithObject(AbstractInteractableGridObject interactableObject)
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
