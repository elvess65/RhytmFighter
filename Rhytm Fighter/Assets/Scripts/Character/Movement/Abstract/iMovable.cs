using RhytmFighter.Core;
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
        void NotifyView_StartMove(Vector3[] path);
        void NotifyView_Teleport(Vector3 pos);
        void NotifyView_StopMove();
        void NotifyView_StartRotate(Quaternion targetRotation, bool onlyAnimation);
    }
}
