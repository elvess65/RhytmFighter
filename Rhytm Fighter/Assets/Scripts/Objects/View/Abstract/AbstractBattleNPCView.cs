using RhytmFighter.Battle;
using RhytmFighter.Characters.Animation;
using RhytmFighter.Characters.Movement;
using RhytmFighter.Interfaces;
using UnityEngine;

namespace RhytmFighter.Objects.View
{
    public class AbstractBattleNPCView : AbstractNPCView, iBattleModelViewProxy, iMovable
    {
        public event System.Action<int> OnMovementFinished;
        public event System.Action<int> OnCellVisited;

        private iMovementStrategy m_MoveStrategy;
        private iBattleNPCAnimationController m_AnimationController;

        public bool IsMoving => m_MoveStrategy.IsMoving;


        #region iMovable
        public void Initialize(float moveSpeed)
        {
            //Movement
            m_MoveStrategy = new Bezier_MovementStrategy(transform, moveSpeed);
            m_MoveStrategy.OnMovementFinished += MovementFinishedHandler;
            m_MoveStrategy.OnCellVisited += CellVisitedHandler;

            //Animation
            m_AnimationController = GetComponent<iBattleNPCAnimationController>();
        }

        public void StartMove(Vector3[] path)
        {
            m_MoveStrategy.StartMove(path);
            m_AnimationController.PlayMoveAnimation();
        }

        public void StopMove()
        {
            m_MoveStrategy.StopMove();
        }

        //iUpdatable
        public void PerformUpdate(float deltaTime)
        {
            m_MoveStrategy.Update(deltaTime);
        }


        void MovementFinishedHandler(int index)
        {
            m_AnimationController.PlayIdleAnimation();

            OnMovementFinished?.Invoke(index);
        }

        void CellVisitedHandler(int index) => OnCellVisited?.Invoke(index);
        #endregion

        #region iBattleModelViewProxy
        public virtual void ExecuteAction()
        {
            m_AnimationController.PlayAttackAnimation();
        }

        public virtual void TakeDamage()
        {
            m_AnimationController.PlayTakeDamageAnimation();
        }

        public void IncreaseHP()
        {

        }

        public virtual void Destroy()
        {
            m_AnimationController.PlayDestroyAnimation();
        }
        #endregion
    }
}
