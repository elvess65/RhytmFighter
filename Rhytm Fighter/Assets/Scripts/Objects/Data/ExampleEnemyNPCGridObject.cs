using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using RhytmFighter.Battle.Action;
using RhytmFighter.Battle.Health;
using RhytmFighter.Objects.View;
using UnityEngine;

namespace RhytmFighter.Objects.Data
{
    public class ExampleEnemyNPCGridObject : AbstractBattleNPCGridObject
    {
        public ExampleEnemyNPCGridObject(int id, GridCellData correspondingCell, iBattleActionBehaviour actionBehaviour, iHealthBehaviour healthBehaviour) : 
            base(id, correspondingCell, actionBehaviour, healthBehaviour, true)
        {
        }

        protected override AbstractGridObjectView CreateView(CellView cellView)
        {
            GameObject view = GameObject.CreatePrimitive(PrimitiveType.Cube);
            view.GetComponent<Collider>().enabled = false;
            view.transform.localScale = Vector3.one * 0.2f;
            view.transform.position = cellView.transform.position + new Vector3(0, 0.2f, 0);

            return view.AddComponent<ExampleNPCView>();
        }
    }
}
