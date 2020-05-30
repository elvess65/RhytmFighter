using RhytmFighter.Battle.Command;
using RhytmFighter.CameraSystem;
using RhytmFighter.Characters;
using RhytmFighter.Input;
using RhytmFighter.Level;
using RhytmFighter.Rhytm;

namespace RhytmFighter.Battle.Core
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
            PlayerCharacterController = new PlayerCharacterController();
            BattleController = new BattleController(LevelController, CameraController, PlayerCharacterController);
            CommandsController = new CommandsController();
            RhytmInputProxy = new RhytmInputProxy();
        }
    }
}
