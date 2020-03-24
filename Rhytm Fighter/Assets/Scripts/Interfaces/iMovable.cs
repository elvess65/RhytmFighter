using RhytmFighter.Objects.View;
using UnityEngine;

namespace RhytmFighter.Interfaces
{
    public interface iMovable : iUpdatable
    {
        event System.Action OnMovementFinished;
        event System.Action<int> OnCellVisited;

        int ID { get; }
        bool IsMoving { get; }
        AbstractGridObjectView View { get; }

        void StartMove(Vector3[] path);
        void StopMove();
    }
}
