using UnityEngine;

namespace RhytmFighter.Characters.Movement
{
    public interface iMovementStrategy
    {
        event System.Action<int> OnMovementFinished;
        event System.Action<int> OnCellVisited;

        bool IsMoving { get; }

        void StartMove(Vector3[] path);

        void StopMove();

        void Update(float deltaTime);
    }
}
