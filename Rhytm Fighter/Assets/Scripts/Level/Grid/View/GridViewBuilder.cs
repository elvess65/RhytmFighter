using Frameworks.Grid.Data;
using Frameworks.Grid.View.Cell;
using RhytmFighter.Level.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Frameworks.Grid.View
{
    public class GridViewBuilder 
    {
        public System.Action OnGridVisualAppearAnimationFinished;

        private float m_CellOffset => 1.1f;
        private Dictionary<int, GridViewData> m_GridViews;  //room id : views[,]

        public GridViewBuilder()
        {
            m_GridViews = new Dictionary<int, GridViewData>();
        }


        /// <summary>
        /// Create graphics for grid
        /// </summary>
        public void Build(LevelRoomData roomData, Vector3 startPos)
        {
            //Create parent
            Transform gridParent = new GameObject().transform;
            gridParent.gameObject.name = "Grid Parent " + roomData.ID;
            gridParent.transform.position = startPos;

            //Create grid
            CellView[,] cellViews = new CellView[roomData.GridData.WidthInCells, roomData.GridData.HeightInCells];

            //Create views
            for (int i = 0; i < roomData.GridData.WidthInCells; i++)
            {
                for (int j = 0; j < roomData.GridData.HeightInCells; j++)
                {
                    //CellData
                    GridCellData cellData = roomData.GridData.GetCellByCoord(i, j);

                    //Create view
                    CellView cellView = CreateCellView(cellData, startPos, gridParent);

                    //Add view to array
                    cellViews[i, j] = cellView;
                }
            }

            //Add created data to views
            if (!m_GridViews.ContainsKey(roomData.ID))
                m_GridViews.Add(roomData.ID, new GridViewData(gridParent, cellViews));
        }

        /// <summary>
        /// Remove room graphics
        /// </summary>
        /// <param name="roomID"></param>
        public void RemoveRoom(int roomID)
        {
            if (m_GridViews.ContainsKey(roomID))
            {
                //Remove parent - remove all grids
                MonoBehaviour.Destroy(m_GridViews[roomID].GridParent.gameObject);

                //Remove data
                m_GridViews.Remove(roomID);
            }
        }

        /// <summary>
        /// Get cell graphics
        /// </summary>
        /// <returns></returns>
        public CellView GetCellVisual(int roomID, int x, int y)
        {
            if (m_GridViews.ContainsKey(roomID))
                return m_GridViews[roomID].GetCellVisual(x, y);

            return null;
        }


        /// <summary>
        /// Hide all cells of the room with exception
        /// </summary>
        public void HideAllUnvisitedCells(LevelRoomData roomData, GridCellData exceptionalCell)
        {
            for (int i = 0; i < roomData.GridData.WidthInCells; i++)
            {
                for (int j = 0; j < roomData.GridData.HeightInCells; j++)
                {
                    //Get cell view
                    CellView cellView = GetCellVisual(roomData.ID, i, j);

                    //If cell view is not exceptional - try hide if is not visited
                    if (!cellView.CorrespondingCellData.IsEqualCoord(exceptionalCell) && !cellView.CorrespondingCellData.IsVisited)
                        cellView.HideCell();
                }
            }
        }

        /// <summary>
        /// Show cells (Field of view)
        /// </summary>
        public void ExtendView(LevelRoomData roomData, GridCellData anchorCellData)
        {
            ExtendViewVertical(roomData, anchorCellData, 1);
            ExtendViewVertical(roomData, anchorCellData, -1);
            ExtendViewHorizontal(roomData, anchorCellData, 1);
            ExtendViewHorizontal(roomData, anchorCellData, -1);
        }


        /// <summary>
        /// Get start position for next grid view
        /// </summary>
        /// <param name="gateCellData"></param>
        /// <param name="inputNodeX"></param>
        /// <returns></returns>
        public Vector3 GetStartPositionForNextView(GridCellData gateCellData, int inputNodeX)
        {
            CellView gateCellView = GetCellVisual(gateCellData.CorrespondingRoomID, gateCellData.X, gateCellData.Y);
            return new Vector3(gateCellView.transform.position.x - inputNodeX * m_CellOffset - m_CellOffset / 2, 0, gateCellView.transform.position.z + m_CellOffset / 2);
        }

        /// <summary>
        /// Get start position for parent grid view
        /// </summary>
        /// <returns></returns>
        public Vector3 GetStartPositionForParentView(GridCellData parentCellData, GridCellData gateCellData, int nodeDirectionOffset)
        {
            CellView gateCellView = GetCellVisual(parentCellData.CorrespondingRoomID, parentCellData.X, parentCellData.Y);
            return new Vector3(gateCellView.transform.position.x + gateCellData.X * m_CellOffset * nodeDirectionOffset - m_CellOffset / 2, 0, gateCellView.transform.position.z - gateCellData.Y * m_CellOffset - m_CellOffset - m_CellOffset / 2);
        }


        void ExtendViewVertical(LevelRoomData roomData, GridCellData anchorCellData, int verticalOffset)
        {
            //Move vertical by step until reach obstacle or end of the grid

            CellView cellView = GetCellVisual(roomData.ID, anchorCellData.X, anchorCellData.Y + verticalOffset);
            while (cellView != null && cellView.CorrespondingCellData.CellType != CellTypes.Obstacle)
            {
                //Show cellView
                cellView.ShowCell();

                //Get next cellView
                cellView = GetCellVisual(roomData.ID, cellView.CorrespondingCellData.X, cellView.CorrespondingCellData.Y + verticalOffset);
            }

            //If obstacle was reached - show it also
            if (cellView != null)
                cellView.ShowCell();
        }

        void ExtendViewHorizontal(LevelRoomData roomData, GridCellData anchorCellData, int horizontalOffset)
        {
            //Move horizontal by step until reach obstacle or end of the grid

            CellView cellView = GetCellVisual(roomData.ID, anchorCellData.X + horizontalOffset, anchorCellData.Y);
            while (cellView != null && cellView.CorrespondingCellData.CellType != CellTypes.Obstacle)
            {
                //Moving step horizontal move all possible steps vertical
                ExtendViewVertical(roomData, cellView.CorrespondingCellData, 1);
                ExtendViewVertical(roomData, cellView.CorrespondingCellData, -1);

                //Show cellView
                cellView.ShowCell();

                //Get next cellView
                cellView = GetCellVisual(roomData.ID, cellView.CorrespondingCellData.X + horizontalOffset, cellView.CorrespondingCellData.Y);
            }

            //If obstacle was reached - show it also
            if (cellView != null)
                cellView.ShowCell();
        }


        CellView CreateCellView(GridCellData cellData, Vector3 startPos, Transform gridParent)
        {
            //Create view
            GameObject cellViewObj = new GameObject();
            cellViewObj.transform.position = new Vector3(startPos.x + m_CellOffset / 2 + cellData.X * m_CellOffset, 0, startPos.z + m_CellOffset / 2 + cellData.Y * m_CellOffset);
            cellViewObj.transform.localScale = Vector3.one;
            cellViewObj.transform.parent = gridParent;
            cellViewObj.gameObject.name = $"Cell_(X - {cellData.X}. Y - {cellData.Y})";

            //Create content
            GameObject cellContentObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cellContentObj.transform.localScale = new Vector3(1, cellData.CellType == CellTypes.Normal ? 0.1f : 1, 1);

            Abstract_CellContent cellContent = cellContentObj.AddComponent<DummyCellContent>();

            //Apply properties
            if (cellData.HasProperty)
                ApplyProperty(cellData, cellContentObj);

            //Initialize view
            CellView cellView = cellViewObj.AddComponent<CellView>();
            cellView.Initialize(cellData, cellContent);

            return cellView;
        }

        void ApplyProperty(GridCellData cellData, GameObject cellContentObj)
        {
            switch (cellData.CellProperty)
            {
                case GridCellProperty_GateToNode gatesToNodeProperty:
                    {
                        switch (gatesToNodeProperty.GateType)
                        {
                            case GridCellProperty_GateToNode.GateTypes.ToParentNode:
                                cellContentObj.transform.localScale *= 0.5f;
                                break;

                            case GridCellProperty_GateToNode.GateTypes.ToNextNode:
                                cellContentObj.transform.localScale *= 0.3f;
                                break;
                        }
                    }
                    break;
            }
        }
    }


    /// <summary>
    /// Grid visual data. Represents room grid graphics. (can exists multiple grid visuals at a time)
    /// </summary>
    public class GridViewData
    {
        private CellView[,] m_CellViews;

        public Transform GridParent { get; private set; }

        public GridViewData(Transform gridParent, CellView[,] cellViews)
        {
            GridParent = gridParent;
            m_CellViews = cellViews;
        }

        /// <summary>
        /// Get cell graphics
        /// </summary>
        /// <returns></returns>
        public CellView GetCellVisual(int x, int y)
        {
            if (x >= 0 && x < m_CellViews.GetLength(0) && y >= 0 && y < m_CellViews.GetLength(1))
                return m_CellViews[x, y];

            return null;
        }
    }
}
