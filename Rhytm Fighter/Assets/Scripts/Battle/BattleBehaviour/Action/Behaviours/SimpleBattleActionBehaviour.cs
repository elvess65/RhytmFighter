using RhytmFighter.Battle.Command.Model;

namespace RhytmFighter.Battle.Action.Behaviours
{
    public class SimpleBattleActionBehaviour : iBattleActionBehaviour
    {
        public event System.Action<AbstractCommandModel> OnActionExecuted;

        public iBattleObject Target { get; set; }

        protected iBattleObject m_ControlledObject;
        protected int m_ApplyDelay;
        protected int m_UseDelay;
        protected int m_Damage;

        

        public SimpleBattleActionBehaviour(int applyDelay, int useDelay, int damage)
        {
            m_ApplyDelay = applyDelay;
            m_UseDelay = useDelay;
            m_Damage = damage;
        }

        public void SetControlledObject(iBattleObject controlledObject) => m_ControlledObject = controlledObject;

        public void SetTarget(iBattleObject target) => Target = target;


        public virtual void ExecuteAction(int currentTick)
        {
            //ExecuteCommand(new SimpleAttackCommand(m_ControlledObject, Target, m_ApplyDelay, m_UseDelay, m_Damage));
            ExecuteCommand(new DefenceCommandModel(m_ControlledObject));
        }


        protected void ExecuteCommand(AbstractCommandModel command) => OnActionExecuted?.Invoke(command); 
    }
}
