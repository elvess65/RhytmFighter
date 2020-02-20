using UnityEngine;

namespace RhytmFighter.Settings
{
    [CreateAssetMenu(fileName = "New Settings Simulation", menuName = "Settings/Simulation", order = 101)]
    public class SimulationSettings : ScriptableObject
    {
        public bool UseSimulation = true;
        public bool SimulateSuccess = true;
    }
}
