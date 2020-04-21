using RhytmFighter.Battle;
using RhytmFighter.Characters.Animation;
using RhytmFighter.Characters.Movement;
using RhytmFighter.Interfaces;
using RhytmFighter.Objects.Model;
using RhytmFighter.UI;
using UnityEngine;

namespace RhytmFighter.Objects.View
{
    public abstract class AbstractBattleNPCView : AbstractNPCView, iBattleModelViewProxy, iMovable, iUIOwner
    {
        public event System.Action<int> OnMovementFinished;
        public event System.Action<int> OnCellVisited;

        public Transform ProjectileSpawnParent;
        public Transform ProjectileHitParent;
        public Transform DefenceSpawnParent;

        private iMovementStrategy m_MoveStrategy;
        private iBattleNPCAnimationController m_AnimationController;

        protected AbstractBattleNPCModel m_ModelAsBattleModel;

        public bool IsMoving => m_MoveStrategy.IsMoving;
        public Vector3 ProjectileHitPosition => ProjectileHitParent.position;
        public Vector3 ProjectileSpawnPosition => ProjectileSpawnParent.position;
        public Vector3 DefenceSpawnPosition => DefenceSpawnParent.position;


        public override void Show(AbstractGridObjectModel correspondingModel)
        {
            base.Show(correspondingModel);

            m_ModelAsBattleModel = CorrespondingModel as AbstractBattleNPCModel;
        }

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
        public virtual void PerformUpdate(float deltaTime)
        {
            m_MoveStrategy.Update(deltaTime);
        }


        void MovementFinishedHandler(int index)
        {
            m_AnimationController.PlayIdleAnimation();

            OnMovementFinished?.Invoke(index);
        }

        void CellVisitedHandler(int index)
        {
            OnCellVisited?.Invoke(index);
        }
        #endregion

        #region iBattleModelViewProxy
        public virtual void NotifyView_ExecuteAction()
        {
            m_AnimationController.PlayAttackAnimation();
        }

        public virtual void NotifyView_TakeDamage(int dmg)
        {
            m_AnimationController.PlayTakeDamageAnimation();
            UpdateHealthBar();
        }

        public virtual void NotifyView_IncreaseHP(int amount)
        {
            UpdateHealthBar();
        }

        public virtual void NotifyView_Destroyed()
        {
            m_AnimationController.PlayDestroyAnimation();

            HideUI();
        }
        #endregion

        #region iUIOwner
        public void CreateUI()
        {
            CreateHealthBar();
            UpdateHealthBar();
        }

        public void HideUI()
        {
            DestroyHealthBar();
        }


        protected abstract void CreateHealthBar();

        protected abstract void UpdateHealthBar();

        protected abstract void DestroyHealthBar();
        #endregion
    }
}
