using UnityEngine;

namespace RhytmFighter.Data.DataBase.Simulation
{
    [CreateAssetMenu(fileName = "New Simulation_PlayerData", menuName = "DBSimulation/PlayerData", order = 101)]
    public class DBSimulation_PlayerData : ScriptableObject
    {
        public int CurrentLevelID;

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
            public int CharacterExp;

            [Tooltip("Character HP")]
            public int HP;

            [Tooltip("Character Damage")]
            public int Damage;

            [Header("First level")]
            [Tooltip("Current character HP at first level start")]
            public int FirstLevelCurrentHP;
        }

        [System.Serializable]
        public class InventoryData
        {
            public int PotionsAmount;
        }
    }
}
