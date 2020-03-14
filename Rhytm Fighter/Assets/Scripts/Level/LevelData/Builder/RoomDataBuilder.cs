using Frameworks.Grid.Data;
using RhytmFighter.Objects;
using UnityEngine;

namespace RhytmFighter.Level.Data
{
    public class RoomDataBuilder 
    {
        public LevelRoomData Build(LevelNodeData node, int minWidth, int maxWidth, int minHeight, int maxheight, float cellSize, int fillPercent)
        {
            Random.InitState(node.NodeSeed);

            int width = Random.Range(minWidth, maxWidth);
            int height = Random.Range(minHeight, maxheight);

            SquareGrid roomGrid = new SquareGrid(width, height, cellSize, Vector2.zero);
            ApplyDataToGrid(roomGrid, node, fillPercent);

            return new LevelRoomData(roomGrid, node);
        }


        void ApplyDataToGrid(SquareGrid grid, LevelNodeData node, int fillPercent)
        {
            //Properties
            //GateToParentNode
            int parentGateCellX = grid.WidthInCells / 2;
            int parentGateCellY = 0;

            //GateToNextNode
            int leftGateCellX = 0;
            int leftGateCellY = grid.HeightInCells - 1;

            int rightGateCellX = grid.WidthInCells - 1;
            int rightGateCellY = grid.HeightInCells - 1;

            for (int i = 0; i < grid.WidthInCells; i++)
            {
                for (int j = 0; j < grid.HeightInCells; j++)
                {
                    GridCellData cell = grid.GetCellByCoord(i, j);
                    CellTypes cellType = Random.Range(0, 100) < fillPercent ? CellTypes.Normal : CellTypes.Obstacle;

                    //Add property GateToParentNode
                    if (node.ParentNode != null && i == parentGateCellX && j == parentGateCellY)
                    {
                        if (cellType != CellTypes.Normal)
                            cellType = CellTypes.Normal;

                        cell.SetCellProperty(GridCellProperty_GateToNode.CreateProperty(node.ParentNode.ID, GridCellProperty_GateToNode.GateTypes.ToParentNode));
                        grid.ParentNodeGate = cell;
                    }
                    //Add property GateToLeftNode
                    else if (node.LeftNode != null && i == leftGateCellX && j == leftGateCellY)
                    {
                        if (cellType != CellTypes.Normal)
                            cellType = CellTypes.Normal;

                        cell.SetCellProperty(GridCellProperty_GateToNode.CreateProperty(node.LeftNode.ID, GridCellProperty_GateToNode.GateTypes.ToNextNode));
                        grid.LeftNodeGate = cell;
                    }
                    //Add property GateToRightNode
                    else if (node.RightNode != null && i == rightGateCellX && j == rightGateCellY)
                    {
                        if (cellType != CellTypes.Normal)
                            cellType = CellTypes.Normal;

                        cell.SetCellProperty(GridCellProperty_GateToNode.CreateProperty(node.RightNode.ID, GridCellProperty_GateToNode.GateTypes.ToNextNode));
                        grid.RightNodeGate = cell;
                    }

                    cell.SetCellType(cellType);
                    cell.SetRoomID(node.ID);

                    if (i == 2 && j == 3)
                        cell.AddObject(new DummyObject());
                }
            }
        }
    }
}