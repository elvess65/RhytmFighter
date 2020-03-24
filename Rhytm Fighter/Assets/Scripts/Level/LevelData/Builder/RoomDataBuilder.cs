using Frameworks.Grid.Data;
using RhytmFighter.Battle.Action.Behaviours;
using RhytmFighter.Battle.Health.Behaviours;
using RhytmFighter.Objects.Data;
using System.Collections.Generic;
using UnityEngine;

namespace RhytmFighter.Level.Data
{
    public class RoomDataBuilder 
    {
        private List<GridCellData> m_EmptyCells;

        public LevelRoomData Build(LevelNodeData node, int minWidth, int maxWidth, int minHeight, int maxheight, float cellSize, int fillPercent)
        {
            m_EmptyCells = new List<GridCellData>();

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
                    else
                        m_EmptyCells.Add(cell);

                    cell.SetCellType(cellType);
                    cell.SetRoomID(node.ID);

                    /*if (i == 1 && j == 0)
                        cell.AddObject(new ExampleItemGridObject(1, cell));
                    else if (i == 2 && j == 3)
                        cell.AddObject(new ExampleEnemyNPCGridObject(2, cell, null));*/
                }
            }

            
            int rndIndex = Random.Range(0, m_EmptyCells.Count);

            ExampleItemGridObject item = new ExampleItemGridObject(1, m_EmptyCells[rndIndex]);
            m_EmptyCells[rndIndex].AddObject(item);
            m_EmptyCells.RemoveAt(rndIndex);

            if (!node.IsStartNode)
            {
                rndIndex = Random.Range(0, m_EmptyCells.Count);

                ExampleEnemyNPCGridObject enemyNPC = new ExampleEnemyNPCGridObject(2, m_EmptyCells[rndIndex], new ExampleBattleActionBehaviour(), new ExampleHealthBehaviour(2, 3));
                m_EmptyCells[rndIndex].AddObject(enemyNPC);
                m_EmptyCells.RemoveAt(rndIndex);
            }
        }
    }
}