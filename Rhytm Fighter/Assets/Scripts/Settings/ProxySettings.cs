using UnityEngine;

namespace RhytmFighter.Settings.Proxy
{
    public class ProxySettings : ScriptableObject
    {
        public virtual bool UseSimulation { get; }
    }

    [CreateAssetMenu(fileName = "New ProxySettings_Simulation", menuName = "Settings/Proxy/Simulation", order = 101)]
    public class ProxySettings_Simulation : ProxySettings
    {
        public override bool UseSimulation => true;

        [Header("Simulation")]
        public bool SimulateSuccess = true;
    }

    [CreateAssetMenu(fileName = "New ProxySettings_RemoteExample", menuName = "Settings/Proxy/RemoteExample", order = 101)]
    public class ProxySettings_RemoteExample : ProxySettings
    {
        public override bool UseSimulation => false;

        [Header("Remote")]
        public string IP = "localHost";
    }
}
