using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using RhytmFighter.Level;
using RhytmFighter.Objects.Model;
using RhytmFighter.Persistant.Abstract;
using System.Collections.Generic;
using UnityEngine;

namespace RhytmFighter.Characters.Movement
{
    public class ModelMovementController : iUpdatable
    {
        public event System.Action<GridCellData> OnMovementFinished;
        public event System.Action<GridCellData> OnCellVisited;
        public event System.Action<AbstractInteractableObjectModel> OnInteractsWithObject;
        public event System.Action OnRotationFinished;

        private event System.Action m_OnMovementFinishedInternal;

        public iMovableModel Model { get; private set; }

        private LevelController m_LevelController;
        private GridCellData[] m_PathCells;

        private const float m_CLOSEST_WALKABLE_CELL_RANGE = 1.5f;


        public ModelMovementController(LevelController levelController)
        {
            m_LevelController = levelController;
        }


        public void SetModel(iMovableModel model)
        {
            //Clear event from previous model
            if (Model != null)
            {
                Model.OnCellVisited -= CellVisitedHandler;
                Model.OnMovementFinished -= MovementFinishedHandler;
                Model.OnRotationFinished -= RotationFinishedHandler;
            }

            //Initialize model
            Model = model;
            Model.OnCellVisited += CellVisitedHandler;
            Model.OnMovementFinished += MovementFinishedHandler;
            Model.OnRotationFinished += RotationFinishedHandler;
        }

        public void MoveCharacter(CellView targetCellView, bool ignoreHidedCells)
        {
            //Clear internal event if exists
            if (m_OnMovementFinishedInternal != null)
                m_OnMovementFinishedInternal = null;

            GridCellData targetCellData = targetCellView.CorrespondingCellData;

            //If cell has object - move to the closest cell
            if (targetCellData.HasObject)
            {
                AbstractInteractableObjectModel interactableGridObject = targetCellView.CorrespondingCellData.GetObject() as AbstractInteractableObjectModel;
                if (interactableGridObject == null)
                {
                    Debug.LogError("ERROR: Trying to interact with non interactable object");
                    return;
                }

                //Get closest walkable cell to cell with object
                targetCellData = m_LevelController.Model.GetCurrenRoomData().GridData.GetClosestWalkableCell(Model.CorrespondingCell, 
                                                                                                             targetCellView.CorrespondingCellData, 
                                                                                                             m_CLOSEST_WALKABLE_CELL_RANGE,
                                                                                                             ignoreHidedCells);

                //If cant find closest cell - supposedly cells are neighbours
                if (targetCellData == null)
                {
                    //If distance between cells less than 1.5 (horizontal/vertical = 1, diagonal = 1.4) - cell are neighbours 
                    if (m_LevelController.Model.GetCurrenRoomData().GridData.GetDistanceBetweenCells(Model.CorrespondingCell, targetCellView.CorrespondingCellData) < m_CLOSEST_WALKABLE_CELL_RANGE)
                    {
                        MovementFinishedHandler(m_PathCells.Length - 1);
                        PlayerInteractsWithObjectHandler(interactableGridObject);
                    }
                    else
                        Debug.LogError("ERROR: Can not interact with object");

                    return;
                }

                //If moves to the cell with object inside - subscribe for interaction with object on arrival
                m_OnMovementFinishedInternal += () => PlayerInteractsWithObjectHandler(interactableGridObject);

                //Set closest cell view as target cell view
                targetCellView = m_LevelController.RoomViewBuilder.GetCellVisual(targetCellData.CorrespondingRoomID, targetCellData.X, targetCellData.Y);
            }

            //Find path of cells
            m_PathCells = m_LevelController.Model.GetCurrenRoomData().GridData.FindPathCells(Model.CorrespondingCell, targetCellData, ignoreHidedCells);
            if (m_PathCells == null)
            {
                Debug.LogError($"Error: Cant find path to the cell {targetCellView.CorrespondingCellData}");
                return;
            }

            //Convert gridCellData to positions
            List<Vector3> pathPos = new List<Vector3>();
            foreach (GridCellData pathCell in m_PathCells)
                pathPos.Add(m_LevelController.RoomViewBuilder.GetCellVisual(pathCell.CorrespondingRoomID, pathCell.X, pathCell.Y).transform.position);

            //Fix error with 2 points path
            if (pathPos.Count == 2)
                pathPos.Insert(1, (pathPos[0] + pathPos[1]) / 2);

            //Start move character
            Model.StartMove(pathPos.ToArray());
        }

        public void StopMove()
        {
            Model.StopMove();
        }

        public void RotateCharacter(Quaternion targetRotation)
        {
            Model.StartRotate(targetRotation, false);
        }

        public void TeleportCharacter(CellView targetCellView)
        {
            m_PathCells = new GridCellData[] { targetCellView.CorrespondingCellData };

            //Start teleport character
            Model.StartMove(new Vector3[] { targetCellView.transform.position });
        }

        public void PerformUpdate(float deltaTime)
        {
            Model?.PerformUpdate(deltaTime);
        }


        private void MovementFinishedHandler(int index)
        {
            //Clamp index of cell which player finished movement
            if (index >= m_PathCells.Length)
                index = m_PathCells.Length - 1;

            //Update corresponding cell data
            Model.MovementFinishedReverseCallback(m_PathCells[index]);

            //Events
            OnMovementFinished?.Invoke(m_PathCells[index]);

            //Internal event (if model interacted with something)
            m_OnMovementFinishedInternal?.Invoke();
        }

        private void CellVisitedHandler(int index)
        {
            OnCellVisited?.Invoke(m_PathCells[index]);
        }

        private void RotationFinishedHandler()
        {
            OnRotationFinished?.Invoke();
        }

        private void PlayerInteractsWithObjectHandler(AbstractInteractableObjectModel interactableObject)
        {
            OnInteractsWithObject?.Invoke(interactableObject);
        }
    }
}
