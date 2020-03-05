﻿using RhytmFighter.Data.DataBase;

namespace RhytmFighter.Data
{
    /// <summary>
    /// Holder for data related objects
    /// </summary>
    [System.Serializable]
    public class DataHolder
    {
        public DBProxy DBProxy;

        public InfoData InfoData;
        public PlayerData PlayerDataModel;

        public DataHolder()
        {
            DBProxy = new DBProxy();
        }
    }
}
