using UnityEngine;

namespace RhytmFighter.Battle
{
    public interface iBattleModelViewProxy
    {
        Vector3 ProjectileSpawnPosition { get; }
        Vector3 ProjectileHitPosition { get; }

        void NotifyView_ExecuteAction();
        void NotifyView_TakeDamage(int curHP, int maxHP, int dmg);
        void NotifyView_IncreaseHP();
        void NotifyView_Destroyed();
    }
}
