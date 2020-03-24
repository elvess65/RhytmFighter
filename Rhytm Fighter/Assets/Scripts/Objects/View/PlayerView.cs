﻿using RhytmFighter.Characters.Movement;
using RhytmFighter.Interfaces;
using UnityEngine;

namespace RhytmFighter.Objects.View
{
    public class PlayerView : AbstractBattleNPCView, iUpdatable
    {
        public event System.Action OnMovementFinished;
        public event System.Action<int> OnCellVisited;

        private iMovementStrategy m_MoveStrategy;

        public int ID { get; private set; }
        public bool IsMoving => m_MoveStrategy.IsMoving;


        public void Initialize(Vector3 pos, float moveSpeed)
        {
            transform.position = pos;

            //Movement
            m_MoveStrategy = new Bezier_MovementStrategy(transform, moveSpeed);
            m_MoveStrategy.OnMovementFinished += MovementFinishedHandler;
            m_MoveStrategy.OnCellVisited += CellVisitedHandler;
        }


        public void StartMove(Vector3[] path) => m_MoveStrategy.StartMove(path);

        public void StopMove() => m_MoveStrategy.StopMove();


        public override void ExecuteAction()
        {
            Debug.Log("Player view execture action animation");
        }

        public override void TakeDamage()
        {
            Debug.Log("Player view execture take damage animation");
        }


        public void PerformUpdate(float deltaTime)
        {
            m_MoveStrategy.Update(deltaTime);
        }


        void MovementFinishedHandler() => OnMovementFinished?.Invoke();

        void CellVisitedHandler(int index) => OnCellVisited?.Invoke(index);
    }
}
