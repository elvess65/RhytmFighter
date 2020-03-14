using System.Collections;
using System.Collections.Generic;
using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using RhytmFighter.Characters;
using RhytmFighter.Input;
using UnityEngine;

namespace RhytmFighter.GameState
{
    public class GameState_Adventure : GameState_Abstract
    {
        private GridInputProxy m_GridInputProxy;
        private GridPositionTrackingController m_GridPositionTrackingController;


        public GameState_Adventure(GridPositionTrackingController gridPositionTrackingController, PlayerCharacterController playerCharacterController) : base(playerCharacterController)
        {
            m_GridInputProxy = new GridInputProxy();
            m_GridInputProxy.OnCellInput += CellInputHandler;

            m_GridPositionTrackingController = gridPositionTrackingController;
        }


        public override void EnterState()
        {
            Debug.Log("ENTER ADVENTURE STATE");

            m_PlayerCharacterController.OnCellVisited += CellVisitedHandler;
            m_PlayerCharacterController.OnMovementFinished += MovementFinishedHandler;
            m_PlayerCharacterController.OnMovementInterrupted += MovementInterruptedHandler;
        }

        public override void ExitState()
        {
            Debug.Log("EXIT ADVENTURE STATE");

            m_PlayerCharacterController.OnCellVisited -= CellVisitedHandler;
            m_PlayerCharacterController.OnMovementFinished -= MovementFinishedHandler;
            m_PlayerCharacterController.OnMovementInterrupted -= MovementInterruptedHandler;
        }

        public override void HandleTouch(Vector3 mouseScreenPos)
        {
            m_GridInputProxy.TryGetCellFromInput(mouseScreenPos);
        }


        private void CellInputHandler(CellView cellView)
        {
            m_PlayerCharacterController.MoveCharacter(cellView);
        }


        private void MovementFinishedHandler(GridCellData cellData)
        {
            //Refresh grid
            m_GridPositionTrackingController.Refresh(cellData);
        }

        private void CellVisitedHandler(GridCellData cellData)
        {
            //Refresh grid
            m_GridPositionTrackingController.Refresh(cellData);
        }

        private void MovementInterruptedHandler()
        {
            Debug.LogError("MovementInterruptedHandler");
        }
    }
}
