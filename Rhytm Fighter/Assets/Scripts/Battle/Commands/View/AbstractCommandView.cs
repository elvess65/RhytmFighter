using RhytmFighter.Interfaces;
using UnityEngine;

namespace RhytmFighter.Battle.Command.View
{
    public abstract class AbstractCommandView : MonoBehaviour, iUpdatable
    {
        public abstract void Initialize(Vector3 senderPos, Vector3 targetPos, float applyTime);

        public abstract void PerformUpdate(float deltaTime);


        public virtual void Dispose()
        {
            Destroy(gameObject);
        }
    }
}
