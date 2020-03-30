using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using RhytmFighter.Battle;
using RhytmFighter.Battle.Action;
using RhytmFighter.Battle.Command;
using RhytmFighter.Battle.Health;
using RhytmFighter.Interfaces;
using System;
using UnityEngine;

namespace RhytmFighter.Objects.Model
{
    public abstract class AbstractBattleNPCModel : AbstractNPCModel, iBattleObject, iMovable
    {
        public event Action OnMovementFinished;
        public event Action<int> OnCellVisited;
        public event Action<iBattleObject> OnDestroyed;

        public bool IsMoving => m_ViewAsMovable.IsMoving;
        public bool IsEnemy { get; protected set; }
        public Vector3 ViewPosition => View.transform.position;

        public iBattleObject Target
        {
            get { return ActionBehaviour.Target; }
            set { ActionBehaviour.Target = value; }
        }
        public iBattleActionBehaviour ActionBehaviour { get; private set; }
        public iHealthBehaviour HealthBehaviour { get; private set; }

        private iBattleModelViewProxy m_ViewAsBattle;
        private iMovable m_ViewAsMovable;
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
        }

        public override void ShowView(CellView cellView)
        {
            base.ShowView(cellView);

            //Bind view
            m_ViewAsBattle = View as iBattleModelViewProxy;
            m_ViewAsMovable = View as iMovable;
            Initialize(m_MoveSpeed);
        }

        public void ApplyCommand(BattleCommand command)
        {
            switch(command.Type)
            {
                case CommandTypes.SimpleAttack:

                    SimpleAttackCommand attackCommand = command as SimpleAttackCommand;
                    HealthBehaviour.ReduceHP(attackCommand.Damage);

                    break;
            }
        }


        #region ActionBehaviour
        private void ActionBehaviour_OnActionExecutedHandler(BattleCommand command)
        {
            CommandsController.AddCommand(command);

            m_ViewAsBattle.ExecuteAction();
        }
        #endregion

        #region HealthBehaviour
        private void HealthBehaviour_OnHPReduced(int dmg)
        {
            m_ViewAsBattle.TakeDamage();
        }

        private void HealthBehaviour_OnHPIncreased(int amount)
        {
            m_ViewAsBattle.IncreaseHP();
        }

        private void HealthBehaviour_OnDestroyed()
        {
            HideView();

            if (CorrespondingCell.HasObject && CorrespondingCell.GetObject().ID.Equals(ID))
                CorrespondingCell.RemoveObject();

            OnDestroyed?.Invoke(this);
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
