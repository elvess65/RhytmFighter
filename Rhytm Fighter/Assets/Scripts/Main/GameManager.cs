using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using RhytmFighter.Characters;
using RhytmFighter.Data;
using RhytmFighter.GameState;
using RhytmFighter.Interfaces;
using RhytmFighter.Objects;
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
            m_GameStateBattle = new GameState_Battle(m_ControllersHolder.PlayerCharacterController);
            m_GameStateAdventure = new GameState_Adventure(m_ControllersHolder.LevelController, m_ControllersHolder.PlayerCharacterController);
            m_GameStateAdventure.OnPlayerInteractWithItem += PlayerInteractWithItemHandler;
            m_GameStateMachine.Initialize(m_GameStateIdle);

            //Initialize updatables
            m_Updateables = new List<iUpdatable>
            {
                m_ControllersHolder.InputController,
                m_ControllersHolder.CameraController,
                m_GameStateMachine
            };

            //Initialize connection
            m_DataHolder.DBProxy.OnConnectionSuccess += ConnectionResultSuccess;
            m_DataHolder.DBProxy.OnConnectionError += ConnectionResultError;
            m_DataHolder.DBProxy.Initialize(ManagersHolder.SettingsManager.ProxySettings);

            //Subscribe for events
            m_ControllersHolder.InputController.OnTouch += m_GameStateMachine.HandleTouch;
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
            m_ControllersHolder.LevelController.RoomViewBuilder.OnCellWithObjectDetected +=
                (CellView cell, iGridObject objectInCell) =>
                {
                    //For update view
                    Debug.Log("Cell with object was detected in: " + cell + " Object: " + objectInCell.Type);
                    cell.transform.position += new Vector3 (0, -0.1f, 0);
                };
        }

        private void CreatePlayer()
        {
            //Temp 
            //TODO Get from data
            float playerMoveSpeed = 1;

            //Create player character
            m_ControllersHolder.PlayerCharacterController.CreateCharacter(Player, playerMoveSpeed, m_ControllersHolder.LevelController.RoomViewBuilder.GetCellVisual(m_ControllersHolder.LevelController.Model.GetCurrenRoomData().ID, 0, 0), m_ControllersHolder.LevelController);

            //Focus camera on character
            m_ControllersHolder.CameraController.InitializeCamera(CameraRoot, m_ControllersHolder.PlayerCharacterController.PlayerCharacter.transform, ManagersHolder.SettingsManager.CameraSettings.NormalMoveSpeed);

            //Chacge state
            m_GameStateMachine.ChangeState(m_GameStateAdventure);
        }


        private void PlayerInteractWithItemHandler(iGridObject gridObject)
        {
            Debug.LogError("Interact with item " + gridObject.ID + " " + gridObject.Type);
            m_GameStateMachine.ChangeState(m_GameStateIdle);
            StartCoroutine(TEMP_INTERATCION_COROUTINE());
        }

        System.Collections.IEnumerator TEMP_INTERATCION_COROUTINE()
        {
            yield return new WaitForSeconds(1);

            m_GameStateMachine.ChangeState(m_GameStateBattle);
        }
    }
}
