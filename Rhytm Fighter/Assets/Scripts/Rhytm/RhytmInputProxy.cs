using RhytmFighter.Core.Enums;

namespace RhytmFighter.Rhytm
{
    public class RhytmInputProxy 
    {
        private static RhytmInputProxy m_Instance;

        private double m_InputPrecious;
        private float m_LastInputTime;
        private const float m_TICK_DURATION_REDUCE = 0.4f;


        public static bool IsInUseRange => 1 - RhytmController.GetInstance().ProgressToNextTickAnalog < m_Instance.m_InputPrecious ||
                                               RhytmController.GetInstance().ProgressToNextTickAnalog < m_Instance.m_InputPrecious;


        public RhytmInputProxy()
        {
            if (m_Instance == null)
                m_Instance = this;
        }

        public void SetInputPrecious(double inputPrecious)
        {
            m_Instance = this;

            m_InputPrecious = inputPrecious;
        }

        public void RegisterInput()
        {
            m_LastInputTime = UnityEngine.Time.time;
        }

        public bool IsInputAllowed()
        {
            float deltaInput = UnityEngine.Time.time - m_LastInputTime;

            return deltaInput > RhytmController.GetInstance().TickDurationSeconds * m_TICK_DURATION_REDUCE;
        }

        public bool IsInputTickValid()
        {
            double progressToNextTickAnalog = RhytmController.GetInstance().ProgressToNextTickAnalog;

            //Pre tick
            if (progressToNextTickAnalog >= 0.5f)
            {
                RhytmController.GetInstance().DeltaInput = -RhytmController.GetInstance().TimeToNextTick;
                RhytmController.GetInstance().InputTickResult = InputTickResult.PreTick;

                //UnityEngine.Debug.Log("RhytmInputProxy: Pre Tick: " + RhytmController.GetInstance().DeltaInput);

                return 1 - progressToNextTickAnalog <= m_InputPrecious;
            }
            //Post tick
            else
            {
                RhytmController.GetInstance().DeltaInput = RhytmController.GetInstance().TickDurationSeconds - RhytmController.GetInstance().TimeToNextTick;
                RhytmController.GetInstance().InputTickResult = InputTickResult.PostTick;

                //UnityEngine.Debug.Log("RhytmInputProxy: Post Tick: " + RhytmController.GetInstance().DeltaInput);

                return progressToNextTickAnalog <= m_InputPrecious;
            }
        }
    }
}
