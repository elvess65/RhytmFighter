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

        private GridCellData m_LastVisitedCell;
        private int m_CreatedOtherRoomID = -1;


        public GridPositionTrackingController(LevelController lvlController)
        {
            m_LevelController = lvlController;
        }


        public void Refresh(GridCellData cellData)
        {
            //Cell was changed
            if (!cellData.IsEqualCoord(m_LastVisitedCell))
            {
                //Click the cell in the same room
                if (cellData.CorrespondingRoomID.Equals(m_LevelController.Model.CurrentRoomID))
                {
                    Debug.Log("MOVE THE SAVE ROOM");
                    //If other room was created - hide created room
                    if (m_CreatedOtherRoomID >= 0)
                    {
                        Debug.Log("HIDE CREATED ROOM");
                        m_LevelController.RemoveRoom(m_CreatedOtherRoomID);
                        m_CreatedOtherRoomID = -1;
                    }

                    //Get room data that cell belongs to (cuurent room)
                    LevelRoomData cellRoomData = m_LevelController.Model.GetCurrenRoomData();

                    //Visit parent node check
                    if (cellData.IsEqualCoord(cellRoomData.GridData.ParentNodeGate))
                    {
                        Debug.Log("Parent node was visited");
                        bool isRightRoom = false;

                        //Add room 
                        m_LevelController.AddParentRoom(cellRoomData.NodeData.ParentNode, out isRightRoom);

                        //Cache created room id
                        m_CreatedOtherRoomID = cellRoomData.NodeData.ParentNode.ID;

                        //Hide all cells of created room exept gate cell
                        if (isRightRoom)
                            m_LevelController.RoomViewBuilder.HideAllUnvisitedCells(m_LevelController.Model.GetRoomDataByID(m_CreatedOtherRoomID),
                                                                                m_LevelController.Model.GetRoomDataByID(m_CreatedOtherRoomID).GridData.RightNodeGate);
                        else
                            m_LevelController.RoomViewBuilder.HideAllUnvisitedCells(m_LevelController.Model.GetRoomDataByID(m_CreatedOtherRoomID),
                                                                                m_LevelController.Model.GetRoomDataByID(m_CreatedOtherRoomID).GridData.LeftNodeGate);
                    }
                    //Visit right node check
                    else if (cellData.IsEqualCoord(cellRoomData.GridData.RightNodeGate))
                    {
                        Debug.Log("Right node was visited");
                        //Add room 
                        m_LevelController.AddNextRoom(cellRoomData.NodeData.RightNode, true);

                        //Cache created room id
                        m_CreatedOtherRoomID = cellRoomData.NodeData.RightNode.ID;

                        //Hide all cells of created room exept parent gate cell
                        m_LevelController.RoomViewBuilder.HideAllUnvisitedCells(m_LevelController.Model.GetRoomDataByID(m_CreatedOtherRoomID),
                                                                            m_LevelController.Model.GetRoomDataByID(m_CreatedOtherRoomID).GridData.ParentNodeGate);
                    }
                    //Visit left node check
                    else if (cellData.IsEqualCoord(cellRoomData.GridData.LeftNodeGate))
                    {
                        Debug.Log("Left node was visited");
                        //Add room 
                        m_LevelController.AddNextRoom(cellRoomData.NodeData.LeftNode, false);

                        //Cache created room id
                        m_CreatedOtherRoomID = cellRoomData.NodeData.LeftNode.ID;

                        //Hide all cells of created room exept parent gate cell
                        m_LevelController.RoomViewBuilder.HideAllUnvisitedCells(m_LevelController.Model.GetRoomDataByID(m_CreatedOtherRoomID),
                                                                            m_LevelController.Model.GetRoomDataByID(m_CreatedOtherRoomID).GridData.ParentNodeGate);
                    }
                }
                //Click the cell in other room
                else
                {
                    Debug.Log("Transition to other room");

                    //Cache current room id
                    int currentRoomID = m_LevelController.Model.CurrentRoomID;

                    //Set transitioned room as current
                    m_LevelController.Model.SetRoomAsCurrent(m_CreatedOtherRoomID);

                    //Set created room as previous current (in case player continue to move the new room previous room will be removed on next move)
                    //If player moves back the previous room - swaps back ids to make entered room to be created and to make it remove if player move the previous room
                    m_CreatedOtherRoomID = currentRoomID;
                }

                //Cache last visited cell
                m_LastVisitedCell = cellData;

                //Extend view
                m_LevelController.RoomViewBuilder.ExtendView(m_LevelController.Model.GetCurrenRoomData(), m_LastVisitedCell);
            }
        }
    }
}
