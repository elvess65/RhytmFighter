using Frameworks.Grid.Data;
using RhytmFighter.Level;
using RhytmFighter.Level.Data;
using UnityEngine;

namespace RhytmFighter.Input
{
    /// <summary>
    /// Tracking change position of target object on grid
    /// </summary>
    public class GridPositionTrackingController
    {
        private LevelController m_LevelController;

        private GridCellData m_PrevCell;
        private int m_CreatedOtherRoomID = -1;


        public GridPositionTrackingController(LevelController lvlController)
        {
            m_LevelController = lvlController;
        }


        public void Refresh(GridCellData cellData)
        {
            //Cell was changed
            if (!cellData.IsEqualCoord(m_PrevCell))
            {
                //Click the cell in the same room
                if (cellData.CorrespondingRoomID.Equals(m_LevelController.Model.CurrentRoomID))
                {
                    //If other room was created
                    if (m_CreatedOtherRoomID >= 0)
                    {
                        m_LevelController.RemoveRoom(m_CreatedOtherRoomID);
                        m_CreatedOtherRoomID = -1;
                    }

                    //Get room data that cell belongs to
                    LevelRoomData cellRoomData = m_LevelController.Model.GetRoomDataByID(cellData.CorrespondingRoomID);

                    //Visit parent node
                    if (cellData.IsEqualCoord(cellRoomData.GridData.ParentNodeGate))
                    {
                        m_LevelController.AddParentRoom(cellRoomData.NodeData.ParentNode);
                        m_CreatedOtherRoomID = cellRoomData.NodeData.ParentNode.ID;
                    }
                    //Visit right node
                    else if (cellData.IsEqualCoord(cellRoomData.GridData.RightNodeGate))
                    {
                        m_LevelController.AddNextRoom(cellRoomData.NodeData.RightNode, true);
                        m_CreatedOtherRoomID = cellRoomData.NodeData.RightNode.ID;
                    }
                    //Visit left node
                    else if (cellData.IsEqualCoord(cellRoomData.GridData.LeftNodeGate))
                    {
                        m_LevelController.AddNextRoom(cellRoomData.NodeData.LeftNode, false);
                        m_CreatedOtherRoomID = cellRoomData.NodeData.LeftNode.ID;//m_ControllersHolder.LevelController.Model.GetCurrenRoomData().NodeData.LeftNode.ID;
                    }
                }
                //Click the cell in other room
                else
                {
                    Debug.LogError("Transition to other room");

                    //Cache current room id
                    int currentRoomID = m_LevelController.Model.CurrentRoomID;

                    //Set transitioned room as current
                    m_LevelController.Model.SetRoomAsCurrent(m_CreatedOtherRoomID);

                    //Set created room as previous current (in case player continue to move the new room previous room will be removed on next move)
                    //If player moves back the previous room - swaps back ids to make entered room to be created and to make it remove if player move the previous room
                    m_CreatedOtherRoomID = currentRoomID;
                }

                //Cache prev cell
                m_PrevCell = cellData;
            }
        }
    }
}
