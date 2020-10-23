using RhytmFighter.Persistant.Enums;
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
                    SetTrigger(key);
                    break;

                case AnimationTypes.TakeDamage:
                    //if (IsPlayingBattleIdle())
                    //    SetTrigger(key);
                    break;

                case AnimationTypes.IncreaseHP:
                    SetTrigger(key);
                    break;

                case AnimationTypes.StartMove:
                    SetBool(key, true);
                    break;

                case AnimationTypes.StopMove:
                    key = GetAnimationName(AnimationTypes.StartMove);
                    SetBool(key, false);
                    break;


                case AnimationTypes.Idle:
                    SetBool(GetAnimationName(AnimationTypes.BattleIdle), false);
                    break;

                case AnimationTypes.BattleIdle:
                
                    if (GetBool(GetAnimationName(AnimationTypes.StrafeLeft)))
                        SetBool(GetAnimationName(AnimationTypes.StrafeLeft), false);

                    if (GetBool(GetAnimationName(AnimationTypes.StrafeRight)))
                        SetBool(GetAnimationName(AnimationTypes.StrafeRight), false);

                    SetBool(key, true);
                    break;
               

                case AnimationTypes.StrafeLeft:
                    SetBool(key, true);
                    break;

                case AnimationTypes.StrafeRight:
                    SetBool(key, true);
                    break;

                case AnimationTypes.Teleport:
                    SetTrigger(key);
                    break;

                case AnimationTypes.Victory:
                    SetTrigger(key);
                    break;

                case AnimationTypes.MenuAction:
                    SetTrigger(key);
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

