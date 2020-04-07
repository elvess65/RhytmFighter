namespace RhytmFighter.Rhytm
{
    public class RhytmInputProxy 
    {
        private double m_InputPrecious;

        public RhytmInputProxy()
        {
        }

        public void SetInputPrecious(double inputPrecious)
        {
            m_InputPrecious = inputPrecious;
        }

        public bool IsInputValid()
        {
            //If timeSinceLast beat is more than half of tick rate - consider calculating time before next beat
            //In other case consider to calculate time after previous beat

            double inputDelta = 0;
            double timeSinceLastBeat = RhytmController.GetInstance().TickDurationSeconds - RhytmController.GetInstance().TimeToNextTick;

            //Pre tick
            if (timeSinceLastBeat >= RhytmController.GetInstance().TickDurationSeconds / 2)
            {
                UnityEngine.Debug.Log("PRE TICK");
                inputDelta = RhytmController.GetInstance().TimeToNextTick;
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
