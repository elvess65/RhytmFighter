using UnityEngine;

namespace RhytmFighter.Enviroment.Presets
{
    [CreateAssetMenu(fileName = "New BattleEnviromentPreset", menuName = "Assets/Presets/Enviroment", order = 101)]
    public class BattleEnviromentPreset : ScriptableObject
    {
        public Material EnviromentNormalSource;
        public Material EnviromentLightSource;
        public Material EnviromentDarkSource;
        public Material ObstaclesSource;
        public Material SkyboxSource;

        public BattleEnviromentPreset(BattleEnviromentPreset source)
        {
            if (Battle.Core.BattleManager.Instance.ManagersHolder.SettingsManager.GeneralSettings.ClonePresetMaterials)
            {
                EnviromentNormalSource = new Material(source.EnviromentNormalSource);
                EnviromentLightSource = new Material(source.EnviromentLightSource);
                EnviromentDarkSource = new Material(source.EnviromentDarkSource);
                ObstaclesSource = new Material(source.ObstaclesSource);
                SkyboxSource = new Material(source.SkyboxSource);
            }
            else
            {
                EnviromentNormalSource = source.EnviromentNormalSource;
                EnviromentLightSource = source.EnviromentLightSource;
                EnviromentDarkSource = source.EnviromentDarkSource;
                ObstaclesSource = source.ObstaclesSource;
                SkyboxSource = source.SkyboxSource;
            }
        }
    }
}
