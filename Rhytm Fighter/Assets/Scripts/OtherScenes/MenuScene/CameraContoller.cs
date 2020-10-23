using System.Collections;
using System.Collections.Generic;
using FrameworkPackage.Utils;
using RhytmFighter.Persistant.Abstract;
using UnityEngine;

namespace RhytmFighter.OtherScenes.MenuScene
{
    public class CameraContoller : MonoBehaviour, iUpdatable
    {
        public System.Action OnCameraPushKeyFrameReached;

        [SerializeField] private Transform CameraTransform;
        [SerializeField] private AnimationCurve AnimationCurve;

        private InterpolationData<Vector3> m_LerpData;

        private const float m_PUSH_DURATION = 1;
        private const float m_EVENT_EXECUTE_PROGRESS = 0.2f;
        private readonly Vector3 m_PushOffset = new Vector3(0, 0, 4);

        public void Initialize()
        {
            m_LerpData = new InterpolationData<Vector3>(m_PUSH_DURATION);
            m_LerpData.From = CameraTransform.position;
            m_LerpData.To = CameraTransform.position + m_PushOffset;
        }

        public void PushCamera()
        {
            m_LerpData.Start();
        }

        public void PerformUpdate(float deltaTime)
        {
            if (m_LerpData.IsStarted)
            {
                m_LerpData.Increment();
                CameraTransform.position = Vector3.Lerp(m_LerpData.From, m_LerpData.To, AnimationCurve.Evaluate(m_LerpData.Progress));

                if (m_LerpData.Progress >= m_EVENT_EXECUTE_PROGRESS)
                {
                    OnCameraPushKeyFrameReached?.Invoke();
                    OnCameraPushKeyFrameReached = null;
                }

                if (m_LerpData.Overtime())
                    m_LerpData.Stop();                    
            }
        }
    }
}
