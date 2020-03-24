using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using RhytmFighter.Objects.View;
using UnityEngine;

namespace RhytmFighter.Objects.Model
{
    public class StandardItemModel : AbstractItemModel
    {        
        public StandardItemModel(int id, GridCellData correspondingCell) : base(id, correspondingCell)
        {
        }

        protected override AbstractGridObjectView CreateView(CellView cellView)
        {
            GameObject view = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            view.GetComponent<Collider>().enabled = false;
            view.transform.localScale = Vector3.one * 0.2f;
            view.transform.position = cellView.transform.position + new Vector3(0, 0.2f, 0);

            return view.AddComponent<StandardItemView>();
        }
    }
}
