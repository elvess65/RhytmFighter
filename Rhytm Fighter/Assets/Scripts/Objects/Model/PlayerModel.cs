using System;
using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using RhytmFighter.Battle.Action;
using RhytmFighter.Battle.Health;
using RhytmFighter.Interfaces;
using RhytmFighter.Objects.View;
using UnityEngine;

namespace RhytmFighter.Objects.Model
{
    public class PlayerModel : AbstractBattleNPCModel, iMovable
    {
        public event Action OnMovementFinished;
        public event Action<int> OnCellVisited;

        public bool IsMoving => m_PlayerView.IsMoving;

        private PlayerView m_PlayerView;


        public PlayerModel(int id, CellView startCellView, float moveSpeed, iBattleActionBehaviour actionBehaviour, iHealthBehaviour healthBehaviour)
            : base(id, null, actionBehaviour, healthBehaviour, false)
        {
            ShowView(startCellView);

            m_PlayerView.Initialize(startCellView.transform.position, moveSpeed);
            m_PlayerView.OnMovementFinished += MovementFinishedHandler;
            m_PlayerView.OnCellVisited += CellVisitedHandler;
        }

        public override void ShowView(CellView cellView)
        {
            base.ShowView(cellView);

            //Bind view
            m_PlayerView = View as PlayerView;
        }


        public void StartMove(Vector3[] path)
        {
            m_PlayerView.StartMove(path);
        }

        public void StopMove()
        {
            m_PlayerView.StopMove();
        }

        public void PerformUpdate(float deltaTime)
        {
            m_PlayerView.PerformUpdate(deltaTime);
        }


        protected override AbstractGridObjectView CreateView(CellView cellView)
        {
            return GameObject.FindObjectOfType<PlayerView>();
        }


        private void MovementFinishedHandler() => OnMovementFinished?.Invoke();

        private void CellVisitedHandler(int index) => OnCellVisited?.Invoke(index);

 
    }
}
