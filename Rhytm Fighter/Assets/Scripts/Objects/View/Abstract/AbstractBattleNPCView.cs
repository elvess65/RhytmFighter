using RhytmFighter.Characters.Animation;
using RhytmFighter.Characters.Movement;
using RhytmFighter.Core.Converters;
using RhytmFighter.Core.Enums;
using RhytmFighter.Objects.Model;
using UnityEngine;

namespace RhytmFighter.Objects.View
{
    public abstract class AbstractBattleNPCView : AbstractNPCView, iMovable
    {
        public event System.Action<int> OnMovementFinished;
        public event System.Action<int> OnCellVisited;

        public Transform ProjectileSpawnParent;
        public Transform ProjectileHitParent;
        public Transform DefenceSpawnParent;

        private iMovementStrategy m_MoveStrategy;
        private AbstractAnimationController m_AnimationController;

        protected AbstractBattleNPCModel m_ModelAsBattleModel;
       
        public bool IsMoving => m_MoveStrategy.IsMoving;
        public float ActionEventExecutionTime => 0;
        public Vector3 ProjectileHitPosition => ProjectileHitParent.position;
        public Vector3 ProjectileSpawnPosition => ProjectileSpawnParent.position;
        public Vector3 DefenceSpawnPosition => DefenceSpawnParent.position;
        

        public override void Show(AbstractGridObjectModel correspondingModel)
        {
            base.Show(correspondingModel);

            m_ModelAsBattleModel = CorrespondingModel as AbstractBattleNPCModel;
        }

        public void Initialize(float moveSpeed)
        {
            //Movement
            m_MoveStrategy = new Bezier_MovementStrategy(transform, moveSpeed);
            m_MoveStrategy.OnMovementFinished += MovementFinishedHandler;
            m_MoveStrategy.OnCellVisited += CellVisitedHandler;

            //Animation
            m_AnimationController = GetComponent<AbstractAnimationController>();
            m_AnimationController.Initialize();
        }


        #region Movement
        public void NotifyView_StartMove(Vector3[] path)
        {
            m_MoveStrategy.StartMove(path);
            m_AnimationController.PlayAnimation(AnimationTypes.StartMove);
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
            m_AnimationController.PlayAnimation(AnimationTypes.StopMove);

            OnMovementFinished?.Invoke(index);
        }

        void CellVisitedHandler(int index)
        {
            OnCellVisited?.Invoke(index);
        }
        #endregion

        #region Battle
        public virtual void NotifyView_ExecuteCommand(CommandTypes type)
        {
            m_AnimationController.PlayAnimation(ConvertersCollection.Command2Animation(type));
        }

        public virtual void NotifyView_TakeDamage(int dmg)
        {
            m_AnimationController.PlayAnimation(AnimationTypes.TakeDamage);
            UpdateHealthBar();
        }

        public virtual void NotifyView_IncreaseHP(int amount)
        {
            m_AnimationController.PlayAnimation(AnimationTypes.IncreaseHP);
            UpdateHealthBar();
        }

        public virtual void NotifyView_Destroyed()
        {
            m_AnimationController.PlayAnimation(AnimationTypes.Destroy);
            HideUI();
        }

        public float GetActionEventExecuteTime(CommandTypes type)
        {
            return m_AnimationController.GetActionEventExecuteTime(ConvertersCollection.Command2Animation(type));
        }
        #endregion

        #region UI
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
