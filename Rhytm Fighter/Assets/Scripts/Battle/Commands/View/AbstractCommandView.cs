using FrameworkPackage.Utils;
using RhytmFighter.Interfaces;
using UnityEngine;

namespace RhytmFighter.Battle.Command.View
{
    public abstract class AbstractCommandView : MonoBehaviour, iUpdatable
    {
        public System.Action<AbstractCommandView> OnViewDisposed;

        protected float m_ExistTime;
        protected InterpolationData<Vector3> m_LerpData;


        public virtual void Initialize(Vector3 senderPos, float existTime)
        {
            transform.position = senderPos;

            m_LerpData.TotalTime = existTime;
            m_LerpData.From = senderPos;
            m_LerpData.Start();
        }

        public virtual void PerformUpdate(float deltaTime)
        {
            if (m_LerpData.IsStarted)
            {
                m_LerpData.Increment();
                ProcessUpdate();

                if (m_LerpData.Overtime())
                    FinalizeView();
            }
        }


        protected abstract void ProcessUpdate();

        protected virtual void FinalizeView()
        {
            DisposeView();
        }

        protected void DisposeView()
        {
            OnViewDisposed?.Invoke(this);
            OnViewDisposed = null;

            Destroy(gameObject);
        }
    }
}
