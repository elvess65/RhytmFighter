using Frameworks.Grid.Data;
using RhytmFighter.Level.Data;
using RhytmFighter.Level.Scheme.View;
using UnityEngine;

namespace RhytmFighter.Level.Scheme.Builder
{
    /// <summary>
    /// Build sheme of the room
    /// </summary>
    public class RoomSchemeBuilder : AbstractSchemeBuilder
    {
        private SchemeCellView[,] m_GridViews;

        new private Vector3 m_INIT_POSITION = new Vector3(105, 105, 105);
        private const float m_CELL_OFFSET = 0.1f;


        public SchemeCellView this[int x, int y] => m_GridViews[x, y];

        public override bool HasData => m_GridViews != null;


        public RoomSchemeBuilder() : base()
        {

        }

        public void Build(LevelRoomData roomData)
        {
            m_GridViews = new SchemeCellView[roomData.GridData.WidthInCells, roomData.GridData.HeightInCells];

            for (int i = 0; i < roomData.GridData.WidthInCells; i++)
            {
                for (int j = 0; j < roomData.GridData.HeightInCells; j++)
                {
                    GridCellData cellData = roomData.GridData.GetCellByCoord(i, j);

                    SchemeCellView schemeView = CreateCellSchemeView(cellData);

                    m_GridViews[i, j] = schemeView;
                }
            }
        }

        public override void ShowAllAsNormal()
        {
            if (!HasData)
                return;

            for (int i = 0; i < m_GridViews.GetLength(0); i++)
            {
                for (int j = 0; j < m_GridViews.GetLength(1); j++)
                {
                    if (m_GridViews[i, j] != null)
                        m_GridViews[i, j].ShowAsNormal();
                }
            }
        }

        public override void Dispose()
        {
            //Clear grid
            if (HasData)
            {
                for (int i = 0; i < m_GridViews.GetLength(0); i++)
                {
                    for (int j = 0; j < m_GridViews.GetLength(1); j++)
                    {
                        if (m_GridViews[i, j] != null)
                            Object.DestroyImmediate(m_GridViews[i, j].gameObject);
                    }
                }
                m_GridViews = null;
            }

            //Clear other schemeCellViews
            SchemeCellView[] cellsInScene = Object.FindObjectsOfType<SchemeCellView>();
            if (cellsInScene.Length > 0)
            {
                foreach (SchemeCellView cellScheme in cellsInScene)
                {
                    if (cellScheme != null)
                        Object.DestroyImmediate(cellScheme.gameObject);
                }
            }
        }


        SchemeCellView CreateCellSchemeView(GridCellData cellData)
        {
            Vector3 pos = m_INIT_POSITION + new Vector3(cellData.X + m_CELL_OFFSET, 0, cellData.Y + m_CELL_OFFSET);

            GameObject cellObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            cellObj.transform.position = pos;
            cellObj.transform.localScale = Vector3.one;

            //Apply properties
            if (cellData.HasProperty)
                ApplyProperty(cellData, cellObj);

            SchemeCellView schemeView = cellObj.AddComponent<SchemeCellView>();
            schemeView.Initialize(cellData);

            return schemeView;
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
}
