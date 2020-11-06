using System.Collections.Generic;

namespace RhytmFighter.Data.Models.DataTableModels
{
    /// <summary>
    /// Информация об опыте и уровнях 
    /// </summary>
    public class LevelingDataModel : DeserializableDataModel<LevelingDataModel>
    {
        public CharacterEquipementLevelingData[] CharactersEquipementLevelingData;

        private Dictionary<int, CharacterEquipementLevelingData> m_CharactersEquipement;


        public override void ReorganizeData()
        {
            m_CharactersEquipement = new Dictionary<int, CharacterEquipementLevelingData>();

            foreach (CharacterEquipementLevelingData levelingData in CharactersEquipementLevelingData)
                m_CharactersEquipement[levelingData.CharacterID] = levelingData;
        }



        public int GetWeaponLevelByExp(int characterID, int expAmount)
        {
            return GetEquipementLevelingData(characterID).WeaponLevelingProgressionConfig.EvaluateLevel(expAmount);
        }

        public int GetWeaponExpForLevel(int characterID, int level)
        {
            return GetEquipementLevelingData(characterID).WeaponLevelingProgressionConfig.EvaluateExpForLevel(level); 
        }


        private CharacterEquipementLevelingData GetEquipementLevelingData(int characterID)
        {
            if (m_CharactersEquipement.ContainsKey(characterID))
                return m_CharactersEquipement[characterID];

            return null;
        }

        [System.Serializable]
        public class CharacterEquipementLevelingData
        {
            public int CharacterID;
            public LevelingProgressionConfig WeaponLevelingProgressionConfig;
        }
    }
}
