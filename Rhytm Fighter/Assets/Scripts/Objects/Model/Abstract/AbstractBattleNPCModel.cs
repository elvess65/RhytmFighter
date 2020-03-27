using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using RhytmFighter.Battle;
using RhytmFighter.Battle.Action;
using RhytmFighter.Battle.Command;
using RhytmFighter.Battle.Health;

namespace RhytmFighter.Objects.Model
{
    public abstract class AbstractBattleNPCModel : AbstractNPCModel, iBattleObject
    {
        public event System.Action<iBattleObject> OnDestroyed;

        public bool IsEnemy { get; protected set; }
        public UnityEngine.Vector3 ViewPosition => View.transform.position;

        public iBattleObject Target
        {
            get { return ActionBehaviour.Target; }
            set { ActionBehaviour.Target = value; }
        }
        public iBattleActionBehaviour ActionBehaviour { get; private set; }
        public iHealthBehaviour HealthBehaviour { get; private set; }

        private iBattleModelViewProxy m_ViewAsBattle;


        public AbstractBattleNPCModel(int id, 
                                      GridCellData correspondingCell, 
                                      iBattleActionBehaviour actionBehaviour, 
                                      iHealthBehaviour healthBehaviour, 
                                      bool isEnemy) : base(id, correspondingCell)
        {
            IsEnemy = isEnemy;

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


        private void ActionBehaviour_OnActionExecutedHandler(BattleCommand command)
        {
            CommandsController.AddCommand(command);

            m_ViewAsBattle.ExecuteAction();
        }


        private void HealthBehaviour_OnHPReduced(int dmg)
        {
            UnityEngine.Debug.Log("REDUCE HP BY" + dmg);
            m_ViewAsBattle.TakeDamage();
        }

        private void HealthBehaviour_OnHPIncreased(int amount)
        {
            UnityEngine.Debug.Log("INCREASE HP BY " + amount);
            m_ViewAsBattle.IncreaseHP();
        }

        private void HealthBehaviour_OnDestroyed()
        {
            UnityEngine.Debug.Log("DESTROY");
            
            m_ViewAsBattle.Destroy();
            OnDestroyed?.Invoke(this);
        }
    }
}
