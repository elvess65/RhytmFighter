using Frameworks.Grid.Data;
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
        public GameObject Player; //temp

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
                m_Updateables[i].Update(Time.deltaTime);
        }

        private void Initialize()
        {
            //Initialize objects
            m_DataHolder = new DataHolder();
            m_ControllersHolder = new ControllersHolder();

            //Initialize updatables
            m_Updateables = new List<iUpdateable>();
            m_Updateables.Add(m_ControllersHolder.InputController);

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
            m_DataHolder.PlayerData = PlayerData.DeserializeData(serializedPlayerData);
            m_DataHolder.InfoData = new InfoData(serializedLevelsData);

            //Build level
            BuildLevel();

            //Create player
            CreatePlayer();
        }

        private void ConnectionResultError(int errorCode) => Debug.LogError($"Connection error {errorCode}");


        private void BuildLevel()
        {
            m_ControllersHolder.LevelController.GenerateLevel(m_DataHolder.InfoData.LevelsData.LevelDepth, m_DataHolder.InfoData.LevelsData.LevelSeed, false, true);
        }

        private void CreatePlayer()
        {
            //Move player temp
            Player.transform.position = m_ControllersHolder.LevelController.RoomViewBuilder.GetCellVisual(m_ControllersHolder.LevelController.Model.GetCurrenRoomData().NodeData.ID, 0, 0).transform.position;
        }


        private void CellInputHandler(Frameworks.Grid.View.CellView cellView)
        {
            //Move player temp
            Player.transform.position = cellView.transform.position;

            m_ControllersHolder.GridPositionTrackingController.Refresh(cellView.CorrespondingCellData);
        }
    }
}
