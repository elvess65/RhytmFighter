using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using RhytmFighter.Battle;
using RhytmFighter.Battle.Action;
using RhytmFighter.Battle.AI;
using RhytmFighter.Battle.Command;
using RhytmFighter.Battle.Command.Model;
using RhytmFighter.Battle.Health;
using RhytmFighter.Characters.Movement;
using RhytmFighter.Core;
using RhytmFighter.Core.Enums;
using RhytmFighter.Objects.View;
using System;
using UnityEngine;

namespace RhytmFighter.Objects.Model
{
    public abstract class AbstractBattleNPCModel : AbstractNPCModel, iBattleObject, iMovableModel
    {
        public event Action<int> OnMovementFinished;
        public event Action<int> OnCellVisited;
        public event Action OnRotationFinished;
        public event Action<iBattleObject> OnDestroyed;
        
        public bool IsMoving => m_BattleView.IsMoving;
        public float ActionExecutionTime => m_BattleView.ActionEventExecutionTime;
        public Vector3 ViewPosition => View.transform.position;
        public Vector3 ViewForwardDir => View.transform.forward;
        public Vector3 ProjectileHitPosition => m_BattleView.ProjectileHitPosition;
        public Vector3 ProjectileSpawnPosition => m_BattleView.ProjectileSpawnPosition;
        public Vector3 DefenceSpawnPosition => m_BattleView.DefenceSpawnPosition;


        public bool IsEnemy { get; protected set; }
        public AbstractAI AI { get; protected set; }
        public BattleCommandsModificatorProcessor ModificatorsProcessor { get; private set; }
        public iBattleActionBehaviour ActionBehaviour { get; private set; }
        public iHealthBehaviour HealthBehaviour { get; private set; }
        public iBattleObject Target
        {
            get { return ActionBehaviour.Target; }
            set { ActionBehaviour.Target = value; }
        }

        private float m_MoveSpeed;
        private AbstractBattleNPCView m_BattleView;


        public AbstractBattleNPCModel(int id, GridCellData correspondingCell, float moveSpeed,
                                      iBattleActionBehaviour actionBehaviour, 
                                      iHealthBehaviour healthBehaviour, bool isEnemy) 
                                      : base(id, correspondingCell)
        {
            IsEnemy = isEnemy;
            m_MoveSpeed = moveSpeed;

            //Battle behaviour
            ActionBehaviour = actionBehaviour;
            ActionBehaviour.SetControlledObject(this);
            ActionBehaviour.OnActionExecuted += ActionBehaviour_OnActionExecutedHandler;

            //Health behaviour
            HealthBehaviour = healthBehaviour;
            HealthBehaviour.OnHPReduced += HealthBehaviour_OnHPReduced;
            HealthBehaviour.OnHPIncreased += HealthBehaviour_OnHPIncreased;
            HealthBehaviour.OnDestroyed += HealthBehaviour_OnDestroyed;

            //Battle effects processor
            ModificatorsProcessor = new BattleCommandsModificatorProcessor();
        }

        public override void ShowView(CellView cellView)
        {
            base.ShowView(cellView);

            //Bind view
            m_BattleView = View as AbstractBattleNPCView;
            m_BattleView.CreateUI();

            Initialize(m_MoveSpeed);
        }

        public void ApplyCommand(AbstractCommandModel command)
        {
            ModificatorsProcessor.ProcessApplyCommand(command);

            switch (command)
            {
                case AttackCommandModel attackCommand:

                    if (attackCommand.Damage > 0)
                    {
                        HealthBehaviour.ReduceHP(attackCommand.Damage);
                    }
                    else
                    {
                        if (ModificatorsProcessor.HasModificator(CommandTypes.Defence))
                        {
                            Battle.Command.View.SimpleDefenceView dView = GameObject.FindObjectOfType<Battle.Command.View.SimpleDefenceView>();
                            if (dView != null)
                                dView.C(m_BattleView.DefenceBreachParent.position);
                        }

                        GameManager.Instance.DefenceSound.Play();
                    }

                    break;

                case DefenceCommandModel defenceCommand: 

                    break;
            }
        }

        public void ReleaseCommand(AbstractCommandModel command)
        {
            ModificatorsProcessor.ProcessReleaseCommand(command);

            switch(command)
            {
                case DefenceCommandModel defenceCommand:

                    break;
            }   
        }

        public void NotifyViewAboutCommand(CommandTypes commandType)
        {
            //Notify view
            m_BattleView.NotifyView_ExecuteCommand(commandType);
        }

        public float GetActionEventExecuteTime(CommandTypes commandType)
        {
            return m_BattleView.GetActionEventExecuteTime(commandType);
        }


        #region ActionBehaviour
        private void ActionBehaviour_OnActionExecutedHandler(AbstractCommandModel command)
        {
            if (command.Type == CommandTypes.Attack)
                GameManager.Instance.AttackSound.Play();
            else if (command.Type == CommandTypes.Defence)
                GameManager.Instance.DefenceExecuteSound.Play();

            CommandsController.AddCommand(command);
        }
        #endregion

        #region HealthBehaviour
        private void HealthBehaviour_OnHPReduced(int dmg)
        {
            GameManager.Instance.HitSound.Play();

            //Notify view
            m_BattleView.NotifyView_TakeDamage(dmg);
        }

        private void HealthBehaviour_OnHPIncreased(int amount)
        {
            //Notify view
            m_BattleView.NotifyView_IncreaseHP(amount);
        }

        private void HealthBehaviour_OnDestroyed()
        {
            GameManager.Instance.DestroySound.Play();

            //Notify view
            m_BattleView.NotifyView_Destroyed();

            //Remove object from cell
            if (CorrespondingCell.HasObject && CorrespondingCell.GetObject().ID.Equals(ID))
                CorrespondingCell.RemoveObject();

            //Event
            OnDestroyed?.Invoke(this);
        }
        #endregion

        #region iMovableModel
        public void Initialize(float moveSpeed)
        {
            m_BattleView.Initialize(moveSpeed);
            m_BattleView.OnCellVisited += CellVisitedHandler;
            m_BattleView.OnMovementFinished += MovementFinishedHandler;
            m_BattleView.OnRotationFinished += RotationFinishedHandler;
        }

        public void NotifyView_StartMove(Vector3[] path)
        {
            //Notify view
            m_BattleView.NotifyView_StartMove(path);
        }

        public void NotifyView_StopMove()
        {
            //Notify view
            m_BattleView.NotifyView_StopMove();
        }

        public void NotifyView_StartRotate(Quaternion targetRotation)
        {
            //Notify view
            m_BattleView.NotifyView_StartRotate(targetRotation);
        }

        public void MovementFinishedReverseCallback(GridCellData cellData)
        {
            if (CorrespondingCell.HasObject && CorrespondingCell.GetObject().ID == ID)
            {
                CorrespondingCell.RemoveObject();
                CorrespondingCell = cellData;
                CorrespondingCell.AddObject(this);
            }
        }

        //iUpdatable
        public void PerformUpdate(float deltaTime)
        {
            m_BattleView.PerformUpdate(deltaTime);
        }


        private void MovementFinishedHandler(int index)
        {
            OnMovementFinished?.Invoke(index);
        }

        private void CellVisitedHandler(int index)
        {
            OnCellVisited?.Invoke(index);
        }

        private void RotationFinishedHandler()
        {
            OnRotationFinished?.Invoke();
        }
        #endregion
    }
}
