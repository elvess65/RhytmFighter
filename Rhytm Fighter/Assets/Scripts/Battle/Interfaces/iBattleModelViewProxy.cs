using UnityEngine;

namespace RhytmFighter.Battle
{
    public interface iBattleModelViewProxy
    {
        Vector3 ProjectileSpawnPosition { get; }
        Vector3 ProjectileHitPosition { get; }
        Vector3 DefenceSpawnPosition { get; }

        void NotifyView_ExecuteAction();
        void NotifyView_TakeDamage(int dmg);
        void NotifyView_IncreaseHP();
        void NotifyView_Destroyed();
    }
}
