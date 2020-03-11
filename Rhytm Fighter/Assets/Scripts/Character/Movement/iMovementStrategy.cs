using UnityEngine;

namespace RhytmFighter.Characters.Movement
{
    public interface iMovementStrategy
    {
        event System.Action OnMovementFinished;
        event System.Action<int> OnCellVisited;
        event System.Action OnMovementInterrupted;

        bool IsMoving { get; }

        void StartMove(Vector3[] path);

        void StopMove();

        void Update(float deltaTime);
    }
}
