using Frameworks.Grid.Data;

namespace RhytmFighter.Objects.Model
{
    public abstract class AbstractInteractableObjectModel : AbstractGridObjectModel
    {
        public AbstractInteractableObjectModel(int id, GridCellData correspondingCell) : base(id, correspondingCell)
        {
        }

        public abstract void Interact();
    }
}
