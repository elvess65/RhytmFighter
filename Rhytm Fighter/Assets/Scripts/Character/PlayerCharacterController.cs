using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using RhytmFighter.Interfaces;
using RhytmFighter.Level;
using RhytmFighter.Objects;
using System.Collections.Generic;
using UnityEngine;

namespace RhytmFighter.Characters
{
    /// <summary>
    /// Controller for player character
    /// </summary>
    public class PlayerCharacterController : iUpdatable
    {
        public event System.Action<GridCellData> OnMovementFinished;
        public event System.Action<GridCellData> OnCellVisited;
        public event System.Action OnMovementInterrupted;
        public event System.Action<iGridObject> OnPlayerInteractsWithObject;

        private event System.Action m_OnMovementFinishedInternal;

        public CharacterWrapper PlayerCharacter { get; private set; }

        private GridCellData[] m_PathCells;
        private CellView m_CurrentPlayerCell;
        private LevelController m_LevelController;

        private const float m_CLOSEST_WALKABLE_CELL_RANGE = 1.5f;


        public void CreateCharacter(CharacterWrapper characterWrapper, float moveSpeed, CellView startCellView, LevelController levelController)
        {
            m_LevelController = levelController;

            //Init start cell
            startCellView.CorrespondingCellData.IsVisited = true;
            m_CurrentPlayerCell = startCellView;

            //Place player on start cell
            PlayerCharacter = characterWrapper;
            PlayerCharacter.Initialize(startCellView.transform.position, moveSpeed);
            PlayerCharacter.OnMovementFinished += MovementFinishedHandler;
            PlayerCharacter.OnCellVisited += CellVisitedHandler;
            PlayerCharacter.OnMovementInterrupted += MovementInterruptedHandler;

            //Hide all cells except start cell
            m_LevelController.RoomViewBuilder.HideAllUnvisitedCells(m_LevelController.Model.GetCurrenRoomData());

            //Extend view
            m_LevelController.RoomViewBuilder.ExtendView(m_LevelController.Model.GetCurrenRoomData(), startCellView.CorrespondingCellData);
        }

        public void MoveCharacter(CellView targetCellView)
        {
            //Clear internal event if exists
            if (m_OnMovementFinishedInternal != null)
                m_OnMovementFinishedInternal = null;

            GridCellData targetCellData = targetCellView.CorrespondingCellData;


            //If cell has object - move to the closest cell
            if (targetCellData.HasObject)
            {
                GridCellData objectToInteractCellData = targetCellView.CorrespondingCellData;
                iGridObject objectToInteract = objectToInteractCellData.GetObject();
                

                //Get closest walkable cell to cell with object
                targetCellData = m_LevelController.Model.GetCurrenRoomData().GridData.GetClosestWalkableCell(m_CurrentPlayerCell.CorrespondingCellData, targetCellView.CorrespondingCellData, m_CLOSEST_WALKABLE_CELL_RANGE);

                //If cant find closest cell - supposedly cells are neighbours
                if (targetCellData == null)
                {
                    //If distance between cells less than 1.5 (horizontal/vertical = 1, diagonal = 1.4) - cell are neighbours 
                    if (m_LevelController.Model.GetCurrenRoomData().GridData.GetDistanceBetweenCells(m_CurrentPlayerCell.CorrespondingCellData, targetCellView.CorrespondingCellData) < m_CLOSEST_WALKABLE_CELL_RANGE)
                    {
                        MovementFinishedHandler();
                        PlayerInteractsWithObjectHandler(objectToInteract);
                    }
                    else
                        Debug.LogError("ERROR: Can not interact with object");

                    return;
                }

                //If moves to the cell with object inside - subscribe for interaction with object on arrival
                m_OnMovementFinishedInternal += () => PlayerInteractsWithObjectHandler(objectToInteract);

                //Set closest cell view as target cell view
                targetCellView = m_LevelController.RoomViewBuilder.GetCellVisual(targetCellData.CorrespondingRoomID, targetCellData.X, targetCellData.Y);
            }


            //Find path of cells
            m_PathCells = m_LevelController.Model.GetCurrenRoomData().GridData.FindPathCells(m_CurrentPlayerCell.CorrespondingCellData, targetCellData);
            m_CurrentPlayerCell = targetCellView;

            //Convert gridCellData to positions
            List <Vector3> pathPos = new List<Vector3>();
            foreach (GridCellData pathCell in m_PathCells)
                pathPos.Add(m_LevelController.RoomViewBuilder.GetCellVisual(pathCell.CorrespondingRoomID, pathCell.X, pathCell.Y).transform.position);

            //Fix error with 2 points path
            if (pathPos.Count == 2)
                pathPos.Insert(1, (pathPos[0] + pathPos[1]) / 2);

            //Start move character
            PlayerCharacter.StartMove(pathPos.ToArray());
        }


        public void PerformUpdate(float deltaTime)
        {
            PlayerCharacter?.PerformUpdate(deltaTime);
        }


        private void MovementFinishedHandler()
        {
            OnMovementFinished?.Invoke(m_PathCells[m_PathCells.Length - 1]);

            m_OnMovementFinishedInternal?.Invoke();
        }

        private void CellVisitedHandler(int index) => OnCellVisited?.Invoke(m_PathCells[index]);

        private void MovementInterruptedHandler() => OnMovementInterrupted?.Invoke();

        private void PlayerInteractsWithObjectHandler(GridCellData cellData, iGridObject gridObject) => OnPlayerInteractsWithObject?.Invoke(gridObject);
    }
}
