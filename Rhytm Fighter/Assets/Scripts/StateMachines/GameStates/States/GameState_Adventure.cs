using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using RhytmFighter.Characters;
using RhytmFighter.Core.Enums;
using RhytmFighter.Enviroment.Effects;
using RhytmFighter.Input;
using RhytmFighter.Level;
using RhytmFighter.Objects.Model;
using RhytmFighter.Rhytm;
using RhytmFighter.StateMachines.GameState;
using UnityEngine;

namespace RhytmFighter.GameState
{
    public class GameState_Adventure : GameState_Abstract
    {
        public System.Action<AbstractItemModel> OnPlayerInteractWithItem;
        public System.Action<AbstractNPCModel> OnPlayerInteractWithNPC;

        private GridInputProxy m_GridInputProxy;
        private LevelController m_LevelController;
        private GridPositionTrackingController m_GridPositionTrackingController;


        public GameState_Adventure(LevelController levelController, PlayerCharacterController playerCharacterController, RhytmInputProxy rhytmInputProxy) :
            base(playerCharacterController, rhytmInputProxy)
        {
            m_LevelController = levelController;

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

            if (m_RhytmInputProxy.IsInputAllowed() && m_RhytmInputProxy.IsInputTickValid())
                Debug.Log("Input is valid");

            base.HandleTouch(mouseScreenPos);
        }


        private void CellInputHandler(CellView cellView)
        {
            Assets.AssetsManager.GetPrefabAssets().InstantiatePrefab<AbstractVisualEffect>(Assets.AssetsManager.GetPrefabAssets().PointerPrefab, cellView.transform.position,
                                                                                           Assets.AssetsManager.GetPrefabAssets().PointerPrefab.transform.rotation).ScheduleHideView();

            //If transition to other - teleport 
            if (cellView.CorrespondingCellData.CorrespondingRoomID == m_LevelController.Model.GetCurrenRoomData().ID)
                m_PlayerCharacterController.MoveCharacter(cellView);
            else
                m_PlayerCharacterController.TeleportCharacter(cellView);
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

        private void PlayerInteractsWithObjectHandler(AbstractInteractableObjectModel interactableObject)
        {
            switch(interactableObject.Type)
            {
                case GridObjectTypes.Item:
                    OnPlayerInteractWithItem?.Invoke(interactableObject as AbstractItemModel);
                    break;

                case GridObjectTypes.NPC:
                    OnPlayerInteractWithNPC?.Invoke(interactableObject as AbstractNPCModel);
                    break;
            }
        }
    }
}
