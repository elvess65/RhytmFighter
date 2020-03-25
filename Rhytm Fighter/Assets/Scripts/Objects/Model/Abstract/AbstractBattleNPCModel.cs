using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using RhytmFighter.Battle;
using RhytmFighter.Battle.Action;
using RhytmFighter.Battle.Health;

namespace RhytmFighter.Objects.Model
{
    public abstract class AbstractBattleNPCModel : AbstractNPCModel, iBattleObject
    {
        public bool IsEnemy { get; protected set; }
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


        private void ActionBehaviour_OnActionExecutedHandler()
        {
            m_ViewAsBattle.ExecuteAction();
        }
    }
}
