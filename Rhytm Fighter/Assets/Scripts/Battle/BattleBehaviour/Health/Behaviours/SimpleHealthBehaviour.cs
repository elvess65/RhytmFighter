using System;
using UnityEngine;

namespace RhytmFighter.Battle.Health.Behaviours
{
    public class SimpleHealthBehaviour : iHealthBehaviour
    {
        public event Action<int> OnHPReduced;
        public event Action<int> OnHPIncreased;
        public event System.Action OnDestroyed;

        public int HP { get; private set; }
        public int MaxHP { get; private set; }


        public SimpleHealthBehaviour(int hp, int maxHP)
        {
            HP = hp;
            MaxHP = maxHP;
        }

        public SimpleHealthBehaviour(int hp)
        {
            HP = hp;
            MaxHP = hp;
        }

        public void ReduceHP(int dmg)
        {
            //HP -= dmg;

            if (HP <= 0)
                OnDestroyed?.Invoke();
            else
                OnHPReduced?.Invoke(dmg);
        }

        public void IncreaseHP(int amount)
        {
            HP = Mathf.Clamp(HP + amount, HP, MaxHP);

            OnHPIncreased?.Invoke(amount);
        }
    }
}
