using FrameworkPackage.Utils;
using RhytmFighter.Persistant.Abstract;
using UnityEngine;

namespace RhytmFighter.UI.Components
{
    public class UIComponent_ActionPointsIndicatorItem : MonoBehaviour, iUpdatable
    {
        [SerializeField] private InterpolatableComponent Image_Restore;

        private InterpolationData<float> m_LerpData;

        public bool IsRestoring => m_LerpData.IsStarted;


        public void Initialize(float restoreDuration)
        {
            m_LerpData = new InterpolationData<float>(restoreDuration);
            m_LerpData.From = 0;
            m_LerpData.To = 1;

            Image_Restore.Initialize();
        }

        public void StartRestoring()
        {
            Image_Restore.PrepareForInterpolation();
            m_LerpData.Start();
        }

        public void Restore()
        {
            m_LerpData.Stop();
            Image_Restore.FinishInterpolation();
        }

        public void PerformUpdate(float deltaTime)
        {
            if (m_LerpData.IsStarted)
            {
                m_LerpData.Increment();
                Image_Restore.ProcessInterpolation(m_LerpData.Progress);

                if (m_LerpData.Overtime())
                    Restore();
            }
        }
    }
}
