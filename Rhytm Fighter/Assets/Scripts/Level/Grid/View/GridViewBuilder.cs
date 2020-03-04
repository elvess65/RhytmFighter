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
        private Dictionary<int, GridViewData> m_GridViews;

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
            gridParent.gameObject.name = "Grid Parent " + roomData.NodeData.ID;
            gridParent.transform.position = startPos;

            //Create grid
            CellView[,] cellViews = new CellView[roomData.GridData.WidthInCells, roomData.GridData.HeightInCells];

            //Create views
            for (int i = 0; i < roomData.GridData.WidthInCells; i++)
            {
                for (int j = 0; j < roomData.GridData.HeightInCells; j++)
                {
                    GridCellData cellData = roomData.GridData.GetCellByCoord(i, j);

                    //Create view
                    CellView cellView = CreateCellView(cellData, startPos, gridParent);

                    //Add view to array
                    cellViews[i, j] = cellView;
                }
            }

            //Add created data to views
            if (!m_GridViews.ContainsKey(roomData.NodeData.ID))
                m_GridViews.Add(roomData.NodeData.ID, new GridViewData(roomData.NodeData.ID, gridParent, cellViews));
        }

        /// <summary>
        /// Remove room graphics
        /// </summary>
        /// <param name="roomID"></param>
        public void RemoveRoom(int roomID)
        {
            if (m_GridViews.ContainsKey(roomID))
            {
                MonoBehaviour.Destroy(m_GridViews[roomID].GetCellVisual(0, 0).transform.parent.gameObject);
                m_GridViews.Remove(roomID);
            }
        }

        /// <summary>
        /// Get cell graphics
        /// </summary>
        /// <returns></returns>
        public CellView GetCellVisual(int id, int x, int y)
        {
            if (m_GridViews.ContainsKey(id))
                return m_GridViews[id].GetCellVisual(x, y);

            return null;
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
    /// Grid visual data (can exists multiple grid visuals at a time)
    /// </summary>
    public class GridViewData
    {
        private int m_ID;
        private Transform m_GridParent;
        private CellView[,] m_CellViews;

        public GridViewData(int id, Transform gridParent, CellView[,] cellViews)
        {
            m_ID = id;
            m_GridParent = gridParent;
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
