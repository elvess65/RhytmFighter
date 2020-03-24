using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using RhytmFighter.Objects.View;

namespace RhytmFighter.Objects.Model
{
    public abstract class AbstractGridObjectModel
    {
        public int ID { get; protected set; }
        public GridObjectTypes Type { get; protected set; }
        public GridCellData CorrespondingCell { get; protected set; }
        public AbstractGridObjectView View { get; private set; }


        public AbstractGridObjectModel(int id, GridCellData correspondingCell)
        {
            ID = id;
            CorrespondingCell = correspondingCell;
        }

        public virtual void ShowView(CellView cellView)
        {
            View = CreateView(cellView);
            View.Show(this);
        }

        public virtual void HideView()
        {
            if (View != null)
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
