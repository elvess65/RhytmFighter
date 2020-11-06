using RhytmFighter.Data.DataBase;
using RhytmFighter.Data.Models;

namespace RhytmFighter.Data
{
    /// <summary>
    /// Holder for data related objects
    /// </summary>
    [System.Serializable]
    public class ModelsHolder
    {
        public DBProxy DBProxy;

        public DataTableModel DataTableModel;
        public AccountModel AccountModel;
        public BattleSessionModel BattleSessionModel;

        public ModelsHolder()
        {
            DBProxy = new DBProxy();
            BattleSessionModel = new BattleSessionModel();
        }
    }
}
