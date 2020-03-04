using Frameworks.Grid.Data;

namespace RhytmFighter.Level.Data
{
    /// <summary>
    /// Room data
    /// </summary>
    public class LevelRoomData 
    {
        /// <summary>
        /// Data about room grid
        /// </summary>
        public SquareGrid GridData { get; private set; }

        /// <summary>
        /// Data about node, that corresponds the current room
        /// </summary>
        public LevelNodeData NodeData { get; private set; }

        public LevelRoomData(SquareGrid gridData, LevelNodeData nodeData)
        {
            GridData = gridData;
            NodeData = nodeData;
        }
    }
}
