using RhytmFighter.Battle;
using UnityEngine;

namespace RhytmFighter.Objects.View
{
    public class AbstractBattleNPCView : AbstractNPCView, iBattleModelViewProxy
    {
        public virtual void ExecuteAction()
        {
            StartCoroutine(ExecuteActionCoroutine());
        }

        public virtual void TakeDamage()
        {
            StartCoroutine(TakeDamageCoroutine());
        }


        System.Collections.IEnumerator ExecuteActionCoroutine()
        {
            transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);

            for (int i = 0; i < 5; i++)
                yield return null;

            transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
        }

        System.Collections.IEnumerator TakeDamageCoroutine()
        {
            transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);

            for (int i = 0; i < 5; i++)
                yield return null;

            transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
        }
    }
}
