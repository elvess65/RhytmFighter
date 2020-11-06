using System.Collections.Generic;
using RhytmFighter.Persistant.Enums;

namespace RhytmFighter.Data.Models
{
    /// <summary>
    /// Модель содержащая данные об аккаунте
    /// </summary>
    public class AccountModel : DeserializableDataModel<AccountModel>
    {
        public int CurrencyAmount;          

        //Нужно для парсинга данный
        public CharacterData[] CharactersData;

        //Нужно для парсинга данный
        public InventoryData Inventory;

        private Dictionary<int, CharacterData> m_CharactersData;


        public CharacterData GetCharacterDataByID(int id)
        {
            if (m_CharactersData.ContainsKey(id))
                return m_CharactersData[id];

            return null;
        }

        public override void ReorganizeData()
        {
            m_CharactersData = new Dictionary<int, CharacterData>();

            foreach (CharacterData characterData in CharactersData)
                m_CharactersData[characterData.ID] = characterData;
        }


        [System.Serializable]
        public class CharacterData
        {
            public int ID;
            public int WeaponExperiance;
            public int HP;
            public int MaxHP;
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
    }
}
