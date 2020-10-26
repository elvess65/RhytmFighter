using RhytmFighter.Persistant.Enums;

namespace RhytmFighter.Data
{
    /// <summary>
    /// Player data
    /// </summary>
    public class PlayerData : AbstractData<PlayerData>
    {
        public int CurrentLevelID;
        public int[] CompletedLevelsIDs;
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
            public PotionData[] Potions;

            public PotionData GetPotionByType(PotionTypes type)
            {
                foreach(PotionData potionData in Potions)
                {
                    if (potionData.Type == type)
                        return potionData;
                }

                return Potions[0];
            }
        }

        [System.Serializable]
        public class PotionData
        {
            public PotionTypes Type;
            public int PiecesAmount;
            public int PiecesPerPotion;

            public int PotionAmount => PiecesAmount / PiecesPerPotion;
            public bool HasPotions => PotionAmount > 0;

            public void IncrementPieceAmount() => PiecesAmount++;

            public void DecrementPotion() => PiecesAmount = UnityEngine.Mathf.Clamp(PiecesAmount - PiecesPerPotion, 0, PiecesAmount);
        }

        //Skills
    }
}
