using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using RhytmFighter.Assets;
using RhytmFighter.Objects.View;

namespace RhytmFighter.Objects.Model
{
    public class StandardItemModel : AbstractItemModel
    {        
        public StandardItemModel(int id, GridCellData correspondingCell) : base(id, correspondingCell)
        {
        }

        protected override AbstractGridObjectView CreateView(CellView cellView)
        {
            return AssetsManager.GetPrefabAssets().InstantiatePrefab(AssetsManager.GetPrefabAssets().StandartItemViewPrefab, cellView.transform.position);
        }
    }
}
