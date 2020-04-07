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
