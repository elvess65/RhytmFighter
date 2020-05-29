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

        public LevelRoomData Build(LevelNodeData node, RhytmFighter.Data.LevelsData.BuildData buildData)
        {
            Debug.Log(node.NodeSeed);
            Random.InitState(node.NodeSeed);

            int width = Random.Range(buildData.MinWidth, buildData.MaxWidth);
            int height = Random.Range(buildData.MinHeight, buildData.MaxHeight);

            SquareGrid roomGrid = new SquareGrid(width, height, buildData.CellSize, Vector2.zero);
            ApplyDataToGrid(roomGrid, node, buildData.ObstacleFillPercent);

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
                    bool cellCanBeUsedAsEmpty = true;
                    GridCellData cell = grid.GetCellByCoord(i, j);
                    CellTypes cellType = Random.Range(0, 100) < 100 - obstacleFillPercent ? CellTypes.Normal : CellTypes.Obstacle;

                    //TODO: Add player spawn cell

                    //Add property GateToParentNode
                    if (node.ParentNode != null && i == parentGateCellX && j == parentGateCellY)
                    {
                        if (cellType != CellTypes.Normal)
                            cellType = CellTypes.Normal;

                        cell.SetCellProperty(GridCellProperty_GateToNode.CreateProperty(node.ParentNode.ID, GateTypes.ToParentNode));
                        grid.ParentNodeGate = cell;
                        cellCanBeUsedAsEmpty = false;
                    }
                    //Add property GateToLeftNode
                    else if (node.LeftNode != null && i == leftGateCellX && j == leftGateCellY)
                    {
                        if (cellType != CellTypes.Normal)
                            cellType = CellTypes.Normal;

                        cell.SetCellProperty(GridCellProperty_GateToNode.CreateProperty(node.LeftNode.ID, GateTypes.ToNextNode));
                        grid.LeftNodeGate = cell;
                        cellCanBeUsedAsEmpty = false;
                    }
                    //Add property GateToRightNode
                    else if (node.RightNode != null && i == rightGateCellX && j == rightGateCellY)
                    {
                        if (cellType != CellTypes.Normal)
                            cellType = CellTypes.Normal;

                        cell.SetCellProperty(GridCellProperty_GateToNode.CreateProperty(node.RightNode.ID, GateTypes.ToNextNode));
                        grid.RightNodeGate = cell;
                        cellCanBeUsedAsEmpty = false;
                    }
                    //Player spawn cell
                    else if (i == grid.WidthInCells / 2 && j == 0)
                    {
                        if (cellType != CellTypes.Normal)
                            cellType = CellTypes.Normal;

                        cellCanBeUsedAsEmpty = false;
                    }

                    cell.SetCellType(cellType);
                    cell.SetRoomID(node.ID);

                    if (cellType == CellTypes.Normal && cellCanBeUsedAsEmpty)
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
            if (emptyCells.Count == 0)
                return;

            //TODO: Amount of items per room
            int amountOfItems = Random.Range(1, 3);

            //TODO: Available content per room
            int[] availableContent = new int[] { 1, 2 };

            for (int i = 0; i < amountOfItems; i++)
            {
                int rndItemContentID = availableContent[Random.Range(0, availableContent.Length)];

                GridCellData rndCell = GetRandomCell(ref emptyCells);
                StandardItemModel item = new StandardItemModel(m_ITEM_ID++, rndItemContentID, rndCell);
                rndCell.AddObject(item);
            }
        }

        void GenerateEnemy(SquareGrid grid, LevelNodeData node, ref List<GridCellData> emptyCells)
        {
            if (emptyCells.Count == 0)
                return;

            //TODO: Amount of enemies per room
            int amountOfEnemies = Random.Range(1, 3);

            //TODO: Available views per room
            int[] availableViews = new int[] { 1, 2 };

            //TODO: Enemy params:
            //  - HP per room
            int enemyHP = Random.Range(1, 10);
            //  - Damage per room
            int enemyDmg = Random.Range(1, 5);

            //TODO:Boss params
            int bossHP = Random.Range(15, 25);
            int bossDmg = Random.Range(7, 15);

            if (!node.IsStartNode)
            {
                for (int i = 0; i < amountOfEnemies; i++)
                {
                    int rndViewID = availableViews[Random.Range(0, availableViews.Length)];

                    GridCellData rndCell = GetRandomCell(ref emptyCells);
                    StandardEnemyNPCModel enemyNPC = new StandardEnemyNPCModel(m_ENEMY_ID++, rndViewID, rndCell, GameManager.Instance.NPCMoveSpeed, 
                                                                               new SimpleBattleActionBehaviour(enemyDmg),
                                                                               new SimpleHealthBehaviour(enemyHP),
                                                                               AITypes.Simple);

                    rndCell.AddObject(enemyNPC);
                }
            }
            else if (node.IsFinishNode)
            {
                int rndViewID = availableViews[Random.Range(0, availableViews.Length)];

                GridCellData rndCell = GetRandomCell(ref emptyCells);
                StandardEnemyNPCModel enemyNPC = new StandardEnemyNPCModel(m_ENEMY_ID++, rndViewID, rndCell, GameManager.Instance.NPCMoveSpeed, 
                                                                           new SimpleBattleActionBehaviour(bossDmg),
                                                                           new SimpleHealthBehaviour(bossHP),
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