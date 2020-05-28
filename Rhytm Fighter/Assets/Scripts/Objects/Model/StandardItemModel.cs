using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using RhytmFighter.Assets;
using RhytmFighter.Objects.View;

namespace RhytmFighter.Objects.Model
{
    public class StandardItemModel : AbstractItemModel
    {
        private int m_ContentID;


        public StandardItemModel(int id, int contentID, GridCellData correspondingCell) : base(id, correspondingCell)
        {
            m_ContentID = contentID;
        }

        public override void Interact()
        {
            UnityEngine.Debug.Log($"Interact with standart item. ID: {ID} ContentID {m_ContentID}");
            base.Interact();
        }


        protected override AbstractGridObjectView CreateView(CellView cellView)
        {
            return AssetsManager.GetPrefabAssets().InstantiatePrefab(AssetsManager.GetPrefabAssets().StandartItemViewPrefab, cellView.transform.position);
        }
    }
}
