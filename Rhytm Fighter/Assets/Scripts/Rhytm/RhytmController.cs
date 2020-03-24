using RhytmFighter.Interfaces;
using UnityEngine;

namespace RhytmFighter.Rhytm
{
    public class RhytmController : iUpdatable
    {
        public System.Action OnBeatStarted;
        public System.Action OnBeatStopped;
        public System.Action OnBeat;

        //Base 
        private float m_BPS;
        public float m_TickRate { get; private set; }

        //Process
        private bool m_IsStarted = false;
        private double m_NextBeatTime;

        public double TimeToNextBeat => m_NextBeatTime - AudioSettings.dspTime;



        public RhytmController(float bps)
        {
            m_BPS = bps;
            m_TickRate = 60f / m_BPS;
        }

        public void StartBeat()
        {
            ExecuteBeat();

            m_IsStarted = true;
            OnBeatStarted?.Invoke();
        }

        public void StopBeat()
        {
            m_IsStarted = false;
            OnBeatStopped?.Invoke();
        }

        public void PerformUpdate(float deltaTime)
        {
            if (m_IsStarted)
            {
                if (TimeToNextBeat <= 0)
                    ExecuteBeat();
            }
        }


        private void ExecuteBeat()
        {
            m_NextBeatTime = AudioSettings.dspTime + m_TickRate;
            OnBeat?.Invoke();
        }
    }
}
