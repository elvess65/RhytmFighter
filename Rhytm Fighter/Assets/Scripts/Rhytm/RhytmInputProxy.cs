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
            double timeSinceLastBeat = m_RhytmController.m_TickRate - m_RhytmController.TimeToNextBeat;

            //Pre beat
            if (timeSinceLastBeat >= m_RhytmController.m_TickRate / 2)
                inputDelta = m_RhytmController.TimeToNextBeat;
            //Post beat
            else
                inputDelta = timeSinceLastBeat;

            return inputDelta <= m_InputPrecious;
        }
    }
}
