using UnityEngine;

namespace RhytmFighter.Characters
{
    public class StandartAnimationController : MonoBehaviour
    {
        public Animator Controller;

        public void PlayMoveAnimation()
        {
            Controller.SetBool("move", true);
        }

        public void PlayIdleAnimation()
        {
            Controller.SetBool("move", false);
        }

        public void PlayAttackAnimation()
        {
            Controller.SetTrigger("attack");
        }

        public void PlayTakeDamageAnimation()
        {

        }

        public void PlayDestroyAnimation()
        {

        }
    }
}

