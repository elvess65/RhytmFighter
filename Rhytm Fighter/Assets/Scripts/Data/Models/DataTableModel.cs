using RhytmFighter.Data.Models.DataTableModels;

namespace RhytmFighter.Data.Models
{
    /// <summary>
    /// Модель содержащая информационные таблицы и конфигурации
    /// </summary>
    public class DataTableModel 
    {
        public EnvironmentDataModel EnvironmentDataModel { get; }
        public LevelingDataModel LevelingDataModel { get; }

        public DataTableModel(string serializedEnviromentData, string serializedLevelingData)
        {
            EnvironmentDataModel = EnvironmentDataModel.DeserializeData(serializedEnviromentData);
            LevelingDataModel = LevelingDataModel.DeserializeData(serializedLevelingData);
        }

        public void ReorganizeData()
        {
            EnvironmentDataModel.ReorganizeData();
            LevelingDataModel.ReorganizeData();
        }
    }
}
