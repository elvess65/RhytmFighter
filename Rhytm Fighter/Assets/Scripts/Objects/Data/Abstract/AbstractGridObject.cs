using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using RhytmFighter.Objects.View;

namespace RhytmFighter.Objects.Data
{
    public abstract class AbstractGridObject
    {
        public int ID { get; protected set; }
        public GridObjectTypes Type { get; protected set; }
        public GridCellData CorrespondingCell { get; protected set; }
        public AbstractGridObjectView View { get; private set; }


        public AbstractGridObject(int id, GridCellData correspondingCell)
        {
            ID = id;
            CorrespondingCell = correspondingCell;
        }

        public void Detect(CellView cellView)
        {
            View = CreateView(cellView);
            View.Show(this);
        }

        public virtual void RemoveView()
        {
            View.Hide();
        }


        protected abstract AbstractGridObjectView CreateView(CellView cellView);

    }

    public enum GridObjectTypes
    {
        Item,
        NPC
    }
}
