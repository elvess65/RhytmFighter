﻿using Frameworks.Grid.Data;

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
            CorrespondingCell.RemoveObject();
            HideView();
        }
    }
}