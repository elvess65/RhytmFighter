﻿using RhytmFighter.Animation;
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
        public event System.Action OnAnimationEvent;

        public Transform ProjectileSpawnParent;
        public Transform ProjectileHitParent;
        public Transform DefenceSpawnParent;
        public Transform DefenceBreachParent;

        private iMovementStrategy m_MoveStrategy;
        private AbstractAnimationController m_AnimationController;

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

        public void Initialize(float moveSpeed)
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
            animationEventsListener.OnEvent += AnimationEventHandler;
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
            OnAnimationEvent += DisableGraphics;
            HideUI();
        }

        public float GetActionEventExecuteTime(CommandTypes type)
        {
            return m_AnimationController.GetActionEventExecuteTime(ConvertersCollection.Command2Animation(type));
        }

        public void NotifyView_BattlePrepare()
        {
            m_AnimationController.PlayAnimation(AnimationTypes.BattleIdle);
        }

        public void NotifyView_BattleFinished()
        {
            m_AnimationController.PlayAnimation(AnimationTypes.Idle);
        }


        private void AnimationEventHandler()
        {
            OnAnimationEvent?.Invoke();
        }

        private void DisableGraphics()
        {
            m_AnimationController.gameObject.SetActive(false);
        }
        #endregion

        #region Movement
        public void NotifyView_StartMove(Vector3[] path)
        {
            m_MoveStrategy.StartMove(path);
            m_AnimationController.PlayAnimation(AnimationTypes.StartMove);
        }

        public void NotifyView_Teleport(Vector3 pos)
        {
            transform.rotation = Quaternion.LookRotation(pos - transform.position);
            m_AnimationController.PlayAnimation(AnimationTypes.Teleport);

            m_MoveStrategy.StartTeleport(pos);

            GameObject teleportEffect = Assets.AssetsManager.GetPrefabAssets().InstantiateGameObject(Assets.AssetsManager.GetPrefabAssets().TeleportEffectPrefab,
                                            DefenceSpawnParent.position, transform.rotation * Quaternion.Euler(0, 180, 0));
            Destroy(teleportEffect, 2);
        }

        public void NotifyView_StopMove()
        {
            m_MoveStrategy.StopMove();
        }

        public void NotifyView_StartRotate(Quaternion targetRotation, bool onlyAnimation)
        {
            if (!onlyAnimation)
                m_MoveStrategy.RotateTo(targetRotation);

            m_AnimationController.PlayAnimation(transform.eulerAngles.y > targetRotation.eulerAngles.y ? AnimationTypes.StrafeLeft : AnimationTypes.StrafeRight);
        }

        public void NotifyView_FinishRotate()
        {
            m_AnimationController.PlayAnimation(AnimationTypes.BattleIdle);
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

        void RotationFinishedHandler()
        {
            NotifyView_FinishRotate();

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
