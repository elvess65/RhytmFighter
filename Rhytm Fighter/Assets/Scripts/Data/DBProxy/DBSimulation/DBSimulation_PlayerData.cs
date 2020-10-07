using RhytmFighter.Persistant.Enums;
using UnityEngine;

namespace RhytmFighter.Data.DataBase.Simulation
{
    [CreateAssetMenu(fileName = "New Simulation_PlayerData", menuName = "DBSimulation/PlayerData", order = 101)]
    public class DBSimulation_PlayerData : ScriptableObject
    {
        public int CurrentLevelID;
        //public int ActionPoints = 3;
        //public int TickToRestoreActionPoint = 4;

        [Header("Selected character")]
        public CharacterData Character;

        [Header("Inventory")]
        public InventoryData Inventory;


        [System.Serializable]
        public class CharacterData
        {
            [Tooltip("Character ID")]
            public int CharacterID;

            [Space(10)]

            [Tooltip("Current character exp")]
            public int CharacterExp = 0;

            [Tooltip("Character HP")]
            public int HP = 2;

            [Tooltip("Character Max HP")]
            public int MaxHP = 3;

            [Tooltip("Character Damage")]
            public int Damage = 3;
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
