using RhytmFighter.Characters;
using System.Collections.Generic;

namespace RhytmFighter.Battle
{
    public class BattleController
    {
        public System.Action OnBattleStarted;
        public System.Action OnBattleFinished;

        private Dictionary<int, iBattleObject> m_Enemies;

        public BattleController()
        {
            m_Enemies = new Dictionary<int, iBattleObject>();
        }

        public void AddEnemyToActiveBattle(iBattleObject battleObject)
        {
            if (!m_Enemies.ContainsKey(1))
                m_Enemies.Add(1, battleObject);

            //Start battle with adding the first enemy
            if (m_Enemies.Count == 1)
                OnBattleStarted();
        }

        private void HandleEnemyDestroyed(int id)
        {
            if (m_Enemies.ContainsKey(id))
                m_Enemies.Remove(id);

            //Finish battle
            if (m_Enemies.Count == 0)
                OnBattleFinished?.Invoke();
        }
    }
}
