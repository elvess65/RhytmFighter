using UnityEngine;

namespace RhytmFighter.Battle.Action.Behaviours
{
    public class PlayerBattleActionBehaviour : iBattleActionBehaviour
    {
        public event System.Action OnActionExecuted;

        public void ExecuteAction()
        {
            Debug.LogError("Execute battle action");
            OnActionExecuted?.Invoke();
        }
    }
}
