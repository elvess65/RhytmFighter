using UnityEngine;

namespace RhytmFighter.Battle.Command.View
{
    public class AbstractDefenceView : AbstractCommandView
    {
        public GameObject DefenceBreachEffectPrefab;

        protected override void ProcessUpdate()
        {
        }

        public void C(Vector3 pos)
        {
            GameObject ob = Instantiate(DefenceBreachEffectPrefab, pos, Quaternion.identity);
            Destroy(ob, 2);

            DisposeView();
        }
    }
}
