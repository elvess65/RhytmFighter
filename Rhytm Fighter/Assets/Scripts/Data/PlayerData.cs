namespace RhytmFighter.Data
{
    /// <summary>
    /// Player data
    /// </summary>
    public class PlayerData : AbstractData<PlayerData>
    {
        public bool IsFirstLevel => CurrentLevelID == 1;

        public int CurrentLevelID;
        public int ActionPoints = 3;
        public int TickToRestoreActionPoint = 4;

        public CharacterData Character;
        public InventoryData Inventory;


        [System.Serializable]
        public class CharacterData
        {
            public int CharacterID;
            public int CharacterExp;
            public int HP;
            public int Damage;

            public int FirstLevelCurrentHP;
        }

        [System.Serializable]
        public class InventoryData
        {
            public int PotionsAmount;
        }

        //Skills
    }
}
