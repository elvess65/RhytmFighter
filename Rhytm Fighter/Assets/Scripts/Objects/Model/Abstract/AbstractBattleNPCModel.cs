using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using RhytmFighter.Assets;
using RhytmFighter.Battle.Action;
using RhytmFighter.Battle.Command;
using RhytmFighter.Battle.Command.Model;
using RhytmFighter.Battle.Command.Model.Modificator;
using RhytmFighter.Battle.Health;
using RhytmFighter.Characters.Movement;
using RhytmFighter.Persistant.Enums;
using RhytmFighter.Enviroment.Effects;
using RhytmFighter.Objects.View;
using System;
using System.Collections.Generic;
using UnityEngine;
using RhytmFighter.Battle.Core.Abstract;
using RhytmFighter.Battle.Core;
using RhytmFighter.Battle.AI.Abstract;

namespace RhytmFighter.Objects.Model
{
    public abstract class AbstractBattleNPCModel : AbstractNPCModel, iBattleObject, iMovableModel
    {
        public event Action<int> OnMovementFinished;
        public event Action<int> OnCellVisited;
        public event Action OnRotationFinished;
        public event Action<iBattleObject> OnDestroyed;

        public bool IsMoving => m_BattleView.IsMoving;
        public bool IsDestroyed { get; private set; }
        public Transform ViewTransform => View.transform;
        public Vector3 ViewPosition => View.transform.position;
        public Vector3 ViewForwardDir => View.transform.forward;
        public Vector3 ProjectileImpactPosition => m_BattleView.ProjectileImpactPosition;
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
        public int ExperianceForDestroy { get; protected set; }

        private float m_MoveSpeed;
        private AbstractBattleNPCView m_BattleView;
        private AbstractCommandModel m_LastExecutedCommand;
        private Action m_InternalActionExecutedHandler;


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
            ActionBehaviour.OnActionExecuted += ActionExecutedHandler;

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

        public void Initialize(float moveSpeed)
        {
            m_BattleView.Initialize(moveSpeed);
            m_BattleView.OnCellVisited += CellVisitedHandler;
            m_BattleView.OnMovementFinished += MovementFinishedHandler;
            m_BattleView.OnRotationFinished += RotationFinishedHandler;
            m_BattleView.OnActionAnimationEvent += ActionAnimationEventHandler;
        }


        #region Battle
        public void ApplyCommand(AbstractCommandModel command)
        {
            List<iCommandModificator> commandTypesWhichModifiedApply = ModificatorsProcessor.ProcessApplyCommand(command);

            switch (command)
            {
                case AttackCommandModel attackCommand:

                    HealthBehaviour.ReduceHP(attackCommand.Damage);

                    //If NPC has defence modificator
                    iCommandModificator defenceModificator = GetModificatorOfType(commandTypesWhichModifiedApply, CommandTypes.Defence);
                    if (defenceModificator != null)
                    {
                        //Show defence effect
                        AssetsManager.GetPrefabAssets().InstantiatePrefab<AbstractVisualEffect>(AssetsManager.GetPrefabAssets().DefenceImpactEffectPrefab,
                                                                                                m_BattleView.DefenceImpactParent.position).ScheduleHideView();

                        //Play sound
                        BattleManager.Instance.DefenceSound.Play();
                    }

                    break;

                case DefenceCommandModel defenceCommand:
                    break;
            }
        }

        public void ReleaseCommand(AbstractCommandModel command)
        {
            ModificatorsProcessor.ProcessReleaseCommand(command);

            switch (command)
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

        public void NotifyViewAboutBattlePrepare()
        {
            m_BattleView.NotifyView_BattlePrepare();
        }

        public void NotifyViewAboutBattleFinish()
        {
            m_BattleView.NotifyView_BattleFinished();
        }

        public void NotifyViewAboutVictory()
        {
            m_BattleView.NotifyView_Victory();
        }

        public float GetActionEventExecuteTime(CommandTypes commandType)
        {
            return m_BattleView.GetActionEventExecuteTime(commandType);
        }



        private void ActionExecutedHandler(AbstractCommandModel command)
        {
            switch (command)
            {
                case AttackCommandModel attackCommand:
                    BattleManager.Instance.AttackSound.Play();
                    break;
                case DefenceCommandModel defenceCommand:
                    BattleManager.Instance.DefenceExecuteSound.Play();
                    break;
            }

            m_LastExecutedCommand = command;
            CommandsController.AddCommand(command);

            m_InternalActionExecutedHandler?.Invoke();
            m_InternalActionExecutedHandler = null;
        }

        private void ActionAnimationEventHandler()
        {
            if (m_LastExecutedCommand != null)
                CreateViewForLastExecutedCommand();
            else
                m_InternalActionExecutedHandler += CreateViewForLastExecutedCommand;
        }


        private void CreateViewForLastExecutedCommand()
        {
            CommandsController.CreateViewForCommand(m_LastExecutedCommand);
            m_LastExecutedCommand = null;
        }

        private iCommandModificator GetModificatorOfType(List<iCommandModificator> commandTypesWhichModifiedApply, CommandTypes targetType)
        {
            for (int i = 0; i < commandTypesWhichModifiedApply.Count; i++)
            {
                if (commandTypesWhichModifiedApply[i].CommandType.Equals(targetType))
                    return commandTypesWhichModifiedApply[i];
            }

            return null;
        }
        #endregion

        #region HealthBehaviour
        private void HealthBehaviour_OnHPReduced(int dmg)
        {
            BattleManager.Instance.HitSound.Play();

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
            BattleManager.Instance.DestroySound.Play();

            IsDestroyed = true;

            //Remove last executed command view
            if (m_LastExecutedCommand != null)
            {
                Battle.Command.View.AbstractCommandView view = CommandsController.TryGetCommandView(m_LastExecutedCommand.ID);
                if (view != null)
                    MonoBehaviour.Destroy(view.gameObject);
            }

            //Unscribe from battle animation events
            m_BattleView.OnActionAnimationEvent -= ActionAnimationEventHandler;

            //Notify view
            m_BattleView.NotifyView_Destroyed();

            //Remove object from cell
            if (CorrespondingCell.HasObject && CorrespondingCell.GetObject().ID.Equals(ID))
                CorrespondingCell.RemoveObject();

            //Event
            OnDestroyed?.Invoke(this);
        }
        #endregion

        #region Movement
        public void StartMove(Vector3[] path)
        {
            //Notify view
            m_BattleView.StartMove(path);
        }

        public void StopMove()
        {
            //Notify view
            m_BattleView.StopMove();
        }

        public void StartRotate(Quaternion targetRotation, bool onlyAnimation)
        {
            //Notify view
            m_BattleView.StartRotate(targetRotation, onlyAnimation);
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
