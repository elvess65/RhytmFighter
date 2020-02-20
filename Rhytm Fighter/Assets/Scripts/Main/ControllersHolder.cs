using RhytmFighter.Level;

namespace RhytmFighter.Main
{
    /// <summary>
    /// Holder for all controllers
    /// </summary>
    public class ControllersHolder 
    {
        public LevelController LevelController { get; private set; }

        public ControllersHolder()
        {
            LevelController = new LevelController();
        }
    }
}
