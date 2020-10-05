using RhytmFighter.Persistant.Enums;
using UnityEngine;

namespace RhytmFighter.Enviroment.Presets
{
    [CreateAssetMenu(fileName = "New BattleEnviromentPreset", menuName = "Assets/Presets/Enviroment", order = 101)]
    public class BattleEnviromentPreset : ScriptableObject
    {
        [Header("Ground")]
        public Material EnviromentNormalSource;
        public Material EnviromentLightSource;
        public Material EnviromentDarkSource;
        public Material ObstaclesSource;

        [Header("Content")]
        public ContentMaterial[] ContentSources;

        [Header("Other")]
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

                ContentSources = new ContentMaterial[source.ContentSources.Length];
                for (int i = 0; i < ContentSources.Length; i++)
                {
                    ContentSources[i].Initialize(source.ContentSources[i]);
                }
            }
            else
            {
                EnviromentNormalSource = source.EnviromentNormalSource;
                EnviromentLightSource = source.EnviromentLightSource;
                EnviromentDarkSource = source.EnviromentDarkSource;
                ObstaclesSource = source.ObstaclesSource;
                SkyboxSource = source.SkyboxSource;
                ContentSources = source.ContentSources;
            }
        }

        [System.Serializable]
        public class ContentMaterial
        {
            public ContentRendererTypes Type;
            public Material MaterialSource;

            public void Initialize(ContentMaterial source)
            {
                MaterialSource = new Material(source.MaterialSource);
                Type = source.Type;
            }
        }
    }
}
