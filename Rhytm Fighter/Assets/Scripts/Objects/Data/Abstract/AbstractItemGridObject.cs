using Frameworks.Grid.Data;

namespace RhytmFighter.Objects.Data
{
    public abstract class AbstractItemGridObject : AbstractInteractableGridObject
    {
        public AbstractItemGridObject(int id, GridCellData correspondingCell) : base(id, correspondingCell)
        {
        }

        public override void Interact()
        {
            CorrespondingCell.RemoveObject();
            RemoveView();
        }
    }
}
