using UnityEngine;

namespace RhytmFighter.Settings
{
    [CreateAssetMenu(fileName = "New General Settings", menuName = "Settings/General", order = 101)]
    public class GeneralSettings : ScriptableObject
    {
        public bool MuteAudio = false;
    }
}
