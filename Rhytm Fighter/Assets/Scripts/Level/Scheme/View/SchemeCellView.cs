using Frameworks.Grid.Data;
using UnityEngine;

namespace RhytmFighter.Level.Scheme.View
{
    /// <summary>
    /// Scheme view for cell visualization
    /// </summary>
    public class SchemeCellView : AbstractSchemeView
    {
        private readonly Color OBSTACLE_CELL_COLOR = Color.red;

        public GridCellData Cell { get; private set; }

        public void Initialize(GridCellData cellData)
        {
            Cell = cellData;

            Initialize($"Cell ({cellData.X} : {cellData.Y})");
        }

        public override void ShowAsNormal()
        {
            switch (Cell.CellType)
            {
                case CellTypes.Normal:
                    base.ShowAsNormal();
                    break;
                case CellTypes.Obstacle:
                    ApplyColorToMaterial(OBSTACLE_CELL_COLOR);
                    break; 
            }
        }
    }
}
