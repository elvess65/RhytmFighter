using RhytmFighter.Battle;
using RhytmFighter.Camera;
using RhytmFighter.Characters;
using RhytmFighter.Input;
using RhytmFighter.Level;
using RhytmFighter.Rhytm;

namespace RhytmFighter.Main
{
    /// <summary>
    /// Holder for all controllers
    /// </summary>
    public class ControllersHolder
    {
        public GridInputProxy GridInputProxy { get; private set; }
        public RhytmInputProxy RhytmInputProxy { get; private set; }

        public LevelController LevelController { get; private set; }
        public InputController InputController { get; private set; }
        public RhytmController RhytmController { get; private set; }
        public CameraController CameraController { get; private set; }
        public BattleController BattleController { get; private set; }
        public CommandsController CommandsController { get; private set; }

        public PlayerCharacterController PlayerCharacterController { get; private set; }
        

        public ControllersHolder()
        {
            GridInputProxy = new GridInputProxy();

            LevelController = new LevelController();
            InputController = new InputController();
            RhytmController = new RhytmController();
            CameraController = new CameraController();
            BattleController = new BattleController(LevelController, CameraController);
            CommandsController = new CommandsController();
            RhytmInputProxy = new RhytmInputProxy();

            PlayerCharacterController = new PlayerCharacterController();

            UnityEngine.AudioListener.volume = 0;
        }
    }
}
