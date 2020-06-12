using Frameworks.Grid.Data;
using RhytmFighter.Battle.Action.Behaviours;
using RhytmFighter.Battle.Health.Behaviours;
using RhytmFighter.Persistant.Enums;
using RhytmFighter.Objects.Model;
using System.Collections.Generic;
using UnityEngine;
using RhytmFighter.Battle.Core;
using RhytmFighter.Data;

namespace RhytmFighter.Level.Data
{
    public class RoomDataBuilder
    {
        private int m_ITEM_ID = 1;
        private int m_ENEMY_ID = 2;


        public LevelRoomData Build(LevelNodeData node, LevelsData.BuildData buildData, LevelsData.ContentData contentData)
        {
            Random.InitState(node.NodeSeed);

            int width = Random.Range(buildData.MinWidth, buildData.MaxWidth);
            int height = Random.Range(buildData.MinHeight, buildData.MaxHeight);

            SquareGrid roomGrid = new SquareGrid(width, height, buildData.CellSize, Vector2.zero);
            ApplyDataToGrid(roomGrid, node, buildData.ObstacleFillPercent, contentData);

            return new LevelRoomData(roomGrid, node);
        }


        void ApplyDataToGrid(SquareGrid grid, LevelNodeData node, int obstacleFillPercent, LevelsData.ContentData contentData)
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

            GenerateContent(grid, node, ref emptyCells, contentData);
        }


        void GenerateContent(SquareGrid grid, LevelNodeData node, ref List<GridCellData> emptyCells, LevelsData.ContentData contentData)
        {
            GenerateItem(node, contentData, ref emptyCells);
            GenerateEnemy(grid, node, ref emptyCells, contentData);
        }

        void GenerateItem(LevelNodeData node, LevelsData.ContentData contentData,ref List<GridCellData> emptyCells)
        {
            if (emptyCells.Count == 0 || node.IsFinishNode)
                return;

            int amountOfItems = Random.Range(contentData.MinAmountOfItems, contentData.MaxAmountOfItems + 1);

            for (int i = 0; i < amountOfItems; i++)
            {
                int rndItemContentID = contentData.AvailableItemsIDs[Random.Range(0, contentData.AvailableItemsIDs.Length)];

                GridCellData rndCell = GetRandomCell(ref emptyCells);
                StandardItemModel item = new StandardItemModel(m_ITEM_ID++, rndItemContentID, rndCell);
                rndCell.AddObject(item);
            }
        }

        void GenerateEnemy(SquareGrid grid, LevelNodeData node, ref List<GridCellData> emptyCells, LevelsData.ContentData contentData)
        {
            if (emptyCells.Count == 0)
                return;

            if (!node.IsStartNode && !node.IsFinishNode)
            {
                int rndAmountOfEnemies = Random.Range(contentData.MinAmountOfEnemies, contentData.MaxAmountOfEnemies + 1);

                for (int i = 0; i < rndAmountOfEnemies; i++)
                {
                    GridCellData rndCell = GetRandomCell(ref emptyCells);
                    int rndViewID = contentData.AvailableEnemyViewIDs[Random.Range(0, contentData.AvailableEnemyViewIDs.Length)];
                    int rndHP = Random.Range(contentData.MinEnemyHP, contentData.MaxEnemyHP + 1);
                    int rndDmg = Random.Range(contentData.MinEnemyDmg, contentData.MaxEnemyDmg + 1);

                    StandardEnemyNPCModel enemyNPC = new StandardEnemyNPCModel(m_ENEMY_ID++, rndViewID, rndCell, BattleManager.Instance.NPCMoveSpeed, 
                                                                               new SimpleBattleActionBehaviour(rndDmg),
                                                                               new SimpleHealthBehaviour(rndHP),
                                                                               AITypes.Simple);

                    rndCell.AddObject(enemyNPC);
                }
            }
            else if (node.IsFinishNode)
            {
                GridCellData rndCell = GetRandomCell(ref emptyCells);
                int rndViewID = contentData.AvailableEnemyViewIDs[Random.Range(0, contentData.AvailableEnemyViewIDs.Length)];
                int rndHP = Random.Range(contentData.MinBossHP, contentData.MaxBossHP + 1);
                int rndDmg = Random.Range(contentData.MinBossDmg, contentData.MaxBossDmg + 1);
                
                StandardEnemyNPCModel enemyNPC = new StandardEnemyNPCModel(m_ENEMY_ID++, rndViewID, rndCell, BattleManager.Instance.NPCMoveSpeed, 
                                                                           new SimpleBattleActionBehaviour(rndDmg),
                                                                           new SimpleHealthBehaviour(rndHP),
                                                                           AITypes.SimpleDefencible);

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