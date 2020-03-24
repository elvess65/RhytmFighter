using System.Collections.Generic;
using UnityEngine;

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
            //Add enemy to process list
            if (!m_Enemies.ContainsKey(battleObject.ID))
                m_Enemies.Add(battleObject.ID, battleObject);

            //Start battle with adding the first enemy
            if (m_Enemies.Count == 1)
                OnBattleStarted();
        }

        public void RhytmBeatHandler()
        {
            foreach (iBattleObject enemy in m_Enemies.Values)
                enemy.ActionBehaviour.ExecuteAction();
        }


        private void EnemyDestroyedHandler(int id)
        {
            if (m_Enemies.ContainsKey(id))
                m_Enemies.Remove(id);

            //Finish battle
            if (m_Enemies.Count == 0)
                OnBattleFinished?.Invoke();
        }
    }
}
