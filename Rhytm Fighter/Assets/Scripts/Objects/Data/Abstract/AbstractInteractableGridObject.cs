using Frameworks.Grid.Data;

namespace RhytmFighter.Objects.Data
{
    public abstract class AbstractInteractableGridObject : AbstractGridObject
    {
        public AbstractInteractableGridObject(int id, GridCellData correspondingCell) : base(id, correspondingCell)
        {
        }

        public abstract void Interact();
    }
}
