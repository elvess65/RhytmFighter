using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using RhytmFighter.Characters;
using RhytmFighter.Data;
using RhytmFighter.Interfaces;
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
        private ControllersHolder m_ControllersHolder;
        private List<iUpdateable> m_Updateables;


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
            m_ControllersHolder = new ControllersHolder();

            //Initialize updatables
            m_Updateables = new List<iUpdateable>();
            m_Updateables.Add(m_ControllersHolder.InputController);
            m_Updateables.Add(m_ControllersHolder.CameraController);
            m_Updateables.Add(m_ControllersHolder.PlayerCharacterController);

            //Subscribe for events
            m_ControllersHolder.InputController.OnTouch += m_ControllersHolder.GridInputProxy.TryGetCellFromInput;
            m_ControllersHolder.GridInputProxy.OnCellInput += CellInputHandler;

            m_ControllersHolder.PlayerCharacterController.OnMovementFinished += MovementFinishedHandler;
            m_ControllersHolder.PlayerCharacterController.OnCellVisited += CellVisitedHandler;
            m_ControllersHolder.PlayerCharacterController.OnMovementInterrupted += MovementInterruptedHandler;

            //Initialize connection
            m_DataHolder.DBProxy.OnConnectionSuccess += ConnectionResultSuccess;
            m_DataHolder.DBProxy.OnConnectionError += ConnectionResultError;
            m_DataHolder.DBProxy.Initialize(ManagersHolder.SettingsManager.ProxySettings);
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
        }


        private void CellInputHandler(CellView cellView)
        {
            Debug.LogError(cellView.CorrespondingCellData.HasObject);
            m_ControllersHolder.PlayerCharacterController.MoveCharacter(cellView);
        }

        private void MovementFinishedHandler(GridCellData cellData)
        {
            Debug.LogError("MovementFinishedHandler " + cellData.ToString());

            //Refresh grid
            m_ControllersHolder.GridPositionTrackingController.Refresh(cellData);
        }

        private void CellVisitedHandler(GridCellData cellData)
        {
            Debug.LogError("CellVisitedHandler " + cellData.ToString());

            //Refresh grid
            m_ControllersHolder.GridPositionTrackingController.Refresh(cellData);
        }

        private void MovementInterruptedHandler()
        {
            Debug.LogError("MovementInterruptedHandler");
        }
    }
}
