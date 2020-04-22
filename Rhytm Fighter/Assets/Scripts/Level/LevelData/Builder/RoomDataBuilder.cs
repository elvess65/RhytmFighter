using Frameworks.Grid.Data;
using RhytmFighter.Battle.Action.Behaviours;
using RhytmFighter.Battle.Health.Behaviours;
using RhytmFighter.Core;
using RhytmFighter.Core.Enums;
using RhytmFighter.Objects.Model;
using System.Collections.Generic;
using UnityEngine;

namespace RhytmFighter.Level.Data
{
    public class RoomDataBuilder
    {
        private int m_ENEMY_ID = 2;
        private List<GridCellData> m_EmptyCells;
        private GridCellData enemyCell_debug;

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

                        cell.SetCellProperty(GridCellProperty_GateToNode.CreateProperty(node.ParentNode.ID, GateTypes.ToParentNode));
                        grid.ParentNodeGate = cell;
                    }
                    //Add property GateToLeftNode
                    else if (node.LeftNode != null && i == leftGateCellX && j == leftGateCellY)
                    {
                        if (cellType != CellTypes.Normal)
                            cellType = CellTypes.Normal;

                        cell.SetCellProperty(GridCellProperty_GateToNode.CreateProperty(node.LeftNode.ID, GateTypes.ToNextNode));
                        grid.LeftNodeGate = cell;
                    }
                    //Add property GateToRightNode
                    else if (node.RightNode != null && i == rightGateCellX && j == rightGateCellY)
                    {
                        if (cellType != CellTypes.Normal)
                            cellType = CellTypes.Normal;

                        cell.SetCellProperty(GridCellProperty_GateToNode.CreateProperty(node.RightNode.ID, GateTypes.ToNextNode));
                        grid.RightNodeGate = cell;
                    }

                    cellType = CellTypes.Normal;
                    cell.SetCellType(cellType);
                    cell.SetRoomID(node.ID);

                    if (grid.CellIsWalkable(cell))
                        m_EmptyCells.Add(cell);

                    //TODO: Macros
                    if (enemyCell_debug == null && i == grid.WidthInCells / 2 && j == grid.HeightInCells / 2 + 1)
                        enemyCell_debug = cell;
                }
            }


            int rndIndex = Random.Range(0, m_EmptyCells.Count);

            //TODO: Macros
            /*StandardItemModel item = new StandardItemModel(1, m_EmptyCells[rndIndex]);
            m_EmptyCells[rndIndex].AddObject(item);
            m_EmptyCells.RemoveAt(rndIndex);*/

            //TODO: Macros
            //if (!node.IsStartNode)
            //if (node.IsStartNode)
            {
                rndIndex = Random.Range(0, m_EmptyCells.Count);

                float enemyMoveSpeed = GameManager.ENEMY_MOVE_SPEED;
                int enemyHP = 10;
                //TODO: Macros
                StandardEnemyNPCModel enemyNPC = new StandardEnemyNPCModel(m_ENEMY_ID++, enemyCell_debug, enemyMoveSpeed, new SimpleBattleActionBehaviour(1, 1, 1), 
                                                                                                                          new SimpleHealthBehaviour(enemyHP),
                                                                                                                          AITypes.Simple);
                enemyCell_debug.AddObject(enemyNPC);
                enemyCell_debug = null;
                //TODO: Macros
                //m_EmptyCells[rndIndex].AddObject(enemyNPC);
                //m_EmptyCells.RemoveAt(rndIndex);
            }
        }
    }
}