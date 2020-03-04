using Frameworks.Grid.Data;
using UnityEngine;

namespace Frameworks.Grid.View
{
    public class GridGizmoDrawer : MonoBehaviour
    {
        private SquareGrid m_Grid;

        public void SetGrid(SquareGrid grid) => m_Grid = grid;

        private void OnDrawGizmos()
        {
            if (m_Grid != null)
                m_Grid.ForEachCell(DrawGizmoForCell);
        }

        void DrawGizmoForCell(GridCellData cell)
        {
            Vector3 pos = m_Grid.GetCellWorldPosByCoord(cell.X, cell.Y);

            Color color = Color.white;
            switch (cell.CellType)
            {
                case CellTypes.Normal:
                    color = Color.white;
                    break;
                case CellTypes.Obstacle:
                    color = Color.blue;
                    break;
            }

            //Gizmos.color = cell.HasObject ? Color.green : color; 

            Gizmos.DrawWireSphere(pos, cell.CellSize / 2);
        }
    }
}
