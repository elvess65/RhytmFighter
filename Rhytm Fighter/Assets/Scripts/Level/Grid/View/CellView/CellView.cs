using Frameworks.Grid.Data;
using Frameworks.Grid.View.Cell;
using UnityEngine;

namespace Frameworks.Grid.View
{
    public class CellView : MonoBehaviour
    {
        private Abstract_CellContent m_CellContent;
        private iCellAppearanceStrategy m_CellAppearanceStrategy;

        public GridCellData CorrespondingCellData { get; private set; }
        public bool IsShowed => m_CellAppearanceStrategy.IsShowed;


        public void Initialize(GridCellData correspondingCellData, Abstract_CellContent cellContent)
        {
            //Cell data
            CorrespondingCellData = correspondingCellData;

            //Cell content
            m_CellContent = cellContent; 
            m_CellContent.transform.parent = transform;
            m_CellContent.transform.localPosition = Vector3.zero;

            //Cell appearance
            m_CellAppearanceStrategy = new ScaleUp_CellAppearanceStrategy(transform);
        }

        public void ShowCell()
        {
            if (IsShowed)
                return;

            //Show cell
            m_CellAppearanceStrategy.Show();

            //Make cell visited
            if (!CorrespondingCellData.IsVisited)
                CorrespondingCellData.IsVisited = true;

            gameObject.name = $"Cell_(X - {CorrespondingCellData.X}. Y - {CorrespondingCellData.Y}) {CorrespondingCellData.IsVisited}";
        }

        public void HideCell()
        {
            if (!IsShowed)
                return;

            m_CellAppearanceStrategy.Hide();

            gameObject.name = $"Cell_(X - {CorrespondingCellData.X}. Y - {CorrespondingCellData.Y}) {CorrespondingCellData.IsVisited}";
        }
    }
}
