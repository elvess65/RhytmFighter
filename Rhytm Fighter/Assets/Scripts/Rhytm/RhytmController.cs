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
        private bool m_IsStarted;
        private double m_NextTickTime;
        private double m_DSPStartTime;
        private double m_TicksSinceStart;
        private double m_LoopPositionInTicks;
        private int m_CompletedLoops;

        /// <summary>
        /// Duration of a tick (in seconds)
        /// </summary>
        public double TickDurationSeconds { get; private set; }

        /// <summary>
        /// Ticks passed since start (in ticks)
        /// </summary>
        public int TicksSinceStart => Mathf.RoundToInt((float)m_TicksSinceStart);

        /// <summary>
        /// Current position in loop (in ticks)
        /// </summary>
        public int LoopPositionTicks => Mathf.RoundToInt((float)m_LoopPositionInTicks);

        /// <summary>
        /// Current position in loop (from 0 to 1)
        /// </summary>
        public double LoopPositionAnalog => m_LoopPositionInTicks / m_TICKS_PER_LOOP;

        /// <summary>
        /// Time to the next tick (in seconds)
        /// </summary>
        public double TimeToNextTick => m_NextTickTime - AudioSettings.dspTime;

        private const float m_TICKS_PER_LOOP = 4;


        public RhytmController(int bpm)
        {
            m_Instance = this;

            m_IsStarted = false;
            m_CompletedLoops = 0;

            m_BPM = bpm;
            TickDurationSeconds = 60.0 / m_BPM;
        }

        public void StartTicking()
        {
            m_DSPStartTime = AudioSettings.dspTime;
            m_NextTickTime = m_DSPStartTime;

            OnStarted?.Invoke();

            m_IsStarted = true;
            ExecuteTick();
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
                double timeSinceStart = AudioSettings.dspTime - m_DSPStartTime;

                //Ticks since start
                m_TicksSinceStart = timeSinceStart / TickDurationSeconds;

                //Loops
                if (m_TicksSinceStart >= (m_CompletedLoops + 1) * m_TICKS_PER_LOOP)
                    m_CompletedLoops++;

                //Loop position (in ticks)
                m_LoopPositionInTicks = m_TicksSinceStart - m_CompletedLoops * m_TICKS_PER_LOOP;
                
                if (TimeToNextTick <= 0)
                    ExecuteTick();
            }
        }


        private void ExecuteTick()
        {
            m_NextTickTime = m_NextTickTime + TickDurationSeconds;
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
    }
}
