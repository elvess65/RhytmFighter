using UnityEngine;

namespace RhytmFighter.Interfaces
{
    public interface iMovable : iUpdatable
    {
        event System.Action<int> OnMovementFinished;
        event System.Action<int> OnCellVisited;

        bool IsMoving { get; }

        void Initialize(float moveSpeed);
        void StartMove(Vector3[] path);
        void StopMove();
    }
}
