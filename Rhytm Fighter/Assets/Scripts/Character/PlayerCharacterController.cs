using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using RhytmFighter.Interfaces;
using RhytmFighter.Level;
using System.Collections.Generic;
using UnityEngine;

namespace RhytmFighter.Characters
{
    /// <summary>
    /// Controller for player character
    /// </summary>
    public class PlayerCharacterController : iUpdateable
    {
        public event System.Action<GridCellData> OnMovementFinished;
        public event System.Action<GridCellData> OnCellVisited;
        public event System.Action OnMovementInterrupted;

        public CharacterWrapper PlayerCharacter { get; private set; }

        private GridCellData[] m_PathCells;
        private CellView m_CurrentPlayerCell;
        private LevelController m_LevelController;


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
            //Find path of cells
            m_PathCells = m_LevelController.Model.GetCurrenRoomData().GridData.FindPathCells(m_CurrentPlayerCell.CorrespondingCellData, targetCellView.CorrespondingCellData);
            m_CurrentPlayerCell = targetCellView;

            //Cells positions
            List <Vector3> pathPos = new List<Vector3>();
            foreach (GridCellData pathCell in m_PathCells)
                pathPos.Add(m_LevelController.RoomViewBuilder.GetCellVisual(pathCell.CorrespondingRoomID, pathCell.X, pathCell.Y).transform.position);

            //Исправление ошибки с путем состоящим из двух точек
            if (pathPos.Count == 2)
                pathPos.Insert(1, (pathPos[0] + pathPos[1]) / 2);

            //Передвижение персонажа по пути
            PlayerCharacter.StartMove(pathPos.ToArray());
        }


        public void PerformUpdate(float deltaTime)
        {
            PlayerCharacter?.PerformUpdate(deltaTime);
        }


        private void MovementFinishedHandler() => OnMovementFinished?.Invoke(m_PathCells[m_PathCells.Length - 1]);

        private void CellVisitedHandler(int index) => OnCellVisited?.Invoke(m_PathCells[index]);

        private void MovementInterruptedHandler() => OnMovementInterrupted?.Invoke();
    }
}
