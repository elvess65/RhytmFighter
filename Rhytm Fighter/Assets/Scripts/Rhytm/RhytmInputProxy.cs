namespace RhytmFighter.Rhytm
{
    public class RhytmInputProxy 
    {
        private double m_InputPrecious;
        private float m_LastInputTime;
        private const float m_TICK_DURATION_REDUCE = 0.4f;


        public void SetInputPrecious(double inputPrecious)
        {
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
                UnityEngine.Debug.Log("PRE TICK: " + (-RhytmController.GetInstance().TimeToNextTick));
                return 1 - progressToNextTickAnalog <= m_InputPrecious;
            }
            //Post tick
            else
            {
                UnityEngine.Debug.Log("POST TICK: " + (RhytmController.GetInstance().TickDurationSeconds - RhytmController.GetInstance().TimeToNextTick));
                return progressToNextTickAnalog <= m_InputPrecious;
            }
        }
    }
}
