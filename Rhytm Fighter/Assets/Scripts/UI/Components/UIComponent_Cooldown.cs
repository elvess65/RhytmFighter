using FrameworkPackage.Utils;
using RhytmFighter.Persistant.Abstract;
using UnityEngine;

namespace RhytmFighter.UI.Components
{
    public class UIComponent_Cooldown : MonoBehaviour, iUpdatable
    {
        [SerializeField] private InterpolatableComponent[] m_ControlledObjects;

        private InterpolationData<float> m_LerpData;

        public bool IsInCooldown => m_LerpData.IsStarted;


        public void Initialize(float cooldownTime)
        {
            m_LerpData = new InterpolationData<float>(cooldownTime);
            m_LerpData.From = 0;
            m_LerpData.To = m_LerpData.TotalTime;

            for (int i = 0; i < m_ControlledObjects.Length; i++)
                m_ControlledObjects[i].Initialize();
        }

        public void Cooldown()
        {
            for (int i = 0; i < m_ControlledObjects.Length; i++)
                m_ControlledObjects[i].PrepareForInterpolation();

            m_LerpData.Start();
        }

        public void PerformUpdate(float deltaTime)
        {
            if (m_LerpData.IsStarted)
            {
                m_LerpData.Increment();
                for (int i = 0; i < m_ControlledObjects.Length; i++)
                    m_ControlledObjects[i].ProcessInterpolation(m_LerpData.Progress);

                if (m_LerpData.Overtime())
                {
                    m_LerpData.Stop();

                    for (int i = 0; i < m_ControlledObjects.Length; i++)
                        m_ControlledObjects[i].FinishInterpolation();
                }
            }
        }
    }
}
