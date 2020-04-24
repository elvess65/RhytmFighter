using RhytmFighter.Core.Enums;
using System.Collections.Generic;
using UnityEngine;

namespace RhytmFighter.Characters.Animation
{
    [System.Serializable]
    public abstract class AbstractAnimationController : MonoBehaviour
    {
        public Animator Controller;
        public AnimationKeys[] ExposedAnimationKeys;

        private Dictionary<AnimationTypes, string> m_AnimationKeys;
        private Dictionary<string, float> m_AnimationActionEventsExecuteTime;


        public virtual void PlayAnimation(AnimationTypes animationType)
        {
            string key = GetAnimationName(animationType);

            switch (animationType)
            {
                case AnimationTypes.Attack:
                    SetTrigger(key);
                    break;
                case AnimationTypes.Defence:
                    SetTrigger(key);
                    break;
                case AnimationTypes.Destroy:
                    SetTrigger(key);
                    break;
                case AnimationTypes.Idle:
                    Controller.SetBool(key, false);
                    break;
                case AnimationTypes.IncreaseHP:
                    SetTrigger(key);
                    break;
                case AnimationTypes.StartMove:
                    Controller.SetBool(key, true);
                    break;
                case AnimationTypes.TakeDamage:
                    SetTrigger(key);
                    break;
            }
        }


        public virtual void Initialize()
        {
            m_AnimationKeys = new Dictionary<AnimationTypes, string>();
            m_AnimationActionEventsExecuteTime = new Dictionary<string, float>();

            //Match action types with keys
            for (int i = 0; i < ExposedAnimationKeys.Length; i++)
            {
                if (!m_AnimationKeys.ContainsKey(ExposedAnimationKeys[i].Type))
                    m_AnimationKeys.Add(ExposedAnimationKeys[i].Type, ExposedAnimationKeys[i].Key);
            }

            //Match action types with delays
            AnimationClip[] clips = Controller.runtimeAnimatorController.animationClips;
            //Loop through all animation clips available in animator controller
            foreach (AnimationClip clip in clips)
            {
                //Loop through all animation keys mentioned in exposed keys list
                foreach (string animKey in m_AnimationKeys.Values)
                {
                    //Compare clip name with key (names should match)
                    if (clip.name.Equals(animKey))
                    {
                        //If there are some events and the key was not added - add to animation delay list
                        if (clip.events.Length > 0 && !m_AnimationActionEventsExecuteTime.ContainsKey(animKey))
                            m_AnimationActionEventsExecuteTime.Add(animKey, clip.events[0].time);
                        
                        continue;
                    }
                }
            }
        }


        protected void SetTrigger(string name)
        {
            Controller.SetTrigger(name);
        }

        protected void SetBool(string name, bool state)
        {
            Controller.SetBool(name, state);
        }

        protected string GetAnimationName(AnimationTypes animationType)
        {
            if (m_AnimationKeys.ContainsKey(animationType))
                return m_AnimationKeys[animationType];

            return string.Empty;
        }


        [System.Serializable]
        public class AnimationKeys
        {
            public AnimationTypes Type;
            public string Key;
        }
    }
}
