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
            foreach (AnimationClip clip in clips)
            {
                foreach (string animKey in m_AnimationKeys.Values)
                {
                    if (clip.name.Equals(animKey))
                    {
                        m_AnimationActionEventsExecuteTime.Add(animKey, clip.events[0].time);
                        continue;
                    }
                }
            }
        }

        public abstract void PlayAnimation(AnimationTypes animationType);


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
