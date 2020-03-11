﻿using RhytmFighter.Camera;
using RhytmFighter.Characters;
using RhytmFighter.Input;
using RhytmFighter.Level;

namespace RhytmFighter.Main
{
    /// <summary>
    /// Holder for all controllers
    /// </summary>
    public class ControllersHolder
    {
        public GridInputProxy GridInputProxy { get; private set; }

        public LevelController LevelController { get; private set; }
        public InputController InputController { get; private set; }
        public CameraController CameraController { get; private set; }
        public PlayerCharacterController PlayerCharacterController { get; private set; }
        public GridPositionTrackingController GridPositionTrackingController { get; private set; }

        public ControllersHolder()
        {
            GridInputProxy = new GridInputProxy();
            LevelController = new LevelController();
            InputController = new InputController();
            CameraController = new CameraController();
            PlayerCharacterController = new PlayerCharacterController();
            GridPositionTrackingController = new GridPositionTrackingController(LevelController);
        }
    }
}