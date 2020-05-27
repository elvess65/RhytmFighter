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
        private int m_ITEM_ID = 1;

        public LevelRoomData Build(LevelNodeData node, int minWidth, int maxWidth, int minHeight, int maxheight, float cellSize, int obstacleFillPercent)
        {
            Random.InitState(node.NodeSeed);

            int width = Random.Range(minWidth, maxWidth);
            int height = Random.Range(minHeight, maxheight);

            SquareGrid roomGrid = new SquareGrid(width, height, cellSize, Vector2.zero);
            ApplyDataToGrid(roomGrid, node, obstacleFillPercent);

            return new LevelRoomData(roomGrid, node);
        }


        void ApplyDataToGrid(SquareGrid grid, LevelNodeData node, int obstacleFillPercent)
        {
            List<GridCellData> emptyCells = new List<GridCellData>();

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
                    bool isPlayerSpawnCell = false;
                    GridCellData cell = grid.GetCellByCoord(i, j);
                    CellTypes cellType = Random.Range(0, 100) < obstacleFillPercent ? CellTypes.Normal : CellTypes.Obstacle;

                    //TODO: Add player spawn cell

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
                    //Player spawn cell
                    else if (i == grid.WidthInCells / 2 && j == 0)
                    {
                        if (cellType != CellTypes.Normal)
                            cellType = CellTypes.Normal;

                        isPlayerSpawnCell = true;
                    }

                    cell.SetCellType(cellType);
                    cell.SetRoomID(node.ID);

                    if (cellType == CellTypes.Normal && !isPlayerSpawnCell)
                        emptyCells.Add(cell);
                }
            }

            GenerateContent(grid, node, ref emptyCells);
        }


        void GenerateContent(SquareGrid grid, LevelNodeData node, ref List<GridCellData> emptyCells)
        {
            GenerateItem(ref emptyCells);
            GenerateEnemy(grid, node, ref emptyCells);
        }

        void GenerateItem(ref List<GridCellData> emptyCells)
        {
            //TODO: Amount of items per room
            //TODO: Available items per room

            if (emptyCells.Count == 0)
                return;

            GridCellData rndCell = GetRandomCell(ref emptyCells);
            StandardItemModel item = new StandardItemModel(m_ITEM_ID++, rndCell);
            rndCell.AddObject(item);
        }

        void GenerateEnemy(SquareGrid grid, LevelNodeData node, ref List<GridCellData> emptyCells)
        {
            if (emptyCells.Count == 0)
                return;

            //TODO: Amount of enemies per room
            //TODO: Available enemies per room
            //TODO: Enemy params:
            //  - HP per room
            //  - Damage per room

            if (!node.IsStartNode)
            {
                int enemyHP = 1;
                int enemyDmg = 1;
                float enemyMoveSpeed = GameManager.ENEMY_MOVE_SPEED;

                for (int i = 0; i < 2; i++)
                {
                    GridCellData rndCell = GetRandomCell(ref emptyCells);
                    StandardEnemyNPCModel enemyNPC = new StandardEnemyNPCModel(m_ENEMY_ID++, rndCell, enemyMoveSpeed, new SimpleBattleActionBehaviour(1, 1, enemyDmg),
                                                                                                                      new SimpleHealthBehaviour(enemyHP),
                                                                                                                      AITypes.Simple);

                    rndCell.AddObject(enemyNPC);
                }
            }
            else if (node.IsFinishNode)
            {
                int enemyHP = 5;
                int enemyDmg = 5;
                float enemyMoveSpeed = GameManager.ENEMY_MOVE_SPEED;


                GridCellData rndCell = GetRandomCell(ref emptyCells);
                StandardEnemyNPCModel enemyNPC = new StandardEnemyNPCModel(m_ENEMY_ID++, rndCell, enemyMoveSpeed, new SimpleBattleActionBehaviour(1, 1, enemyDmg),
                                                                                                                  new SimpleHealthBehaviour(enemyHP),
                                                                                                                  AITypes.Simple);

                rndCell.AddObject(enemyNPC);
            }
        }

        GridCellData GetRandomCell(ref List<GridCellData> emptyCells)
        {
            int rndIndex = Random.Range(0, emptyCells.Count);
            GridCellData rndCell = emptyCells[rndIndex];
            emptyCells.RemoveAt(rndIndex);

            return rndCell;
        }
    }
}