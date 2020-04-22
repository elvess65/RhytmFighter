using System.Collections.Generic;
using UnityEngine;

namespace RhytmFighter.Characters.Animation
{
    [System.Serializable]
    public abstract class AbstractAnimationController : MonoBehaviour
    {
        public Animator Controller;
        public AnimationKeys[] ExposedAnimationKeys;

        private Dictionary<AnimationActionTypes, string> m_ActionKeys;
        private Dictionary<string, float> m_AnimationActionEventsExecuteTime;

        public virtual void Initialize()
        {
            m_ActionKeys = new Dictionary<AnimationActionTypes, string>();
            m_AnimationActionEventsExecuteTime = new Dictionary<string, float>();

            //Match action types with keys
            for (int i = 0; i < ExposedAnimationKeys.Length; i++)
            {
                if (!m_ActionKeys.ContainsKey(ExposedAnimationKeys[i].Type))
                    m_ActionKeys.Add(ExposedAnimationKeys[i].Type, ExposedAnimationKeys[i].Key);
            }

            //Match action types with delays
            AnimationClip[] clips = Controller.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                foreach (string animKey in m_ActionKeys.Values)
                {
                    if (clip.name.Equals(animKey))
                    {
                        m_AnimationActionEventsExecuteTime.Add(animKey, clip.events[0].time);
                        Debug.Log(clip.name + " " + clip.length + " " + clip.events[0].time);
                        continue;
                    }
                }
            }
        }

        [System.Serializable]
        public partial class AnimationKeys
        {
            public AnimationActionTypes Type;
            public string Key;
        }

        //TODO: 
        //AnimActionKeysProxy 
        //CommandAnimActionProxy 
    }
}
