using UnityEngine;

namespace RhytmFighter.StateMachines.GameState
{
    public class GameState_TapToAction : GameState_Abstract
    {
        public System.Action OnTouch;

        public GameState_TapToAction() : base(null, null)
        {
        }

        public override void HandleTouch(Vector3 mouseScreenPos)
        {
            OnTouch?.Invoke();
        }

        public override void PerformUpdate(float deltaTime)
        {
        }
    }
}
