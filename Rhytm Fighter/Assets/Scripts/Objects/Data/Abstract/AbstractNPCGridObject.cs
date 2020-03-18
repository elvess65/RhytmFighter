using Frameworks.Grid.Data;

namespace RhytmFighter.Objects.Data
{
    public abstract class AbstractNPCGridObject : AbstractInteractableGridObject
    {
        public bool IsEnemy { get; protected set; }


        public AbstractNPCGridObject(int id, GridCellData correspondingCell, bool isEnemy) : base(id, correspondingCell)
        {
            Type = GridObjectTypes.NPC;
            IsEnemy = isEnemy;
        }

        public override void Interact()
        { }
    }
}
