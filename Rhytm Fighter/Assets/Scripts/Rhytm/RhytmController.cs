using RhytmFighter.Interfaces;
using UnityEngine;

namespace RhytmFighter.Rhytm
{
    public class RhytmController : iUpdatable
    {
        private static RhytmController m_Instance;

        public System.Action OnStarted;
        public System.Action OnStopped;
        public System.Action OnTick;

        //Base 
        private int m_BPM;

        //Process
        private bool m_IsStarted = false;
        private double m_NextTickTime;

        public double TickRate { get; private set; }
        public double TimeToNextTick => m_NextTickTime - AudioSettings.dspTime;
        

        public RhytmController(int bpm)
        {
            m_Instance = this;

            m_BPM = bpm;
            TickRate = 60.0 / m_BPM;
        }

        public void StartTicking()
        {
            m_NextTickTime = AudioSettings.dspTime;
            ExecuteTick();

            m_IsStarted = true;
            OnStarted?.Invoke();
        }

        public void StopTicking()
        {
            m_IsStarted = false;
            OnStopped?.Invoke();
        }

        public void PerformUpdate(float deltaTime)
        {
            if (m_IsStarted)
            {
                if (TimeToNextTick <= 0)
                    ExecuteTick();
            }
        }


        private void ExecuteTick()
        {
            m_NextTickTime = m_NextTickTime + TickRate;
            OnTick?.Invoke();
        }


        public static void SubscribeForBeatEvent(System.Action action)
        {
            m_Instance.OnTick += action;
        }

        public static void UnscribeFromBeatEvent(System.Action action)
        {
            m_Instance.OnTick -= action;
        }

        public static double GetTickRate()
        {
            return m_Instance.TickRate;
        }
    }
}
