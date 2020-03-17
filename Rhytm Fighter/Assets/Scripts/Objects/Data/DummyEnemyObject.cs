using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using UnityEngine;

namespace RhytmFighter.Objects.Data
{
    public class DummyEnemyObject : AbstractGridObject
    {
        public DummyEnemyObject(int id, GridCellData correspondingCell) : base(id, correspondingCell)
        {
            Type = GridObjectTypes.Enemy;
        }

        public override void RemoveView()
        {
            base.RemoveView();
            Debug.Log("REMOVE VIEW FOR ENEMY " + ID + " in CELL " + CorrespondingCell);
        }

        protected override void CreateView(CellView cellView)
        {
            Debug.Log("CREATE VIEW FOR ENEMY " + ID + " in CELL " + CorrespondingCell);

            m_View = GameObject.CreatePrimitive(PrimitiveType.Cube);
            m_View.GetComponent<Collider>().enabled = false;
            m_View.transform.localScale = Vector3.one * 0.2f;
            m_View.transform.position = cellView.transform.position + new Vector3(0, 0.2f, 0);
        }
    }
}
