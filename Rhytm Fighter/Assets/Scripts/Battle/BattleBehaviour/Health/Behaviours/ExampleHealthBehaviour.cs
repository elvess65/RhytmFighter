using UnityEngine;

namespace RhytmFighter.Battle.Health.Behaviours
{
    public class ExampleHealthBehaviour : iHealthBehaviour
    {
        private int m_HP;
        private readonly int m_MaxHP;

        public ExampleHealthBehaviour(int hp, int maxHP)
        {
            m_HP = hp;
            m_MaxHP = maxHP;
        }

        public void ReduceHP(int dmg)
        {
            m_HP -= dmg;

            if (m_HP <= 0)
                Debug.Log("Object destroyed");
        }

        public void RestoreHP(int amount)
        {
            m_HP += amount;

            if (m_HP > m_MaxHP)
                m_HP = m_MaxHP;
        }
    }
}
