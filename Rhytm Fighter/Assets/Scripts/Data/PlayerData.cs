namespace RhytmFighter.Data
{
    /// <summary>
    /// Player data
    /// </summary>
    public class PlayerData : AbstractData<PlayerData>
    {
        public int CurrentLevelID;
        //public int ActionPoints = 3;
        //public int TickToRestoreActionPoint = 4;

        public CharacterData Character;
        public InventoryData Inventory;


        [System.Serializable]
        public class CharacterData
        {
            public int CharacterID;
            public int CharacterExp;
            public int HP;
            public int MaxHP;
            public int Damage;
        }

        [System.Serializable]
        public class InventoryData
        {
            public int PotionsAmount;
        }

        //Skills
    }
}
