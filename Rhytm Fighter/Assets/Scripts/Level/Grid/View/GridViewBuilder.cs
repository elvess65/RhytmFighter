using Frameworks.Grid.Data;
using Frameworks.Grid.View.Cell;
using RhytmFighter.Assets;
using RhytmFighter.Core;
using RhytmFighter.Core.Enums;
using RhytmFighter.Level.Data;
using RhytmFighter.Objects.Model;
using System.Collections.Generic;
using UnityEngine;

namespace Frameworks.Grid.View
{
    public class GridViewBuilder 
    {
        public System.Action<AbstractGridObjectModel> OnCellWithObjectDetected;

        private float m_CellOffset => 1f;

        private WaitForSeconds m_ExtendViewWait;
        private Dictionary<int, GridViewData> m_GridViews;  //room id : views[,]

        private const int m_ITERATIONS = 3;
        private const float m_EXTEND_VIEW_DELAY = 0.03f;

        public GridViewBuilder()
        {
            m_GridViews = new Dictionary<int, GridViewData>();
            m_ExtendViewWait = new WaitForSeconds(m_EXTEND_VIEW_DELAY);
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
        public void RemoveRoomView(LevelRoomData roomData)
        {
            if (m_GridViews.ContainsKey(roomData.ID))
            {
                for (int i = 0; i < roomData.GridData.WidthInCells; i++)
                {
                    for (int j = 0; j < roomData.GridData.HeightInCells; j++)
                    {
                        CellView cellView = GetCellVisual(roomData.ID, i, j);
                        if (cellView.IsShowed)
                            cellView.HideCell(false);
                    }
                }

                //Remove parent - remove all grids
                MonoBehaviour.Destroy(m_GridViews[roomData.ID].GridParent.gameObject, 2);

                //Remove data
                m_GridViews.Remove(roomData.ID);
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
        /// Hide all cells of the room with exception and with ignore visited options (not ignore visited by default)
        /// </summary>
        public void HideCells(LevelRoomData roomData, bool ignoreVisited = false, GridCellData exceptionalCell = null, bool hideImmediate = false)
        {
            for (int i = 0; i < roomData.GridData.WidthInCells; i++)
            {
                for (int j = 0; j < roomData.GridData.HeightInCells; j++)
                {
                    //Get cell view
                    CellView cellView = GetCellVisual(roomData.ID, i, j);

                    //If cell view is not exceptional and if should ignore visited and current cell is not visited - hide
                    if (!cellView.CorrespondingCellData.IsEqualCoord(exceptionalCell) && (ignoreVisited || !cellView.CorrespondingCellData.IsDiscovered))
                        cellView.HideCell(hideImmediate);
                }
            }
        }

        /// <summary>
        /// Show cells (Field of view)
        /// </summary>
        public void ExtendView(LevelRoomData roomData, GridCellData anchorCellData)
        {
            GameManager.Instance.StartCoroutine(ShowCellsWithDelay(roomData.ID, roomData.GridData.GetFOVCells(anchorCellData)));
        }

        /// <summary>
        /// Show all discovered cells
        /// </summary>
        public void ShowAllDiscoveredCells(LevelRoomData roomData)
        {
            if (m_GridViews.ContainsKey(roomData.ID))
                GameManager.Instance.StartCoroutine(ShowAllDiscoveredCellsWithDelay(roomData));
        }


        /// <summary>
        /// Get start position for next grid view
        /// </summary>
        public Vector3 GetStartPositionForNextView(GridCellData gateCellData, int inputNodeX)
        {
            CellView gateCellView = GetCellVisual(gateCellData.CorrespondingRoomID, gateCellData.X, gateCellData.Y);
            return new Vector3(gateCellView.transform.position.x - inputNodeX * m_CellOffset - m_CellOffset / 2, 0, gateCellView.transform.position.z + m_CellOffset / 2 + m_CellOffset * 2);
        }

        /// <summary>
        /// Get start position for parent grid view
        /// </summary>
        public Vector3 GetStartPositionForParentView(GridCellData parentCellData, GridCellData gateCellData, int nodeDirectionOffset)
        {
            CellView gateCellView = GetCellVisual(parentCellData.CorrespondingRoomID, parentCellData.X, parentCellData.Y);
            return new Vector3(gateCellView.transform.position.x + gateCellData.X * m_CellOffset * nodeDirectionOffset - m_CellOffset / 2, 0, gateCellView.transform.position.z - gateCellData.Y * m_CellOffset - m_CellOffset - m_CellOffset / 2 - m_CellOffset * 2);
        }


        /// <summary>
        /// Debug
        /// </summary>
        public void ShowAllCellsWithObjects_Debug(LevelRoomData roomData)
        {
            for (int i = 0; i < roomData.GridData.WidthInCells; i++)
            {
                for (int j = 0; j < roomData.GridData.HeightInCells; j++)
                {
                    //Get cell view
                    CellView cellView = GetCellVisual(roomData.ID, i, j);
                    if (cellView.CorrespondingCellData.HasObject)
                        ShowCell_Debug(cellView);
                }
            }
        }

        /// <summary>
        /// Debug
        /// </summary>
        public void ShowCell_Debug(CellView cellView, PrimitiveType type = PrimitiveType.Cube)
        {
            GameObject.CreatePrimitive(type).transform.position = cellView.transform.position;
        }


        private void CellWithObjectDetectedHandler(AbstractGridObjectModel objectInCell)
        {
            OnCellWithObjectDetected?.Invoke(objectInCell);
        }

        private CellView CreateCellView(GridCellData cellData, Vector3 startPos, Transform gridParent)
        {
            //Create view
            Vector3 viewPos = new Vector3(startPos.x + m_CellOffset / 2 + cellData.X * m_CellOffset, 0, startPos.z + m_CellOffset / 2 + cellData.Y * m_CellOffset);
            CellView cellView = AssetsManager.GetPrefabAssets().InstantiatePrefab<CellView>(AssetsManager.GetPrefabAssets().CellView_Prefab, viewPos);
            cellView.transform.parent = gridParent;
            cellView.gameObject.name = $"Cell_(X - {cellData.X}. Y - {cellData.Y})";

            //Create content 
            Abstract_CellContentView cellContent = GetCellContentPrefab(cellData);

            //Initialize view
            cellView.Initialize(cellData, cellContent);
            cellView.OnObjectDetected += CellWithObjectDetectedHandler;

            return cellView;
        }

        private Abstract_CellContentView GetCellContentPrefab(GridCellData cellData)
        {
            Abstract_CellContentView cellContent = null;

            if (cellData.HasProperty)
            {
                switch (cellData.CellProperty)
                {
                    case GridCellProperty_GateToNode gatesToNodeProperty:
                        {
                            switch (gatesToNodeProperty.GateType)
                            {
                                case GateTypes.ToParentNode:
                                    cellContent = AssetsManager.GetPrefabAssets().InstantiatePrefab(AssetsManager.GetPrefabAssets().CellContent_Gate_Parent_Prefab);
                                    break;

                                case GateTypes.ToNextNode:
                                    cellContent = AssetsManager.GetPrefabAssets().InstantiatePrefab(AssetsManager.GetPrefabAssets().CellContent_Gate_Next_Prefab);
                                    break;
                            }
                        }
                        break;
                }
            }
            else
                cellContent = AssetsManager.GetPrefabAssets().InstantiatePrefab(AssetsManager.GetPrefabAssets().GetRandomCellContent(cellData.CellType));

            return cellContent;
        }

        private System.Collections.IEnumerator ShowCellsWithDelay(int roomDataID, GridCellData[] cells)
        {
            foreach (GridCellData cellData in cells)
            {
                //Skip showed cell
                if (cellData.IsShowed)
                    continue;

                CellView cellView = GetCellVisual(roomDataID, cellData.X, cellData.Y);
                cellView.ShowCell();

                yield return m_ExtendViewWait;
            }
        }

        private System.Collections.IEnumerator ShowAllDiscoveredCellsWithDelay(LevelRoomData roomData)
        {
            for (int i = 0; i < roomData.GridData.WidthInCells; i++)
            {
                for (int j = 0; j < roomData.GridData.HeightInCells; j++)
                {
                    CellView cellView = GetCellVisual(roomData.ID, i, j);

                    //Skip not discovered cell
                    if (!cellView.CorrespondingCellData.IsDiscovered)
                        continue;

                    cellView.ShowCell();

                    yield return null;
                }
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
