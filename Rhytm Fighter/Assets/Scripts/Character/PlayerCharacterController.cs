﻿using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using RhytmFighter.Characters.Movement;
using RhytmFighter.Core;
using RhytmFighter.Core.Enums;
using RhytmFighter.Level;
using RhytmFighter.Objects.Model;

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

        public void StopMove()
        {
            m_MovementController.StopMove();
        }

        public void PerformUpdate(float deltaTime)
        {
            m_MovementController?.PerformUpdate(deltaTime);
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
    }
}
