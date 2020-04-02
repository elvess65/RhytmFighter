using RhytmFighter.Battle;
using RhytmFighter.Characters.Animation;
using RhytmFighter.Characters.Movement;
using RhytmFighter.Interfaces;
using RhytmFighter.UI;
using UnityEngine;

namespace RhytmFighter.Objects.View
{
    public abstract class AbstractBattleNPCView : AbstractNPCView, iBattleModelViewProxy, iMovable, iUIOwner
    {
        public event System.Action<int> OnMovementFinished;
        public event System.Action<int> OnCellVisited;

        private iMovementStrategy m_MoveStrategy;
        private iBattleNPCAnimationController m_AnimationController;

        public bool IsMoving => m_MoveStrategy.IsMoving;


        #region iMovable
        public void InitializeMovement(float moveSpeed)
        {
            //Movement
            m_MoveStrategy = new Bezier_MovementStrategy(transform, moveSpeed);
            m_MoveStrategy.OnMovementFinished += MovementFinishedHandler;
            m_MoveStrategy.OnCellVisited += CellVisitedHandler;

            //Animation
            m_AnimationController = GetComponent<iBattleNPCAnimationController>();
        }

        public void NotifyView_StartMove(Vector3[] path)
        {
            m_MoveStrategy.StartMove(path);
            m_AnimationController.PlayMoveAnimation();
        }

        public void NotifyView_StopMove()
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
        public virtual void NotifyView_ExecuteAction()
        {
            m_AnimationController.PlayAttackAnimation();
        }

        public virtual void NotifyView_TakeDamage()
        {
            m_AnimationController.PlayTakeDamageAnimation();
        }

        public void NotifyView_IncreaseHP()
        {

        }

        public virtual void NotifyView_Destroyed()
        {
            m_AnimationController.PlayDestroyAnimation();
        }
        #endregion

        #region iUIOwner
        public void CreateUI()
        {
            CreateHealthBar();
        }

        public void HideUI()
        {
            DestroyHealthBar();
        }


        protected abstract void CreateHealthBar();

        protected abstract void DestroyHealthBar();
        #endregion
    }
}
