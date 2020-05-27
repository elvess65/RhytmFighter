using Frameworks.Grid.Data;
using RhytmFighter.Core.Enums;

namespace RhytmFighter.Objects.Model
{
    public abstract class AbstractItemModel : AbstractInteractableObjectModel
    {
        public AbstractItemModel(int id, GridCellData correspondingCell) : base(id, correspondingCell)
        {
            Type = GridObjectTypes.Item;
        }

        public override void Interact()
        {
            UnityEngine.Debug.Log($"Interact: {ID}");
            CorrespondingCell.RemoveObject();
            HideView();
        }
    }
}
