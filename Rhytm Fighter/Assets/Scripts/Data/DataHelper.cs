using RhytmFighter.Persistant;
using static RhytmFighter.Data.Models.AccountModel;

namespace RhytmFighter.Data
{
    /// <summary>
    /// Помошник для получения данных из разных источников
    /// </summary>
    public static class DataHelper
    {
        /// <summary>
        /// Текущий урон персонажа
        /// </summary>
        public static (int, int) GetCharacterDamage(int characterID)
        {
            //Данные о персонаже
            CharacterData characterData = GetCharacterData(characterID);

            //Уровень оружия
            int weaponLevel = GameManager.Instance.DataHolder.DataTableModel.LevelingDataModel.
                              GetWeaponLevelByExp(characterID, characterData.WeaponExperiance);

            //Урон
            (int, int) weaponDamage = GameManager.Instance.DataHolder.DataTableModel.LevelingDataModel.
                                      GetWeaponDamage(characterID, weaponLevel);

            return weaponDamage;
        }

        /// <summary>
        /// Текущая стоимость единицы опыта улучшения оружия персонажа
        /// </summary>
        public static float GetWeaponExperiancePointPrice(int characterID)
        {
            //Данные о персонаже
            CharacterData characterData = GetCharacterData(characterID);

            //Уровень оружия
            int weaponLevel = GameManager.Instance.DataHolder.DataTableModel.LevelingDataModel.
                              GetWeaponLevelByExp(characterID, characterData.WeaponExperiance);

            //Цена единицы опыта
            float price = GameManager.Instance.DataHolder.DataTableModel.LevelingDataModel.
                          GetExperiancePointPrice(characterID, weaponLevel);

            return price;
        }

        public static int GetCharacterHP(int characterID)
        {
            return GetCharacterData(characterID).HP;
        }

        public static CharacterData GetCharacterData(int characterID)
        {
            return GameManager.Instance.DataHolder.AccountModel.GetCharacterDataByID(characterID);
        }
    }
}
