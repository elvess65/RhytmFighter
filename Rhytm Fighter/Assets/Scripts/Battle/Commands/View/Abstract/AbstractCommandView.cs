using FrameworkPackage.Utils;
using RhytmFighter.Enviroment;
using RhytmFighter.Persistant.Abstract;
using UnityEngine;

namespace RhytmFighter.Battle.Command.View
{
    public abstract class AbstractCommandView : AbstractWorldObjectView, iUpdatable
    {
        public System.Action<AbstractCommandView> OnCommandViewDisposed;

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
                    DisposeView();
            }
        }


        protected virtual void ProcessUpdate()
        {
        }

        protected override void DisposeView(float delay)
        {
            OnCommandViewDisposed?.Invoke(this);
            OnCommandViewDisposed = null;

            base.DisposeView(delay);
        }

        protected override void DisposeView()
        {
            OnCommandViewDisposed?.Invoke(this);
            OnCommandViewDisposed = null;

            base.DisposeView();
        }
    }
}
