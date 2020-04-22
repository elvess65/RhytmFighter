using RhytmFighter.Core.Enums;
using RhytmFighter.Objects.Model;
using UnityEngine;

namespace Frameworks.Grid.Data
{
    public enum CellTypes { Normal, Obstacle }

    public class GridCellData
    {
        //Base
        public int X { get; private set; }
        public int Y { get; private set; }
        public float CellSize { get; private set; }
        public CellTypes CellType { get; private set; }
        public GridCellProperty CellProperty { get; private set; }
        public bool HasProperty => CellProperty != null;
        public int CorrespondingRoomID { get; private set; }
        public bool IsVisited { get; set; }
        public bool IsShowed { get; set; }

        //Object
        public bool HasObject => m_Object != null;

        //Pathfinding
        public int GCost;
        public int HCost;
        public int FCost { get { return GCost + HCost; } }
        public Vector2Int CoordAsVec2Int => new Vector2Int(X, Y);
        public GridCellData ParentNodeData;

        private AbstractGridObjectModel m_Object;

        public GridCellData(int xCoord, int yCoord, float cellSize, CellTypes type)
        {
            X = xCoord;
            Y = yCoord;
            CellSize = cellSize;

            IsVisited = false;

            SetCellType(type);
        }


        public void AddObject(AbstractGridObjectModel obj) => m_Object = obj;

        public AbstractGridObjectModel GetObject() => m_Object;

        public void RemoveObject() => m_Object = null;


        public void SetCellType(CellTypes type) => CellType = type;

        public void SetRoomID(int roomID) => CorrespondingRoomID = roomID;

        public void SetCellProperty(GridCellProperty cellProperty) => CellProperty = cellProperty;

        public bool IsEqualCoord(GridCellData otherCell)
        {
            if (otherCell == null)
                return false;

            return X == otherCell.X && Y == otherCell.Y;
        }


        public override string ToString() => $"(x: {X}. y: {Y})";
    }


    public abstract class GridCellProperty
    { }

    public class GridCellProperty_GateToNode : GridCellProperty
    {
        public int LinkedNodeID { get; private set; }
        public GateTypes GateType { get; private set; }

        public GridCellProperty_GateToNode(int nodeID, GateTypes gateToNodeType)
        {
            LinkedNodeID = nodeID;
            GateType = gateToNodeType;
        }

        public static GridCellProperty_GateToNode CreateProperty(int nodeID, GateTypes propertyType) => new GridCellProperty_GateToNode(nodeID, propertyType);
    }
}
