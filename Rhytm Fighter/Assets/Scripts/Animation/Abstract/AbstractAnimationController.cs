using RhytmFighter.Core.Enums;
using System.Collections.Generic;
using UnityEngine;

namespace RhytmFighter.Animation
{
    [System.Serializable]
    public abstract class AbstractAnimationController : MonoBehaviour
    {
        public Animator Controller;
        public AnimationKeys[] ExposedAnimationKeys;

        private Dictionary<AnimationTypes, string> m_AnimationKeys;
        private Dictionary<string, float> m_AnimationActionEventsExecuteTime;

        protected const string m_BASE_LAYER = "Base Layer";


        public abstract void PlayAnimation(AnimationTypes animationType);

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

        public float GetActionEventExecuteTime(AnimationTypes animationType)
        {
            if (m_AnimationActionEventsExecuteTime.TryGetValue(GetAnimationName(animationType), out float eventExecuteTime))
                return eventExecuteTime;

            return 0;
        }


        protected void SetTrigger(string name)
        {
            Controller.SetTrigger(name);
        }

        protected void SetBool(string name, bool state)
        {
            Controller.SetBool(name, state);
        }

        protected bool GetBool(string name)
        {
            return Controller.GetBool(name);
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
