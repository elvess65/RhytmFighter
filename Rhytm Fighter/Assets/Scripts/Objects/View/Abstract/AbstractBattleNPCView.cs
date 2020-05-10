using RhytmFighter.Animation;
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
        public event System.Action OnRotationFinished;
        public event System.Action OnActionAnimationEvent;

        public Transform ProjectileSpawnParent;
        public Transform ProjectileImpactParent;
        public Transform DefenceSpawnParent;
        public Transform DefenceImpactParent;
        public Transform DestroySpawnParent;

        protected iMovementStrategy m_MoveStrategy;
        protected AbstractBattleNPCModel m_ModelAsBattleModel;
        protected AbstractAnimationController m_AnimationController;
        protected System.Action m_OnInternalOtherAnimationEvent;

        public bool IsMoving => m_MoveStrategy.IsMoving;
        public Vector3 ProjectileImpactPosition => ProjectileImpactParent.position;
        public Vector3 ProjectileSpawnPosition => ProjectileSpawnParent.position;
        public Vector3 DefenceSpawnPosition => DefenceSpawnParent.position;
        

        public override void ShowView(AbstractGridObjectModel correspondingModel)
        {
            base.ShowView(correspondingModel);

            m_ModelAsBattleModel = CorrespondingModel as AbstractBattleNPCModel;
        }

        public virtual void Initialize(float moveSpeed)
        {
            //Movement
            m_MoveStrategy = new Bezier_MovementStrategy(transform, moveSpeed);
            m_MoveStrategy.OnMovementFinished += MovementFinishedHandler;
            m_MoveStrategy.OnCellVisited += CellVisitedHandler;
            m_MoveStrategy.OnRotationFinished += RotationFinishedHandler;

            //Animation
            m_AnimationController = GetComponent<AbstractAnimationController>();
            m_AnimationController.Initialize();

            AnimationEventsListener animationEventsListener = m_AnimationController.Controller.GetComponent<AnimationEventsListener>();
            animationEventsListener.OnActionEvent += ActionAnimationEventHandler;
            animationEventsListener.OnDestroyEvent += DestroyAnimationEventHandler;
            animationEventsListener.OnOtherEvent += OtherAnimationEventHandler;
        }


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

        public void NotifyView_BattlePrepare()
        {
            m_AnimationController.PlayAnimation(AnimationTypes.BattleIdle);
        }

        public void NotifyView_BattleFinished()
        {
            m_AnimationController.PlayAnimation(AnimationTypes.Idle);
        }

        public float GetActionEventExecuteTime(CommandTypes type)
        {
            return m_AnimationController.GetActionEventExecuteTime(ConvertersCollection.Command2Animation(type));
        }


        private void ActionAnimationEventHandler()
        {
            OnActionAnimationEvent?.Invoke();
        }

        private void DestroyAnimationEventHandler()
        {
            HideView();
        }

        private void OtherAnimationEventHandler()
        {
            m_OnInternalOtherAnimationEvent?.Invoke();
        }
        #endregion

        #region Movement
        public virtual void StartMove(Vector3[] path)
        {
            m_MoveStrategy.StartMove(path);
            m_AnimationController.PlayAnimation(AnimationTypes.StartMove);
        }

        public void StopMove()
        {
            m_MoveStrategy.StopMove();
        }

        public void StartRotate(Quaternion targetRotation, bool onlyAnimation)
        {
            if (!onlyAnimation)
                m_MoveStrategy.RotateTo(targetRotation);

            m_AnimationController.PlayAnimation(transform.eulerAngles.y > targetRotation.eulerAngles.y ? AnimationTypes.StrafeLeft : AnimationTypes.StrafeRight);
        }

        //iUpdatable
        public virtual void PerformUpdate(float deltaTime)
        {
            m_MoveStrategy.Update(deltaTime);
        }


        protected void FinishRotate()
        {
            m_AnimationController.PlayAnimation(AnimationTypes.BattleIdle);
        }


        protected void MovementFinishedHandler(int index)
        {
            m_AnimationController.PlayAnimation(AnimationTypes.StopMove);

            OnMovementFinished?.Invoke(index);
        }

        void CellVisitedHandler(int index)
        {
            OnCellVisited?.Invoke(index);
        }

        void RotationFinishedHandler()
        {
            FinishRotate();

            OnRotationFinished?.Invoke();
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
