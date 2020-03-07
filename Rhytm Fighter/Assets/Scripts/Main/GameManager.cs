using Frameworks.Grid.Data;
using Frameworks.Grid.View;
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
        public GameObject Player;//temp

        private DataHolder m_DataHolder;
        private ControllersHolder m_ControllersHolder;

        private List<iUpdateable> m_Updateables;

        private Vector3 targetPos; //temp


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
                m_Updateables[i].Update(Time.deltaTime);

            Player.transform.position = Vector3.MoveTowards(Player.transform.position, targetPos, Time.deltaTime * 5);
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

            //Move player 
            CellView startCellView = m_ControllersHolder.LevelController.RoomViewBuilder.GetCellVisual(m_ControllersHolder.LevelController.Model.GetCurrenRoomData().ID, 0, 0);
            Player.transform.position = startCellView.transform.position;

            //Hide all cells except start cell
            m_ControllersHolder.LevelController.RoomViewBuilder.HideAllCellsExept(m_ControllersHolder.LevelController.Model.GetCurrenRoomData(), startCellView.CorrespondingCellData);

            //Extend view
            m_ControllersHolder.LevelController.RoomViewBuilder.ExtendView(m_ControllersHolder.LevelController.Model.GetCurrenRoomData(), startCellView.CorrespondingCellData);

            //Focus camera on player
            m_ControllersHolder.CameraController.InitializeCamera(CameraRoot, Player.transform, ManagersHolder.SettingsManager.CameraSettings.NormalMoveSpeed);
        }

        
        private void CellInputHandler(CellView cellView)
        {
            //Move player temp
            targetPos = cellView.transform.position;

            //Refresh grid
            m_ControllersHolder.GridPositionTrackingController.Refresh(cellView.CorrespondingCellData);
        }
    }
}
