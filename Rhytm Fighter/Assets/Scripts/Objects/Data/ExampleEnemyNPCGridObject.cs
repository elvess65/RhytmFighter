using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using RhytmFighter.Battle;
using RhytmFighter.Objects.View;
using UnityEngine;

namespace RhytmFighter.Objects.Data
{
    public class ExampleEnemyNPCGridObject : AbstractBattleNPCGridObject
    {
        public ExampleEnemyNPCGridObject(int id, GridCellData correspondingCell, iBattleBehaviour battleBehaviour) : base(id, correspondingCell, battleBehaviour, true)
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
