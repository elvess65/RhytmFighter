using UnityEngine;
using RhytmFighter.Level.Data;
using RhytmFighter.Level.Scheme.Builder;
using Frameworks.Grid.View;
using Frameworks.Grid.Data;
using RhytmFighter.Data;
using RhytmFighter.Data.Models.DataTableModels;

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

        private EnvironmentDataModel.LevelParams m_LevelParamsData;
        private float m_CompletionProgress;


        public LevelController()
        {
            m_LevelDataBuilder = new LevelDataBuilder();
            m_RoomDataBuilder = new RoomDataBuilder();
            RoomViewBuilder = new GridViewBuilder();

            Model = new LevelDataModel();
        }

        public void GenerateLevel(EnvironmentDataModel.LevelParams levelParamsData, bool generateOnlyMainPath, bool buildRoomView, float completionProgress)
        {
            Debug.Log($"LevelController: Generate level {levelParamsData.ID}. Completion progress {completionProgress}");

            //Cache level params
            m_LevelParamsData = levelParamsData;
            m_CompletionProgress = completionProgress;

            //Build level data and store start node to model
            BuildLevelData(generateOnlyMainPath);

            //Build room for start node and store it to model
            BuildRoomData(Model.StartNodeData, true, true);

            //Build room view
            if (buildRoomView)
                RoomViewBuilder.Build(Model.GetCurrenRoomData(), Vector3.zero, levelParamsData.EnviromentParams);
        }

        public void BuildLevelData(bool generateOnlyMainPath)
        {
            int depth = GetRandomDepthFromProgression(m_LevelParamsData.BuildParams.LevelProgressionConfig, m_CompletionProgress);
            LevelNodeData nodeData = m_LevelDataBuilder.Build(depth, m_LevelParamsData.BuildParams.Seed, generateOnlyMainPath);
            Model.StartNodeData = nodeData;
        }

        public LevelRoomData BuildRoomData(LevelNodeData node, bool storeToModel, bool isCurrent)
        {
            LevelRoomData roomData = null;

            //Option - store to model
            if (storeToModel)
            {
                //If room was stored - get data from model
                if (Model.HasRoom(node.ID))
                    roomData = Model.GetRoomDataByID(node.ID);
                //If room was not stored - create room and store to model
                else
                { 
                    roomData = CreateRoomData(node);
                    Model.AddRoom(roomData);
                }

                //Option - set as current
                if (isCurrent)
                    Model.SetRoomAsCurrent(node.ID);
            }
            else
                roomData = CreateRoomData(node);

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
                RoomViewBuilder.Build(levelRoomData, RoomViewBuilder.GetStartPositionForNextView(gateCellData, levelRoomData.GridData.ParentNodeGate.X), 
                                      m_LevelParamsData.EnviromentParams);
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
                RoomViewBuilder.Build(levelRoomData, RoomViewBuilder.GetStartPositionForParentView(Model.GetCurrenRoomData().GridData.ParentNodeGate, gateCellData, nodeDirectionOffset),
                                     m_LevelParamsData.EnviromentParams);
        }

        public void RemoveRoom(int roomID)
        {
            //Model.RemoveRoom(roomID);
            RoomViewBuilder.RemoveRoomView(Model.GetRoomDataByID(roomID));
        }


        private LevelRoomData CreateRoomData(LevelNodeData node)
        {
            return m_RoomDataBuilder.Build(node, m_LevelParamsData.BuildParams, m_LevelParamsData.ContentParams, m_CompletionProgress);
        }

        private int GetRandomDepthFromProgression(LevelSizeProgressionConfig progressionConfig, float t)
        {
            (int min, int max) result = progressionConfig.EvaluateDepth(t);
            return Random.Range(result.min, result.max + 1);
        }
    }
}
