using RhytmFighter.Battle;
using RhytmFighter.Characters;
using RhytmFighter.Characters.Movement;
using RhytmFighter.Interfaces;
using UnityEngine;

namespace RhytmFighter.Objects.View
{
    public class AbstractBattleNPCView : AbstractNPCView, iBattleModelViewProxy, iMovable
    {
        public event System.Action OnMovementFinished;
        public event System.Action<int> OnCellVisited;

        private iMovementStrategy m_MoveStrategy;
        private StandartAnimationController m_AnimationController;

        public bool IsMoving => m_MoveStrategy.IsMoving;


        #region iMovable
        public void Initialize(float moveSpeed)
        {
            //Movement
            m_MoveStrategy = new Bezier_MovementStrategy(transform, moveSpeed);
            m_MoveStrategy.OnMovementFinished += MovementFinishedHandler;
            m_MoveStrategy.OnCellVisited += CellVisitedHandler;

            //Animation
            m_AnimationController = GetComponent<StandartAnimationController>();
        }

        public void StartMove(Vector3[] path)
        {
            m_MoveStrategy.StartMove(path);
            m_AnimationController.PlayMoveAnimation();
        }

        public void StopMove()
        {
            m_MoveStrategy.StopMove();
        }

        //iUpdatable
        public void PerformUpdate(float deltaTime)
        {
            m_MoveStrategy.Update(deltaTime);
        }


        void MovementFinishedHandler()
        {
            m_AnimationController.PlayIdleAnimation();

            OnMovementFinished?.Invoke();
        }

        void CellVisitedHandler(int index) => OnCellVisited?.Invoke(index);
        #endregion

        #region iBattleModelViewProxy
        public virtual void ExecuteAction()
        {
            StartCoroutine(ExecuteActionCoroutine());
        }

        public virtual void TakeDamage()
        {
            StartCoroutine(TakeDamageCoroutine());
        }

        public void IncreaseHP()
        {

        }

        public virtual void Destroy()
        {
            Destroy(gameObject);
        }


        System.Collections.IEnumerator ExecuteActionCoroutine()
        {
            transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);

            for (int i = 0; i < 5; i++)
                yield return null;

            transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
        }

        System.Collections.IEnumerator TakeDamageCoroutine()
        {
            transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);

            for (int i = 0; i < 5; i++)
                yield return null;

            transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
        }
        #endregion
    }
}
