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
using RhytmFighter.Data.Models.DataTableModels;

namespace RhytmFighter.Level.Data
{
    public class RoomDataBuilder
    {
        private int m_ITEM_ID = 1;
        private int m_ENEMY_ID = 2;


        public LevelRoomData Build(LevelNodeData node, EnvironmentDataModel.BuildData buildData, EnvironmentDataModel.ContentData contentData, float completionProgress)
        {
            Random.InitState(node.NodeSeed);

            int width = GetRandomWidthFromProgression(buildData.LevelProgressionConfig, completionProgress);
            int height = GetRandomHeightFromProgression(buildData.LevelProgressionConfig, completionProgress);

            SquareGrid roomGrid = new SquareGrid(width, height, buildData.CellSize, Vector2.zero);
            ApplyDataToGrid(roomGrid, node, buildData.ObstacleFillPercent, completionProgress, contentData);

            return new LevelRoomData(roomGrid, node);
        }


        void ApplyDataToGrid(SquareGrid grid, LevelNodeData node, int obstacleFillPercent, float completionProgress, EnvironmentDataModel.ContentData contentData)
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

        void GenerateContent(SquareGrid grid, LevelNodeData node, ref List<GridCellData> emptyCells, EnvironmentDataModel.ContentData contentData, float completionProgress)
        {
            GenerateItem(node, contentData, ref emptyCells, completionProgress);
            GenerateEnemy(node, ref emptyCells, contentData, completionProgress);
        }

        void GenerateItem(LevelNodeData node, EnvironmentDataModel.ContentData contentData,ref List<GridCellData> emptyCells, float completionProgress)
        {
            if (emptyCells.Count == 0 || node.IsFinishNode)
                return;

            int amountOfItems = GetRandomAmountFromProgression(contentData.ItemProgressionConfig, completionProgress);

            for (int i = 0; i < amountOfItems; i++)
            {
                GridCellData rndCell = GetRandomCell(ref emptyCells);

                int rndItemContentID = GetRandomIDFromProgression(contentData.ItemProgressionConfig, completionProgress);

                Debug.Log($"Create item. ContnentID {rndItemContentID}. CompletionProgress {completionProgress}");

                StandardItemModel item = new StandardItemModel(m_ITEM_ID++, rndItemContentID, rndCell);
                rndCell.AddObject(item);
            }
        }

        void GenerateEnemy(LevelNodeData node, ref List<GridCellData> emptyCells, EnvironmentDataModel.ContentData contentData, float completionProgress)
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
                    int dmg = GetDamageFromProgression(contentData.EnemyDataProgressionConfig, completionProgress);
                    int expForDestroy = GetRandomExperianceForEnemyDestroyFromProgression(contentData.EnemyDataProgressionConfig, completionProgress);

                    Debug.Log($"Create enemy. ViewID {rndViewID}. HP {rndHP} and Damage {dmg}. Exp {expForDestroy}. CompletionProgress {completionProgress}");

                    StandardEnemyNPCModel enemyNPC = new StandardEnemyNPCModel(m_ENEMY_ID++, rndViewID, rndCell, BattleManager.Instance.NPCMoveSpeed, 
                                                                               new SimpleBattleActionBehaviour(dmg),
                                                                               new SimpleHealthBehaviour(rndHP),
                                                                               AITypes.Simple, expForDestroy);

                    rndCell.AddObject(enemyNPC);
                }
            }
            else if (node.IsFinishNode)
            {
                GridCellData rndCell = GetRandomCell(ref emptyCells);

                int rndViewID = GetRandomIDFromProgression(contentData.EnemyViewProgressionConfig, completionProgress);
                int rndHP = GetRandomHPFromProgression(contentData.BossDataProgressionConfig, completionProgress);
                int dmg = GetDamageFromProgression(contentData.BossDataProgressionConfig, completionProgress);
                int expForDestroy = GetRandomExperianceForEnemyDestroyFromProgression(contentData.BossDataProgressionConfig, completionProgress);

                Debug.Log($"Create enemy. ViewID {rndViewID}. HP {rndHP} and Damage {dmg}. Exp {expForDestroy}. CompletionProgress {completionProgress}");

                StandardEnemyNPCModel enemyNPC = new StandardEnemyNPCModel(m_ENEMY_ID++, rndViewID, rndCell, BattleManager.Instance.NPCMoveSpeed, 
                                                                           new SimpleBattleActionBehaviour(dmg),
                                                                           new SimpleHealthBehaviour(rndHP),
                                                                           AITypes.SimpleDefencible, expForDestroy);

                rndCell.AddObject(enemyNPC);
            }
        }

        private int GetRandomExperianceForEnemyDestroyFromProgression(NPCProgressionConfig progressionConfig, float t)
        {
            int rawExp = progressionConfig.EvaluateExp(t);
            (int min, int max) spreadPercent = progressionConfig.EvaluateExpSpread(t);

            int minSpread = Mathf.RoundToInt(rawExp * (spreadPercent.min / 100f));
            int maxSpread = Mathf.RoundToInt(rawExp * (spreadPercent.max / 100f));
            int expMin = rawExp - minSpread;
            int expMax = rawExp + maxSpread;

            int exp = Random.Range(expMin, expMax);

            return Mathf.Clamp(exp, 1, exp);
        }

        private int GetRandomWidthFromProgression(LevelSizeProgressionConfig progressionConfig, float t)
        {
            (int min, int max) result = progressionConfig.EvaluateWidth(t);
            return Random.Range(result.min, result.max + 1);
        }

        private int GetRandomHeightFromProgression(LevelSizeProgressionConfig progressionConfig, float t)
        {
            (int min, int max) result = progressionConfig.EvaluateHeight(t);
            return Random.Range(result.min, result.max + 1);
        }

        private int GetRandomAmountFromProgression(ObjectProgressionConfig progressionConfig, float t)
        {
            (int min, int max) result = progressionConfig.EvaluateAmount(t);
            return Random.Range(result.min, result.max + 1);
        }

        private int GetRandomIDFromProgression(ObjectProgressionConfig progressionConfig, float t)
        {
            int[] ids = progressionConfig.EvaluateViewIDs(t);
            return ids[Random.Range(0, ids.Length)];
        }

        private int GetRandomHPFromProgression(NPCProgressionConfig progressionConfig, float t)
        {
            int rawHP = progressionConfig.EvaluateHP(t);
            (int min, int max) spreadPercent = progressionConfig.EvaluateHPSpread(t);

            int minSpread = Mathf.RoundToInt(rawHP * (spreadPercent.min / 100f));
            int maxSpread = Mathf.RoundToInt(rawHP * (spreadPercent.max / 100f));
            int hpMin = rawHP - minSpread;
            int hpMax = rawHP + maxSpread;

            int hp = Random.Range(hpMin, hpMax);

            return Mathf.Clamp(hp, 1, hp);
        }

        private int GetDamageFromProgression(NPCProgressionConfig progressionConfig, float t)
        {
            return progressionConfig.EvaluateDamage(t);
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