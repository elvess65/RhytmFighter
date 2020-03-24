using Frameworks.Grid.Data;

namespace RhytmFighter.Objects.Model
{
    public abstract class AbstractNPCModel : AbstractInteractableObjectModel
    {
        public AbstractNPCModel(int id, GridCellData correspondingCell) : base(id, correspondingCell)
        {
            Type = GridObjectTypes.NPC;
        }

        public override void Interact()
        { }
    }
}
