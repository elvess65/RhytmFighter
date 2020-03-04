using Frameworks.Grid.Data;
using RhytmFighter.Input;
using RhytmFighter.Level;
using UnityEngine;

public class GameManagerSimulation : MonoBehaviour
{
    public GameObject Player;

    private LevelController m_LvlController;
    private InputController m_InputController;
    private GridInputProxy m_GridInputProxy;
    private GridCellData m_PlayerPrevCell;

    private int m_CreatedRoomID = -1;

    void Start()
    {
        m_LvlController = new LevelController();
        m_LvlController.GenerateLevel(4, 10, false, true);

        m_GridInputProxy = new GridInputProxy();
        m_GridInputProxy.OnCellInput += CellTouchHandler;

        m_InputController = new InputController();
        m_InputController.OnTouch += TouchHandler;
        m_InputController.OnTouch += m_GridInputProxy.TryGetCellFromInput;

        UpdatePlayerPosition(m_LvlController.RoomViewBuilder.GetCellVisual(m_LvlController.Model.GetCurrenRoomData().NodeData.ID, 0, 0));
    }

    private void CellTouchHandler(Frameworks.Grid.View.CellView cellView)
    {
        UpdatePlayerPosition(cellView);
    }

    void UpdatePlayerPosition(Frameworks.Grid.View.CellView cellView)
    {
        //Get cell
        GridCellData cellData = cellView.CorrespondingCellData;

        //Cell was changed
        if (!cellData.IsEqualCoord(m_PlayerPrevCell))
        {
            //Click the cell in the same room
            if (cellData.CorrespondingRoomID.Equals(m_LvlController.Model.CurrentRoomID))
            {
                //If transition room was created
                if (m_CreatedRoomID >= 0)
                {
                    m_LvlController.RemoveRoom(m_CreatedRoomID);
                    m_CreatedRoomID = -1;
                }

                //Visit parent node
                if (cellData.IsEqualCoord(m_LvlController.Model.GetRoomDataByID(cellData.CorrespondingRoomID).GridData.ParentNodeGate))
                {
                    Debug.LogError("Visit parent gate");
                    m_LvlController.AddParentRoom(m_LvlController.Model.GetCurrenRoomData().NodeData.ParentNode);
                    m_CreatedRoomID = m_LvlController.Model.GetCurrenRoomData().NodeData.ParentNode.ID;
                }
                //Visit right node
                else if (cellData.IsEqualCoord(m_LvlController.Model.GetRoomDataByID(cellData.CorrespondingRoomID).GridData.RightNodeGate))
                {
                    Debug.LogError("Visit right gate");
                    m_LvlController.AddNextRoom(m_LvlController.Model.GetCurrenRoomData().NodeData.RightNode, true);
                    m_CreatedRoomID = m_LvlController.Model.GetCurrenRoomData().NodeData.RightNode.ID;
                }
                //Visit left node
                else if (cellData.IsEqualCoord(m_LvlController.Model.GetRoomDataByID(cellData.CorrespondingRoomID).GridData.LeftNodeGate))
                {
                    Debug.LogError("Visit left gate");
                    m_LvlController.AddNextRoom(m_LvlController.Model.GetCurrenRoomData().NodeData.LeftNode, false);
                    m_CreatedRoomID = m_LvlController.Model.GetCurrenRoomData().NodeData.LeftNode.ID;
                }
            } 
            //Move to other room
            else
            {
                Debug.LogError("Transition to other room");

                //Save current room id
                int currentRoomID = m_LvlController.Model.CurrentRoomID;

                //Set entered room as current
                m_LvlController.Model.SetRoomAsCurrent(m_CreatedRoomID);

                //Set created room as previous current (in case player continue to move on new room it will remove created)
                //If player moves back the previous room - swaps back ids to make entered room to be current
                m_CreatedRoomID = currentRoomID;
            }
        }

        //Cache prev cell
        m_PlayerPrevCell = cellData;

        //Move player
        Player.transform.position = cellView.transform.position;
    }


    private void TouchHandler(Vector3 mousePos)
    {
        
    }

    private void Update()
    {
        if (m_InputController != null)
            m_InputController.Update(Time.deltaTime);
    }
}
