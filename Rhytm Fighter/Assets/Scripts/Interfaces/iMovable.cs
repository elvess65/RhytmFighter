using UnityEngine;

namespace RhytmFighter.Interfaces
{
    public interface iMovable : iUpdatable
    {
        event System.Action OnMovementFinished;
        event System.Action<int> OnCellVisited;

        bool IsMoving { get; }

        void Initialize(Vector3 pos, float moveSpeed);
        void StartMove(Vector3[] path);
        void StopMove();
    }
}
