using RhytmFighter.Level.Data;
using System.Collections.Generic;

namespace RhytmFighter.Level
{
    /// <summary>
    /// Model to store level related data
    /// </summary>
    public class LevelDataModel
    {
        /// <summary>
        /// Start Node. Sets on level generation and doesnt change during the level
        /// </summary>
        public LevelNodeData StartNodeData;

        /// <summary>
        /// Current room data id. Sets when player changes the room
        /// </summary>
        public int CurrentRoomID { get; private set; }

        private Dictionary<int, LevelRoomData> m_ActiveRooms;


        public LevelDataModel()
        {
            m_ActiveRooms = new Dictionary<int, LevelRoomData>();
        }


        /// <summary>
        /// Add room data to active rooms
        /// </summary>
        /// <param name="roomData">Data</param>
        /// <param name="isCurrent">Weether this room should be current</param>
        public void AddRoom(LevelRoomData roomData)
        {
            if (!m_ActiveRooms.ContainsKey(roomData.ID))
                m_ActiveRooms.Add(roomData.ID, roomData);
        }

        /// <summary>
        /// Remove room data from active rooms
        /// </summary>
        public void RemoveRoom(int id)
        {
            if (m_ActiveRooms.ContainsKey(id))
                m_ActiveRooms.Remove(id);
        }

        /// <summary>
        /// Set room as current
        /// </summary>
        public void SetRoomAsCurrent(int id) => CurrentRoomID = id;

        /// <summary>
        /// Get room daya by id
        /// </summary>
        public LevelRoomData GetRoomDataByID(int id)
        {
            if (m_ActiveRooms.ContainsKey(id))
                return m_ActiveRooms[id];

            return null;
        }

        /// <summary>
        /// Get current room data
        /// </summary>
        public LevelRoomData GetCurrenRoomData() => GetRoomDataByID(CurrentRoomID);
    }
}
