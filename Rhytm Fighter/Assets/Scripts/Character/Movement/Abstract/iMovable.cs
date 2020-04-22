using UnityEngine;

namespace RhytmFighter.Interfaces
{
    public interface iMovable : iUpdatable
    {
        event System.Action<int> OnMovementFinished;
        event System.Action<int> OnCellVisited;

        bool IsMoving { get; }

        void Initialize(float moveSpeed);
        void NotifyView_StartMove(Vector3[] path);
        void NotifyView_StopMove();
    }
}
