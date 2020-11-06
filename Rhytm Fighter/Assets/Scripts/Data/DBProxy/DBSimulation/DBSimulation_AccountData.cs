using RhytmFighter.Persistant.Enums;
using UnityEngine;

namespace RhytmFighter.Data.DataBase.Simulation
{
    [CreateAssetMenu(fileName = "New Simulation AccountData", menuName = "DBSimulation/AccountData", order = 101)]
    public class DBSimulation_AccountData : ScriptableObject
    {
        public int CurrencyAmount;

        [Header("Characters")]
        public CharacterData[] CharactersData;

        [Header("Inventory")]
        public InventoryData Inventory;


        [System.Serializable]
        public class CharacterData
        {
            [Tooltip("Character ID")]
            public int ID;

            [Tooltip("Опыт оружия")]
            public int WeaponExperiance;

            [Tooltip("Character HP")]
            public int HP = 2;

            [Tooltip("Character Max HP")]
            public int MaxHP = 3;
        }

        [System.Serializable]
        public class InventoryData
        {
            public PotionData[] Potions;
        }

        /// <summary>
        /// Данные о зельях (Для одного зелья нужно собрать несколько осколков)
        /// </summary>
        [System.Serializable]
        public class PotionData
        {
            [Tooltip("Тип зелья")]
            public PotionTypes Type;

            [Tooltip("Текущее количество кусков")]
            public int PiecesAmount;

            [Tooltip("Количество необходимых кусков для одного зелья")]
            public int PiecesPerPotion;
        }
    }
}
