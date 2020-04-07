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
            double timeToNextTickAnalog = RhytmController.GetInstance().TimeToNextTickAnalog;

            //Pre tick
            if (timeToNextTickAnalog >= 0.5f)
                return 1 - timeToNextTickAnalog <= m_InputPrecious;
            //Post tick
            else
                return timeToNextTickAnalog <= m_InputPrecious;
        }
    }
}
