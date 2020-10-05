using UnityEngine;

namespace RhytmFighter.Settings
{
    [CreateAssetMenu(fileName = "New General Settings", menuName = "Settings/General", order = 101)]
    public class GeneralSettings : ScriptableObject
    {
        public bool MuteAudio = false;
        [Tooltip("Делать копию материалов из выбранного пресета (true) или использовать ссылки (для редактирования в игре) (false)")]
        public bool ClonePresetMaterials = false;

        [Range(0.1f, 1)]
        public double InputPrecious = 0.25;

        public float MoveSpeedTickDurationMultiplayer = 4;
    }
}
