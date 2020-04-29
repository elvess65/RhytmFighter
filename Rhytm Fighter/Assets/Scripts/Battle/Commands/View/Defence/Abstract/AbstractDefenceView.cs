using UnityEngine;

namespace RhytmFighter.Battle.Command.View
{
    public class AbstractDefenceView : AbstractCommandView
    {
        public GameObject DefenceBreachEffectPrefab;

        protected override void ProcessUpdate()
        {
        }

        public void ExecuteDefence(Vector3 pos)
        {
            //Create effect
            GameObject ob = Instantiate(DefenceBreachEffectPrefab, pos, Quaternion.identity);
            Destroy(ob, 2);

            //Dispose view
            DisposeView();
        }
    }
}
