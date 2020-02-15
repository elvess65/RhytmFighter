using RhytmFighter.Data.DataBase;

namespace RhytmFighter.Data
{
    /// <summary>
    /// Holder for data related objects
    /// </summary>
    [System.Serializable]
    public class DataHolders
    {
        public DBProxy DBProxy;

        public InfoData InfoData;
        public PlayerData PlayerData;
        
    }
}
