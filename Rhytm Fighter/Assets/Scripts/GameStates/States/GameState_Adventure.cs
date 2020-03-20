using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using RhytmFighter.Characters;
using RhytmFighter.Input;
using RhytmFighter.Level;
using RhytmFighter.Objects.Data;
using UnityEngine;

namespace RhytmFighter.GameState
{
    public class GameState_Adventure : GameState_Abstract
    {
        public System.Action<AbstractItemGridObject> OnPlayerInteractWithItem;
        public System.Action<AbstractNPCGridObject> OnPlayerInteractWithNPC;

        private GridInputProxy m_GridInputProxy;
        private GridPositionTrackingController m_GridPositionTrackingController;


        public GameState_Adventure(LevelController levelController, PlayerCharacterController playerCharacterController) : base(playerCharacterController)
        {
            m_GridInputProxy = new GridInputProxy();
            m_GridInputProxy.OnCellInput += CellInputHandler;

            m_GridPositionTrackingController = new GridPositionTrackingController(levelController);

            m_PlayerCharacterController.OnPlayerInteractsWithObject += PlayerInteractsWithObjectHandler;
        }


        public override void EnterState()
        {
            m_PlayerCharacterController.OnCellVisited += CellVisitedHandler;
            m_PlayerCharacterController.OnMovementFinished += MovementFinishedHandler;
        }

        public override void ExitState()
        {
            m_PlayerCharacterController.OnCellVisited -= CellVisitedHandler;
            m_PlayerCharacterController.OnMovementFinished -= MovementFinishedHandler;
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

        private void PlayerInteractsWithObjectHandler(AbstractInteractableGridObject interactableObject)
        {
            switch(interactableObject.Type)
            {
                case GridObjectTypes.Item:
                    OnPlayerInteractWithItem?.Invoke(interactableObject as AbstractItemGridObject);
                    break;

                case GridObjectTypes.NPC:
                    OnPlayerInteractWithNPC?.Invoke(interactableObject as AbstractNPCGridObject);
                    break;
            }
        }
    }
}
