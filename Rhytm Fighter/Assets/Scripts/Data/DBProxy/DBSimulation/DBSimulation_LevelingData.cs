using UnityEngine;

namespace RhytmFighter.Data.DataBase.Simulation
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Simulation LevelingData", menuName = "DBSimulation/LevelingData", order = 101)]
    public class DBSimulation_LevelingData : ScriptableObject
    {
        [Header("Данные о прогрессии снаряжения героев")]

        [Header("Данные по необходимому опыту для получения уровней")]

        [Tooltip("Данные о прогрессии снаряжения героев")]
        public CharacterEquipementLevelingData[] CharactersEquipementLevelingData;

        [System.Serializable]
        public class CharacterEquipementLevelingData
        {
            [Header("ID героя")]
            public int CharacterID;

            [Header("Прогрессия уровня оружия")]
            public LevelingProgressionConfig WeaponLevelingProgressionConfig;

            [Header("Прогрессия улучшения оружия")]
            public WeaponUpgradeProgressionConfig WeaponUpgradingProgressionConfig;
        }
    }
}
