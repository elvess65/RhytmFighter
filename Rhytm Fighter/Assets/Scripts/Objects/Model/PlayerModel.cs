using System;
using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using RhytmFighter.Assets;
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

        public bool IsMoving => m_ViewAsMovable.IsMoving;

        private iMovable m_ViewAsMovable;
        private float m_MoveSpeed;


        public PlayerModel(int id, GridCellData correspondingCell, float moveSpeed, iBattleActionBehaviour actionBehaviour, iHealthBehaviour healthBehaviour)
            : base(id, correspondingCell, actionBehaviour, healthBehaviour, false)
        {
            m_MoveSpeed = moveSpeed;
        }

        #region AbstractModel
        public override void ShowView(CellView cellView)
        {
            base.ShowView(cellView);

            //Bind view
            m_ViewAsMovable = View as iMovable;
            Initialize(m_MoveSpeed);
        }

        protected override AbstractGridObjectView CreateView(CellView cellView)
        {
            return AssetsManager.GetPrefabAssets().InstantiatePrefab(AssetsManager.GetPrefabAssets().PlayerViewPrefab, cellView.transform.position);
        }
        #endregion

        #region iMovable
        public void Initialize(float moveSpeed)
        {
            m_ViewAsMovable.Initialize(moveSpeed);
            m_ViewAsMovable.OnMovementFinished += MovementFinishedHandler;
            m_ViewAsMovable.OnCellVisited += CellVisitedHandler;
        }

        public void StartMove(Vector3[] path)
        {
            m_ViewAsMovable.StartMove(path);
        }

        public void StopMove()
        {
            m_ViewAsMovable.StopMove();
        }

        //iUpdatable
        public void PerformUpdate(float deltaTime)
        {
            m_ViewAsMovable.PerformUpdate(deltaTime);
        }


        private void MovementFinishedHandler() => OnMovementFinished?.Invoke();

        private void CellVisitedHandler(int index) => OnCellVisited?.Invoke(index);
        #endregion  
    }
}
