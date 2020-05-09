using UnityEngine;

namespace RhytmFighter.Characters.Movement
{
    public interface iMovementStrategy
    {
        event System.Action<int> OnMovementFinished;
        event System.Action<int> OnCellVisited;
        event System.Action OnRotationFinished;

        bool IsMoving { get; }

        void StartMove(Vector3[] path);

        void StopMove();

        void StartTeleport(Vector3 pos);

        void RotateTo(Quaternion targetRotation);

        void Update(float deltaTime);
    }
}
