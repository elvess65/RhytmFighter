using FrameworkPackage.Utils;
using RhytmFighter.Core;
using UnityEngine;

namespace RhytmFighter.Battle.Command.View
{
    public abstract class AbstractCommandView : MonoBehaviour, iUpdatable
    {
        public System.Action<AbstractCommandView> OnViewDisposed;

        protected float m_ExistTime;
        protected InterpolationData<Vector3> m_LerpData;

        public int CommandID { get; private set; }


        public virtual void Initialize(Vector3 senderPos, float existTime, int commandID)
        {
            CommandID = commandID;

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

        public void HideView()
        {
            FinalizeView();
        }


        protected virtual void ProcessUpdate()
        {

        }

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
