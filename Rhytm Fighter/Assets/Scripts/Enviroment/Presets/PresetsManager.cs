using UnityEngine;

namespace RhytmFighter.Enviroment.Presets
{
    public class PresetsManager : MonoBehaviour
    {
        public BattleEnviromentPreset[] BattleEnviromentPresets;

        public BattleEnviromentPreset CurrentPreset { get; private set; }


        public void Initialize()
        {
            CurrentPreset = new BattleEnviromentPreset(BattleEnviromentPresets[Random.Range(0, BattleEnviromentPresets.Length)]);

            RenderSettings.skybox = CurrentPreset.SkyboxSource;
        }
    }
}
