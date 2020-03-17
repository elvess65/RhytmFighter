using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using UnityEngine;

namespace RhytmFighter.Objects.Data
{
    public abstract class AbstractGridObject
    {
        public int ID { get; private set; }
        public GridObjectTypes Type { get; protected set; }
        public GridCellData CorrespondingCell { get; private set; }

        protected GameObject m_View;

        public AbstractGridObject(int id, GridCellData correspondingCell)
        {
            ID = id;
            CorrespondingCell = correspondingCell;
        }

        public void Detect(CellView cellView)
        {
            CreateView(cellView);
        }

        public virtual void RemoveView()
        {
            Object.Destroy(m_View);
        }


        protected abstract void CreateView(CellView cellView);

    }

    public enum GridObjectTypes
    {
        Item,
        Enemy
    }
}
