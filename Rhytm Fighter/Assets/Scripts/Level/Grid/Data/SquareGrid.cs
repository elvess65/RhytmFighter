using System.Collections.Generic;
using UnityEngine;

namespace Frameworks.Grid.Data
{
    public class SquareGrid
    {
        //Base 
        public int WidthInCells { get; private set; }
        public int HeightInCells { get; private set; }

        //Gates
        public GridCellData ParentNodeGate  { get => m_ParentNodeGate;  set { m_ParentNodeGate = value; } }
        public GridCellData LeftNodeGate    { get => m_LeftNodeGate;    set { m_LeftNodeGate = value;   } }
        public GridCellData RightNodeGate   { get => m_RightNodeGate;   set { m_RightNodeGate = value;  } }

        //Base
        private float m_CellSize;
        private Vector3 m_Offset;
        private GridCellData[,] m_Grid;
        private GridPathFindController m_GridPathFindController;
        //Gates
        private GridCellData m_ParentNodeGate   { get; set; }
        private GridCellData m_LeftNodeGate     { get; set; }
        private GridCellData m_RightNodeGate    { get; set; }

        private const bool m_ALLOW_DIAGONAL_PATHFINDING = false;


        public SquareGrid(int widthInCells, int heightInCells, float cellSize, Vector2 _offset)
        {
            WidthInCells = widthInCells;
            HeightInCells = heightInCells;
            m_CellSize = cellSize;
            m_Offset = new Vector3(_offset.x, 0, _offset.y);

            m_Grid = new GridCellData[WidthInCells, HeightInCells];
            m_GridPathFindController = new GridPathFindController(this, m_ALLOW_DIAGONAL_PATHFINDING);

            for (int i = 0; i < WidthInCells; i++)
            {
                for (int j = 0; j < HeightInCells; j++)
                {
                    GridCellData gridCell = new GridCellData(i, j, m_CellSize, CellTypes.Normal);
                    m_Grid[i, j] = gridCell;
                }
            }
        }

        public GridCellData[] FindPathCells(GridCellData from, GridCellData to)
        {
            //Find path in the same room
            if (from.CorrespondingRoomID == to.CorrespondingRoomID)
            {
                try
                {
                    return m_GridPathFindController.FindPath(from, to).ToArray();
                }
                catch
                {
                    return null;
                }
            }

            //Find path for room transition
            return new GridCellData[] { from, to };
        }

        public Vector3[] FindPath(GridCellData from, GridCellData to)
        {
            List<GridCellData> gridPath = m_GridPathFindController.FindPath(from, to);

            if (gridPath != null)
            {
                //Создать массив позиций, из которых состоит путь
                List<Vector3> pathPos = new List<Vector3>();

                //Перевести из сетки в мировые координаты
                for (int i = 0; i < gridPath.Count; i++)
                    pathPos.Add(GetCellWorldPosByCoord(gridPath[i].X, gridPath[i].Y));

                //Исправление ошибки с путем состоящим из двух точек
                if (pathPos.Count == 2)
                    pathPos.Insert(1, (pathPos[0] + pathPos[1]) / 2);

                return pathPos.ToArray();
            }

            return null;
        }


        /// <summary>
        /// Получить ячейку по координатам сетки
        /// </summary>
        public GridCellData GetCellByCoord(int x, int y)
        {
            if (CoordIsOnGrid(x, y))
                return m_Grid[x, y];

            return null;
        }


        /// <summary>
        /// Получить расположение ячейки по координатам сетки
        /// </summary>
        public Vector3 GetCellWorldPosByCoord(int x, int y)
        {
            Vector3 pos = new Vector3(x, 0, y) * m_CellSize + m_Offset;
            pos.x += m_CellSize / 2;
            pos.z += m_CellSize / 2;

            return pos;
        }

        /// <summary>
        /// Получить координаты клетки по расположению
        /// </summary>
        public (int x, int y) GetCellCoordByWorldPos(Vector3 pos) => (Mathf.FloorToInt((pos - m_Offset).x / m_CellSize), Mathf.FloorToInt((pos - m_Offset).z / m_CellSize));



        /// <summary>
        /// Список координат 4 соседей для указанной координаты
        /// </summary>
        public (int x, int y)[] GetCell4NeighboursCoord(int x, int y)
        {
            //Если координата, для которой нужно получить список соседей не находится в пределах сетки - вернуть пустой список
            if (!CoordIsOnGrid(x, y))
                return new (int x, int y)[0];

            //Список соседних координат относительно указанной
            List<(int x, int y)> cellNeighbours = new List<(int x, int y)>();

            //Добавить координаты соседних клеток, если они находятся в пределах сетки
            (int x, int y) neighbouCoord = (x - 1, y);
            if (CoordIsOnGrid(neighbouCoord.x, neighbouCoord.y))
                cellNeighbours.Add(neighbouCoord);

            neighbouCoord = (x + 1, y);
            if (CoordIsOnGrid(neighbouCoord.x, neighbouCoord.y))
                cellNeighbours.Add(neighbouCoord);

            neighbouCoord = (x, y - 1);
            if (CoordIsOnGrid(neighbouCoord.x, neighbouCoord.y))
                cellNeighbours.Add(neighbouCoord);

            neighbouCoord = (x, y + 1);
            if (CoordIsOnGrid(neighbouCoord.x, neighbouCoord.y))
                cellNeighbours.Add(neighbouCoord);

            return cellNeighbours.ToArray();
        }

        /// <summary>
        /// Список координат всех соседей для указанной координаты (3*3)
        /// </summary>
        public (int x, int y)[] GetCellNeighboursCoordInRange(int x, int y, int r)
        {
            //Если координата, для которой нужно получить список соседей не находится в пределах сетки - вернуть пустой список
            if (!CoordIsOnGrid(x, y))
                return new (int x, int y)[0];

            //Список соседних координат относительно указанной
            List<(int x, int y)> cellNeighbours = new List<(int x, int y)>();

            for (int i = x - r; i <= x + r; i++)
            {
                for (int j = y - r; j <= y + r; j++)
                {
                    (int x, int y) neighbouCoord = (i, j);
                    if (!(neighbouCoord.x.Equals(x) && neighbouCoord.y.Equals(y)) && CoordIsOnGrid(neighbouCoord.x, neighbouCoord.y))
                            cellNeighbours.Add(neighbouCoord);
                }
            }


            return cellNeighbours.ToArray();
        }

        /// <summary>
        /// Список координат всех доступных для перемещения и показанных соседей для указанной координаты (3*3)
        /// </summary>
        public List<GridCellData> GetCellWalkableAndVisibleNeighboursCoordInRange(int x, int y, int r)
        {
            //Список соседних координат относительно указанной
            List<GridCellData> cellNeighbours = new List<GridCellData>();

            //Если координата, для которой нужно получить список соседей не находится в пределах сетки - вернуть пустой список
            if (!CoordIsOnGrid(x, y))
                return cellNeighbours;

            for (int i = x - r; i <= x + r; i++)
            {
                for (int j = y - r; j <= y + r; j++)
                {
                    GridCellData currecntCell = m_Grid[i, j];
                    if (!(currecntCell.X.Equals(x) && currecntCell.Y.Equals(y)) && CoordIsOnGrid(currecntCell.X, currecntCell.Y) &&
                        CellIsWalkable(m_Grid[currecntCell.X, currecntCell.Y]) && m_Grid[currecntCell.X, currecntCell.Y].IsShowed)
                    {
                        cellNeighbours.Add(currecntCell);
                    }
                }
            }

            return cellNeighbours;
        }

        /// <summary>
        /// Список доступных для перемещения соседей указанной ячейки
        /// </summary>
        public (int x, int y)[] GetWalkableCellNeighboursCoordInRange(int x, int y, int r)
        {
            List<(int x, int y)> cellNeighbours = new List<(int x, int y)>(GetCellNeighboursCoordInRange(x, y, r));
            for (int i = 0; i < cellNeighbours.Count; i++)
            {
                if (CellIsNotWalkable(GetCellByCoord(cellNeighbours[i].x, cellNeighbours[i].y)))
                    cellNeighbours.RemoveAt(i--);
            }

            return cellNeighbours.ToArray();
        }

        /// <summary>
        /// Ближайшая доступная для перемещения ячейка
        /// </summary>
        public GridCellData GetClosestWalkableCell(GridCellData fromCell, GridCellData targetCell, float rangeToFindClosest)
        {
            //If cells are neighbours - do nothing
            float distBetweenCells = GetDistanceBetweenCells(fromCell, targetCell);
            if (distBetweenCells <= rangeToFindClosest)
                return null;

            GridCellData closestCell = null;
            float distToClosestCell = float.MaxValue;

            //Get list of cell neighbours
            (int x, int y)[] targetCellNeighboursCoords = GetCellNeighboursCoordInRange(targetCell.X, targetCell.Y, (int)rangeToFindClosest);

            //Loop through cell neighbours
            foreach ((int x, int y) curNeighbourCellCoord in targetCellNeighboursCoords)
            {
                GridCellData curNeighbourCell = GetCellByCoord(curNeighbourCellCoord.x, curNeighbourCellCoord.y);

                //If neighbour is walkable
                if (!CellIsNotWalkable(curNeighbourCell))
                {
                    //If neigbout is the closest 
                    float distToCell = GetDistanceBetweenCells(curNeighbourCell, fromCell);
                    if (distToCell < distToClosestCell)
                    {
                        distToClosestCell = distToCell;
                        closestCell = curNeighbourCell;
                    }
                }
            }

            return closestCell;
        }

        /// <summary>
        /// Выполнить событие для каждой ячейки
        /// </summary>
        /// <param name="forEachEvent">Событие, которое должно выполняться для каждой ячеки</param>
        public void ForEachCell(System.Action<GridCellData> forEachEvent)
        {
            for (int i = 0; i < WidthInCells; i++)
            {
                for (int j = 0; j < HeightInCells; j++)
                    forEachEvent(m_Grid[i, j]);
            }
        }

        /// <summary>
        /// Расстояние между двумя ячейками
        /// </summary>
        public float GetDistanceBetweenCells(GridCellData a, GridCellData b) => Vector2Int.Distance(a.CoordAsVec2Int, b.CoordAsVec2Int);

        /// <summary>
        /// Находится ли указанная координата в пределах сетки
        /// </summary>
        public bool CoordIsOnGrid(int x, int y) => (x >= 0 && x < WidthInCells) && (y >= 0 && y < HeightInCells);

        /// <summary>
        /// Является ли ячейка недоступной для перемещения
        /// </summary>
        public bool CellIsNotWalkable(GridCellData cell) => cell.CellType != CellTypes.Normal || cell.HasObject;

        /// <summary>
        /// Является ли ячейка доступной для перемещения
        /// </summary>
        public bool CellIsWalkable(GridCellData cell) => !CellIsNotWalkable(cell);

        void CreateDummyObstacle(PrimitiveType type, Vector3 pos)
        {
            GameObject cube = GameObject.CreatePrimitive(type);
            cube.transform.position = pos;
            cube.transform.localScale *= 0.5f;
        }
    }
}