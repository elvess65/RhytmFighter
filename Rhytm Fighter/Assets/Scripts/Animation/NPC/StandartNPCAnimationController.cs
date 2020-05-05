using RhytmFighter.Core.Enums;
using UnityEngine;

namespace RhytmFighter.Animation.NPC
{
    public class StandartNPCAnimationController : AbstractAnimationController
    {
        private int m_IdleHash;
        private int m_BattleIdleHash;

        public override void Initialize()
        {
            base.Initialize();

            //Hash idle animation
            m_IdleHash = Animator.StringToHash($"{m_BASE_LAYER}.{GetAnimationName(AnimationTypes.Idle)}");
            m_BattleIdleHash = Animator.StringToHash($"{m_BASE_LAYER}.{GetAnimationName(AnimationTypes.BattleIdle)}");
        }

        public override void PlayAnimation(AnimationTypes animationType)
        {
            string key = GetAnimationName(animationType);

            switch (animationType)
            {
                case AnimationTypes.Attack:
                    if (IsPlayingBattleIdle())
                        SetTrigger(key);
                    break;
                case AnimationTypes.Defence:
                    if (IsPlayingBattleIdle())
                        SetTrigger(key);
                    break;
                case AnimationTypes.Destroy:
                    if (IsPlayingBattleIdle())
                        SetTrigger(key);
                    break;
                case AnimationTypes.StopMove:
                    key = GetAnimationName(AnimationTypes.StartMove);
                    Controller.SetBool(key, false);
                    break;
                case AnimationTypes.IncreaseHP:
                    SetTrigger(key);
                    break;
                case AnimationTypes.StartMove:
                    Controller.SetBool(key, true);
                    break;
                case AnimationTypes.TakeDamage:
                    //if (IsPlayingIdle())
                    //    SetTrigger(key);
                    break;
                case AnimationTypes.BattleIdle:
                    Controller.SetBool(key, true);
                    break;
                case AnimationTypes.Idle:
                    key = GetAnimationName(AnimationTypes.BattleIdle);
                    Controller.SetBool(key, false);
                    break;

            }
        }

        bool IsPlayingIdle()
        {
            return Controller.GetCurrentAnimatorStateInfo(0).fullPathHash.Equals(m_IdleHash);
        }

        bool IsPlayingBattleIdle()
        {
            return Controller.GetCurrentAnimatorStateInfo(0).fullPathHash.Equals(m_BattleIdleHash);
        }
    }
}

