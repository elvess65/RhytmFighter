﻿using System.Collections.Generic;
using UnityEngine;

namespace Frameworks.Grid.Data
{
    public class GridPathFindController 
    {
        private SquareGrid m_GridController;


        public GridPathFindController(SquareGrid gridController) => m_GridController = gridController;
    

        public List<GridCellData> FindPath(GridCellData startNode, GridCellData targetNode)
        {
            //Create open and close sets
            List<GridCellData> openSet = new List<GridCellData>();
            HashSet<GridCellData> closedSet = new HashSet<GridCellData>();

            //Add start node to open set
            openSet.Add(startNode);
            while (openSet.Count > 0)
            {
                //Cur node is the node with the lowest FCost
                GridCellData curNode = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].FCost < curNode.FCost || openSet[i].FCost == curNode.FCost && openSet[i].HCost < curNode.HCost)
                        curNode = openSet[i];
                }

                //Remove cur form open
                openSet.Remove(curNode);

                //Add cur to closed set
                closedSet.Add(curNode);

                //if cur note is the target node
                if (curNode == targetNode)
                    return RetracePath(startNode, targetNode);

                //Get neighbours
                (int x, int y)[] neighbours = m_GridController.GetCell4NeighboursCoord(curNode.X, curNode.Y);
                for (int i = 0; i < neighbours.Length; i++)
                {
                    GridCellData neighbourNode = m_GridController.GetCellByCoord(neighbours[i].x, neighbours[i].y);

                    bool cellTypeIsIgnorable = m_GridController.CellIsNotWalkable(neighbourNode);

                    //if neighbour is not walkable or is in closed set - skip
                    if (cellTypeIsIgnorable || closedSet.Contains(neighbourNode))
                        continue;

                    int newGCostToNeighbour = curNode.GCost + GetDistanceBetweenCells(curNode, neighbourNode);
                    if (newGCostToNeighbour < neighbourNode.GCost || !openSet.Contains(neighbourNode))
                    {
                        neighbourNode.GCost = newGCostToNeighbour;
                        neighbourNode.HCost = GetDistanceBetweenCells(neighbourNode, targetNode);

                        neighbourNode.ParentNodeData = curNode;

                        if (!openSet.Contains(neighbourNode))
                            openSet.Add(neighbourNode);
                    }
                }
            }

            return null;
        }


        List<GridCellData> RetracePath(GridCellData startNode, GridCellData targetNode)
        {
            List<GridCellData> path = new List<GridCellData>();

            GridCellData curNode = targetNode;
            while (curNode != startNode)
            {
                path.Add(curNode);
                curNode = curNode.ParentNodeData;
            }

            path.Add(startNode);
            path.Reverse();

            return path;
        }

        int GetDistanceBetweenCells(GridCellData a, GridCellData b)
        {
            Vector2Int aCoord = a.CoordAsVec2Int;
            Vector2Int bCoord = b.CoordAsVec2Int;

            int distX = Mathf.Abs(aCoord.x - bCoord.x);
            int distY = Mathf.Abs(aCoord.y - bCoord.y);

            if (distX > distY)
                return 10 * distY + 10 * (distX - distY);

            return 10 * distX + 10 * (distY - distX);
        }
    }
}
