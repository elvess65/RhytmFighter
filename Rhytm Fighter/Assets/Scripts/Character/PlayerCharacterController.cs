using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using RhytmFighter.Characters.Movement;
using RhytmFighter.Core;
using RhytmFighter.Core.Enums;
using RhytmFighter.Level;
using RhytmFighter.Objects.Model;
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
        public event System.Action<AbstractInteractableObjectModel> OnPlayerInteractsWithObject;

        public PlayerModel PlayerModel { get; private set; }

        private ModelMovementController m_MovementController;
        private iBattleObject m_FocusingTarget = null;

        private const float m_FOCUSING_SPEED = 10;

       
        public void CreateCharacter(PlayerModel playerModel, CellView startCellView, LevelController levelController)
        {
            //Cache player model
            PlayerModel = playerModel;

            //Initialize movement controller
            m_MovementController = new ModelMovementController(levelController);
            m_MovementController.SetModel(playerModel);
            m_MovementController.OnMovementFinished += MovementFinishedHandler;
            m_MovementController.OnCellVisited += CellVisitedHandler;
            m_MovementController.OnInteractsWithObject += PlayerInteractsWithObjectHandler;

            //Init start cell
            startCellView.CorrespondingCellData.IsVisited = true;
            startCellView.CorrespondingCellData.AddObject(playerModel);

            //Show player view
            playerModel.ShowView(startCellView);

            //Hide all cells except start cell
            levelController.RoomViewBuilder.HideCells(levelController.Model.GetCurrenRoomData(), false, null, true);

            //Extend view
            levelController.RoomViewBuilder.ExtendView(levelController.Model.GetCurrenRoomData(), startCellView.CorrespondingCellData);
        }


        public void MoveCharacter(CellView targetCellView)
        {
            m_MovementController.MoveCharacter(targetCellView);
        }

        public void TeleportCharacter(CellView targetCellView)
        {
            PlayerModel.NotifyView_SwitchMoveStrategy(MovementStrategyTypes.Teleport);

            m_MovementController.OnMovementFinished += TeleportFinishedHandler;
            m_MovementController.TeleportCharacter(targetCellView);
            
        }

        public void StopMove()
        {
            m_MovementController.StopMove();
        }

        public void StartFocusing(iBattleObject target)
        {
            m_FocusingTarget = target;
        }

        public void StopFocusing()
        {
            PlayerModel.NotifyView_FinishFocusing();
            m_FocusingTarget = null;
        }

        public void ExecuteAction(CommandTypes type)
        {
            PlayerModel.ActionBehaviour.ExecuteAction(type);
            PlayerModel.NotifyViewAboutCommand(type);
        }

        public void PrepareForBattle()
        {
            PlayerModel.NotifyViewAboutBattlePrepare();
        }

        public void FinishBattle()
        {
            PlayerModel.NotifyViewAboutBattleFinish();
        }

        public void PerformUpdate(float deltaTime)
        {
            m_MovementController?.PerformUpdate(deltaTime);

            ProcessFocusing();
        }


        private void MovementFinishedHandler(GridCellData cellData)
        {
            OnMovementFinished?.Invoke(cellData);
        }

        private void CellVisitedHandler(GridCellData cellData)
        {
            OnCellVisited?.Invoke(cellData);
        }

        private void PlayerInteractsWithObjectHandler(AbstractInteractableObjectModel interactableObject)
        {
            OnPlayerInteractsWithObject?.Invoke(interactableObject);
        }


        private void TeleportFinishedHandler(GridCellData cellData)
        {
            m_MovementController.OnMovementFinished -= TeleportFinishedHandler;

            PlayerModel.NotifyView_SwitchMoveStrategy(MovementStrategyTypes.Bezier);
        }


        private void ProcessFocusing()
        {
            if (m_FocusingTarget == null)
                return;

            Quaternion targetRotation = Quaternion.LookRotation(m_FocusingTarget.ViewPosition - PlayerModel.ViewPosition);

            PlayerModel.View.transform.rotation = Quaternion.Slerp(PlayerModel.View.transform.rotation, targetRotation, Time.deltaTime * m_FOCUSING_SPEED);
            PlayerModel.NotifyView_StartRotate(targetRotation, true);
        }
    }
}
