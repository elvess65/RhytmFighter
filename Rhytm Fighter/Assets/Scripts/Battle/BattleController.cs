using System.Collections.Generic;
namespace RhytmFighter.Battle
{
    public class BattleController
    {
        public System.Action OnBattleStarted;
        public System.Action OnBattleFinished;

        private Dictionary<int, iBattleObject> m_Enemies;

        public iBattleObject Player { get; set; }


        public BattleController()
        {
            m_Enemies = new Dictionary<int, iBattleObject>();
        }

        public void AddEnemyToActiveBattle(iBattleObject battleObject)
        {
            //Add enemy to process list
            if (!m_Enemies.ContainsKey(battleObject.ID))
            {
                m_Enemies.Add(battleObject.ID, battleObject);
                battleObject.Target = Player;
            }

            //Start battle with adding the first enemy
            if (m_Enemies.Count == 1)
                OnBattleStarted();
        }

        public void ProcessEnemyActions()
        {
            foreach (iBattleObject enemy in m_Enemies.Values)
                enemy.ActionBehaviour.ExecuteAction();
        }

        public iBattleObject GetClosestEnemy(iBattleObject relativeToBattleObject)
        {
            iBattleObject result = null;
            float closestSqrDistToEnemy = float.MaxValue;
            //If relativeObject already has target set init closestDist to sqr dist to it
            if (relativeToBattleObject.Target != null)
            {
                closestSqrDistToEnemy = (relativeToBattleObject.ViewPosition - relativeToBattleObject.Target.ViewPosition).sqrMagnitude;
                result = relativeToBattleObject.Target;
            }

            foreach(iBattleObject enemy in m_Enemies.Values)
            {
                float sqrDistToEnemy = (relativeToBattleObject.ViewPosition - enemy.ViewPosition).sqrMagnitude;
                if (sqrDistToEnemy < closestSqrDistToEnemy)
                {
                    closestSqrDistToEnemy = sqrDistToEnemy;
                    result = enemy;
                }
            }

            return result;
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
