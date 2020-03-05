using UnityEngine;
using RhytmFighter.Level.Data;
using RhytmFighter.Level.Scheme.Builder;
using Frameworks.Grid.View;
using Frameworks.Grid.Data;
using RhytmFighter.Data;

namespace RhytmFighter.Level
{
    public class LevelController 
    {
        private LevelDataBuilder m_LevelDataBuilder;
        private RoomDataBuilder m_RoomDataBuilder;
        private LevelSchemeBuilder m_LevelSchemeBuilder;
        private RoomSchemeBuilder m_RoomSchemeBuilder;

        public LevelDataModel Model { get; private set; }
        public LevelSchemeBuilder LevelSchemeBuilder
        {
            get
            {
                if (m_LevelSchemeBuilder == null)
                    m_LevelSchemeBuilder = new LevelSchemeBuilder();

                return m_LevelSchemeBuilder;
            }
        }
        public RoomSchemeBuilder RoomSchemeBuilder
        {
            get
            {
                if (m_RoomSchemeBuilder == null)
                    m_RoomSchemeBuilder = new RoomSchemeBuilder();

                return m_RoomSchemeBuilder;
            }
        }
        public GridViewBuilder RoomViewBuilder { get; private set; }

        private LevelsData.LevelParams m_LevelParamsData;


        public LevelController()
        {
            m_LevelDataBuilder = new LevelDataBuilder();
            m_RoomDataBuilder = new RoomDataBuilder();
            RoomViewBuilder = new GridViewBuilder();

            Model = new LevelDataModel();
        }

        public void GenerateLevel(LevelsData.LevelParams levelParamsData, bool generateOnlyMainPath, bool buildRoomView)
        {
            Debug.Log($"LevelController: Generate level {levelParamsData.ID}");

            //Cache level params
            m_LevelParamsData = levelParamsData;

            //Build level data and store start node to model
            BuildLevelData(generateOnlyMainPath);

            //Build room for start node and store it to model
            BuildRoomData(Model.StartNodeData, true, true);

            //Build room view
            if (buildRoomView)
                RoomViewBuilder.Build(Model.GetCurrenRoomData(), Vector3.zero);
        }

        public void BuildLevelData(bool generateOnlyMainPath)
        {
            LevelNodeData nodeData = m_LevelDataBuilder.Build(m_LevelParamsData.LevelDepth, m_LevelParamsData.LevelSeed, generateOnlyMainPath);
            Model.StartNodeData = nodeData;
        }

        public LevelRoomData BuildRoomData(LevelNodeData node, bool storeToModel, bool isCurrent)
        {
            //Build room data anyway
            LevelRoomData roomData = m_RoomDataBuilder.Build(node, m_LevelParamsData.MinWidth, m_LevelParamsData.MaxWidth, 
                                                                   m_LevelParamsData.MinHeight, m_LevelParamsData.MaxWidth, 
                                                                   m_LevelParamsData.CellSize, m_LevelParamsData.FillPercent);

            //Option - store to model
            if (storeToModel)
            {
                //Add room to model active rooms
                Model.AddRoom(roomData);

                //Option - set as current
                if (isCurrent)
                    Model.SetRoomAsCurrent(roomData.ID);
            }

            return roomData;
        }


        public void AddNextRoom(LevelNodeData nodeData, bool isRightNode)
        {
            //Build room data
            LevelRoomData levelRoomData = BuildRoomData(nodeData, true, false);

            //Build room view
            //Take right or left node cell
            GridCellData gateCellData = isRightNode ? Model.GetCurrenRoomData().GridData.RightNodeGate : Model.GetCurrenRoomData().GridData.LeftNodeGate;
            if (gateCellData != null)
                RoomViewBuilder.Build(levelRoomData, RoomViewBuilder.GetStartPositionForNextView(gateCellData, levelRoomData.GridData.ParentNodeGate.X));
        }

        public void AddParentRoom(LevelNodeData nodeData, out bool isRightRoom)
        {
            //Left room by default
            isRightRoom = false;

            //Build room data
            LevelRoomData levelRoomData = BuildRoomData(nodeData, true, false);

            //Right or left node
            int nodeDirectionOffset = 1;
            GridCellData gateCellData = null;

            //Check right room
            if (nodeData.RightNode != null && Model.CurrentRoomID == nodeData.RightNode.ID)
            {
                gateCellData = levelRoomData.GridData.RightNodeGate;
                nodeDirectionOffset = -1;
                isRightRoom = true;
            }
            //Check left room
            else if (nodeData.LeftNode != null && Model.CurrentRoomID == nodeData.LeftNode.ID)
                gateCellData = levelRoomData.GridData.LeftNodeGate;

            //Build room view
            if (gateCellData != null)
                RoomViewBuilder.Build(levelRoomData, RoomViewBuilder.GetStartPositionForParentView(Model.GetCurrenRoomData().GridData.ParentNodeGate, gateCellData, nodeDirectionOffset));
        }

        public void RemoveRoom(int roomID)
        {
            Model.RemoveRoom(roomID);
            RoomViewBuilder.RemoveRoom(roomID);
        }
    }
}
