using RhytmFighter.Characters;
using System.Collections.Generic;

namespace RhytmFighter.Main
{
    public class BattleController
    {
        public System.Action OnBattleStarted;
        public System.Action OnBattleFinished;

        private Dictionary<int, CharacterWrapper> m_Enemies;

        public BattleController()
        {
            m_Enemies = new Dictionary<int, CharacterWrapper>();
        }

        public void AddEnemyToActiveBattle(CharacterWrapper enemyCharacterWrapper)
        {
            if (!m_Enemies.ContainsKey(enemyCharacterWrapper.ID))
                m_Enemies.Add(enemyCharacterWrapper.ID, enemyCharacterWrapper);

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
