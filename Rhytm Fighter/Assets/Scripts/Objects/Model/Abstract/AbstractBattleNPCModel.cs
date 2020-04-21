using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using RhytmFighter.Battle;
using RhytmFighter.Battle.Action;
using RhytmFighter.Battle.AI;
using RhytmFighter.Battle.Command;
using RhytmFighter.Battle.Command.Model;
using RhytmFighter.Battle.Health;
using RhytmFighter.Interfaces;
using RhytmFighter.UI;
using System;
using UnityEngine;

namespace RhytmFighter.Objects.Model
{
    public abstract class AbstractBattleNPCModel : AbstractNPCModel, iBattleObject, iMovableModel
    {
        public event Action<int> OnMovementFinished;
        public event Action<int> OnCellVisited;
        public event Action<iBattleObject> OnDestroyed;

        public bool IsMoving => m_ViewAsMovable.IsMoving;
        public bool IsEnemy { get; protected set; }
        public Vector3 ViewPosition => View.transform.position;
        public Vector3 ProjectileHitPosition => m_ViewAsBattle.ProjectileHitPosition;
        public Vector3 ProjectileSpawnPosition => m_ViewAsBattle.ProjectileSpawnPosition;
        public Vector3 DefenceSpawnPosition => m_ViewAsBattle.DefenceSpawnPosition;

        public iBattleObject Target
        {
            get { return ActionBehaviour.Target; }
            set { ActionBehaviour.Target = value; }
        }
        public iBattleActionBehaviour ActionBehaviour { get; private set; }
        public iHealthBehaviour HealthBehaviour { get; private set; }
        public BattleCommandsModificatorProcessor ModificatorsProcessor { get; private set; }
        public AbstractAI AI { get; protected set; }

        private iBattleModelViewProxy m_ViewAsBattle;
        private iMovable m_ViewAsMovable;
        private iUIOwner m_ViewAsUIOwner;
        private float m_MoveSpeed;


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
            m_ViewAsBattle = View as iBattleModelViewProxy;
            m_ViewAsMovable = View as iMovable;
            m_ViewAsUIOwner = View as iUIOwner;
            m_ViewAsUIOwner.CreateUI();

            InitializeMovement(m_MoveSpeed);
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
                        Main.GameManager.Instance.DefenceSound.Play();
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


        #region ActionBehaviour
        private void ActionBehaviour_OnActionExecutedHandler(AbstractCommandModel command)
        {
            CommandsController.AddCommand(command);

            if (command.Type == CommandTypes.Attack)
                Main.GameManager.Instance.AttackSound.Play();

            m_ViewAsBattle.NotifyView_ExecuteAction();
        }
        #endregion

        #region HealthBehaviour
        private void HealthBehaviour_OnHPReduced(int dmg)
        {
            m_ViewAsBattle.NotifyView_TakeDamage(dmg);
            Main.GameManager.Instance.HitSound.Play();
        }

        private void HealthBehaviour_OnHPIncreased(int amount)
        {
            m_ViewAsBattle.NotifyView_IncreaseHP(amount);
        }

        private void HealthBehaviour_OnDestroyed()
        {
            Main.GameManager.Instance.DestroySound.Play();

            //Notify view
            m_ViewAsBattle.NotifyView_Destroyed();

            //Remove object from cell
            if (CorrespondingCell.HasObject && CorrespondingCell.GetObject().ID.Equals(ID))
                CorrespondingCell.RemoveObject();

            //Event
            OnDestroyed?.Invoke(this);
        }
        #endregion

        #region iMovableModel
        public void InitializeMovement(float moveSpeed)
        {
            m_ViewAsMovable.InitializeMovement(moveSpeed);
            m_ViewAsMovable.OnMovementFinished += MovementFinishedHandler;
            m_ViewAsMovable.OnCellVisited += CellVisitedHandler;
        }

        public void NotifyView_StartMove(Vector3[] path)
        {
            m_ViewAsMovable.NotifyView_StartMove(path);
        }

        public void NotifyView_StopMove()
        {
            m_ViewAsMovable.NotifyView_StopMove();
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
            m_ViewAsMovable.PerformUpdate(deltaTime);
        }


        private void MovementFinishedHandler(int index)
        {
            OnMovementFinished?.Invoke(index);
        }

        private void CellVisitedHandler(int index) => OnCellVisited?.Invoke(index);
        #endregion
    }
}
