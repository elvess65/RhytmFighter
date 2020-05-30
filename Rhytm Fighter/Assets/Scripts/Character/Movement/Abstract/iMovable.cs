using RhytmFighter.Persistant.Abstract;
using UnityEngine;

namespace RhytmFighter.Characters.Movement
{
    public interface iMovable : iUpdatable
    {
        event System.Action<int> OnMovementFinished;
        event System.Action<int> OnCellVisited;
        event System.Action OnRotationFinished;

        bool IsMoving { get; }

        void Initialize(float moveSpeed);
        void StartMove(Vector3[] path);
        void StopMove();
        void StartRotate(Quaternion targetRotation, bool onlyAnimation);
    }
}
