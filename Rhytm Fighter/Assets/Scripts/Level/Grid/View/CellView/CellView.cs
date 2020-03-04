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
    }
}
