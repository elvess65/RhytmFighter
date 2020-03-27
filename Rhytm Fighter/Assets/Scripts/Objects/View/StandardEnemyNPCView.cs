using UnityEngine;

namespace RhytmFighter.Objects.View
{
    public class StandardEnemyNPCView : AbstractBattleNPCView
    {
        public override void ExecuteAction()
        {
            base.ExecuteAction();
            //Debug.Log("StandardEnemyNPCView: " + gameObject + " is playing execute action animation");
        }

        public override void TakeDamage()
        {
            base.TakeDamage();
            //Debug.Log("StandardEnemyNPCView: " + gameObject + " is playing take damage animation");
        }
    }
}
