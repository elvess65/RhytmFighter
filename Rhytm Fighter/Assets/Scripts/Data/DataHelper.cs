using RhytmFighter.Persistant;
using static RhytmFighter.Data.Models.AccountModel;

namespace RhytmFighter.Data
{
    /// <summary>
    /// Помошник для получения данных из разных источников
    /// </summary>
    public static class DataHelper
    {
        public static int GetCharacterDamage(int characterID)
        {
            CharacterData characterData = GetCharacterData(characterID);
            int weaponLevel = GameManager.Instance.DataHolder.DataTableModel.LevelingDataModel.
                              GetWeaponLevelByExp(characterID, characterData.WeaponExperiance);

            //TODO: Get weapon damage by weaponLevel
            int weaponDamageByLevel = 1;

            return weaponDamageByLevel;
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
