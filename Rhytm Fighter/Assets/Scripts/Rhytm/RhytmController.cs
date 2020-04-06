using RhytmFighter.Interfaces;
using UnityEngine;

namespace RhytmFighter.Rhytm
{
    public class RhytmController : iUpdatable
    {
        private static RhytmController m_Instance;

        public System.Action OnBeatStarted;
        public System.Action OnBeatStopped;
        public System.Action OnBeat;

        //Base 
        private int m_BPM;

        //Process
        private bool m_IsStarted = false;
        private double m_NextBeatTime;

        public double TickRate { get; private set; }
        public double TimeToNextBeat => m_NextBeatTime - AudioSettings.dspTime;
        

        public RhytmController(int bpm)
        {
            m_Instance = this;

            m_BPM = bpm;
            TickRate = 60.0 / m_BPM;
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
            m_NextBeatTime = AudioSettings.dspTime + TickRate;
            OnBeat?.Invoke();
        }


        public static void SubscribeForBeatEvent(System.Action action)
        {
            m_Instance.OnBeat += action;
        }

        public static void UnscribeFromBeatEvent(System.Action action)
        {
            m_Instance.OnBeat -= action;
        }

        public static double GetTickRate()
        {
            return m_Instance.TickRate;
        }
    }
}
