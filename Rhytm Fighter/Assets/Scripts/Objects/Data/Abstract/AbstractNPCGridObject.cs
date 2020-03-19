using Frameworks.Grid.Data;

namespace RhytmFighter.Objects.Data
{
    public abstract class AbstractNPCGridObject : AbstractInteractableGridObject
    {
        public AbstractNPCGridObject(int id, GridCellData correspondingCell) : base(id, correspondingCell)
        {
            Type = GridObjectTypes.NPC;
        }

        public override void Interact()
        { }
    }
}
