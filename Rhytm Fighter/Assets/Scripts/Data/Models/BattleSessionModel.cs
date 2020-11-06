using System.Collections.Generic;

namespace RhytmFighter.Data.Models
{
    /// <summary>
    /// Модель содержащая данные, меняющиеся во время забега по уровню
    /// </summary>
    public class BattleSessionModel 
    {
        public int SelectedCharactedID;         //ID выбранного героя
        public int CurrentLevelID;              //Текущий уровень
        public List<int> CompletedLevelsIDs;    //Пройденные уровни

        public BattleSessionModel()
        {
            CompletedLevelsIDs = new List<int>();
        }
    }
}
