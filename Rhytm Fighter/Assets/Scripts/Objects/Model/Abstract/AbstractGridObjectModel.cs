﻿using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using RhytmFighter.Core.Enums;
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
            if (View == null)
                View = CreateView(cellView);

            View.ShowView(this);
        }

        public virtual void HideView()
        {
            if (View != null)
                View.HideView();
        }


        protected abstract AbstractGridObjectView CreateView(CellView cellView);

    }
}
