using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using RhytmFighter.Objects.View;
using UnityEngine;

namespace RhytmFighter.Objects.Data
{
    public class ExampleEnemyNPCGridObject : AbstractNPCGridObject
    {
        public ExampleEnemyNPCGridObject(int id, GridCellData correspondingCell) : base(id, correspondingCell, true)
        {
        }

        public override void RemoveView()
        {
            base.RemoveView();
            Debug.Log("REMOVE VIEW FOR ENEMY " + ID + " in CELL " + CorrespondingCell);
        }

        protected override AbstractGridObjectView CreateView(CellView cellView)
        {
            Debug.Log("CREATE VIEW FOR ENEMY " + ID + " in CELL " + CorrespondingCell);

            GameObject view = GameObject.CreatePrimitive(PrimitiveType.Cube);
            view.GetComponent<Collider>().enabled = false;
            view.transform.localScale = Vector3.one * 0.2f;
            view.transform.position = cellView.transform.position + new Vector3(0, 0.2f, 0);

            return view.AddComponent<ExampleNPCView>();
        }
    }
}
