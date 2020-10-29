using Frameworks.Grid.Data;
using RhytmFighter.Battle.Action.Behaviours;
using RhytmFighter.Battle.Health.Behaviours;
using RhytmFighter.Persistant.Enums;
using RhytmFighter.Objects.Model;
using System.Collections.Generic;
using UnityEngine;
using RhytmFighter.Battle.Core;
using RhytmFighter.Data;
using RhytmFighter.Persistant.Helpers;

namespace RhytmFighter.Level.Data
{
    public class RoomDataBuilder
    {
        private int m_ITEM_ID = 1;
        private int m_ENEMY_ID = 2;


        public LevelRoomData Build(LevelNodeData node, LevelsData.BuildData buildData, LevelsData.ContentData contentData, float completionProgress)
        {
            Random.InitState(node.NodeSeed);

            int width = Random.Range(buildData.MinWidth, buildData.MaxWidth);
            int height = Random.Range(buildData.MinHeight, buildData.MaxHeight);

            SquareGrid roomGrid = new SquareGrid(width, height, buildData.CellSize, Vector2.zero);
            ApplyDataToGrid(roomGrid, node, buildData.ObstacleFillPercent, completionProgress, contentData);

            return new LevelRoomData(roomGrid, node);
        }


        void ApplyDataToGrid(SquareGrid grid, LevelNodeData node, int obstacleFillPercent, float completionProgress, LevelsData.ContentData contentData)
        {
            GenerateGrid(grid, node, obstacleFillPercent, out List<GridCellData> emptyCells);
            GenerateContent(grid, node, ref emptyCells, contentData, completionProgress);
        }


        void GenerateGrid(SquareGrid grid, LevelNodeData node, int obstacleFillPercent, out List<GridCellData> emptyCells)
        {
            emptyCells = new List<GridCellData>();

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
                    CellTypes cellType = HelpersCollection.IsInRandomRange(obstacleFillPercent) ? CellTypes.Obstacle : CellTypes.Normal;

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
        }

        void GenerateContent(SquareGrid grid, LevelNodeData node, ref List<GridCellData> emptyCells, LevelsData.ContentData contentData, float completionProgress)
        {
            GenerateItem(node, contentData, ref emptyCells, completionProgress);
            GenerateEnemy(node, ref emptyCells, contentData, completionProgress);
        }

        void GenerateItem(LevelNodeData node, LevelsData.ContentData contentData,ref List<GridCellData> emptyCells, float completionProgress)
        {
            if (emptyCells.Count == 0 || node.IsFinishNode)
                return;

            int amountOfItems = GetRandomAmountFromProgression(contentData.ItemProgressionConfig, completionProgress);

            for (int i = 0; i < amountOfItems; i++)
            {
                int rndItemContentID = GetRandomIDFromProgression(contentData.ItemProgressionConfig, completionProgress);

                GridCellData rndCell = GetRandomCell(ref emptyCells);
                StandardItemModel item = new StandardItemModel(m_ITEM_ID++, rndItemContentID, rndCell);
                rndCell.AddObject(item);
            }
        }

        void GenerateEnemy(LevelNodeData node, ref List<GridCellData> emptyCells, LevelsData.ContentData contentData, float completionProgress)
        {
            if (emptyCells.Count == 0)
                return;

            if (!node.IsStartNode && !node.IsFinishNode)
            {
                int rndAmountOfEnemies = GetRandomAmountFromProgression(contentData.EnemyViewProgressionConfig, completionProgress);

                for (int i = 0; i < rndAmountOfEnemies; i++)
                {
                    GridCellData rndCell = GetRandomCell(ref emptyCells);

                    int rndViewID = GetRandomIDFromProgression(contentData.EnemyViewProgressionConfig, completionProgress);
                    int rndHP = GetRandomHPFromProgression(contentData.EnemyDataProgressionConfig, completionProgress);
                    int rndDmg = GetRandomDamageFromProgression(contentData.EnemyDataProgressionConfig, completionProgress);

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

                int rndViewID = GetRandomIDFromProgression(contentData.EnemyViewProgressionConfig, completionProgress);
                int rndHP = GetRandomHPFromProgression(contentData.BossDataProgressionConfig, completionProgress);
                int rndDmg = GetRandomDamageFromProgression(contentData.BossDataProgressionConfig, completionProgress);

                Debug.Log($"Create enemy. ViewID {rndViewID}. HP {rndHP} and Damage {rndDmg}. CompletionProgress {completionProgress}");

                StandardEnemyNPCModel enemyNPC = new StandardEnemyNPCModel(m_ENEMY_ID++, rndViewID, rndCell, BattleManager.Instance.NPCMoveSpeed, 
                                                                           new SimpleBattleActionBehaviour(rndDmg),
                                                                           new SimpleHealthBehaviour(rndHP),
                                                                           AITypes.SimpleDefencible);

                rndCell.AddObject(enemyNPC);
            }
        }


        private int GetRandomAmountFromProgression(ObjectProgressionConfig progressionConfig, float t)
        {
            (int min, int max) result = progressionConfig.EvaluateAmountInt(t);
            return Random.Range(result.min, result.max + 1);
        }

        private int GetRandomIDFromProgression(ObjectProgressionConfig progressionConfig, float t)
        {
            int[] ids = progressionConfig.EvaluateViewIDs(t);
            return ids[Random.Range(0, ids.Length)];
        }

        private int GetRandomHPFromProgression(NPCProgressionConfig progressionConfig, float t)
        {
            return GetRandomValueFromProgression(progressionConfig.EvaluateHP(t),
                                                 progressionConfig.EvaluateHPSpreadMin(t),
                                                 progressionConfig.EvaluateHPSpreadMax(t));
        }

        private int GetRandomDamageFromProgression(NPCProgressionConfig progressionConfig, float t)
        {
            return GetRandomValueFromProgression(progressionConfig.EvaluateDamage(t),
                                                 progressionConfig.EvaluateDamageSpreadMin(t),
                                                 progressionConfig.EvaluateHPSpreadMax(t));
        }

        private int GetRandomValueFromProgression(float baseHP, float spreadMin, float spreadMax)
        {
            return (int)Random.Range(baseHP - spreadMin, baseHP + spreadMax);
        }

        private GridCellData GetRandomCell(ref List<GridCellData> emptyCells)
        {
            int rndIndex = Random.Range(0, emptyCells.Count);
            GridCellData rndCell = emptyCells[rndIndex];
            emptyCells.RemoveAt(rndIndex);

            return rndCell;
        }
    }
}