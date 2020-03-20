using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using RhytmFighter.Objects.View;
using UnityEngine;

namespace RhytmFighter.Objects.Data
{
    public class ExampleItemGridObject : AbstractItemGridObject
    {        
        public ExampleItemGridObject(int id, GridCellData correspondingCell) : base(id, correspondingCell)
        {
        }

        protected override AbstractGridObjectView CreateView(CellView cellView)
        {
            GameObject view = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            view.GetComponent<Collider>().enabled = false;
            view.transform.localScale = Vector3.one * 0.2f;
            view.transform.position = cellView.transform.position + new Vector3(0, 0.2f, 0);

            return view.AddComponent<ExampleItemVIew>();
        }
    }
}
