using System;
using UnityEngine;

namespace RhytmFighter.Characters.Movement
{
    public class Teleport_MovementStrategy : iMovementStrategy
    {
        public bool IsMoving => false;

        public event Action<int> OnMovementFinished;
        public event Action<int> OnCellVisited;
        public event Action OnRotationFinished;

        private Transform m_ControlledTransform;


        public Teleport_MovementStrategy(Transform controlledTransform)
        {
            m_ControlledTransform = controlledTransform;
        }

        public void StartMove(Vector3[] path)
        {
            //Rotate in move direction
            RotateTo(Quaternion.LookRotation(path[0] - m_ControlledTransform.position));

            //Move
            m_ControlledTransform.transform.position = path[0];

            //Call movement finished event
            OnMovementFinished?.Invoke(0);
        }

        public void StopMove()
        {
        }

        public void RotateTo(Quaternion targetRotation)
        {
            m_ControlledTransform.rotation = targetRotation;
        }

        public void Update(float deltaTime)
        {
        }
    }
}
