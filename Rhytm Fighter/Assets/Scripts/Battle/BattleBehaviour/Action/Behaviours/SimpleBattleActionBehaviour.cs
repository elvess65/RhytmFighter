using RhytmFighter.Battle.Command;

namespace RhytmFighter.Battle.Action.Behaviours
{
    public class SimpleBattleActionBehaviour : iBattleActionBehaviour
    {
        public event System.Action<BattleCommand> OnActionExecuted;

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


        public virtual void ExecuteAction()
        {
            ExecuteCommand(new SimpleAttackCommand(m_ControlledObject, Target, m_ApplyDelay, m_UseDelay, m_Damage));
        }


        protected void ExecuteCommand(BattleCommand command) => OnActionExecuted?.Invoke(command); 
    }
}
