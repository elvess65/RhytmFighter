namespace RhytmFighter.Rhytm
{
    public class RhytmInputProxy 
    {
        private readonly RhytmController m_RhytmController;
        private readonly double m_InputPrecious;

        public RhytmInputProxy(RhytmController rhytmController, double inputPrecious)
        {
            m_RhytmController = rhytmController;
            m_InputPrecious = inputPrecious;
        }

        public bool IsInputValid()
        {
            //If timeSinceLast beat is more than half of tick rate - consider calculating time before next beat
            //In other case consider to calculate time after previous beat

            double inputDelta = 0;
            double timeSinceLastBeat = m_RhytmController.TickRate - m_RhytmController.TimeToNextTick;

            //Pre tick
            if (timeSinceLastBeat >= m_RhytmController.TickRate / 2)
            {
                UnityEngine.Debug.Log("PRE TICK");
                inputDelta = m_RhytmController.TimeToNextTick;
            }
            //Post tick
            else
            {
                UnityEngine.Debug.Log("POST TICK");
                inputDelta = timeSinceLastBeat;
            }

            return inputDelta <= m_InputPrecious;
        }
    }
}
