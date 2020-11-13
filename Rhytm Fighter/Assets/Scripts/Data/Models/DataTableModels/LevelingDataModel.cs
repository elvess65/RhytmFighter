using System.Collections.Generic;

namespace RhytmFighter.Data.Models.DataTableModels
{
    /// <summary>
    /// Информация об опыте и уровнях 
    /// </summary>
    public class LevelingDataModel : DeserializableDataModel<LevelingDataModel>
    {
        /// <summary>
        /// Используется только для парсинга данных
        /// </summary>
        public CharacterEquipementLevelingData[] CharactersEquipementLevelingData;

        private Dictionary<int, CharacterEquipementLevelingData> m_CharactersEquipement;

        /// <summary>
        /// Используется только для парсинга данных
        /// </summary>
        public override void ReorganizeData()
        {
            m_CharactersEquipement = new Dictionary<int, CharacterEquipementLevelingData>();

            foreach (CharacterEquipementLevelingData levelingData in CharactersEquipementLevelingData)
                m_CharactersEquipement[levelingData.CharacterID] = levelingData;
        }


        /// <summary>
        /// Получить уровень оружия для героя по количеству опыта
        /// </summary>
        public int GetWeaponLevelByExp(int characterID, int expAmount)
        {
            return GetEquipementLevelingData(characterID).WeaponLevelingProgressionConfig.EvaluateLevel(expAmount);
        }

        /// <summary>
        /// Получить количество опыта, необходимое для получения уровня
        /// </summary>
        public int GetWeaponExpForLevel(int characterID, int targetWeaponLevel)
        {
            return GetEquipementLevelingData(characterID).WeaponLevelingProgressionConfig.EvaluateExpForLevel(targetWeaponLevel); 
        }

        /// <summary>
        /// Получить урон оружия указанного уровня для героя 
        /// </summary>
        public (int, int) GetWeaponDamage(int characterID, int weaponLevel)
        {
            CharacterEquipementLevelingData equipementLevelingData = GetEquipementLevelingData(characterID);
            int totalWeaponLevels = equipementLevelingData.WeaponLevelingProgressionConfig.TotalLevels;
            float t = (float)weaponLevel / totalWeaponLevels;

            return equipementLevelingData.WeaponUpgradingProgressionConfig.EvaluateDamage(t);
        }

        /// <summary>
        /// Получить цену покупки единицы опыта оружия указанного уровня для героя
        /// </summary>
        public float GetExperiancePointPrice(int characterID, int weaponLevel)
        {
            CharacterEquipementLevelingData equipementLevelingData = GetEquipementLevelingData(characterID);
            int totalWeaponLevels = equipementLevelingData.WeaponLevelingProgressionConfig.TotalLevels;
            float t = (float)weaponLevel / totalWeaponLevels;

            return equipementLevelingData.WeaponUpgradingProgressionConfig.EvaluateExperincePointPrice(t);
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
            public WeaponUpgradeProgressionConfig WeaponUpgradingProgressionConfig;
        }
    }
}
