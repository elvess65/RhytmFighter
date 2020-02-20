using UnityEngine;

namespace RhytmFighter.Data.DataBase.Simulation
{
    [CreateAssetMenu(fileName = "New Simulation_PlayerData", menuName = "DBSimulation/PlayerData", order = 101)]
    public class DBSimulation_PlayerData : ScriptableObject
    {
        public int Experiance;
    }
}
