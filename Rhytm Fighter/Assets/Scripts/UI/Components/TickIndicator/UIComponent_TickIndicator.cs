using FrameworkPackage.Utils;
using RhytmFighter.Core;
using UnityEngine;

namespace RhytmFighter.UI.Components
{
    public class UIComponent_TickIndicator : MonoBehaviour, iUpdatable
    {
        [SerializeField] UIComponent_TickIndicatorTick m_Tick;
        [SerializeField] UIComponent_TickIndicatorArrow[] m_TickArrows;
   
        private InterpolationData<float> m_LerpData;
        private bool m_IsInBattleState = false;


        public void Initialize(float tickDuration)
        {
            //Lerp
            m_LerpData = new InterpolationData<float>();

            //Tick
            m_Tick.Initialize(tickDuration);

            //Arrows
            for (int i = 0; i < m_TickArrows.Length; i++)
                m_TickArrows[i].Initialize();
        }

        public void ToNormalState()
        {
            m_IsInBattleState = false;

            //Tick
            m_Tick.ToNormalState();

            //Arrows
            m_LerpData.Stop();
            for (int i = 0; i < m_TickArrows.Length; i++)
                m_TickArrows[i].FinishInterpolation();
        }

        public void ToPrepareState()
        {
            m_IsInBattleState = false;

            //Tick
            m_Tick.ToPrepareState();

            //Arrows
            m_LerpData.Stop();
            for (int i = 0; i < m_TickArrows.Length; i++)
                m_TickArrows[i].FinishInterpolation();
        }

        public void ToBattleState()
        {
            m_IsInBattleState = true;

            //Tick
            m_Tick.ToBattleState();
        }


        public void PlayTickAnimation()
        {
            m_Tick.PlayTickAnimation();
        }

        public void PlayArrowsAnimation()
        {
            if (m_IsInBattleState)
            {
                //Arrows
                for (int i = 0; i < m_TickArrows.Length; i++)
                    m_TickArrows[i].PrepareForInterpolation();

                //Lerp
                m_LerpData.TotalTime = (float)Rhytm.RhytmController.GetInstance().TimeToNextTick + (float)Rhytm.RhytmController.GetInstance().ProcessTickDelta;
                m_LerpData.Start();
            }
        }

        
        public void PerformUpdate(float deltaTime)
        {
            if (m_LerpData.IsStarted)
            {
                //Increment
                m_LerpData.Increment();

                //Process
                for (int i = 0; i < m_TickArrows.Length; i++)
                    m_TickArrows[i].ProcessInterpolation(m_LerpData.Progress);

                //Overtime
                if (m_LerpData.Overtime())
                {
                    m_LerpData.Stop();

                    for (int i = 0; i < m_TickArrows.Length; i++)
                        m_TickArrows[i].FinishInterpolation();
                }
            }
        }
    }
}
