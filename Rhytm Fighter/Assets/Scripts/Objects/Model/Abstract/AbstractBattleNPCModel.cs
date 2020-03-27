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
        }

        public override void ShowView(CellView cellView)
        {
            base.ShowView(cellView);

            //Bind view
            m_ViewAsBattle = View as iBattleModelViewProxy;
        }

        public void ApplyCommand(BattleCommand command)
        {
            UnityEngine.Debug.Log("APPLY COMMAND: " + command);
        }


        private void ActionBehaviour_OnActionExecutedHandler(BattleCommand command)
        {
            CommandsController.AddCommand(command);

            m_ViewAsBattle.ExecuteAction();
        }
    }
}
