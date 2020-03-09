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
        public Character Player;//temp

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

            //Subscribe for events
            m_ControllersHolder.InputController.OnTouch += m_ControllersHolder.GridInputProxy.TryGetCellFromInput;
            m_ControllersHolder.GridInputProxy.OnCellInput += CellInputHandler;

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

            //Init start cell
            CellView startCellView = m_ControllersHolder.LevelController.RoomViewBuilder.GetCellVisual(m_ControllersHolder.LevelController.Model.GetCurrenRoomData().ID, 0, 0);
            startCellView.CorrespondingCellData.IsVisited = true;
            currentCell = startCellView;

            //Place player on start cell
            Player.Initialize(startCellView.transform.position, 1);
            Player.OnMovementFinished += PlayerArrivedHandler;
            Player.OnCellVisited += CellVisited;
            m_Updateables.Add(Player);

            //Focus camera on player
            m_ControllersHolder.CameraController.InitializeCamera(CameraRoot, Player.transform, ManagersHolder.SettingsManager.CameraSettings.NormalMoveSpeed);

            //Hide all cells except start cell
            m_ControllersHolder.LevelController.RoomViewBuilder.HideAllUnvisitedCells(m_ControllersHolder.LevelController.Model.GetCurrenRoomData(), startCellView.CorrespondingCellData);

            //Extend view
            m_ControllersHolder.LevelController.RoomViewBuilder.ExtendView(m_ControllersHolder.LevelController.Model.GetCurrenRoomData(), startCellView.CorrespondingCellData);
        }

        private CellView currentCell;
        private void CellInputHandler(CellView cellView)
        {
            GridCellData[] path = m_ControllersHolder.LevelController.Model.GetCurrenRoomData().GridData.FindPathCells(currentCell.CorrespondingCellData, cellView.CorrespondingCellData);
            currentCell = cellView;

            List<Vector3> positions = new List<Vector3>();
            foreach (GridCellData p in path)
                positions.Add(m_ControllersHolder.LevelController.RoomViewBuilder.GetCellVisual(p.CorrespondingRoomID, p.X, p.Y).transform.position);

            //Move player temp
            Player.StartMove(positions.ToArray());
        }

        void CellVisited(int index)
        {
            Debug.Log("cell visited " + index);
        }

        //Move to player controler
        private void PlayerArrivedHandler(int index)
        {
            Debug.Log("Movement finished " + index);
            //Refresh grid
            //m_ControllersHolder.GridPositionTrackingController.Refresh(cellView.CorrespondingCellData);
        }
    }
}
